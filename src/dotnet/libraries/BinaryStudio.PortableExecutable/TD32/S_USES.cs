using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.CodeView;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.TD32
    {
    [UsedImplicitly]
    [TD32Symbol(TD32SymbolIndex.S_USES)]
    internal class S_USES : TD32Symbol
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [DebuggerDisplay(@"\{{Type}\}")]
        private struct HEADER
            {
            private readonly Int16 Length;
            private readonly TD32SymbolIndex Type;
            public  readonly Int32 NameIndex;
            }

        public override TD32SymbolIndex Type { get { return TD32SymbolIndex.S_USES; }}
        public Int32 NameIndex { get; }

        public unsafe S_USES(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (HEADER*)Content;
            NameIndex = Header->NameIndex;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} Unit:{{{3}}}:{{{4}}}", LinePrefix,Offset,Type,
                NameIndex.ToString("x8"),NameTable[NameIndex-1]);
            }
        }
    }