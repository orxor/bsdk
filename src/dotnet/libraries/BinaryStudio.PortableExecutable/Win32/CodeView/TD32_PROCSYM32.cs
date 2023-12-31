﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.TD32;

namespace BinaryStudio.PortableExecutable.Win32
    {
    /// <summary>
    /// This structure is related to
    /// <see cref="TD32SymbolIndex.S_LPROC32"/>,
    /// <see cref="TD32SymbolIndex.S_GPROC32"/>
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct TD32_PROCSYM32
        {
        [FieldOffset(0)] public readonly Int16 Length;
        [FieldOffset(2)] public readonly TD32SymbolIndex Type;
        [FieldOffset(4)] public readonly Int32 Parent;
        [FieldOffset(8)] public readonly Int32 End;
        [FieldOffset(12)] public readonly Int32 Next;
        [FieldOffset(16)] public readonly Int32 ProcedureLength;
        [FieldOffset(20)] public readonly Int32 DbgStart;
        [FieldOffset(24)] public readonly Int32 DbgEnd;
        [FieldOffset(28)] public readonly Int32 Offset;
        [FieldOffset(32)] public readonly Int16 Segment;
        [FieldOffset(36)] public readonly Int16 TypeIndex;
        [FieldOffset(38)] public readonly CV_PFLAG Flags;
        [FieldOffset(40)] public readonly Int32 NameIndex;
        }
    }