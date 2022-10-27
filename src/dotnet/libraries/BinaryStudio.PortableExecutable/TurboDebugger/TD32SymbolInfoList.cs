using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TD32SymbolInfoList
        {
        public readonly UInt32 Signature;
        // TD32SymbolInfo Symbols[]
        }
    }