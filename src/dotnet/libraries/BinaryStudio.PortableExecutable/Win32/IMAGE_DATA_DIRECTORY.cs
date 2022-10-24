using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{VirtualAddress}:{Size}")]
    public struct IMAGE_DATA_DIRECTORY
        {
        public readonly UInt32 VirtualAddress;
        public readonly UInt32 Size;
        }
    }