using System;

namespace BinaryStudio.PortableExecutable
    {
    public class OMFSSectionIndexAttribute : Attribute
        {
        public OMFSSectionIndex Index { get; }
        public OMFSSectionIndexAttribute(OMFSSectionIndex Index)
            {
            this.Index = Index;
            }
        }
    }