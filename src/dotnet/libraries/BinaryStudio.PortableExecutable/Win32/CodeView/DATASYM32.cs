using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    public struct DATASYM32
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly Int32 Offset;
        public readonly Int16 Segment;
        public readonly Int32 TypeIndex;
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    public struct DATASYM16_32
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly UInt32 Offset;
        public readonly Int16 Segment;
        public readonly Int16 TypeIndex;
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    public struct DATASYM16_16
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly UInt16 Offset;
        public readonly Int16 Segment;
        public readonly UInt16 TypeIndex;
        }
    }