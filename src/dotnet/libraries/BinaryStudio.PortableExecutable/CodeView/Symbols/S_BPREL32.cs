using BinaryStudio.PortableExecutable.Win32;
using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
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
    }