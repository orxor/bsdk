using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    using CRYPT_OBJID_BLOB = CRYPT_BLOB;

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CERT_EXTENSION
        {
        public IntPtr pszObjId;
        [MarshalAs(UnmanagedType.Bool)] public Boolean fCritical;
        public CRYPT_OBJID_BLOB Value;

        public override String ToString() {
            return (pszObjId != IntPtr.Zero)
                ? Marshal.PtrToStringAnsi(pszObjId)
                : "CERT_EXTENSION";
            }
        }
    }