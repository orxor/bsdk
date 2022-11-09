using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.CodeView;

namespace BinaryStudio.PortableExecutable.TD32
    {
    internal abstract class S_UDTSYM : TD32Symbol
        {
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        [DebuggerDisplay(@"\{{Type}\}")]
        internal struct UDTSYM
            {
            [FieldOffset( 0)] private readonly Int16 Length;
            [FieldOffset( 2)] private readonly TD32SymbolIndex Type;
            [FieldOffset( 4)] public readonly Int32 TypeIndex;
            [FieldOffset( 8)] public readonly Int16 Flags;
            [FieldOffset(10)] public readonly Int32 NameIndex;
            [FieldOffset(14)] public readonly Int16 BrowserOffset;
            }

        public Int32 TypeIndex { get; }
        public Int32 NameIndex { get; }
        public Int16 Flags { get; }
        public Int16 BrowserOffset { get; }

        protected unsafe S_UDTSYM(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (UDTSYM*)Content;
            TypeIndex = Header->TypeIndex;
            NameIndex = Header->NameIndex;
            Flags = Header->Flags;
            BrowserOffset = Header->BrowserOffset;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2}", LinePrefix,Offset,Type);
            Writer.WriteLine("{0}  TypeIndex:{1:x4} Flags:{2:x4}", LinePrefix,TypeIndex,this.Flags);
            Writer.WriteLine("{0}  NameIndex:{{{1}}}:{{{2}}}", LinePrefix,NameIndex.ToString("x8"),NameTable[NameIndex-1]);
            Writer.WriteLine("{0}  BrowserOffset:{1:x4}", LinePrefix,BrowserOffset);
            }
        }
    }