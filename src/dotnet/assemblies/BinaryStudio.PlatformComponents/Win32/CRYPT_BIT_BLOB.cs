using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CRYPT_BIT_BLOB
        {
        public readonly Int32  cbData;
        public readonly IntPtr pbData;
        public readonly Int32  cUnusedBits;
        }
    }