using System;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_BPREL16)]
    internal class S_BPREL16 : S_BPRELSYM16
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_BPREL16; }}
        public S_BPREL16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            }
        }
    }