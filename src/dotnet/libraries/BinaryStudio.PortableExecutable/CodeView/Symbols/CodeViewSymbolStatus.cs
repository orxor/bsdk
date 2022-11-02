using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [Flags]
    public enum CodeViewSymbolStatus
        {
        HasJsonWrite = 0x0001,
        HasFileDumpWrite = 0x0002
        }
    }