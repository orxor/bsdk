using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    using DWORD = UInt32;
    using CERT_NAME_BLOB = CRYPT_BLOB;
    using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CRL_INFO
        {
        public readonly DWORD Version;
        public readonly CRYPT_ALGORITHM_IDENTIFIER SignatureAlgorithm;
        public readonly CERT_NAME_BLOB Issuer;
        public readonly FILETIME ThisUpdate;
        public readonly FILETIME NextUpdate;
        DWORD cCRLEntry;
        unsafe CRL_ENTRY* rgCRLEntry;
        DWORD cExtension;
        unsafe CERT_EXTENSION* rgExtension;
        }
    }
