using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public interface ICodeViewBlockStart
        {
        Int32 CodeOffset { get; }
        Int32 CodeLength { get; }
        }
    }