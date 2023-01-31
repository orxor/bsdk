using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_OID_INFO
        {
        public readonly Int32 Size;
        public readonly IntPtr OID;
        public readonly IntPtr Name;
        public readonly Int32 GroupId;
        public readonly Int32 Value;
        public readonly CRYPT_BLOB ExtraInfo;
        #if CRYPT_OID_INFO_HAS_EXTRA_FIELDS
        public readonly IntPtr CNGAlgid;
        public readonly IntPtr CNGExtraAlgid;
        #endif
        }
    }