using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinaryStudio.IO;
using BinaryStudio.Security.Cryptography;
using BinaryStudio.Security.Cryptography.Certificates;
using Options;

namespace Operations
    {
    internal class TestOperation : Operation
        {
        public TestOperation(IList<OperationOption> args)
            : base(args)
            {
            }

        public override void Execute(TextWriter output)
            {
            foreach(var filename in Directory.EnumerateFiles(@"C:\TFS\bsdk\src\dotnet\tests\UnitTestData\cer","*.cer").ToArray()) {
                var certificate = new X509Certificate(File.ReadAllBytes(filename));
                var thumbprint = certificate.Thumbprint;
                File.Move(filename,Path.Combine(Path.GetDirectoryName(filename),$"{{{certificate.Country}}}{{{thumbprint}}}.cer"));
                }
            }
        }
    }
