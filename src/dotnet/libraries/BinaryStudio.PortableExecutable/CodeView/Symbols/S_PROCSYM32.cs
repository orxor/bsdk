using BinaryStudio.PortableExecutable.Win32;
using System;
using System.IO;
using System.Text;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    internal abstract class S_PROCSYM32 : CodeViewSymbol,ICodeViewProcedureStart
        {
        public DEBUG_TYPE_ENUM ProcedureType { get; }
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

        Int32 ICodeViewBlockStart.CodeLength { get { return ProcedureLength; }}
        Int32 ICodeViewBlockStart.CodeOffset { get { return ProcedureOffset; }}
        protected unsafe S_PROCSYM32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (CODEVIEW_PROCSYM32*)Content;
            ProcedureType = Header->ProcedureType;
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

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(Offset),base.Offset.ToString("x8"));
                writer.WriteValue(nameof(Type),Type);
                writer.WriteValue(nameof(Parent),Parent.ToString("x8"));
                writer.WriteValue(nameof(End),End.ToString("x8"));
                writer.WriteValue(nameof(Next),Next.ToString("x8"));
                writer.WriteValue(nameof(ProcedureLength),ProcedureLength.ToString("x8"));
                writer.WriteValue(nameof(DbgStart),DbgStart.ToString("x8"));
                writer.WriteValue(nameof(DbgEnd),DbgEnd.ToString("x8"));
                writer.WriteValue(nameof(Flags),Flags);
                writer.WriteValue(nameof(SegmentIndex),SegmentIndex.ToString("x4"));
                writer.WriteValue(nameof(ProcedureType),ProcedureType);
                writer.WriteValue(nameof(ProcedureOffset),ProcedureOffset.ToString("x4"));
                writer.WriteValue(nameof(Name),Name);
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} {3:x4}:{4:x8}-{5:x8}", LinePrefix,Offset,Type,SegmentIndex,ProcedureOffset,ProcedureOffset+ProcedureLength-1);
            Writer.WriteLine("{0}  Debug:{1:x4}:{2:x8}-{3:x8} ProcedureType:{4}", LinePrefix,SegmentIndex,ProcedureOffset+DbgStart,ProcedureOffset+DbgEnd,ProcedureType);
            var builder = new StringBuilder();
            using (var writer = new DefaultJsonWriter(new JsonTextWriter(new StringWriter(builder)) {
                Formatting = Formatting.Indented,
                Indentation = 2,
                IndentChar = ' '
                }))
                {
                WriteTo(writer);
                }
            foreach (var i in builder.ToString().Split('\n')) {
                Writer.WriteLine("{0}{1}", LinePrefix,i);
                }
            }
        }
    }