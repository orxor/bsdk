using System;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_BLOCK16)]
    [UsedImplicitly]
    internal class S_BLOCK16 : S_BLOCKSYM16,ICodeViewBlockStart
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_BLOCK16; }}
        public S_BLOCK16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            }
        }

    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_BLOCK32)]
    internal class S_BLOCK32 : S_BLOCKSYM32
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_BLOCK32; }}
        public S_BLOCK32(CodeViewSymbolsSSection section, Int32 offset, IntPtr content, Int32 length)
            : base(section, offset, content, length)
            {
            }
        }
    }