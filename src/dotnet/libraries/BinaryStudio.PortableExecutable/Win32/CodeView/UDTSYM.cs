using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    internal struct UDTSYM
        {
        public readonly Int16 Length;                   // Record length.
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly Int32 TypeIndex;
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    internal struct UDTSYM16
        {
        public readonly Int16 Length;                   // Record length.
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly Int16 TypeIndex;
        }
    }