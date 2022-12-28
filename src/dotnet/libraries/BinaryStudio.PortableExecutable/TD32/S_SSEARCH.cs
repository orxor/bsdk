using System;
using System.IO;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;

namespace BinaryStudio.PortableExecutable.TD32
    {
    /// <summary>{<see cref="TD32SymbolIndex.S_SSEARCH"/>} TD32 specific <b>Start Search</b>.</summary>
    [TD32Symbol(TD32SymbolIndex.S_SSEARCH)]
    internal class S_SSEARCH : TD32Symbol
        {
        /// <summary>
        /// This property is related to <see cref="TD32_SEARCHSYM.CodeSyms"/>.
        /// </summary>
        public Int16 CodeSyms { get; }
        /// <summary>
        /// This property is related to <see cref="TD32_SEARCHSYM.DataSyms"/>.
        /// </summary>
        public Int16 DataSyms { get; }
        /// <summary>
        /// This property is related to <see cref="TD32_SEARCHSYM.FirstData"/>.
        /// </summary>
        public Int32 FirstData { get; }
        /// <summary>
        /// $$SYMBOL offset of the procedure or thunk record for this module that has the lowest offset for the specified segment.
        /// This property is related to <see cref="TD32_SEARCHSYM.ProcedureOrThunkRecordOffset"/>.
        /// </summary>
        public Int32 ProcedureOrThunkRecordOffset { get; }
        /// <summary>
        /// Segment (PE section) to which this <see cref="S_SSEARCH"/> refers.
        /// This property is related to <see cref="TD32_SEARCHSYM.SegmentIndex"/>.
        /// </summary>
        public Int16 SegmentIndex { get; }
        public override TD32SymbolIndex Type { get { return TD32SymbolIndex.S_SSEARCH; }}
        public unsafe S_SSEARCH(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            :base(Section, Offset, Content, Length)
            {
            var Header = (TD32_SEARCHSYM*)Content;
            if (Header->Type != TD32SymbolIndex.S_SSEARCH) { throw new ArgumentOutOfRangeException(nameof(Content)); }
            CodeSyms = Header->CodeSyms;
            DataSyms = Header->DataSyms;
            FirstData = Header->FirstData;
            ProcedureOrThunkRecordOffset = Header->ProcedureOrThunkRecordOffset;
            SegmentIndex = Header->SegmentIndex;
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
                writer.WriteValue(nameof(CodeSyms),CodeSyms.ToString("x4"));
                writer.WriteValue(nameof(DataSyms),DataSyms.ToString("x4"));
                writer.WriteValue(nameof(FirstData),FirstData.ToString("x8"));
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} {3:x4}:{4:x8} CodeSyms:{5:x4} DataSyms:{6:x4} FirstData:{7:x8}",
                LinePrefix,Offset,Type,SegmentIndex,ProcedureOrThunkRecordOffset,
                CodeSyms,DataSyms,FirstData);
            }
        }
    }