using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public interface ICodeViewNameTable
        {
        String this[Int32 Index] { get; }
        }
    }