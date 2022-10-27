using System;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TD32SymbolInfo
        {
        public readonly Int16 Size;
        public readonly DEBUG_SYMBOL_INDEX SymbolType;
        }
    }