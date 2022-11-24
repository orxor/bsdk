using System;

namespace BinaryStudio.PortableExecutable
    {
    public class OMFSegmentInfo
        {
        public Int16 Index { get; }
        public Int32 Offset { get; }
        public Int32 Size { get; }
        public Int16 Flags { get; }
        internal OMFSegmentInfo(Int16 index, Int32 offset, Int32 size, Int16 flags)
            {
            Index = index;
            Offset = offset;
            Size = size;
            Flags = flags;
            }
        }
    }