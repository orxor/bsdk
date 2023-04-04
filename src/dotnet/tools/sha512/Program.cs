using System;
using System.IO;
using System.Security.Cryptography;

namespace sha512
    {
    class Program
        {
        static void Main(String[] args) {
            if (args.Length > 0) {
                var block = new Byte[1024];
                using (var engine = new SHA512Managed()) {
                    using (var input = File.OpenRead(args[0])) {
                        engine.Initialize();
                        var r = engine.ComputeHash(input);
                        File.WriteAllText(args[0]+".sha512",Convert.ToBase64String(r));
                        }
                    }
                }
            }
        }
    }
