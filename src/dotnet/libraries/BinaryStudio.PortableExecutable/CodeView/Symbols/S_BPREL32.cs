using BinaryStudio.PortableExecutable.Win32;
using System;
using System.Drawing;
using System.IO;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_BPREL32)]
    internal class S_BPREL32 : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_BPREL32; }}
        public Int32 TypeIndexOrMetadataToken { get; }
        public new Int32 Offset { get; }
        public String Value { get; }
        public unsafe S_BPREL32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (BPRELSYM32*)Content;
            TypeIndexOrMetadataToken = r->TypeIndexOrMetadataToken;
            this.Offset = r->Offset;
            Value = ToString(Section.Section.Encoding, (Byte*)(r + 1), Section.Section.IsLengthPrefixedString);
            }
        }

    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_BPREL32_16)]
    internal class S_BPREL32_16 : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_BPREL32_16; }}
        public Int16 TypeIndex { get; }
        public new Int32 Offset { get; }
        public String Value { get; }
        public unsafe S_BPREL32_16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (BPRELSYM32_16*)Content;
            TypeIndex = r->TypeIndex;
            this.Offset = r->Offset;
            Value = ToString(Section.Section.Encoding, (Byte*)(r + 1), Section.Section.IsLengthPrefixedString);
            }
        }

    internal class S_BPREL32_16_TD32 : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_BPREL32_16; }}
        public Int32 TypeIndex { get; }
        public new Int32 Offset { get; }
        public Int32 NameIndex { get; }
        public Int32 BrowserOffset { get; }
        public unsafe S_BPREL32_16_TD32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
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
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} [ebp+{3}]", LinePrefix,base.Offset,Type,this.Offset);
            Writer.WriteLine("{0}  NameIndex:{{{1}}}:{{{2}}}", LinePrefix,NameIndex.ToString("x8"),NameTable[NameIndex-1]);
            Writer.WriteLine("{0}  BrowserOffset:{1:x4}", LinePrefix,BrowserOffset);
            }
        }
    }