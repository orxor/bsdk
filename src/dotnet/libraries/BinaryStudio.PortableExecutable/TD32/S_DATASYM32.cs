using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.CodeView;

namespace BinaryStudio.PortableExecutable.TD32
    {
    internal abstract class S_DATASYM32 : TD32Symbol
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [DebuggerDisplay(@"\{{Type}\}")]
        private struct DATASYM32
            {
            private readonly Int16 Length;
            private readonly TD32SymbolIndex Type;
            public readonly Int32 SymbolOffset;
            public readonly Int32 SegmentIndex;
            public readonly Int32 TypeIndex;
            public readonly Int32 NameIndex;
            public readonly Int16 BrowserOffset;
            }

        public Int32 SymbolOffset { get; }
        public Int32 SegmentIndex { get; }
        public Int32 TypeIndex { get; }
        public Int32 NameIndex { get; }
        public Int16 BrowserOffset { get; }

        protected unsafe S_DATASYM32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (DATASYM32*)Content;
            SymbolOffset = Header->SymbolOffset;
            SegmentIndex = Header->SegmentIndex;
            TypeIndex = Header->TypeIndex;
            NameIndex = Header->NameIndex;
            BrowserOffset = Header->BrowserOffset;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} {3:x4}:{4:x8}", LinePrefix,Offset,Type,SegmentIndex,SymbolOffset);
            Writer.WriteLine("{0}  TypeIndex:{1:x4}", LinePrefix,TypeIndex);
            Writer.WriteLine("{0}  NameIndex:{{{1}}}:{{{2}}}", LinePrefix,NameIndex.ToString("x8"),NameTable[NameIndex-1]);
            Writer.WriteLine("{0}  BrowserOffset:{1:x4}", LinePrefix,BrowserOffset);
            }
        }
    }