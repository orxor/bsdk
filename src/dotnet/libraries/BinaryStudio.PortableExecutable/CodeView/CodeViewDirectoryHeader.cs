using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct CodeViewDirectoryHeader
        {
        public readonly CodeViewDirectorySignature Signature;
        public readonly Int32 Offset;
        }
    }