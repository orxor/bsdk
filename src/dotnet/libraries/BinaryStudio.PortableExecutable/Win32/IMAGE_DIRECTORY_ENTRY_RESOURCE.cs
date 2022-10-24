using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IMAGE_DIRECTORY_ENTRY_RESOURCE
        {
        [FieldOffset(0)] public readonly UInt32 NameOffset;
        [FieldOffset(0)] public readonly UInt32 IntegerId;
        [FieldOffset(4)] public readonly UInt32 DataEntryOffset;
        }
    }