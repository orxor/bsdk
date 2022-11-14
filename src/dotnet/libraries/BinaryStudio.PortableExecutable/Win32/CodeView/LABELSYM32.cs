using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    internal struct LABELSYM32
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly UInt32 Offset;
        public readonly UInt16 Segment;
        public readonly CV_PFLAG Flags;
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    internal struct LABELSYM16
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly UInt16 Offset;
        public readonly UInt16 Segment;
        public readonly CV_PFLAG Flags;
        }
    }