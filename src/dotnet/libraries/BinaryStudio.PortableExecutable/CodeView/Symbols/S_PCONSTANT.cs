﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    internal class S_PCONSTANT_TD32 : CodeViewSymbol
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [DebuggerDisplay(@"\{{Type}\}")]
        private struct HEADER
            {
            private readonly Int16 Length;
            private readonly DEBUG_SYMBOL_INDEX Type;
            public  readonly Int32 TypeIndex;
            public  readonly Int16 BrowserOffset;
            public  readonly Int32 NameIndex;
            public  readonly Int32 PropertyIndex;
            public  readonly Int32 PropertyValue;
            }

        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_PCONSTANT; }}
        public Int32 TypeIndex { get; }
        public Int32 NameIndex { get; }
        public Int16 BrowserOffset { get; }
        public Int32 PropertyIndex { get; }
        public Int32 PropertyValue { get; }

        public unsafe S_PCONSTANT_TD32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (HEADER*)Content;
            TypeIndex = Header->TypeIndex;
            NameIndex = Header->NameIndex;
            BrowserOffset = Header->BrowserOffset;
            PropertyIndex = Header->PropertyIndex;
            PropertyValue = Header->PropertyValue;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} TypeIndex:{3:x4}", LinePrefix,Offset,Type,TypeIndex);
            Writer.WriteLine("{0}  NameIndex:{{{1}}}:{{{2}}}", LinePrefix,NameIndex.ToString("x8"),NameTable[NameIndex-1]);
            Writer.WriteLine("{0}  BrowserOffset:{1:x4}", LinePrefix,BrowserOffset);
            Writer.WriteLine("{0}  PropertyIndex:{1:x4}", LinePrefix,PropertyIndex);
            Writer.WriteLine("{0}  PropertyValue:{1:x4}", LinePrefix,PropertyValue);
            }
        }
    }