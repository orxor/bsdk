using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    internal struct BPRELSYM32
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly Int32 Offset;
        public readonly Int32 TypeIndexOrMetadataToken;
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    internal struct BPRELSYM16
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly UInt16 Offset;
        public readonly Int16 TypeIndex;
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    internal struct BPRELSYM32_16
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly Int32 Offset;
        public readonly Int16 TypeIndex;
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    internal struct BPRELSYM32_16_TD32
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly Int32 Offset;
        public readonly Int32 TypeIndex;
        public readonly Int32 NameIndex;
        public readonly Int32 BrowserOffset;
        }
    }