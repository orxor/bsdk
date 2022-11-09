using System;
using BinaryStudio.PortableExecutable.CodeView;

namespace BinaryStudio.PortableExecutable.TD32
    {
    public class TD32Symbol : CodeViewSymbol<TD32SymbolAttribute,TD32SymbolIndex>
        {
        private class DefaultFactory : CodeViewSymbolFactory<TD32SymbolAttribute,TD32SymbolIndex> {
            protected override unsafe ICodeViewSymbol CreateDefault(CodeViewSymbolsSSection Section, Int32 Offset, TD32SymbolIndex Index,Byte* Content, Int32 Length) {
                return new TD32Symbol(Section,Offset,Index,Content,Length);
                }
            }

        protected TD32Symbol(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            }

        private unsafe TD32Symbol(CodeViewSymbolsSSection Section, Int32 Offset, TD32SymbolIndex Type,Byte* Content, Int32 Length)
            : base(Section, Offset, Type, Content, Length)
            {
            }

        public static unsafe TD32Symbol From(CodeViewSymbolsSSection Section, Int32 Offset, TD32SymbolIndex Index, Byte* Content, Int32 Length) {
            return (TD32Symbol)(new DefaultFactory()).Create(Section,Offset,Index,Content,Length);
            }
        }
    }