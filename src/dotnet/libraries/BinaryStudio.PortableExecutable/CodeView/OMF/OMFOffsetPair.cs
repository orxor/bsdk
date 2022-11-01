using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OMFOffsetPair
        {
        public readonly Int32 StartOffset;
        public readonly Int32 EndOffset;
        }
    }