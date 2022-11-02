using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct CODEVIEW_SYMBOL_RECORD_HEADER
        {
        /// <summary>
        /// Length of record, excluding the length field.
        /// </summary>
        public readonly UInt16 Length;
        /// <summary>
        /// Type of symbol.
        /// </summary>
        public readonly DEBUG_SYMBOL_INDEX Type;
        }
    }