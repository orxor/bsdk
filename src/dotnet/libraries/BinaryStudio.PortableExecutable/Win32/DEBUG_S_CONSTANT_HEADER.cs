using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct DEBUG_S_CONSTANT_HEADER
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly Int16 FieldTypeIndex;
        public readonly LEAF_ENUM FieldValue;
        }
    }