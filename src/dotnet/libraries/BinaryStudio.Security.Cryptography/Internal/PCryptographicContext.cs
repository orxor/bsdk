using System;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

namespace BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    internal class PCryptographicContext : CryptographicContext, ICryptoAPI
        {
        public override IntPtr Handle { get; }

        Boolean ICryptoAPI.CertFreeCertificateContext(IntPtr CertContext) { return CertFreeCertificateContext(CertContext); }
        Boolean ICryptoAPI.CertCloseStore(IntPtr handle, UInt32 flags) { return CertCloseStore(handle, flags); }
        Boolean ICryptoAPI.CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r) { return CertAddCertificateContextToStore(store,context,disposition,r); }
        Boolean ICryptoAPI.CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, out IntPtr r) { return CertAddCertificateContextToStore(store,context,disposition,out r); }
        Boolean ICryptoAPI.CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus) { return CertVerifyCertificateChainPolicy(Policy,ChainContext,ref PolicyPara,ref PolicyStatus); }
        IntPtr ICryptoAPI.CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, ref CMSG_STREAM_INFO si) { return CryptMsgOpenToDecode(MsgEncodingType,Flags,Type,CryptProv,RecipientInfo,ref si); }
        IntPtr ICryptoAPI.CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, IntPtr si) { return CryptMsgOpenToDecode(MsgEncodingType,Flags,Type,CryptProv,RecipientInfo,si); }
        Boolean ICryptoAPI.CryptMsgClose(IntPtr Message) { return CryptMsgClose(Message); }
        Boolean ICryptoAPI.CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, IntPtr CtrlPara) { return CryptMsgControl(Message,Flags,CtrlType,CtrlPara); }
        Boolean ICryptoAPI.CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, ref CMSG_CTRL_DECRYPT_PARA CtrlPara) { return CryptMsgControl(Message,Flags,CtrlType,ref CtrlPara); }
        Boolean ICryptoAPI.CryptMsgUpdate(IntPtr Message, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, Int32 Size, Boolean Final) { return CryptMsgUpdate(Message,Data,Size,Final); }
        Boolean ICryptoAPI.CryptMsgUpdate(IntPtr Message, IntPtr Data, Int32 Size, Boolean Final) { return CryptMsgUpdate(Message,Data,Size,Final); }
        Boolean ICryptoAPI.CryptMsgGetParam(IntPtr Message, CMSG_PARAM Parameter, Int32 SignerIndex, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, ref Int32 Size) { return CryptMsgGetParam(Message,Parameter,SignerIndex,Data,ref Size); }

        IntPtr ICryptoAPI.CertDuplicateCertificateContext([In] IntPtr CertContext){ return CertDuplicateCertificateContext(CertContext); }
        IntPtr ICryptoAPI.CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, [In] IntPtr Para) { return CertOpenStore(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para); }
        IntPtr ICryptoAPI.CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext) { return CertEnumCertificatesInStore(CertStore,PrevCertContext); }
        IntPtr ICryptoAPI.CertCreateCertificateContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size) { return CertCreateCertificateContext(CertEncodingType,blob,size); }
        IntPtr ICryptoAPI.CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext) { return CertEnumCRLsInStore(CertStore,PrevCrlContext); }
        unsafe Boolean ICryptoAPI.CertGetCertificateChain(IntPtr ChainEngine, IntPtr Context, ref FILETIME time, IntPtr AdditionalStore, ref CERT_CHAIN_PARA ChainPara, CERT_CHAIN_FLAGS Flags, IntPtr Reserved, CERT_CHAIN_CONTEXT** ChainContext) { return CertGetCertificateChain(ChainEngine,Context,ref time,AdditionalStore,ref ChainPara,Flags,Reserved,ChainContext); }
        unsafe IntPtr ICryptoAPI.CertGetSubjectCertificateFromStore(IntPtr CertStore, Int32 MsgAndCertEncodingType, CERT_INFO* CertId) { return CertGetSubjectCertificateFromStore(CertStore,MsgAndCertEncodingType,CertId); }

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
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCertificateContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCRLContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        [DllImport("libcrypt32", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext);
        [DllImport("libcrypt32", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertCloseStore(IntPtr handle, UInt32 flags);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern unsafe Boolean CertGetCertificateChain(IntPtr ChainEngine, IntPtr Context, ref FILETIME time, IntPtr AdditionalStore, ref CERT_CHAIN_PARA ChainPara, CERT_CHAIN_FLAGS Flags, IntPtr Reserved, CERT_CHAIN_CONTEXT** ChainContext);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, ref CMSG_STREAM_INFO si);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, IntPtr si);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgClose(IntPtr Message);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, IntPtr CtrlPara);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, ref CMSG_CTRL_DECRYPT_PARA CtrlPara);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgUpdate(IntPtr Message, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, Int32 Size, Boolean Final);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgUpdate(IntPtr Message, IntPtr Data, Int32 Size, Boolean Final);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgGetParam(IntPtr Message, CMSG_PARAM Parameter, Int32 SignerIndex, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, ref Int32 Size);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern unsafe IntPtr CertGetSubjectCertificateFromStore(IntPtr CertStore, Int32 MsgAndCertEncodingType, CERT_INFO* CertId);
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
