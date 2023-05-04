using System;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    internal class CodeViewLeafIndexAttribute : Attribute
        {
        public virtual LEAF_ENUM LeafIndex { get; }
        public CodeViewLeafIndexAttribute(LEAF_ENUM LeafIndex)
            {
            this.LeafIndex = LeafIndex;
            }
        }
    }