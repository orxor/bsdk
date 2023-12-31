﻿using System;
using System.IO;
using BinaryStudio.PortableExecutable.CodeView;

namespace BinaryStudio.PortableExecutable.TD32
    {
    [TD32Symbol(TD32SymbolIndex.S_END)]
    internal class S_END : TD32Symbol, ICodeViewBlockEnd, ICodeViewBlockElement
        {
        public ICodeViewBlockStart BlockStart { get;set; }
        public override TD32SymbolIndex Type { get { return TD32SymbolIndex.S_END; }}
        public S_END(CodeViewSymbolsSSection section, Int32 offset, IntPtr content, Int32 length)
            : base(section, offset, content, length)
            {
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2}",LinePrefix,Offset,Type);
            }
        }
    }