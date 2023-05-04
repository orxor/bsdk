using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.CodeView;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.TD32
    {
    [UsedImplicitly]
    [TD32Symbol(TD32SymbolIndex.S_USING)]
    internal class S_USING : TD32Symbol
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [DebuggerDisplay(@"\{{Type}\}")]
        private struct HEADER
            {
            private readonly Int16 Length;
            private readonly TD32SymbolIndex Type;
            public  readonly Int16 UsingCount;
            }

        public Int32[] UsingUnits { get; }
        public override TD32SymbolIndex Type { get { return TD32SymbolIndex.S_USING; }}
        public unsafe S_USING(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (HEADER*)Content;
            UsingUnits = new Int32[Header->UsingCount];
            for (var i = 0; i < Header->UsingCount; i++) {
                UsingUnits[i] = ((Int32*)(Header + 1))[i];
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2}", LinePrefix,Offset,Type);
            Writer.WriteLine("{0}  UsingCount:{1}",LinePrefix,UsingUnits.Length);
            for (var i = 0; i < UsingUnits.Length; i++) {
                Writer.WriteLine("{0}    Unit:{{{1}}}:{{{2}}}",LinePrefix,UsingUnits[i].ToString("x8"),NameTable[UsingUnits[i]-1]);
                }
            }
        }
    }