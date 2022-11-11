using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    internal struct OBJNAMESYM
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly UInt32 Signature;
        }
    }