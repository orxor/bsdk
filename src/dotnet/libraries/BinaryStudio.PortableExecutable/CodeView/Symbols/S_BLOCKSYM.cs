using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    internal abstract class S_BLOCKSYM16 : CodeViewSymbol,ICodeViewBlockElement
        {
        public UInt16 SegmentIndex { get; }
        public Int32 CodeOffset { get; }
        public String Name { get; }
        public UInt32 Parent { get; }
        public UInt32 End { get; }
        public Int32 CodeLength { get; }

        public ICodeViewBlockStart BlockStart { get;set; }
        protected unsafe S_BLOCKSYM16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (BLOCKSYM16*)Content;
            CodeLength = Header->len;
            Parent = Header->pParent;
            End = Header->pEnd;
            SegmentIndex = Header->seg;
            CodeOffset = Header->off;
            Name = ToString(Encoding, (Byte*)(Header + 1), IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} Segment:{3:x4}:{4:x8}-{5:x8}", LinePrefix,Offset,Type,SegmentIndex,
                CodeOffset,
                CodeOffset + CodeLength - 1);
            if (!String.IsNullOrWhiteSpace(Name)) {
                Writer.WriteLine("{0}  Name:{{{1}}}", LinePrefix,Name);
                }
            Writer.WriteLine("{0}  Parent:{1:x8} End:{2:x8}", LinePrefix,Parent,End);
            }
        }

    internal abstract class S_BLOCKSYM32 : CodeViewSymbol
        {
        public UInt16 Segment { get; }
        public new UInt32 Offset { get; }
        public String Value { get; }
        public UInt32 Parent { get; }
        public UInt32 End { get; }
        public UInt32 len { get; }
        protected unsafe S_BLOCKSYM32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (BLOCKSYM32*)Content;
            len = Header->len;
            Parent = Header->pParent;
            End = Header->pEnd;
            Segment = Header->seg;
            this.Offset = Header->off;
            Value = ToString(Section.Section.Encoding, (Byte*)(Header + 1), Section.Section.IsLengthPrefixedString);
            }
        }
    }