using System;

namespace BinaryStudio.VSShellServices
    {
    public class ToolWindowCommandAttribute : Attribute
        {
        public Int32 CommandId { get;set; }
        public String CommandSet { get;set; }
        }
    }