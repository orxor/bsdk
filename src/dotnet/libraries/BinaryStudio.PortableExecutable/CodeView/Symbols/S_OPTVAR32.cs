using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_OPTVAR32)]
    internal class S_OPTVAR32 : CodeViewSymbol, ICodeViewBlockElement
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [DebuggerDisplay(@"\{{Type}\}")]
        private struct HEADER
            {
            public readonly Int16 Length;                   // Record length.
            public readonly DEBUG_SYMBOL_INDEX Type;        // S_REGISTER
            public readonly Int16 Segment;
            public readonly Int32 Offset;
            public readonly Int32 Size;
            public readonly UInt16 Register;
            }

        public ICodeViewBlockStart BlockStart { get;set; }
        public ICodeViewProcedureStart ProcedureStart { get { return BlockStart as ICodeViewProcedureStart; }}
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_OPTVAR32; }}
        public Int16 Segment { get; }
        public new Int32 Offset { get; }
        public Int32 Size { get; }
        public UInt16 Register { get; }
        public unsafe S_OPTVAR32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (HEADER*)Content;
            Segment = Header->Segment;
            this.Offset = Header->Offset;
            Size = Header->Size;
            Register = Header->Register;
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(Offset),base.Offset.ToString("x8"));
                writer.WriteValue(nameof(Type),Type);
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} {3:x4}:{4:x8}-{5:x8} Register:{6}", LinePrefix,base.Offset,Type,
                ProcedureStart.SegmentIndex,
                ProcedureStart.ProcedureOffset + Offset,
                ProcedureStart.ProcedureOffset + Offset + Size - 1,
                DecodeRegister(Register));
            }
        }
    }