using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    /// <summary>
    /// This structure is related to
    /// <see cref="DEBUG_SYMBOL_INDEX.S_GPROC32"/>,
    /// <see cref="DEBUG_SYMBOL_INDEX.S_GPROC32_ID"/>,
    /// <see cref="DEBUG_SYMBOL_INDEX.S_LPROC32"/>,
    /// <see cref="DEBUG_SYMBOL_INDEX.S_LPROC32_16"/>,
    /// <see cref="DEBUG_SYMBOL_INDEX.S_LPROC32_ID"/>,
    /// <see cref="DEBUG_SYMBOL_INDEX.S_LPROC32_DPC"/>,
    /// <see cref="DEBUG_SYMBOL_INDEX.S_LPROC32_DPC_ID"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    [Obsolete]
    public struct CODEVIEW_PROCSYM32
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly Int32 Parent;
        public readonly Int32 End;
        public readonly Int32 Next;
        public readonly Int32 ProcedureLength;
        public readonly Int32 DbgStart;
        public readonly Int32 DbgEnd;
        public readonly Int32 Offset;
        public readonly Int16 Segment;
        public readonly DEBUG_TYPE_ENUM ProcedureType;
        public readonly CV_PFLAG Flags;
        }

    /// <summary>
    /// This structure is related to
    /// <see cref="DEBUG_SYMBOL_INDEX.S_GPROC16"/>,
    /// <see cref="DEBUG_SYMBOL_INDEX.S_LPROC16"/>,
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    public struct CODEVIEW_PROCSYM16
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly Int32 Parent;
        public readonly Int32 End;
        public readonly Int32 Next;
        public readonly UInt16 ProcedureLength;
        public readonly UInt16 DbgStart;
        public readonly UInt16 DbgEnd;
        public readonly UInt16 Offset;
        public readonly Int16 Segment;
        public readonly Int16 ProcedureTypeIndex;
        public readonly CV_PFLAG Flags;
        }

    /// <summary>
    /// This structure is related to
    /// <see cref="DEBUG_SYMBOL_INDEX.S_LPROC32_16"/>,
    /// <see cref="DEBUG_SYMBOL_INDEX.S_GPROC32_16"/>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
    public struct CODEVIEW_PROCSYM32_16
        {
        public readonly Int16 Length;
        public readonly DEBUG_SYMBOL_INDEX Type;
        public readonly Int32 Parent;
        public readonly Int32 End;
        public readonly Int32 Next;
        public readonly Int32 ProcedureLength;
        public readonly Int32 DbgStart;
        public readonly Int32 DbgEnd;
        public readonly Int32 Offset;
        public readonly Int16 Segment;
        public readonly DEBUG_TYPE_ENUM TypeIndex;
        public readonly CV_PFLAG Flags;
        }
    }