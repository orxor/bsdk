﻿using System;
using System.Globalization;
using System.IO;
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
        static void Main(string[] args) {
            var culture = CultureInfo.GetCultureInfo("ru-RU");
            Console.OutputEncoding = Encoding.UTF8;
            foreach (var name in Enum.GetNames(typeof(HResult))) {
                var scode = unchecked((UInt32)(Int32)(HResult)Enum.Parse(typeof(HResult),name));
                var value = HResultException.FormatMessage(scode,culture);
                //Console.Error.WriteLine($"{{{name}}}:{{{value}}}");
                }
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
