using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.DiagnosticServices.Logging;
using BinaryStudio.DirectoryServices;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;
using Newtonsoft.Json;
using Options;
using Options.Descriptors;
using static System.Formats.Asn1.AsnWriter;

namespace Operations
{
    internal abstract class Operation
        {
        public static readonly Object console = new Object();
        public static ILogger Logger { get;set; }
        public static LocalClient LocalClient { get;set; }

        protected Operation(IList<OperationOption> args)
            {
            }

        public static IList<OperationOption> Parse(String[] args) {
            var r = new List<OperationOption>();
            for (var i = 0; i < args.Length; ++i) {
                foreach (var descriptor in descriptors) {
                    if (descriptor.TryParse(args[i], out var option)) {
                        r.Add(option);
                        }
                    }
                }
            return r;
            }

        protected static readonly IList<OptionDescriptor> descriptors = new List<OptionDescriptor>();
        static Operation()
            {
            foreach (var type in typeof(OptionDescriptor).Assembly.GetTypes()) {
                if ((typeof(OptionDescriptor).IsAssignableFrom(type)) && (!type.IsAbstract)) {
                    descriptors.Add((OptionDescriptor)Activator.CreateInstance(type));
                    }
                }
            }

        public virtual void ValidatePermission()
            {
            }

        public virtual void Break()
            {
            }

        public abstract void Execute(TextWriter output);
        protected static Boolean IsNullOrEmpty<T>(ICollection<T> value) {
            return (value == null) || (value.Count == 0);
            }

        protected static unsafe void RequestConsoleSecureStringEventHandler(Object sender, RequestSecureStringEventArgs e)
            {
            Console.WriteLine($@"Type pin-code for container ""{e.Container}""");
            Console.Write("Pin-code:");
            var o = Console.ReadLine();
            fixed (Char* c = o)
                {
                e.SecureString = new SecureString(c, o.Length);
                }
            }

        #region M:WriteLine(ConsoleColor,String,Object[])
        protected void WriteLine(TextWriter writer, ConsoleColor color, String format, params Object[] args) {
            lock(ColorScope.SyncRoot) {
                using (new ColorScope(color)) {
                    writer.WriteLine(format, args);
                    }
                }
            }
        #endregion
        #region M:WriteLine(ConsoleColor,String)
        protected void WriteLine(TextWriter writer, ConsoleColor color, String message) {
            lock(ColorScope.SyncRoot) {
                using (new ColorScope(color)) {
                    writer.WriteLine(message);
                    }
                }
            }
        #endregion
        #region M:Write(ConsoleColor,String)
        protected void Write(TextWriter writer, ConsoleColor color, String message) {
            lock(ColorScope.SyncRoot) {
                using (new ColorScope(color)) {
                    writer.Write(message);
                    }
                }
            }
        #endregion

        protected static void Dispose<T>(ref T o)
            where T: IDisposable
            {
            if (o != null) {
                o.Dispose();
                o = default;
                }
            }

        protected static void RequestWindowSecureStringEventHandler(Object sender, RequestSecureStringEventArgs e)
            {
            e.SecureString = new SecureString();
            e.SecureString.AppendChar('1');
            e.SecureString.AppendChar('2');
            e.SecureString.AppendChar('3');
            e.SecureString.AppendChar('4');
            e.SecureString.AppendChar('5');
            e.SecureString.AppendChar('6');
            e.SecureString.AppendChar('7');
            e.SecureString.AppendChar('8');
            e.StoreSecureString = true;
            }

        public virtual Object GetService(Object source, Type service)
            {
            if (service == typeof(IFileService))      { return DirectoryService.GetService(source, service); }
            if (service == typeof(IDirectoryService)) { return DirectoryService.GetService(source, service); }
            return source;
            }

        public T GetService<T>(Object source)
            {
            return (T)GetService(source, typeof(T));
            }

        #region M:JsonSerialize(Object,TextWriter)
        protected static void JsonSerialize(Object value, TextWriter output) {
            using (var writer = new DefaultJsonWriter(new JsonTextWriter(output){
                Formatting = Formatting.Indented,
                Indentation = 2,
                IndentChar = ' '}))
                {
                if (value is IJsonSerializable o)
                    {
                    o.WriteTo(writer);
                    }
                }
            }
        #endregion
        }
    }