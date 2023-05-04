using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_FILE_HEADER
        {
        public readonly IMAGE_FILE_MACHINE Machine;
        public readonly UInt16 NumberOfSections;
        public readonly UInt32 TimeDateStamp;
        public readonly UInt32 PointerToSymbolTable;
        public readonly UInt32 NumberOfSymbols;
        public readonly UInt16 SizeOfOptionalHeader;
        public readonly IMAGE_FILE_CHARACTERISTIC Characteristics;
        }
    }