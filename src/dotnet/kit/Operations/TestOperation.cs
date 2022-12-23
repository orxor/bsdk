using System;
using System.Collections.Generic;
using System.IO;
using BinaryStudio.IO;
using BinaryStudio.Security.Cryptography;
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
            
            }
        }
    }
