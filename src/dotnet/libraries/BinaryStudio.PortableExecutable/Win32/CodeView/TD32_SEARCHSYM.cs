using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    /// <summary>{<see cref="DEBUG_SYMBOL_INDEX.S_SSEARCH"/>} Start Search.</summary>
    /// <remarks>
    /// These records are always the first symbol records in a module's $$SYMBOL section.
    /// There is one <b>Start Search</b> symbol for each segment (PE section) to which the module contributes code.
    /// Each <b>Start Search</b> symbol contains the segment (PE section) number and $$SYMBOL offset of the record
    /// of the outermost lexical scope in this module that physically appears first in the specified segment of the load image.
    /// This referenced symbol is the symbol used to initiate context searches within this module.
    /// The <b>Start Search</b> symbols are inserted into the $$SYMBOLS table by the CVPACK utility
    /// and must not be emitted by the language processor.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct TD32_SEARCHSYM
        {
        /// <summary>
        /// Length of record, excluding the length field.
        /// </summary>
        public readonly UInt16 Length;
        /// <summary>
        /// Type of symbol {<see cref="DEBUG_SYMBOL_INDEX.S_SSEARCH"/>}.
        /// </summary>
        public readonly DEBUG_SYMBOL_INDEX Type;
        /// <summary>
        /// $$SYMBOL offset of the procedure or thunk record for this module that has the lowest offset for the specified segment.
        /// </summary>
        public readonly Int32 Offset;
        /// <summary>
        /// Segment (PE section) to which this <b>Start Search</b> refers.
        /// </summary>
        public readonly Int16 Segment;
        public readonly Int16 CodeSyms;
        public readonly Int16 DataSyms;
        public readonly Int32 FirstData;
        }
    }