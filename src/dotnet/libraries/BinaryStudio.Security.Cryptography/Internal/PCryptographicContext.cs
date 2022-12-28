﻿using BinaryStudio.PlatformComponents.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    internal class PCryptographicContext : CryptographicContext, ICryptoAPI
        {
        public override IntPtr Handle { get; }

        Boolean ICryptoAPI.CertFreeCertificateContext(IntPtr CertContext) { return CertFreeCertificateContext(CertContext); }
        Boolean ICryptoAPI.CertCloseStore(IntPtr handle, UInt32 flags) { return CertCloseStore(handle, flags); }
        Boolean ICryptoAPI.CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r) { return CertAddCertificateContextToStore(store,context,disposition,r); }
        Boolean ICryptoAPI.CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, out IntPtr r) { return CertAddCertificateContextToStore(store,context,disposition,out r); }

        IntPtr ICryptoAPI.CertDuplicateCertificateContext([In] IntPtr CertContext){ return CertDuplicateCertificateContext(CertContext); }
        IntPtr ICryptoAPI.CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, [In] IntPtr Para) { return CertOpenStore(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para); }
        IntPtr ICryptoAPI.CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext) { return CertEnumCertificatesInStore(CertStore,PrevCertContext); }
        IntPtr ICryptoAPI.CertCreateCertificateContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size) { return CertCreateCertificateContext(CertEncodingType,blob,size); }

        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertFreeCertificateContext(IntPtr pCertContext);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertDuplicateCertificateContext([In] IntPtr CertContext);
        [DllImport("libcrypt32", CharSet = CharSet.Auto, SetLastError = true)] private static extern bool CertControlStore([In] IntPtr CertStore, [In] uint Flags, [In] uint CtrlType, [In] IntPtr CtrlPara);
        [DllImport("libcrypt32", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumSystemStoreLocation(Int32 flags, IntPtr args, PFN_CERT_ENUM_SYSTEM_STORE_LOCATION pfn);
        [DllImport("libcrypt32", CharSet = CharSet.Unicode, SetLastError = true)] private static extern IntPtr CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, IntPtr Para);
        [DllImport("libcrypt32", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r);
        [DllImport("libcrypt32", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, out IntPtr r);
        [DllImport("libcrypt32", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertAddCRLContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r);
        [DllImport("libcrypt32", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS Flags, IntPtr SystemStoreLocationPara, IntPtr Arg, CertEnumSystemStoreCallbackIntPtr Callback);
        [DllImport("libcrypt32", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumPhysicalStore(IntPtr SystemStore, CERT_SYSTEM_STORE_FLAGS Flags, IntPtr Arg, PFN_CERT_ENUM_PHYSICAL_STORE Callback);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCertificateContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCRLContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        [DllImport("libcrypt32", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext);
        [DllImport("libcrypt32", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertCloseStore(IntPtr handle, UInt32 flags);
        [DllImport("libkernel32")] private static extern Int32 GetLastError();

        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="service">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="service"/>.
        /// -or-
        /// <see langword="null"/> if there is no service object of type <paramref name="service"/>.</returns>
        public override object GetService(Type service) {
            if (service == typeof(ICryptoAPI)) { return this; }
            return base.GetService(service);
            }

        #region M:GetLastWin32Error:Int32
        /// <summary>
        /// Returns the error code returned by the last unmanaged function that was called.
        /// using platform invoke that has the System.Runtime.InteropServices.DllImportAttribute.SetLastError flag set.
        /// </summary>
        /// <returns>The last error code set by a call to the Win32 SetLastError function.</returns>
        protected override Int32 GetLastWin32Error()
            {
            return GetLastError();
            }
        #endregion

        [DllImport("c", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl, EntryPoint ="wcslen")]  private static extern Int64 wcslen(IntPtr str);
        [DllImport("c", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl, EntryPoint ="wcsncpy")] private static extern Int64 wcsncpy([MarshalAs(UnmanagedType.LPArray)] Byte[] dest,IntPtr src, Int64 count);

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
                    var size = wcslen(SystemStore);
                    var r = new Byte[size*sizeof(Int32)];
                    wcsncpy(r,SystemStore,size);
                    return Callback(Encoding.UTF32.GetString(r), StoreFlags,ref StoreInfo,Reserved,StoreArg);
                    });
            }

        /// <summary>The CertOpenStore function opens a certificate store by using a specified store provider type.</summary>
        /// <param name="StoreProvider">A pointer to a null-terminated UTF32 string that contains the store provider type.</param>
        /// <param name="MsgAndCertEncodingType">Specifies the certificate encoding type and message encoding type.</param>
        /// <param name="CryptProv">This parameter is not used and should be set to NULL.</param>
        /// <param name="Flags">These values consist of high-word and low-word values combined by using a bitwise-OR operation.</param>
        /// <param name="Para">Additional information for this function.</param>
        /// <returns>If the function succeeds, the function returns a handle to the certificate store.</returns>
        public unsafe IntPtr CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, String Para) {
            if (StoreProvider == CERT_STORE_PROV_SYSTEM_W) {
                StoreProvider = CERT_STORE_PROV_SYSTEM_A;
                return CertOpenStore(
                    StoreProvider, MsgAndCertEncodingType,
                    CryptProv,Flags,
                    (IntPtr)LocalMemoryManager.StringToMem(Para,Encoding.ASCII));
                }
            return CertOpenStore(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para);
            }
        }
    }
