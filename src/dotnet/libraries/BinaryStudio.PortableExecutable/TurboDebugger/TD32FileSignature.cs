using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TD32FileSignature
        {
        public readonly Int32 Signature;
        public readonly Int32 Offset;
        }
    }