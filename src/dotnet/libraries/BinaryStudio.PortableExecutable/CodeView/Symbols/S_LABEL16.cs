using BinaryStudio.PortableExecutable.Win32;
using System;
using System.IO;
using JetBrains.Annotations;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_LABEL16)]
    [UsedImplicitly]
    internal class S_LABEL16 : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_LABEL16; }}
        public UInt16 SegmentIndex { get; }
        public UInt16 LabelOffset { get; }
        public String Name { get; }
        public CV_PFLAG Flags { get; }
        public unsafe S_LABEL16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (LABELSYM16*)Content;
            SegmentIndex = r->Segment;
            LabelOffset = r->Offset;
            Flags = r->Flags;
            Name = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="DumpFlags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags DumpFlags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} Segment:{3:x4}:{4:x4} {6} {5}", LinePrefix,Offset,Type,SegmentIndex,LabelOffset,Name,Flags);
            }
        }
    }