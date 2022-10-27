using System;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TD32SymbolTypeInfo
        {
        public readonly UInt16 Size;
        public readonly LEAF_ENUM  Leaf;
        }
    }