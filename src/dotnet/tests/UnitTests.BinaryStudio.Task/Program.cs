using System;
using System.Diagnostics;
using System.Reflection;
using BinaryStudio.DiagnosticServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BinaryStudio.PortableExecutable;
using UnitTests.BinaryStudio.Security.Cryptography.Certificates;

namespace UnitTests.BinaryStudio.Task
    {
    internal class Program
        {
        private static void Main(string[] args)
            {
            Execute(typeof(MZMetadataObjectT),nameof(MZMetadataObjectT.JsonS));
            }

        #region M:Execute(Action)
        private static void Execute(Action action)
            {
            Execute(null, action);
            }
        #endregion
        #region M:Execute(String,Action)
        private static void Execute(String source, Action action) {
            if (action == null) { throw new ArgumentNullException(nameof(action)); }
            try
                {
                action();
                if (source != null) {
                    Write(ConsoleColor.Green, "{ok}");
                    Write(ConsoleColor.Gray, $":{{{source}}}");
                    Console.WriteLine();
                    }
                }
            catch (Exception e)
                {
                Debug.WriteLine(Exceptions.ToString(e));
                WriteLine(ConsoleColor.Gray,$"Exception:{e.GetType().FullName}");
                WriteLine(ConsoleColor.Cyan,$"Message:{e.Message}");
                WriteLine(ConsoleColor.Yellow,Exceptions.ToString(e));
                if (source != null) {
                    Write(ConsoleColor.Red, "{error}");
                    Write(ConsoleColor.Gray, $":{{{source}}}");
                    Console.WriteLine();
                    }
                }
            }
        #endregion
        #region M:Execute(Type)
        private static void Execute(Type type)
            {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            Execute(()=>
                {
                var engine = Activator.CreateInstance(type);
                foreach (var methodinfo in type.GetMethods(BindingFlags.Instance|BindingFlags.Public)) {
                    if ((methodinfo.GetCustomAttribute<TestMethodAttribute>() != null) &&
                        (methodinfo.GetCustomAttribute<IgnoreAttribute>() == null))
                        {
                        Execute($"{type.Name}.{methodinfo.Name}", () => methodinfo.Invoke(engine, null));
                        }
                    }
                });
            }
        #endregion
        #region M:Execute(String,Type,String)
        private static void Execute(Type type,String MethodName) {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            Execute(()=>
                {
                var engine = Activator.CreateInstance(type);
                foreach (var methodinfo in type.GetMethods(BindingFlags.Instance|BindingFlags.Public)) {
                    if ((methodinfo.GetCustomAttribute<TestMethodAttribute>() != null) &&
                        (methodinfo.GetCustomAttribute<IgnoreAttribute>() == null))
                        {
                        if (MethodName == methodinfo.Name)
                            {
                            Execute($"{type.Name}.{methodinfo.Name}", () => methodinfo.Invoke(engine, null));
                            return;
                            }
                        }
                    }
                });
            }
        #endregion
        #region M:Write(ConsoleColor,String)
        private static void Write(ConsoleColor color, String message) {
            using (new ColorScope(color)) {
                Console.Write(message);
                }
            }
        #endregion
        #region M:WriteLine(ConsoleColor,String)
        private static void WriteLine(ConsoleColor color, String message) {
            using (new ColorScope(color)) {
                Console.WriteLine(message);
                }
            }
        #endregion

        private class ColorScope : IDisposable
            {
            private readonly ConsoleColor color;
            public ColorScope(ConsoleColor color)
                {
                this.color = Console.ForegroundColor;
                Console.ForegroundColor = color;
                }

            void IDisposable.Dispose()
                {
                Console.ForegroundColor = color;
                }
            }
        }
    }
