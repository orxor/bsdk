using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TD32ModuleInfo
        {
        public  readonly Int16 OverlayNumber;  // Overlay number
        public  readonly Int16 LibraryIndex;   // Index into sstLibraries subsection if this module was linked from a library
        public  readonly Int16 SegmentCount;   // Count of the number of code segments this module contributes to
        public  readonly Int16 DebuggingStyle; // Debugging style  for this  module.
        public  readonly Int32 NameIndex;      // Name index of module.
        public  readonly Int32 TimeStamp;      // Time stamp from the OBJ file.
        private unsafe fixed Byte Reserved[12];
        // TD32SegmentInfo Segments[SegmentCount];
        // Detailed information about each segment
        // that code is contributed to.
        // This is an array of cSeg count segment
        // information descriptor structures.
        }
    }