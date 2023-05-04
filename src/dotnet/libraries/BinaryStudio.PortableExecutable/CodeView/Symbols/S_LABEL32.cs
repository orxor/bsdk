using BinaryStudio.PortableExecutable.Win32;
using System;
using JetBrains.Annotations;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_LABEL32)]
    [UsedImplicitly]
    internal class S_LABEL32 : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_LABEL32; }}
        public UInt16 Segment { get; }
        public new UInt32 Offset { get; }
        public String Name { get; }
        public CV_PFLAG Flags { get; }
        public unsafe S_LABEL32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (LABELSYM32*)Content;
            Segment = r->Segment;
            this.Offset = r->Offset;
            Flags = r->Flags;
            Name = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString);
            }
        }
    }