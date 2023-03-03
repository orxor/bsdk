using System;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

namespace BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    using CERT_NAME_BLOB = CRYPT_BLOB;
    internal class CryptographicContextC : CryptographicContext, ICryptoAPI
        {
        public override IntPtr Handle { get; }
        public Encoding UnicodeEncoding { get { return Encoding.Unicode; }}
        public override CRYPT_PROVIDER_TYPE ProviderType { get { return CRYPT_PROVIDER_TYPE.PROV_GOST_2012_512; }}

        Boolean ICryptoAPI.CertFreeCertificateContext(IntPtr pCertContext) { return CertFreeCertificateContext(pCertContext); }
        Boolean ICryptoAPI.CertCloseStore(IntPtr handle, UInt32 flags) { return CertCloseStore(handle, flags); }
        Boolean ICryptoAPI.CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r) { return CertAddCertificateContextToStore(store,context,disposition,r); }
        Boolean ICryptoAPI.CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, out IntPtr r) { return CertAddCertificateContextToStore(store,context,disposition,out r); }
        Boolean ICryptoAPI.CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus) { return CertVerifyCertificateChainPolicy(Policy,ChainContext,ref PolicyPara,ref PolicyStatus); }
        IntPtr ICryptoAPI.CertDuplicateCertificateContext([In] IntPtr pCertContext){ return CertDuplicateCertificateContext(pCertContext); }
        IntPtr ICryptoAPI.CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, IntPtr Para) { return CertOpenStoreA(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para); }
        IntPtr ICryptoAPI.CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, String Para) { return CertOpenStoreA(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para); }
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
        Int32 ICryptoAPI.CertNameToStr(Int32 CertEncodingType, ref CERT_NAME_BLOB Name, Int32 StrType, IntPtr psz, Int32 csz) { return CertNameToStr(CertEncodingType,ref Name,StrType,psz,csz); }
        unsafe IntPtr ICryptoAPI.CertGetSubjectCertificateFromStore(IntPtr CertStore, Int32 MsgAndCertEncodingType, CERT_INFO* CertId) { return CertGetSubjectCertificateFromStore(CertStore,MsgAndCertEncodingType,CertId); }
        Boolean ICryptoAPI.CryptEnumProviders(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, StringBuilder name, ref Int32 sz) { return CryptEnumProviders(index,reserved,flags,out type,name,ref sz); }
        Boolean ICryptoAPI.CryptEnumProviderTypes(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, StringBuilder name, ref Int32 sz) { return CryptEnumProviderTypes(index,reserved,flags,out type,name,ref sz); }
        Boolean ICryptoAPI.CryptAcquireContext(out IntPtr CryptProv, String Container, String Provider, Int32 ProvType, Int32 Flags) { return CryptAcquireContext(out CryptProv,Container,Provider,ProvType,Flags); }
        Boolean ICryptoAPI.CryptGetProvParam(IntPtr Provider, CRYPT_PARAM Parameter, IntPtr Data, ref Int32 DataSize, Int32 Flags) { return CryptGetProvParam(Provider,Parameter,Data,ref DataSize,Flags); }
        Boolean ICryptoAPI.CryptGetProvParam(IntPtr Provider, CRYPT_PARAM Parameter, Byte[] Data, ref Int32 DataSize, Int32 Flags) { return CryptGetProvParam(Provider,Parameter,Data,ref DataSize,Flags); }
        ALG_ID ICryptoAPI.CertOIDToAlgId(String Id) { return CertOIDToAlgId(Id); }
        IntPtr ICryptoAPI.CryptFindOIDInfo(CRYPT_OID_INFO_KEY_TYPE KeyType,IntPtr Key,Int32 GroupId) { return CryptFindOIDInfo((Int32)KeyType,Key,GroupId); }
        Boolean ICryptoAPI.CryptEnumOIDInfo(CRYPT_ALG_OID_GROUP_ID GroupId,IntPtr Arg,CryptEnumOidInfoCallback Callback) { return CryptEnumOIDInfo((Int32)GroupId,0,Arg,Callback); }
        Boolean ICryptoAPI.CryptAcquireCertificatePrivateKey(IntPtr Certificate, CRYPT_ACQUIRE_FLAGS Flags, IntPtr Parameters,out IntPtr CryptProvOrNCryptKey, out KEY_SPEC_TYPE KeySpec, out Boolean CallerFreeProvOrNCryptKey) { return CryptAcquireCertificatePrivateKey(Certificate,Flags,Parameters,out CryptProvOrNCryptKey,out KeySpec,out CallerFreeProvOrNCryptKey); }
        Int32 ILastErrorProvider.GetLastError() { return GetLastWin32Error(); }
        Boolean ICryptoAPI.CertGetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Byte[] Data, ref Int32 Size) { return CertGetCertificateContextProperty(Context,PropertyIndex,Data,ref Size); }
        Boolean ICryptoAPI.CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, ref CRYPT_KEY_PROV_INFO Data) { return CertSetCertificateContextProperty(Context,PropertyIndex,Flags,ref Data); }
        Boolean ICryptoAPI.CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, IntPtr Data) { return CertSetCertificateContextProperty(Context,PropertyIndex,Flags,Data); }
        #region Key Generation and Exchange Functions
        Boolean KeyGenerationAndExchangeFunctions.CryptDeriveKey(IntPtr Context,ALG_ID AlgId,Int32 Flags,out IntPtr r) { return CryptDeriveKey(Context,AlgId,Flags,out r); }
        Boolean KeyGenerationAndExchangeFunctions.CryptDestroyKey(IntPtr Key) { return CryptDestroyKey(Key); }
        Boolean KeyGenerationAndExchangeFunctions.CryptDuplicateKey(IntPtr Key,IntPtr Reserved,Int32 Flags,out IntPtr r) { return CryptDuplicateKey(Key,Reserved,Flags,out r); }
        Boolean KeyGenerationAndExchangeFunctions.CryptExportKey(IntPtr Key,IntPtr ExpKey,Int32 BlobType,Int32 Flags,Byte[] Data,ref Int32 DataLen) { return CryptExportKey(Key,ExpKey,BlobType,Flags,Data,ref DataLen); }
        Boolean KeyGenerationAndExchangeFunctions.CryptGenKey(IntPtr Context,ALG_ID AlgId,Int32 Flags,out IntPtr r) { return CryptGenKey(Context,AlgId,Flags,out r); }
        Boolean KeyGenerationAndExchangeFunctions.CryptGenRandom(IntPtr Context,Int32 Length,Byte[] Buffer) { return CryptGenRandom(Context,Length,Buffer); }
        Boolean KeyGenerationAndExchangeFunctions.CryptGetKeyParam(IntPtr Key,KEY_PARAM Param,Byte[] Data,ref Int32 DataSize, Int32 Flags) { return CryptGetKeyParam(Key,Param,Data,ref DataSize,Flags); }
        Boolean KeyGenerationAndExchangeFunctions.CryptGetUserKey(IntPtr Context,KEY_SPEC_TYPE KeySpec,out IntPtr r) { return CryptGetUserKey(Context,KeySpec,out r); }
        Boolean KeyGenerationAndExchangeFunctions.CryptImportKey(IntPtr Context,Byte[] Data,Int32 DataLen,IntPtr PubKey,Int32 Flags,out IntPtr r) { return CryptImportKey(Context,Data,DataLen,PubKey,Flags,out r); }
        Boolean KeyGenerationAndExchangeFunctions.CryptSetKeyParam(IntPtr Key,KEY_PARAM Param,Byte[] Data,Int32 Flags) { return CryptSetKeyParam(Key,Param,Data,Flags); }
        #endregion

        [DllImport("libcapi20", SetLastError = true)] private static extern Boolean CertFreeCertificateContext(IntPtr pCertContext);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertDuplicateCertificateContext([In] IntPtr pCertContext);
        [DllImport("libcapi20", CharSet = CharSet.Auto, SetLastError = true)] private static extern bool CertControlStore([In] IntPtr hCertStore, [In] uint dwFlags, [In] uint dwCtrlType, [In] IntPtr pvCtrlPara);
        [DllImport("libcapi20", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumSystemStoreLocation(Int32 flags, IntPtr args, PFN_CERT_ENUM_SYSTEM_STORE_LOCATION pfn);
        [DllImport("libcapi20", CharSet = CharSet.None, SetLastError = true,EntryPoint="CertOpenStore")] private static extern IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv,Int32 Flags, IntPtr Para);
        [DllImport("libcapi20", CharSet = CharSet.None, SetLastError = true,EntryPoint="CertOpenStore")] private static extern IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv,Int32 Flags, [MarshalAs(UnmanagedType.LPStr)] String Para);
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
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)] private static extern Int32 CertNameToStr(Int32 CertEncodingType, ref CERT_NAME_BLOB Name, Int32 StrType, IntPtr psz, Int32 csz);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)] private static extern Boolean CryptEnumProviders(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, [In][Out] StringBuilder name, ref Int32 sz);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)] private static extern Boolean CryptEnumProviderTypes(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, [In][Out] StringBuilder name, ref Int32 sz);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "CryptAcquireContextA", SetLastError = true)] private static extern Boolean CryptAcquireContext(out IntPtr CryptProv, [MarshalAs(UnmanagedType.LPStr)] String Container, [MarshalAs(UnmanagedType.LPStr)]String Provider, Int32 ProvType, Int32 Flags);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern ALG_ID CertOIDToAlgId([MarshalAs(UnmanagedType.LPStr)] String Id);
        [DllImport("libcapi20", SetLastError = true)] [return: MarshalAs(UnmanagedType.Bool)] private static extern Boolean CryptGetProvParam(IntPtr Provider, CRYPT_PARAM Parameter, IntPtr Data, ref Int32 DataSize, Int32 Flags);
        [DllImport("libcapi20", SetLastError = true)] [return: MarshalAs(UnmanagedType.Bool)] private static extern Boolean CryptGetProvParam(IntPtr Provider, CRYPT_PARAM Parameter, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, ref Int32 DataSize, Int32 Flags);
        [DllImport("libcapi20", SetLastError = true)] private static extern IntPtr CryptFindOIDInfo(Int32 KeyType,IntPtr Key,Int32 GroupId);
        [DllImport("libcapi20", SetLastError = true)] private static extern Boolean CryptEnumOIDInfo(Int32 GroupId,Int32 Flags,IntPtr Arg,CryptEnumOidInfoCallback Callback);
        [DllImport("librdrsup")] private static extern Int32 GetLastError();
        #region Key Generation and Exchange Functions
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptDeriveKey(IntPtr Context,ALG_ID AlgId,Int32 Flags,out IntPtr r);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptDestroyKey(IntPtr Key);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptDuplicateKey(IntPtr Key,IntPtr Reserved,Int32 Flags,out IntPtr r);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptExportKey(IntPtr Key,IntPtr ExpKey,Int32 BlobType,Int32 Flags, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data,ref Int32 DataLen);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptGenKey(IntPtr Context,ALG_ID AlgId,Int32 Flags,out IntPtr r);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptGenRandom(IntPtr Context,Int32 Length,[MarshalAs(UnmanagedType.LPArray)] Byte[] Buffer);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptGetKeyParam(IntPtr Key,KEY_PARAM Param,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,ref Int32 DataLen,Int32 Flags);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptGetUserKey(IntPtr Context,KEY_SPEC_TYPE KeySpec,out IntPtr UserKey);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptImportKey(IntPtr Context,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,Int32 DataLen,IntPtr PubKey,Int32 Flags,out IntPtr r);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptSetKeyParam(IntPtr Key,KEY_PARAM Param,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,Int32 Flags);
        #endregion
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptAcquireCertificatePrivateKey(IntPtr Certificate, CRYPT_ACQUIRE_FLAGS Flags, IntPtr Parameters,out IntPtr CryptProvOrNCryptKey, out KEY_SPEC_TYPE KeySpec, out Boolean CallerFreeProvOrNCryptKey);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertGetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, [MarshalAs(UnmanagedType.LPArray)]Byte[] Data, ref Int32 Size);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, ref CRYPT_KEY_PROV_INFO Data);
        [DllImport("libcapi20", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, IntPtr Data);

        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="service">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="service"/>.
        /// -or-
        /// <see langword="null"/> if there is no service object of type <paramref name="service"/>.</returns>
        public override Object GetService(Type service) {
            if (service == typeof(ICryptoAPI)) { return this; }
            if (service == typeof(KeyGenerationAndExchangeFunctions)) { return this; }
            return base.GetService(service);
            }

        #region M:GetLastWin32Error:Int32
        /// <summary>
        /// Returns the error code returned by the last unmanaged function that was called.
        /// using platform invoke that has the System.Runtime.InteropServices.DllImportAttribute.SetLastError flag set.
        /// </summary>
        /// <returns>The last error code set by a call to the Win32 SetLastError function.</returns>
        protected internal override Int32 GetLastWin32Error()
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
