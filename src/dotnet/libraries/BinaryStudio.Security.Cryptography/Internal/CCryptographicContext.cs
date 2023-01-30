using System;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

namespace BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    internal class CCryptographicContext : CryptographicContext, ICryptoAPI
        {
        public override IntPtr Handle { get; }

        Boolean ICryptoAPI.CertFreeCertificateContext(IntPtr pCertContext) { return CertFreeCertificateContext(pCertContext); }
        Boolean ICryptoAPI.CertCloseStore(IntPtr handle, UInt32 flags) { return CertCloseStore(handle, flags); }
        Boolean ICryptoAPI.CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r) { return CertAddCertificateContextToStore(store,context,disposition,r); }
        Boolean ICryptoAPI.CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, out IntPtr r) { return CertAddCertificateContextToStore(store,context,disposition,out r); }
        Boolean ICryptoAPI.CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus) { return CertVerifyCertificateChainPolicy(Policy,ChainContext,ref PolicyPara,ref PolicyStatus); }
        IntPtr ICryptoAPI.CertDuplicateCertificateContext([In] IntPtr pCertContext){ return CertDuplicateCertificateContext(pCertContext); }
        unsafe IntPtr ICryptoAPI.CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, [In] String Para) {
            if (StoreProvider == CERT_STORE_PROV_SYSTEM_W) {
                StoreProvider = CERT_STORE_PROV_SYSTEM_A;
                return CertOpenStore(
                    StoreProvider, MsgAndCertEncodingType,
                    CryptProv,Flags,
                    (IntPtr)LocalMemoryManager.StringToMem(Para,Encoding.ASCII));
                }
            return CertOpenStore(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para);
            }
        IntPtr ICryptoAPI.CertOpenStore(IntPtr lpszStoreProvider, UInt32 dwMsgAndCertEncodingType, IntPtr hCryptProv, UInt32 dwFlags, [In] IntPtr pvPara) { return CertOpenStore(lpszStoreProvider, dwMsgAndCertEncodingType,hCryptProv,dwFlags,pvPara); }
        IntPtr ICryptoAPI.CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext) { return CertEnumCertificatesInStore(CertStore,PrevCertContext); }
        IntPtr ICryptoAPI.CertCreateCertificateContext(UInt32 dwCertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size) { return CertCreateCertificateContext(dwCertEncodingType,blob,size); }
        IntPtr ICryptoAPI.CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext) { return CertEnumCRLsInStore(CertStore,PrevCrlContext); }
        unsafe Boolean ICryptoAPI.CertGetCertificateChain(IntPtr ChainEngine, IntPtr Context, ref FILETIME time, IntPtr AdditionalStore, ref CERT_CHAIN_PARA ChainPara, CERT_CHAIN_FLAGS Flags, IntPtr Reserved, CERT_CHAIN_CONTEXT** ChainContext) { return CertGetCertificateChain(ChainEngine,Context,ref time,AdditionalStore,ref ChainPara,Flags,Reserved,ChainContext); }
        IntPtr ICryptoAPI.CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, ref CMSG_STREAM_INFO si) { return CryptMsgOpenToDecode(MsgEncodingType,Flags,Type,CryptProv,RecipientInfo,ref si); }
        IntPtr ICryptoAPI.CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, IntPtr si) { return CryptMsgOpenToDecode(MsgEncodingType,Flags,Type,CryptProv,RecipientInfo,si); }
        Boolean ICryptoAPI.CryptMsgClose(IntPtr Message) { return CryptMsgClose(Message); }
        Boolean ICryptoAPI.CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, IntPtr CtrlPara) { return CryptMsgControl(Message,Flags,CtrlType,CtrlPara); }
        Boolean ICryptoAPI.CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, ref CMSG_CTRL_DECRYPT_PARA CtrlPara) { return CryptMsgControl(Message,Flags,CtrlType,ref CtrlPara); }
        Boolean ICryptoAPI.CryptMsgUpdate(IntPtr Message, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, Int32 Size, Boolean Final) { return CryptMsgUpdate(Message,Data,Size,Final); }
        Boolean ICryptoAPI.CryptMsgUpdate(IntPtr Message, IntPtr Data, Int32 Size, Boolean Final) { return CryptMsgUpdate(Message,Data,Size,Final); }
        Boolean ICryptoAPI.CryptMsgGetParam(IntPtr Message, CMSG_PARAM Parameter, Int32 SignerIndex, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, ref Int32 Size) { return CryptMsgGetParam(Message,Parameter,SignerIndex,Data,ref Size); }
        unsafe IntPtr ICryptoAPI.CertGetSubjectCertificateFromStore(IntPtr CertStore, Int32 MsgAndCertEncodingType, CERT_INFO* CertId) { return CertGetSubjectCertificateFromStore(CertStore,MsgAndCertEncodingType,CertId); }

        [DllImport("libcapi20", SetLastError = true)] private static extern Boolean CertFreeCertificateContext(IntPtr pCertContext);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertDuplicateCertificateContext([In] IntPtr pCertContext);
        [DllImport("libcapi20", CharSet = CharSet.Auto, SetLastError = true)] private static extern bool CertControlStore([In] IntPtr hCertStore, [In] uint dwFlags, [In] uint dwCtrlType, [In] IntPtr pvCtrlPara);
        [DllImport("libcapi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumSystemStoreLocation(Int32 flags, IntPtr args, PFN_CERT_ENUM_SYSTEM_STORE_LOCATION pfn);
        [DllImport("libcapi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern IntPtr CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, [In] String Para);
        [DllImport("libcapi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern IntPtr CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, [In] IntPtr Para);
        [DllImport("libcapi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r);
        [DllImport("libcapi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, out IntPtr r);
        [DllImport("libcapi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertAddCRLContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r);
        [DllImport("libcapi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS flags, IntPtr pvSystemStoreLocationPara, IntPtr pvArg, CertEnumSystemStoreCallbackIntPtr pfnEnum);
        [DllImport("libcapi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumPhysicalStore(IntPtr pvSystemStore, CERT_SYSTEM_STORE_FLAGS flags, IntPtr pvArg, PFN_CERT_ENUM_PHYSICAL_STORE pfnEnum);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCertificateContext(UInt32 dwCertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCRLContext(UInt32 dwCertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        [DllImport("libcapi20", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext);
        [DllImport("libcapi20", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext);
        [DllImport("libcapi20", SetLastError = true)] private static extern Boolean CertCloseStore(IntPtr handle, UInt32 flags);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern unsafe Boolean CertGetCertificateChain(IntPtr ChainEngine, IntPtr Context, ref FILETIME time, IntPtr AdditionalStore, ref CERT_CHAIN_PARA ChainPara, CERT_CHAIN_FLAGS Flags, IntPtr Reserved, CERT_CHAIN_CONTEXT** ChainContext);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, ref CMSG_STREAM_INFO si);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, IntPtr si);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgClose(IntPtr Message);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, IntPtr CtrlPara);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, ref CMSG_CTRL_DECRYPT_PARA CtrlPara);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgUpdate(IntPtr Message, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, Int32 Size, Boolean Final);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgUpdate(IntPtr Message, IntPtr Data, Int32 Size, Boolean Final);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgGetParam(IntPtr Message, CMSG_PARAM Parameter, Int32 SignerIndex, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, ref Int32 Size);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern unsafe IntPtr CertGetSubjectCertificateFromStore(IntPtr CertStore, Int32 MsgAndCertEncodingType, CERT_INFO* CertId);
        [DllImport("librdrsup")] private static extern Int32 GetLastError();

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
                    return Callback(Marshal.PtrToStringAnsi(SystemStore), StoreFlags,ref StoreInfo,Reserved,StoreArg);
                    });
            }
        }
    }
