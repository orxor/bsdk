using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct OMFSegInfo
        {
        public readonly Int16 Segment; // Segment that this structure describes
        public readonly Int16 Flags;
        public readonly Int32 Offset;  // Offset in segment where the code starts
        public readonly Int32 Size;    // Count of the number of bytes of code in the segment
        }
    }