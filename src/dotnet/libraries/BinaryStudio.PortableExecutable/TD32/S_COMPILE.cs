using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.TD32
    {
    [UsedImplicitly]
    [TD32Symbol(TD32SymbolIndex.S_COMPILE)]
    internal class S_COMPILE : TD32Symbol
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [DebuggerDisplay(@"\{{Type}\}")]
        private struct HEADER
            {
            private readonly Int16 Length;
            private readonly TD32SymbolIndex Type;
            public readonly UInt32 Flags;
            }

        public UInt32 Flags { get; }
        public CV_CFL_LANG Language { get; }
        public CV_CPU_TYPE CPU { get; }
        public String CompilerVersion { get; }
        public override TD32SymbolIndex Type { get { return TD32SymbolIndex.S_COMPILE; }}
        protected override Boolean IsLengthPrefixedString { get { return true; }}

        public unsafe S_COMPILE(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (HEADER*)Content;
            Flags = Header->Flags;
            Language = (CV_CFL_LANG)((Flags & 0xff00) >> 8);
            CPU = (CV_CPU_TYPE)(Flags & 0xff);
            CompilerVersion = ToString(Encoding, (Byte*)(Header + 1), IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2}", LinePrefix,Offset,Type);
            Writer.WriteLine("{0}  CPU:{1} Language:{2}", LinePrefix,CPU,Language);
            Writer.WriteLine("{0}  Compiler:{1}", LinePrefix,CompilerVersion);
            Writer.WriteLine("{0}  Flags:{1:x8}", LinePrefix,this.Flags);
            }
        }
    }