using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    internal class S_PROCSYM16 : CodeViewSymbol
        {
        public Int16 ProcedureTypeIndex { get; }
        public Int16 SegmentIndex { get; }
        public Int32 ProcedureOffset { get; }
        public Int32 ProcedureLength { get; }
        public CV_PFLAG Flags { get; }
        public Int32 Parent { get; }
        public Int32 End { get; }
        public Int32 Next { get; }
        public Int32 DbgStart { get; }
        public Int32 DbgEnd { get; }
        public virtual String Name { get; }

        protected unsafe S_PROCSYM16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (CODEVIEW_PROCSYM16*)Content;
            ProcedureTypeIndex = Header->ProcedureTypeIndex;
            SegmentIndex = Header->Segment;
            Flags     = Header->Flags;
            Parent    = Header->Parent;
            End       = Header->End;
            Next      = Header->Next;
            DbgStart  = Header->DbgStart;
            DbgEnd    = Header->DbgEnd;
            Name      = ToString(Encoding,(Byte*)(Header + 1),IsLengthPrefixedString);
            ProcedureOffset = Header->Offset;
            ProcedureLength = Header->ProcedureLength;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="DumpFlags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags DumpFlags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} {3:x4}:{4:x4}-{5:x4}", LinePrefix,Offset,Type,SegmentIndex,ProcedureOffset,ProcedureOffset+ProcedureLength-1);
            Writer.WriteLine("{0}  Debug:{1:x4}:{2:x4}-{3:x4} ProcedureTypeIndex:{4:x4}", LinePrefix,SegmentIndex,ProcedureOffset+DbgStart,ProcedureOffset+DbgEnd,ProcedureTypeIndex);
            Writer.WriteLine("{0}  Name:{1} Flags:{2}", LinePrefix,Name,Flags);
            Writer.WriteLine("{0}  Parent:{1:x8} End:{2:x8} Next:{3:x8}", LinePrefix,Parent,End,Next);
            }
        }
    }