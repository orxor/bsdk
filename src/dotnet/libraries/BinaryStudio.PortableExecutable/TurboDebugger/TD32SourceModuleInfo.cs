using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    /// <summary>
    /// The module header structure describes the source file and code segment
    /// organization of the module.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TD32SourceModuleInfo
        {
        public readonly Int16 FileCount;    // The number of source files contributing code to segments.
        public readonly Int16 SegmentCount; // The number of code segments receiving code from this module.
        // Int32 BaseSrcFiles[FileCount];
        // Int64 SegmentAddress[SegmentCount];
        // Int16 SegmentIndexes[SegmentCount];
        }
    }