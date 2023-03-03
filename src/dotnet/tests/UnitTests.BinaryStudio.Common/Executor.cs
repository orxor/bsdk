using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BinaryStudio.DiagnosticServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.BinaryStudio.Common
    {
    public class Executor
        {
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
        #region M:Execute(Action)
        public void Execute(Action action)
            {
            Execute(null, action);
            }
        #endregion
        #region M:Execute(String,Action)
        public void Execute(String source, Action action) {
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
        #region M:Execute({out}Exception,Boolean,String,Action):Boolean
        private static Boolean Execute(out Exception e, Boolean console,String source, Action action) {
            if (action == null) { throw new ArgumentNullException(nameof(action)); }
            var timerS = String.Empty;
            e = null;
            try
                {
                var timerT = new Stopwatch();
                timerT.Start();
                try
                    {
                    action();
                    }
                finally
                    {
                    timerT.Stop();
                    timerS = timerT.Elapsed.ToString("hh\\:mm\\:ss\\.fffff");
                    }
                if ((source != null) && (console)) {
                    Write(ConsoleColor.Green, $"{timerS}:{{ok}}");
                    Write(ConsoleColor.Gray, $":{{{source}}}");
                    Console.WriteLine();
                    }
                return true;
                }
            catch (Exception x)
                {
                if (console) {
                    Debug.WriteLine(Exceptions.ToString(e));
                    WriteLine(ConsoleColor.Gray,$"Exception:{e.GetType().FullName}");
                    WriteLine(ConsoleColor.Cyan,$"Message:{e.Message}");
                    WriteLine(ConsoleColor.Yellow,Exceptions.ToString(e));
                    if (source != null) {
                        Write(ConsoleColor.Red, $"{timerS}:{{error}}");
                        Write(ConsoleColor.Gray, $":{{{source}}}");
                        Console.WriteLine();
                        }
                    }
                e = x;
                return false;
                }
            }
        #endregion
        #region M:Execute(Type,IEnumerable<MethodInfo>)
        private void Execute(Type type, IEnumerable<MethodInfo> Methods)
            {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            Execute(()=>
                {
                var SyncO = new Object();
                var TestInitialize = type.GetMethods(BindingFlags.Instance|BindingFlags.Public).FirstOrDefault(i => Attribute.IsDefined(i,typeof(TestInitializeAttribute)));
                var TestCleanup    = type.GetMethods(BindingFlags.Instance|BindingFlags.Public).FirstOrDefault(i => Attribute.IsDefined(i,typeof(TestCleanupAttribute)));
                var NumberOfThreads = (type.GetCustomAttribute<ParallelOptionsAttribute>(false) ?? new ParallelOptionsAttribute()).MaxDegreeOfParallelism;
                var ok = 0;
                var error = 0;
                Parallel.ForEach(Methods,
                    new ParallelOptions{
                        MaxDegreeOfParallelism = NumberOfThreads
                        },methodinfo =>
                    {
                    var timerS = String.Empty;
                    var TestMethodAttribute = methodinfo.GetCustomAttribute<TestMethodAttribute>() ?? new TestMethodAttribute(methodinfo.Name);
                    var source = $"{type.Name}.{TestMethodAttribute.DisplayName}";
                    var engine = Activator.CreateInstance(type);
                    var timerT = new Stopwatch();
                    var status = true;
                    Exception e = null;
                    timerT.Start();
                    try
                        {
                        if (TestInitialize != null) { TestInitialize.Invoke(engine, null); }
                        try
                            {
                            if (Execute(out e, false, $"{source}", () => methodinfo.Invoke(engine, null)))
                                {
                                ok++;
                                }
                            else
                                {
                                status = false;
                                error++;
                                }
                            }
                        finally
                            {
                            if (TestCleanup != null) { TestCleanup.Invoke(engine, null); }
                            }
                        }
                    finally
                        {
                        timerT.Stop();
                        timerS = timerT.Elapsed.ToString("hh\\:mm\\:ss\\.fffff");
                        lock(SyncO) {
                            if (status) {
                                Write(ConsoleColor.Green, $"{timerS}:{{ok}}");
                                Write(ConsoleColor.Gray, $":{{{source}}}");
                                Console.WriteLine();
                                }
                            else
                                {
                                Debug.WriteLine(Exceptions.ToString(e));
                                WriteLine(ConsoleColor.Gray,$"Exception:{e.GetType().FullName}");
                                WriteLine(ConsoleColor.Cyan,$"Message:{e.Message}");
                                WriteLine(ConsoleColor.Yellow,Exceptions.ToString(e));
                                Write(ConsoleColor.Red, $"{timerS}:{{error}}");
                                Write(ConsoleColor.Gray, $":{{{source}}}");
                                Console.WriteLine();
                                }
                            }
                        }
                    });
                Write(ConsoleColor.Green, "{ok}");
                WriteLine(ConsoleColor.Gray, $":{{{ok}}}");
                Write(ConsoleColor.Red, "{error}");
                WriteLine(ConsoleColor.Gray, $":{{{error}}}");
                });
            }
        #endregion
        #region M:Execute(Type)
        public void Execute(Type Type) {
            if (Type == null) { throw new ArgumentNullException(nameof(Type)); }
            Execute(Type,Type.GetMethods(BindingFlags.Instance|BindingFlags.Public).
                Where(i =>Attribute.IsDefined(i,typeof(TestMethodAttribute)) && !Attribute.IsDefined(i,typeof(IgnoreAttribute))).
                OrderBy(i=> (i.GetCustomAttribute<PriorityAttribute>() ?? new PriorityAttribute(0)).Priority).
                ThenBy(i => i.Name));
            }
        #endregion
        #region M:Execute(Type,String)
        public void Execute(Type Type,String MethodName) {
            if (Type == null) { throw new ArgumentNullException(nameof(Type)); }
            if (MethodName == null) { throw new ArgumentNullException(nameof(MethodName)); }
            if (String.IsNullOrEmpty(MethodName)) { throw new ArgumentOutOfRangeException(nameof(MethodName)); }
            Execute(Type,Type.GetMethods(BindingFlags.Instance|BindingFlags.Public).
                Where(i =>Attribute.IsDefined(i,typeof(TestMethodAttribute)) && !Attribute.IsDefined(i,typeof(IgnoreAttribute)) && (String.Equals(i.Name,MethodName))).
                OrderBy(i=> (i.GetCustomAttribute<PriorityAttribute>() ?? new PriorityAttribute(0)).Priority));
            }
        #endregion
        }
    }
