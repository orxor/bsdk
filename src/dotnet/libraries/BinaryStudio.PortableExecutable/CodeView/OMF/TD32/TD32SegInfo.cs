using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TD32SegInfo
        {
        public readonly Int16 Segment; // Segment that this structure describes
        public readonly Int16 Flags;   // Attributes for the logical segment.
                                       // The following attributes are defined:
                                       //   $0000  Data segment
                                       //   $0001  Code segment
        public readonly Int32 Offset;  // Offset in segment where the code starts
        public readonly Int32 Size;    // Count of the number of bytes of code in the segment
        }
    }