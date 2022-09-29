using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.PortableExecutable;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

namespace DllResources
    {
    class Program
        {
        private static void EnumReport<T>(CultureInfo culture)
            where T: Enum
            {
            foreach (var name in Enum.GetNames(typeof(T)).OrderBy(i=>{
                    return unchecked((UInt32)(Int32)(Enum.Parse(typeof(T), i)));
                    }))
                {
                var scode = unchecked((UInt32)(Int32)(Enum.Parse(typeof(T), name)));
                var value = HResultException.FormatMessage(scode, culture);
                Console.Out.WriteLine($"{{{scode.ToString("x8")}}}:{{{name}}}:{{{value}}}");
                //Console.Out.WriteLine($"  <Message Language=\"0x0409\" SCode=\"{scode.ToString("x8")}\">{value}</Message>");
                }
            }

        static void Main(string[] args) {
            var culture = CultureInfo.GetCultureInfo("en-US");
            Console.OutputEncoding = Encoding.UTF8;
            EnumReport<HResult>(culture);
            EnumReport<Win32ErrorCode>(culture);
            if (args.Length == 0) {
                Console.WriteLine($"USAGE: {{this.exe}} {{full-library-path}}");
                return;
                }
            if (args[0].Contains("*")) {
                var folder = Path.GetDirectoryName(args[0]);
                var pattern = Path.GetFileName(args[0]);
                foreach (var filename in Directory.EnumerateFiles(folder, pattern)) {
                    Console.Error.WriteLine(filename);
                    using (var Scope = new MetadataScope()) {
                        var o = Scope.Load(filename);
                        if (o != null) {
                            using (var writer = new DefaultJsonWriter(new JsonTextWriter(Console.Out){
                                Formatting = Formatting.Indented,
                                Indentation = 2,
                                IndentChar = ' '
                                }))
                                {
                                o.WriteTo(writer);
                                }
                            }
                        }
                    }
                }
            else
                {
                using (var Scope = new MetadataScope()) {
                    var o = Scope.Load(args[0]);
                    if (o != null) {
                        using (var writer = new DefaultJsonWriter(new JsonTextWriter(Console.Out){
                            Formatting = Formatting.Indented,
                            Indentation = 2,
                            IndentChar = ' '
                            }))
                            {
                            o.WriteTo(writer);
                            }
                        }
                    }
                }
            }
        }
    }
