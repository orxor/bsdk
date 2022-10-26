using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TD32SymbolTypeInfo
        {
        public readonly Int32  TypeId;
        public readonly Int32  NameIndex;
        public readonly UInt16 Size;
        public readonly Byte   MaxSize;
        public readonly Int32  ParentIndex;
        }
    }