using System;
using System.IO;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;

// ReSharper disable ParameterHidesMember

namespace BinaryStudio.PortableExecutable.TD32
    {
    internal abstract class S_PROCSYM32 : TD32Symbol,ICodeViewProcedureStart
        {
        public Int16 TypeIndex { get; }
        public Int16 SegmentIndex { get; }
        public Int32 ProcedureOffset { get; }
        public Int32 ProcedureLength { get; }
        public CV_PFLAG Flags { get; }
        public Int32 Parent { get; }
        public Int32 End { get; }
        public Int32 Next { get; }
        public Int32 DbgStart { get; }
        public Int32 DbgEnd { get; }
        public Int32 NameIndex { get; }

        Int32 ICodeViewBlockStart.CodeLength { get { return ProcedureLength; }}
        Int32 ICodeViewBlockStart.CodeOffset { get { return ProcedureOffset; }}
        protected unsafe S_PROCSYM32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (TD32_PROCSYM32*)Content;
            TypeIndex = Header->TypeIndex;
            SegmentIndex = Header->Segment;
            Flags     = Header->Flags;
            Parent    = Header->Parent;
            End       = Header->End;
            Next      = Header->Next;
            DbgStart  = Header->DbgStart;
            DbgEnd    = Header->DbgEnd;
            NameIndex = Header->NameIndex;
            ProcedureOffset = Header->Offset;
            ProcedureLength = Header->ProcedureLength;
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(Offset),Offset.ToString("x8"));
                writer.WriteValue(nameof(Type),Type);
                writer.WriteValue(nameof(Parent),Parent.ToString("x8"));
                writer.WriteValue(nameof(End),End.ToString("x8"));
                writer.WriteValue(nameof(Next),Next.ToString("x8"));
                writer.WriteValue(nameof(ProcedureLength),ProcedureLength.ToString("x8"));
                writer.WriteValue(nameof(DbgStart),DbgStart.ToString("x8"));
                writer.WriteValue(nameof(DbgEnd),DbgEnd.ToString("x8"));
                writer.WriteValue(nameof(Flags),Flags);
                writer.WriteValue(nameof(SegmentIndex),SegmentIndex.ToString("x4"));
                writer.WriteValue(nameof(TypeIndex),TypeIndex.ToString("x4"));
                writer.WriteValue(nameof(ProcedureOffset),ProcedureOffset.ToString("x4"));
                writer.WriteValue(nameof(NameIndex),NameIndex.ToString("x8"));
                writer.WriteValue("Name",NameTable[NameIndex-1]);
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} {3:x4}:{4:x8}-{5:x8}", LinePrefix,Offset,Type,SegmentIndex,ProcedureOffset,ProcedureOffset+ProcedureLength-1);
            Writer.WriteLine("{0}  Debug:{1:x4}:{2:x8}-{3:x8} TypeIndex:{4:x4}", LinePrefix,SegmentIndex,ProcedureOffset+DbgStart,ProcedureOffset+DbgEnd,TypeIndex);
            Writer.WriteLine("{0}  NameIndex:{{{1}}}:{{{2}}}", LinePrefix,NameIndex.ToString("x8"),NameTable[NameIndex-1]);
            Writer.WriteLine("{0}  Parent:{1:x8} End:{2:x8} Next:{3:x8}", LinePrefix,Parent,End,Next);
            }
        }
    }