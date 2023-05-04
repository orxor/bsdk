using System;
using System.IO;

namespace Options.Descriptors
    {
    internal class ContainersOptionDescriptor : OptionDescriptor
        {
        public override String OptionName { get { return "containers"; }}
        public override Boolean TryParse(String source, out OperationOption option)
            {
            option = null;
            if (!String.IsNullOrWhiteSpace(source)) {
                source = source.Trim();
                if (source.StartsWith("containers:")) {
                    option = new ContainersOption(source.Substring(11).Trim().Split(';'));
                    return true;
                    }
                }
            return false;
            }

        public override void Usage(TextWriter output)
            {
            output.Write("containers:{value}");
            }
        }
    }