using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.CodeView;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.TD32
    {
    [UsedImplicitly]
    [TD32Symbol(TD32SymbolIndex.S_NAMESPACE)]
    internal class S_NAMESPACE : TD32Symbol
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [DebuggerDisplay(@"\{{Type}\}")]
        private struct HEADER
            {
            private readonly Int16 Length;
            private readonly TD32SymbolIndex Type;
            public  readonly Int32 NameIndex;
            public  readonly Int32 BrowserOffset;
            public  readonly Int16 UsingCount;
            }

        public override TD32SymbolIndex Type { get { return TD32SymbolIndex.S_NAMESPACE; }}
        public Int32 NameIndex { get; }
        public Int32 BrowserOffset { get; }
        public Int32[] UsingUnits { get; }

        public unsafe S_NAMESPACE(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (HEADER*)Content;
            NameIndex = Header->NameIndex;
            BrowserOffset = Header->BrowserOffset;
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
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} Namespace:{{{3}}}:{{{4}}}", LinePrefix,Offset,Type,NameIndex.ToString("x8"),NameTable[NameIndex-1]);
            Writer.WriteLine("{0}  UsingCount:{1}",LinePrefix,UsingUnits.Length);
            for (var i = 0; i < UsingUnits.Length; i++) {
                Writer.WriteLine("{0}    Unit:{{{1}}}:{{{2}}}",LinePrefix,UsingUnits[i].ToString("x8"),NameTable[UsingUnits[i]-1]);
                }
            Writer.WriteLine("{0}  BrowserOffset:{1:x4}", LinePrefix,BrowserOffset);
            }
        }
    }