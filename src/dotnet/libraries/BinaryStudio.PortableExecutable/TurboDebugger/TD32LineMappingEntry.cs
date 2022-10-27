using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    /// <summary>
    /// The line number to address mapping information is contained in a table.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TD32LineMappingEntry
        {
        public readonly Int16 SegmentIndex;
        public readonly Int16 PairCount;
        // Int32 Offsets[PairCount];
        // Int16 LineNumbers[PairCount];
        }
    }