using System;
using BinaryStudio.PortableExecutable;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

namespace DllResources
    {
    class Program
        {
        static void Main(string[] args) {
            using (var Scope = new MetadataScope()) {
                var o = Scope.Load(@"C:\Windows\System32\winmm.dll");
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
