using System;
using System.Collections.Generic;

namespace Options
    {
    internal class ContainersOption : OperationOptionWithParameters
        {
        public ContainersOption(IList<String> values)
            : base(values)
            {
            }

        public override String OptionName { get { return "containers"; }}
        }
    }