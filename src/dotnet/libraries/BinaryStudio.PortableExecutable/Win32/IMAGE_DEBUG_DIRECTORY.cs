using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_DEBUG_DIRECTORY
        {
        public readonly UInt32 Characteristics;
        public readonly UInt32 TimeDateStamp;
        public readonly UInt16 MajorVersion;
        public readonly UInt16 MinorVersion;
        public readonly IMAGE_DEBUG_TYPE Type;
        public readonly Int32 SizeOfData;
        public readonly Int32 AddressOfRawData;
        public readonly Int32 PointerToRawData;
        }
    }