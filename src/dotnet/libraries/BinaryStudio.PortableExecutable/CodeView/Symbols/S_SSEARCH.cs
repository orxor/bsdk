using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    /// <summary>{<see cref="DEBUG_SYMBOL_INDEX.S_SSEARCH"/>} Start Search.</summary>
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_SSEARCH)]
    internal class S_SSEARCH  : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_SSEARCH; }}
        /// <summary>
        /// $$SYMBOL offset of the procedure or thunk record for this module that has the lowest offset for the specified segment.
        /// This property is related to <see cref="CODEVIEW_SEARCHSYM.Offset"/>.
        /// </summary>
        public Int32 ProcedureOrThunkRecordOffset { get; }
        /// <summary>
        /// Segment (PE section) to which this <see cref="S_SSEARCH"/> refers.
        /// This property is related to <see cref="CODEVIEW_SEARCHSYM.Segment"/>.
        /// </summary>
        public Int16 SegmentIndex { get; }

        public unsafe S_SSEARCH(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (CODEVIEW_SEARCHSYM*)Content;
            if (Header->Type != DEBUG_SYMBOL_INDEX.S_SSEARCH) { throw new ArgumentOutOfRangeException(nameof(Content)); }
            ProcedureOrThunkRecordOffset = Header->Offset;
            SegmentIndex = Header->Segment;
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(Offset),Offset.ToString("x8"));
                writer.WriteValue(nameof(Type),Type);
                writer.WriteValue(nameof(ProcedureOrThunkRecordOffset),ProcedureOrThunkRecordOffset.ToString("x8"));
                writer.WriteValue(nameof(SegmentIndex),SegmentIndex.ToString("x4"));
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} {3:x4}:{4:x8}",
                LinePrefix,Offset,Type,SegmentIndex,ProcedureOrThunkRecordOffset);
            }
        }
    }