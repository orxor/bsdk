using System;
using BinaryStudio.PortableExecutable.CodeView;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.TD32
    {
    [TD32Symbol(TD32SymbolIndex.S_GDATA32)]
    [UsedImplicitly]
    internal class S_GDATA32 : S_DATASYM32
        {
        public override TD32SymbolIndex Type { get { return TD32SymbolIndex.S_GDATA32; }}
        public S_GDATA32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            }
        }
    }