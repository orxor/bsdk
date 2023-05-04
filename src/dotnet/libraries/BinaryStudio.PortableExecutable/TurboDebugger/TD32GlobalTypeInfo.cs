using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TD32GlobalTypeInfo
        {
        public readonly Int32 Flags;
        public readonly Int32 TypeCount;
        // Int32 Offsets[TypeCount]
        }
    }