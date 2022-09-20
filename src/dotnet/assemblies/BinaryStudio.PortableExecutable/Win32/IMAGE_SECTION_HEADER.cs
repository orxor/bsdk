using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [DebuggerDisplay("{ToString()}")]
    public struct IMAGE_SECTION_HEADER
        {
        [DebuggerDisplay("{ToString()}")] public unsafe fixed Byte Name[8];
        public readonly UInt32 VirtualSize;
        public readonly UInt32 VirtualAddress;
        public readonly UInt32 SizeOfRawData;
        public readonly UInt32 PointerToRawData;
        public readonly UInt32 PointerToRelocations;
        public readonly UInt32 PointerToLineNumbers;
        public readonly UInt16 NumberOfRelocations;
        public readonly UInt16 NumberOfLineNumbers;
        public readonly IMAGE_SCN Characteristics;

        public override unsafe String ToString() {
            var r = new StringBuilder(8);
            fixed (Byte* bytes = Name) {
                for (var i = 0; i < 8; i++) {
                    if (bytes[i] == 0) { break; }
                    r.Append((Char)bytes[i]);
                    }
                }
            return r.ToString();
            }
        }
    }