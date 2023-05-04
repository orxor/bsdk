using System;
using BinaryStudio.PortableExecutable.CodeView;

namespace BinaryStudio.PortableExecutable.TD32
    {
    public class TD32SymbolAttribute : Attribute, ICodeViewSymbolAttribute
        {
        UInt16 ICodeViewSymbolAttribute.Key { get { return (UInt16)Key; }}
        public TD32SymbolIndex Key { get; }
        public TD32SymbolAttribute(TD32SymbolIndex key)
            {
            Key = key;
            }
        }
    }