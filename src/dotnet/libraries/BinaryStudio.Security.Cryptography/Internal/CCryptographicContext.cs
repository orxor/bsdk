﻿using BinaryStudio.PlatformComponents.Win32;
using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    internal class CCryptographicContext : CryptographicContext, ICryptoAPI
        {
        public override IntPtr Handle { get; }

        Boolean ICryptoAPI.CertFreeCertificateContext(IntPtr pCertContext) { return CertFreeCertificateContext(pCertContext); }
        Boolean ICryptoAPI.CertCloseStore(IntPtr handle, UInt32 flags) { return CertCloseStore(handle, flags); }
        IntPtr ICryptoAPI.CertDuplicateCertificateContext([In] IntPtr pCertContext){ return CertDuplicateCertificateContext(pCertContext); }
        IntPtr ICryptoAPI.CertOpenStore(IntPtr lpszStoreProvider, UInt32 dwMsgAndCertEncodingType, IntPtr hCryptProv, UInt32 dwFlags, [In] String pvPara) { return CertOpenStore(lpszStoreProvider, dwMsgAndCertEncodingType,hCryptProv,dwFlags,pvPara); }
        IntPtr ICryptoAPI.CertOpenStore(IntPtr lpszStoreProvider, UInt32 dwMsgAndCertEncodingType, IntPtr hCryptProv, UInt32 dwFlags, [In] IntPtr pvPara) { return CertOpenStore(lpszStoreProvider, dwMsgAndCertEncodingType,hCryptProv,dwFlags,pvPara); }
        IntPtr ICryptoAPI.CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext) { return CertEnumCertificatesInStore(CertStore,PrevCertContext); }

        [DllImport("capi20", SetLastError = true)] private static extern Boolean CertFreeCertificateContext(IntPtr pCertContext);
        [DllImport("capi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertDuplicateCertificateContext([In] IntPtr pCertContext);
        [DllImport("capi20", CharSet = CharSet.Auto, SetLastError = true)] private static extern bool CertControlStore([In] IntPtr hCertStore, [In] uint dwFlags, [In] uint dwCtrlType, [In] IntPtr pvCtrlPara);
        [DllImport("capi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumSystemStoreLocation(Int32 flags, IntPtr args, PFN_CERT_ENUM_SYSTEM_STORE_LOCATION pfn);
        [DllImport("capi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern IntPtr CertOpenStore(IntPtr lpszStoreProvider, UInt32 dwMsgAndCertEncodingType, IntPtr hCryptProv, UInt32 dwFlags, [In] String pvPara);
        [DllImport("capi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern IntPtr CertOpenStore(IntPtr lpszStoreProvider, UInt32 dwMsgAndCertEncodingType, IntPtr hCryptProv, UInt32 dwFlags, [In] IntPtr pvPara);
        [DllImport("capi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r);
        [DllImport("capi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertAddCRLContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r);
        [DllImport("capi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS flags, IntPtr pvSystemStoreLocationPara, IntPtr pvArg, PFN_CERT_ENUM_SYSTEM_STORE pfnEnum);
        [DllImport("capi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumPhysicalStore(IntPtr pvSystemStore, CERT_SYSTEM_STORE_FLAGS flags, IntPtr pvArg, PFN_CERT_ENUM_PHYSICAL_STORE pfnEnum);
        [DllImport("capi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCertificateContext(UInt32 dwCertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        [DllImport("capi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCRLContext(UInt32 dwCertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        [DllImport("capi20", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext);
        [DllImport("capi20", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext);
        [DllImport("capi20", SetLastError = true)] private static extern Boolean CertCloseStore(IntPtr handle, UInt32 flags);

        public override object GetService(Type service) {
            if (service == typeof(ICryptoAPI)) { return this; }
            return base.GetService(service);
            }
        }
    }
