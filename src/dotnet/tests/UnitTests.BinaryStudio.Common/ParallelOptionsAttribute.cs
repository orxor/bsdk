using System;

namespace UnitTests.BinaryStudio.Common
    {
    public class ParallelOptionsAttribute : Attribute
        {
        public Int32 MaxDegreeOfParallelism { get;set; }
        public ParallelOptionsAttribute()
            {
            MaxDegreeOfParallelism = 1;
            }
        }
    }