using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    internal abstract class S_BLOCKSYM32 : CodeViewSymbol
        {
        public UInt16 Segment { get; }
        public new UInt32 Offset { get; }
        public String Value { get; }
        public UInt32 pParent { get; }
        public UInt32 pEnd { get; }
        public UInt32 len { get; }
        protected unsafe S_BLOCKSYM32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (BLOCKSYM32*)Content;
            len = Header->len;
            pParent = Header->pParent;
            pEnd = Header->pEnd;
            Segment = Header->seg;
            this.Offset = Header->off;
            Value = ToString(Section.Section.Encoding, (Byte*)(Header + 1), Section.Section.IsLengthPrefixedString);
            }
        }
    }