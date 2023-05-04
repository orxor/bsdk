﻿using System;
using System.IO;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.TD32
    {
    [TD32Symbol(TD32SymbolIndex.S_REGISTER)]
    internal class S_REGISTER : TD32Symbol
        {
        public override TD32SymbolIndex Type { get { return TD32SymbolIndex.S_REGISTER; }}
        public Int16 TypeIndex { get; }
        public Int32 NameIndex { get; }
        public UInt16 Register { get; }
        public Int16 BrowserOffset { get; }
        public unsafe S_REGISTER(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (TD32_REGSYM*)Content;
            TypeIndex = Header->TypeIndex;
            Register = Header->Register;
            BrowserOffset = Header->BrowserOffset;
            NameIndex = Header->NameIndex;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} TypeIndex:{3:x4} Register:{4}", LinePrefix,Offset,Type,TypeIndex,DecodeRegister(Register));
            Writer.WriteLine("{0}  NameIndex:{{{1}}}:{{{2}}}", LinePrefix,NameIndex.ToString("x8"),NameTable[NameIndex-1]);
            Writer.WriteLine("{0}  BrowserOffset:{1:x4}", LinePrefix,BrowserOffset);
            }
        }
    }