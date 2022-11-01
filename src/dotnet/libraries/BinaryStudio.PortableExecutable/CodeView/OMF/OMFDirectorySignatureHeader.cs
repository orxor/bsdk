using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Signature}\}")]
    public struct OMFDirectorySignatureHeader
        {
        public readonly OMFDirectorySignature Signature;
        public readonly Int32 Offset;
        }
    }