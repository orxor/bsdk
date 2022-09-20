using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_ROM_OPTIONAL_HEADER
        {
        private readonly UInt16 Magic;
        private readonly Byte MajorLinkerVersion;
        private readonly Byte MinorLinkerVersion;
        private readonly UInt32 SizeOfCode;
        private readonly UInt32 SizeOfInitializedData;
        private readonly UInt32 SizeOfUninitializedData;
        private readonly UInt32 AddressOfEntryPoint;
        private readonly UInt32 BaseOfCode;
        private readonly UInt32 BaseOfData;
        private readonly UInt32 BaseOfBss;
        private readonly UInt32 GprMask;
        private unsafe fixed UInt32 CprMask[4];
        private readonly UInt32 GpValue;
        }
    }