using System;
using System.IO;

namespace Options.Descriptors
    {
    internal class TestOptionDescriptor : OptionDescriptor
        {
        public override string OptionName { get { return "test"; }}

        public override bool TryParse(string source, out OperationOption option) {
            option = null;
            if (!String.IsNullOrWhiteSpace(source)) {
                source = source.Trim();
                if (source == "test") {
                    option = new TestOption();
                    return true;
                    }
                }
            return false;
            }

        public override void Usage(TextWriter output)
            {
            output.Write("test");
            }
        }
    }
