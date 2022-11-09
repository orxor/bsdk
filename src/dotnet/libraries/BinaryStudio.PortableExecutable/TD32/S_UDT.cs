using System;
using BinaryStudio.PortableExecutable.CodeView;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.TD32
    {
    [TD32Symbol(TD32SymbolIndex.S_UDT)]
    [UsedImplicitly]
    internal class S_UDT : S_UDTSYM
        {
        public override TD32SymbolIndex Type { get { return TD32SymbolIndex.S_UDT; }}
        public S_UDT(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            }
        }
    }