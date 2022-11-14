using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    internal abstract class S_REFSYM_ST : CodeViewSymbol
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
        private struct HEADER
            {
            private readonly Int16 Length;
            private readonly DEBUG_SYMBOL_INDEX Type;
            public readonly UInt32 Checksum;
            public readonly UInt32 ReferenceOffset;
            public readonly Int16 ModuleIndex;
            }

        public UInt32 Checksum { get; }
        public UInt32 ReferenceOffset { get; }
        public Int16 ModuleIndex { get; }

        protected unsafe S_REFSYM_ST(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (HEADER*)Content;
            Checksum = r->Checksum;
            ReferenceOffset = r->ReferenceOffset;
            ModuleIndex = r->ModuleIndex;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="DumpFlags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags DumpFlags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} ModuleIndex:{3:x4} ReferenceOffset:{4:x8} Checksum:{5:x8}",
                LinePrefix,Offset,Type,ModuleIndex,ReferenceOffset,Checksum);
            }
        }
    }