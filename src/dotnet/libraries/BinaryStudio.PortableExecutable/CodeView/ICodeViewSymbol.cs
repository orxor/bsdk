using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public interface ICodeViewSymbol
        {
        UInt16 Type { get; }
        }
    }