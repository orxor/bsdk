﻿using System;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_PUB16)]
    [UsedImplicitly]
    internal class S_PUB16_16 : S_DATASYM16_16
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_PUB16; }}
        public S_PUB16_16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            }
        }

    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_PUB16_32)]
    [UsedImplicitly]
    internal class S_PUB16_32 : S_DATASYM16_32
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_PUB16_32; }}
        public S_PUB16_32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            }
        }
    }