using System;
using System.Collections.Generic;

namespace Options
    {
    internal class TestOption : OperationOptionWithParameters
        {
        public TestOption(IList<String> values)
            :base(values)
            {
            }

        public TestOption()
            {
            }

        public override String OptionName { get { return "test"; }}
        }
    }
