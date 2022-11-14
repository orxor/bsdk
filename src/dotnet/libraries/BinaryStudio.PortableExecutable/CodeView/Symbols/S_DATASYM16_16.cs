using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    internal abstract class S_DATASYM16_16 : CodeViewSymbol
        {
        public Int32 TypeIndex { get; }
        public Int16 SegmentIndex { get; }
        public Int32 SymbolOffset { get; }
        public String Name { get; }

        protected unsafe S_DATASYM16_16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (DATASYM16*)Content;
            TypeIndex = r->TypeIndex;
            SegmentIndex = r->Segment;
            SymbolOffset = r->Offset;
            Name = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} TypeIndex:{3:x4} Segment:{4:x4}:{5:x4} {6}",
                LinePrefix,Offset,Type,
                TypeIndex,SegmentIndex,SymbolOffset,Name);
            }
        }

    internal abstract class S_DATASYM16_32 : CodeViewSymbol
        {
        public Int16 TypeIndex { get; }
        public Int16 SegmentIndex { get; }
        public Int32 SymbolOffset { get; }
        public String Name { get; }

        protected unsafe S_DATASYM16_32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (DATASYM32_16*)Content;
            TypeIndex = r->TypeIndex;
            SegmentIndex = r->Segment;
            SymbolOffset = r->Offset;
            Name = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} TypeIndex:{3:x4} Segment:{4:x4}:{5:x8} {6}",
                LinePrefix,Offset,Type,
                TypeIndex,SegmentIndex,SymbolOffset,Name);
            }
        }
    }