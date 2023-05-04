using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    /// <summary>
    /// The file table describes the code segments that receive code from this
    /// source file.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TD32SourceFileEntry
        {
        public readonly Int16 SegmentCount;  // Number of segments that receive code from this source file.
        public readonly Int32 NameIndex;     // Name index of Source file name.
        // Int32 BaseSrcFiles[SegmentCount]; // An array of offsets for the line/address mapping
                                             // tables for each of the segments that receive code
                                             // from this source file.

        // Int64 SegmentAddress[SegmentCount];
        //     ? Name;
        }
    }