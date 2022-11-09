using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.TD32;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    internal struct BLOCKSYM32
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly UInt32 pParent;    // pointer to the parent
        public readonly UInt32 pEnd;       // pointer to this blocks end
        public readonly UInt32 len;        // Block length
        public readonly UInt32 off;        // Offset in code segment
        public readonly UInt16 seg;        // segment of label
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    internal struct BLOCKSYM32_TD32
        {
        private readonly Int16 Length;
        private readonly TD32SymbolIndex Type;
        public readonly Int32 Parent;    // pointer to the parent
        public readonly Int32 CodeLength; // Code length.
        public readonly Int32 CodeOffset; // Offset in code segment
        public readonly Int16 SegmentIndex;
        public readonly Int16 Flags;
        public readonly Int32 TypeIndex;
        public readonly Int32 NameIndex;
        public readonly Int32 VariableOffset;
        }
    }