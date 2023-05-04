using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IMAGE_RUNTIME_FUNCTION_ENTRY
        {
        [FieldOffset(0)] public  readonly UInt32 BeginAddress;
        [FieldOffset(4)] public  readonly UInt32 EndAddress;
        [FieldOffset(8)] public  readonly UInt32 UnwindInfoAddress;
        [FieldOffset(8)] private readonly UInt32 UnwindData;
        }
    }