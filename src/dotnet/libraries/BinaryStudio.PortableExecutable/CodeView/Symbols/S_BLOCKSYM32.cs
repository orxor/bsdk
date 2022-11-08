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

    internal abstract class S_BLOCKSYM32_TD32 : CodeViewSymbol,ICodeViewBlockElement
        {
        public Int16 SegmentIndex { get; }
        public Int32 CodeOffset { get; }
        public Int32 NameIndex { get; }
        public Int32 TypeIndex { get; }
        public Int32 Parent { get; }
        public Int32 CodeLength { get; }
        public Int32 VariableOffset { get; }
        public Int16 Flags { get; }

        public ICodeViewBlockStart BlockStart { get;set; }
        public ICodeViewProcedureStart ProcedureStart { get { return BlockStart as ICodeViewProcedureStart; }}
        protected unsafe S_BLOCKSYM32_TD32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (BLOCKSYM32_TD32*)Content;
            CodeOffset = Header->CodeOffset;
            Parent = Header->Parent;
            CodeLength = Header->CodeLength;
            SegmentIndex = Header->SegmentIndex;
            TypeIndex = Header->TypeIndex;
            NameIndex = Header->NameIndex;
            VariableOffset = Header->VariableOffset;
            Flags = Header->Flags;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} Segment:{3:x4}:{4:x8}-{5:x8}", LinePrefix,Offset,Type,SegmentIndex,
                ProcedureStart.ProcedureOffset + CodeOffset,
                ProcedureStart.ProcedureOffset + CodeOffset + CodeLength - 1);
            Writer.WriteLine("{0}  TypeIndex:{1:x4} Flags:{2:x4}", LinePrefix,TypeIndex,this.Flags);
            Writer.WriteLine("{0}  NameIndex:{{{1}}}:{{{2}}}", LinePrefix,NameIndex.ToString("x8"),NameTable[NameIndex-1]);
            Writer.WriteLine("{0}  Parent:{1:x8} VariableOffset:{2:x8}", LinePrefix,Parent,VariableOffset);
            }

        }
    }