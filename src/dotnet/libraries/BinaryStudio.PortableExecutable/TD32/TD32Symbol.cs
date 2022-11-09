using System;
using BinaryStudio.PortableExecutable.CodeView;

namespace BinaryStudio.PortableExecutable.TD32
    {
    public class TD32Symbol : CodeViewSymbol<TD32SymbolAttribute,TD32SymbolIndex>
        {
        protected TD32Symbol(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            }

        public new static unsafe TD32Symbol From(CodeViewSymbolsSSection Section, Int32 Offset, TD32SymbolIndex Index, Byte* Content, Int32 Length) {
            return (TD32Symbol)CodeViewSymbol<TD32SymbolAttribute,TD32SymbolIndex>.From(
                Section,Offset,Index,Content,Length);
            }
        }
    }