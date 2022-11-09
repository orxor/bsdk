using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.TD32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct TD32SymbolRecordHeader
        {
        /// <summary>
        /// Length of record, excluding the length field.
        /// </summary>
        public readonly UInt16 Length;
        /// <summary>
        /// Type of symbol.
        /// </summary>
        public readonly TD32SymbolIndex Type;
        }
    }