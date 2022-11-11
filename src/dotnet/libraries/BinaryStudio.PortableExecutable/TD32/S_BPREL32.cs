using System;
using System.IO;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.TD32
    {
    [TD32Symbol(TD32SymbolIndex.S_BPREL32)]
    [UsedImplicitly]
    internal class S_BPREL32 : TD32Symbol
        {
        public override TD32SymbolIndex Type { get { return TD32SymbolIndex.S_BPREL32; }}
        public Int32 TypeIndex { get; }
        public new Int32 Offset { get; }
        public Int32 NameIndex { get; }
        public Int32 BrowserOffset { get; }

        public unsafe S_BPREL32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (BPRELSYM32_16_TD32*)Content;
            TypeIndex = Header->TypeIndex;
            this.Offset = Header->Offset;
            NameIndex = Header->NameIndex;
            BrowserOffset = Header->BrowserOffset;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} [ebp+{3:D4}]", LinePrefix,base.Offset,Type,this.Offset);
            Writer.WriteLine("{0}  NameIndex:{{{1}}}:{{{2}}}", LinePrefix,NameIndex.ToString("x8"),NameTable[NameIndex-1]);
            Writer.WriteLine("{0}  BrowserOffset:{1:x4}", LinePrefix,BrowserOffset);
            }
        }
    }