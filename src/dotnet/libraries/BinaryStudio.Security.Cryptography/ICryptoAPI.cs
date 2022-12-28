using System;
using System.Runtime.InteropServices;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean PFN_CERT_ENUM_SYSTEM_STORE_LOCATION([MarshalAs(UnmanagedType.LPWStr)] String name,CERT_SYSTEM_STORE_FLAGS flags,IntPtr reserved,IntPtr arg);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean PFN_CERT_ENUM_SYSTEM_STORE(IntPtr pvSystemStore, CERT_SYSTEM_STORE_FLAGS flags, ref CERT_SYSTEM_STORE_INFO pStoreInfo, IntPtr pvReserved, IntPtr pvArg);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean PFN_CERT_ENUM_PHYSICAL_STORE(IntPtr pvSystemStore, CERT_SYSTEM_STORE_FLAGS flags, [MarshalAs(UnmanagedType.LPWStr)] String name, ref CERT_PHYSICAL_STORE_INFO pStoreInfo, IntPtr pvReserved, IntPtr pvArg);

    internal interface ICryptoAPI
        {
        Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r);
        Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, out IntPtr r);
        Boolean CertFreeCertificateContext(IntPtr pCertContext);
        Boolean CertCloseStore(IntPtr handle, UInt32 flags);
        Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS flags, IntPtr pvSystemStoreLocationPara, IntPtr pvArg, PFN_CERT_ENUM_SYSTEM_STORE pfnEnum);
        IntPtr CertDuplicateCertificateContext([In] IntPtr pCertContext);
        IntPtr CertOpenStore(IntPtr lpszStoreProvider, UInt32 dwMsgAndCertEncodingType, IntPtr hCryptProv, UInt32 dwFlags, [In] String pvPara);
        IntPtr CertOpenStore(IntPtr lpszStoreProvider, UInt32 dwMsgAndCertEncodingType, IntPtr hCryptProv, UInt32 dwFlags, [In] IntPtr pvPara);
        IntPtr CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext);
        IntPtr CertCreateCertificateContext(UInt32 dwCertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        }
    }
