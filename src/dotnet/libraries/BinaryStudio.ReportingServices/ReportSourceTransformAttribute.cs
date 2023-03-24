using System;

namespace BinaryStudio.Reporting
    {
    public class ReportSourceTransformAttribute : Attribute
        {
        public String Stylesheet { get;set; }
        public Type Type { get;set; }
        public ReportSourceTransformAttribute(String stylesheet)
            {
            Stylesheet = stylesheet;
            }

        public ReportSourceTransformAttribute(Type type)
            {
            Type = type;
            }

        public ReportSourceTransformAttribute()
            {
            }
        }
    }