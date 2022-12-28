using System;
using System.Runtime.InteropServices;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    internal class SCryptographicContext : CryptographicContext, ICryptoAPI
        {
        public override IntPtr Handle { get; }

        Boolean ICryptoAPI.CertFreeCertificateContext(IntPtr pCertContext) { return CertFreeCertificateContext(pCertContext); }
        Boolean ICryptoAPI.CertCloseStore(IntPtr handle, UInt32 flags) { return CertCloseStore(handle, flags); }
        Boolean ICryptoAPI.CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r) { return CertAddCertificateContextToStore(store,context,disposition,r); }
        Boolean ICryptoAPI.CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, out IntPtr r) { return CertAddCertificateContextToStore(store,context,disposition,out r); }
        IntPtr ICryptoAPI.CertDuplicateCertificateContext([In] IntPtr pCertContext){ return CertDuplicateCertificateContext(pCertContext); }
        IntPtr ICryptoAPI.CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, [In] String Para) { return CertOpenStore(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para); }
        IntPtr ICryptoAPI.CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, [In] IntPtr Para) { return CertOpenStore(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para); }
        IntPtr ICryptoAPI.CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext) { return CertEnumCertificatesInStore(CertStore,PrevCertContext); }
        IntPtr ICryptoAPI.CertCreateCertificateContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size) { return CertCreateCertificateContext(CertEncodingType,blob,size); }

        [DllImport("crypt32.dll", SetLastError = true)] private static extern Boolean CertFreeCertificateContext(IntPtr pCertContext);
        [DllImport("crypt32.dll", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertDuplicateCertificateContext([In] IntPtr pCertContext);
        [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern bool CertControlStore([In] IntPtr CertStore, [In] uint Flags, [In] uint CtrlType, [In] IntPtr CtrlPara);
        [DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumSystemStoreLocation(Int32 flags, IntPtr args, PFN_CERT_ENUM_SYSTEM_STORE_LOCATION pfn);
        [DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)] private static extern IntPtr CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, [In] String Para);
        [DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)] private static extern IntPtr CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, [In] IntPtr Para);
        [DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r);
        [DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, out IntPtr r);
        [DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertAddCRLContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r);
        [DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS flags, IntPtr pvSystemStoreLocationPara, IntPtr pvArg, CertEnumSystemStoreCallbackIntPtr Callback);
        [DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumPhysicalStore(IntPtr pvSystemStore, CERT_SYSTEM_STORE_FLAGS flags, IntPtr pvArg, PFN_CERT_ENUM_PHYSICAL_STORE Callback);
        [DllImport("crypt32.dll", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCertificateContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        [DllImport("crypt32.dll", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCRLContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext);
        [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext);
        [DllImport("crypt32.dll", SetLastError = true)] private static extern Boolean CertCloseStore(IntPtr handle, UInt32 flags);

        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="service">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="service"/>.
        /// -or-
        /// <see langword="null"/> if there is no service object of type <paramref name="service"/>.</returns>
        public override object GetService(Type service) {
            if (service == typeof(ICryptoAPI)) { return this; }
            return base.GetService(service);
            }

        /// <summary>The CertEnumSystemStore function retrieves the system stores available. The function calls the provided callback function for each system store found.</summary>
        /// <param name="Flags">Specifies the location of the system store.</param>
        /// <param name="SystemStoreLocationPara">Specific argument specific for <paramref name="Flags"/>.</param>
        /// <param name="Args">A pointer to a void that allows the application to declare, define, and initialize a structure to hold any information to be passed to the callback enumeration function.</param>
        /// <param name="Callback">A pointer to the callback function used to show the details for each system store. This callback function determines the content and format for the presentation of information on each system store.</param>
        /// <returns>If the function succeeds, the function returns <see langword="true"/>. If the function fails, it returns <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS Flags, IntPtr SystemStoreLocationPara, IntPtr Args, CertEnumSystemStoreCallbackString Callback) {
            if (Callback == null) { throw new ArgumentNullException(nameof(Callback)); }
            return CertEnumSystemStore(Flags,SystemStoreLocationPara,Args,
                delegate(IntPtr SystemStore, CERT_SYSTEM_STORE_FLAGS StoreFlags, ref CERT_SYSTEM_STORE_INFO StoreInfo, IntPtr Reserved, IntPtr StoreArg) {
                    return Callback(Marshal.PtrToStringUni(SystemStore), StoreFlags,ref StoreInfo,Reserved,StoreArg);
                    });
            }
        }
    }
