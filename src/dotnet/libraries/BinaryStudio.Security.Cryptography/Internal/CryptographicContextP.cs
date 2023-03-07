using System;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

namespace BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    using CERT_NAME_BLOB = CRYPT_BLOB;
    using HCERT_SERVER_OCSP_RESPONSE=IntPtr;
    using HCRYPTPROV_OR_NCRYPT_KEY_HANDLE=IntPtr;
    using HCERTSTORE=IntPtr;

    internal class CryptographicContextP : CryptographicContext, ICryptoAPI
        {
        public override IntPtr Handle { get; }
        public Encoding UnicodeEncoding { get { return Encoding.UTF32; }}
        public override CRYPT_PROVIDER_TYPE ProviderType { get { return CRYPT_PROVIDER_TYPE.VPN_PROV_TYPE_2012_1024; }}

        Boolean ICryptoAPI.CertCloseStore(IntPtr handle, UInt32 flags) { return CertCloseStore(handle, flags); }
        Boolean ICryptoAPI.CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus) { return CertVerifyCertificateChainPolicy(Policy,ChainContext,ref PolicyPara,ref PolicyStatus); }
        IntPtr ICryptoAPI.CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, ref CMSG_STREAM_INFO si) { return CryptMsgOpenToDecode(MsgEncodingType,Flags,Type,CryptProv,RecipientInfo,ref si); }
        IntPtr ICryptoAPI.CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, IntPtr si) { return CryptMsgOpenToDecode(MsgEncodingType,Flags,Type,CryptProv,RecipientInfo,si); }
        Boolean ICryptoAPI.CryptMsgClose(IntPtr Message) { return CryptMsgClose(Message); }
        Boolean ICryptoAPI.CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, IntPtr CtrlPara) { return CryptMsgControl(Message,Flags,CtrlType,CtrlPara); }
        Boolean ICryptoAPI.CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, ref CMSG_CTRL_DECRYPT_PARA CtrlPara) { return CryptMsgControl(Message,Flags,CtrlType,ref CtrlPara); }
        Boolean ICryptoAPI.CryptMsgUpdate(IntPtr Message, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, Int32 Size, Boolean Final) { return CryptMsgUpdate(Message,Data,Size,Final); }
        Boolean ICryptoAPI.CryptMsgUpdate(IntPtr Message, IntPtr Data, Int32 Size, Boolean Final) { return CryptMsgUpdate(Message,Data,Size,Final); }
        Boolean ICryptoAPI.CryptMsgGetParam(IntPtr Message, CMSG_PARAM Parameter, Int32 SignerIndex, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, ref Int32 Size) { return CryptMsgGetParam(Message,Parameter,SignerIndex,Data,ref Size); }

        IntPtr ICryptoAPI.CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, IntPtr Para) { return CertOpenStoreA(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para); }
        IntPtr ICryptoAPI.CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, String Para) { return CertOpenStoreA(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para); }
        IntPtr ICryptoAPI.CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext) { return CertEnumCRLsInStore(CertStore,PrevCrlContext); }
        unsafe Boolean ICryptoAPI.CertGetCertificateChain(IntPtr ChainEngine, IntPtr Context, ref FILETIME time, IntPtr AdditionalStore, ref CERT_CHAIN_PARA ChainPara, CERT_CHAIN_FLAGS Flags, IntPtr Reserved, CERT_CHAIN_CONTEXT** ChainContext) { return CertGetCertificateChain(ChainEngine,Context,ref time,AdditionalStore,ref ChainPara,Flags,Reserved,ChainContext); }
        Boolean ICryptoAPI.CryptEnumProviders(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, StringBuilder name, ref Int32 sz) { return CryptEnumProviders(index,reserved,flags,out type,name,ref sz); }
        Boolean ICryptoAPI.CryptEnumProviderTypes(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, StringBuilder name, ref Int32 sz) { return CryptEnumProviderTypes(index,reserved,flags,out type,name,ref sz); }
        Boolean ICryptoAPI.CryptAcquireContext(out IntPtr CryptProv, String Container, String Provider, Int32 ProvType, Int32 Flags) { return CryptAcquireContext(out CryptProv,Container,Provider,ProvType,Flags); }
        Boolean ICryptoAPI.CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,ref Int32 DataSize,Int32 Flags) { return CryptGetProvParam(Context,Parameter,Data,ref DataSize,Flags); }
        Boolean ICryptoAPI.CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,Byte[] Data,ref Int32 DataSize,Int32 Flags) { return CryptGetProvParam(Context,Parameter,Data,ref DataSize,Flags); }
        Boolean ICryptoAPI.CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,Byte[] Data,Int32 Flags) { return CryptSetProvParam(Context,Parameter,Data,Flags); }
        Boolean ICryptoAPI.CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,Int32 Flags) { return CryptSetProvParam(Context,Parameter,Data,Flags); }
        unsafe IntPtr ICryptoAPI.CryptFindOIDInfo(CRYPT_OID_INFO_KEY_TYPE KeyType,void* Key,Int32 GroupId) { return CryptFindOIDInfo((Int32)KeyType,Key,GroupId); }
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
        #region Certificate Functions
        void CertificateFunctions.CertAddRefServerOcspResponseContext(HCERT_SERVER_OCSP_RESPONSE ServerOcspResponseContext) { CertAddRefServerOcspResponseContext(ServerOcspResponseContext); }
        void CertificateFunctions.CertCloseServerOcspResponse(HCERT_SERVER_OCSP_RESPONSE ServerOcspResponse,Int32 Flags) { CertCloseServerOcspResponse(ServerOcspResponse,Flags); }
        IntPtr CertificateFunctions.CertCreateCertificateContext(Int32 CertEncodingType,Byte[] CertEncodedBytes,Int32 CertEncodedLength) { return CertCreateCertificateContext(CertEncodingType,CertEncodedBytes,CertEncodedLength); }
        IntPtr CertificateFunctions.CertDuplicateCertificateContext(IntPtr CertContext) { return CertDuplicateCertificateContext(CertContext); }
        IntPtr CertificateFunctions.CertEnumCertificatesInStore(HCERTSTORE CertStore,IntPtr PrevCertContext) { return CertEnumCertificatesInStore(CertStore,PrevCertContext); }
        IntPtr CertificateFunctions.CertFindCertificateInStore(HCERTSTORE CertStore,Int32 CertEncodingType,Int32 FindFlags,Int32 FindType,IntPtr FindPara,IntPtr PrevCertContext) { return CertFindCertificateInStore(CertStore,CertEncodingType,FindFlags,FindType,FindPara,PrevCertContext); }
        IntPtr CertificateFunctions.CertGetIssuerCertificateFromStore(HCERTSTORE CertStore,IntPtr SubjectContext,IntPtr PrevIssuerContext,ref Int32 Flags) { return CertGetIssuerCertificateFromStore(CertStore,SubjectContext,PrevIssuerContext,ref Flags); }
        Boolean CertificateFunctions.CertAddCertificateContextToStore(HCERTSTORE CertStore,IntPtr CertContext,Int32 AddDisposition,IntPtr Zero) { return CertAddCertificateContextToStore(CertStore,CertContext,AddDisposition,Zero); }
        Boolean CertificateFunctions.CertAddCertificateLinkToStore(HCERTSTORE CertStore,IntPtr CertContext,Int32 AddDisposition,out IntPtr StoreContext) { return CertAddCertificateLinkToStore(CertStore,CertContext,AddDisposition,out StoreContext); }
        Boolean CertificateFunctions.CertAddEncodedCertificateToStore(HCERTSTORE CertStore,Int32 CertEncodingType,Byte[] CertEncodedData,Int32 CertEncodedLength,Int32 AddDisposition,out IntPtr CertContext) { return CertAddEncodedCertificateToStore(CertStore,CertEncodingType,CertEncodedData,CertEncodedLength,AddDisposition,out CertContext); }
        Boolean CertificateFunctions.CertDeleteCertificateFromStore(IntPtr CertContext) { return CertDeleteCertificateFromStore(CertContext); }
        Boolean CertificateFunctions.CertFreeCertificateContext(IntPtr CertContext) { return CertFreeCertificateContext(CertContext); }
        Boolean CertificateFunctions.CertRetrieveLogoOrBiometricInfo(IntPtr CertContext,String LogoOrBiometricType,Int32 RetrievalFlags,Int32 Timeout,Int32 Flags,IntPtr Reserved,out IntPtr ppbData,out Int32 pcbData,out IntPtr ppwszMimeType) { return CertRetrieveLogoOrBiometricInfo(CertContext,LogoOrBiometricType,RetrievalFlags,Timeout,Flags,Reserved,out ppbData,out pcbData,out ppwszMimeType); }
        Boolean CertificateFunctions.CertSerializeCertificateStoreElement(IntPtr CertContext,Int32 Flags,Byte[] pbElement,ref Int32 pcbElement) { return CertSerializeCertificateStoreElement(CertContext,Flags,pbElement,ref pcbElement); }
        Boolean CertificateFunctions.CertVerifySubjectCertificateContext(IntPtr Subject,IntPtr Issuer,ref Int32 Flags) { return CertVerifySubjectCertificateContext(Subject,Issuer,ref Flags); }
        unsafe HCERT_SERVER_OCSP_RESPONSE CertificateFunctions.CertAddRefServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara) { return CertAddRefServerOcspResponse(ChainContext,Flags,OpenPara); }
        unsafe HCERT_SERVER_OCSP_RESPONSE CertificateFunctions.CertOpenServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara) { return CertOpenServerOcspResponse(ChainContext,Flags,OpenPara); }
        unsafe CERT_SERVER_OCSP_RESPONSE_CONTEXT* CertificateFunctions.CertGetServerOcspResponseContext(HCERT_SERVER_OCSP_RESPONSE ServerOcspResponse,Int32 Flags,IntPtr Reserved) { return CertGetServerOcspResponseContext(ServerOcspResponse,Flags,Reserved); }
        unsafe IntPtr CertificateFunctions.CertCreateSelfSignCertificate(HCRYPTPROV_OR_NCRYPT_KEY_HANDLE CryptProvOrNCryptKey,ref CERT_NAME_BLOB SubjectIssuerBlob,Int32 Flags,CRYPT_KEY_PROV_INFO* KeyProvInfo,CRYPT_ALGORITHM_IDENTIFIER* SignatureAlgorithm,SYSTEMTIME* StartTime,SYSTEMTIME* EndTime,CERT_EXTENSIONS* Extensions) { return CertCreateSelfSignCertificate(CryptProvOrNCryptKey,ref SubjectIssuerBlob,Flags,KeyProvInfo,SignatureAlgorithm,StartTime,EndTime,Extensions); }
        unsafe IntPtr CertificateFunctions.CertGetSubjectCertificateFromStore(HCERTSTORE CertStore,Int32 CertEncodingType,CERT_INFO* CertId) { return CertGetSubjectCertificateFromStore(CertStore,CertEncodingType,CertId); }
        unsafe Boolean CertificateFunctions.CertGetValidUsages(Int32 cCerts,CERT_CONTEXT* rghCerts,Int32* cNumOIDs,IntPtr rghOIDs,Int32* pcbOIDs) { return CertGetValidUsages(cCerts,rghCerts,cNumOIDs,rghOIDs,pcbOIDs); }
        unsafe Boolean CertificateFunctions.CertSelectCertificateChains(ref Guid SelectionContext,Int32 Flags,CERT_SELECT_CHAIN_PARA* ChainParameters,Int32 cCriteria,CERT_SELECT_CRITERIA* rgpCriteria,HCERTSTORE Store,out Int32 pcSelection,out CERT_CHAIN_CONTEXT* pprgpSelection) { return CertSelectCertificateChains(ref SelectionContext,Flags,ChainParameters,cCriteria,rgpCriteria,Store,out pcSelection,out pprgpSelection); }
        #endregion
        #region Data Conversion Functions
        Int32 DataConversionFunctions.CertNameToStrA(Int32 CertEncodingType, ref CERT_NAME_BLOB Name, Int32 StrType, IntPtr psz, Int32 csz) { return CertNameToStrA(CertEncodingType,ref Name,StrType,psz,csz); }
        Int32 DataConversionFunctions.CertNameToStrW(Int32 CertEncodingType, ref CERT_NAME_BLOB Name, Int32 StrType, IntPtr psz, Int32 csz) { return CertNameToStrW(CertEncodingType,ref Name,StrType,psz,csz); }
        ALG_ID DataConversionFunctions.CertOIDToAlgId(String Id) { return CertOIDToAlgId(Id); }
        IntPtr DataConversionFunctions.CertAlgIdToOID(ALG_ID Id) { return CertAlgIdToOID(Id); }
        Boolean DataConversionFunctions.CertStrToName(Int32 CertEncodingType,String Name,Int32 StrType,IntPtr Reserved,Byte[] EncodedBytes,ref Int32 EncodedLength,IntPtr Error) { return CertStrToNameA(CertEncodingType,Name,StrType,Reserved,EncodedBytes,ref EncodedLength,Error); }
        #endregion

        [DllImport("libcrypt32", CharSet = CharSet.Auto, SetLastError = true)] private static extern bool CertControlStore([In] IntPtr CertStore, [In] uint Flags, [In] uint CtrlType, [In] IntPtr CtrlPara);
        [DllImport("libcrypt32", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumSystemStoreLocation(Int32 flags, IntPtr args, PFN_CERT_ENUM_SYSTEM_STORE_LOCATION pfn);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true,EntryPoint="CertOpenStore")] private static extern IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, IntPtr Para);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true,EntryPoint="CertOpenStore")] private static extern IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, [MarshalAs(UnmanagedType.LPStr)] String Para);
        [DllImport("libcrypt32", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS Flags, IntPtr SystemStoreLocationPara, IntPtr Arg, CertEnumSystemStoreCallbackIntPtr Callback);
        [DllImport("libcrypt32", CharSet = CharSet.Unicode, SetLastError = true)] private static extern Boolean CertEnumPhysicalStore(IntPtr SystemStore, CERT_SYSTEM_STORE_FLAGS Flags, IntPtr Arg, PFN_CERT_ENUM_PHYSICAL_STORE Callback);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCRLContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
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
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)] private static extern Boolean CryptEnumProviders(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, [In][Out] StringBuilder name, ref Int32 sz);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)] private static extern Boolean CryptEnumProviderTypes(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, [In][Out] StringBuilder name, ref Int32 sz);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "CryptAcquireContextA", SetLastError = true)] private static extern Boolean CryptAcquireContext(out IntPtr CryptProv, [MarshalAs(UnmanagedType.LPStr)] String Container, [MarshalAs(UnmanagedType.LPStr)]String Provider, Int32 ProvType, Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,ref Int32 DataSize,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,ref Int32 DataSize,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern unsafe IntPtr CryptFindOIDInfo(Int32 KeyType,void* Key,Int32 GroupId);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptEnumOIDInfo(Int32 GroupId,Int32 Flags,IntPtr Arg,CryptEnumOidInfoCallback Callback);
        [DllImport("libkernel32")] private static extern Int32 GetLastError();
        [DllImport("libkernel32")] private static extern void SetLastError(Int32 code);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptAcquireCertificatePrivateKey(IntPtr Certificate, CRYPT_ACQUIRE_FLAGS Flags, IntPtr Parameters,out IntPtr CryptProvOrNCryptKey, out KEY_SPEC_TYPE KeySpec, out Boolean CallerFreeProvOrNCryptKey);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertGetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, [MarshalAs(UnmanagedType.LPArray)]Byte[] Data, ref Int32 Size);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, ref CRYPT_KEY_PROV_INFO Data);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, IntPtr Data);
        #region Key Generation and Exchange Functions
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptDeriveKey(IntPtr Context,ALG_ID AlgId,Int32 Flags,out IntPtr r);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptDestroyKey(IntPtr Key);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptDuplicateKey(IntPtr Key,IntPtr Reserved,Int32 Flags,out IntPtr r);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptExportKey(IntPtr Key,IntPtr ExpKey,Int32 BlobType,Int32 Flags, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data,ref Int32 DataLen);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptGenKey(IntPtr Context,ALG_ID AlgId,Int32 Flags,out IntPtr r);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptGenRandom(IntPtr Context,Int32 Length,[MarshalAs(UnmanagedType.LPArray)] Byte[] Buffer);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptGetKeyParam(IntPtr Key,KEY_PARAM Param,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,ref Int32 DataLen,Int32 Flags);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptGetUserKey(IntPtr Context,KEY_SPEC_TYPE KeySpec,out IntPtr UserKey);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptImportKey(IntPtr Context,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,Int32 DataLen,IntPtr PubKey,Int32 Flags,out IntPtr r);
        [DllImport("libcrypt32", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptSetKeyParam(IntPtr Key,KEY_PARAM Param,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,Int32 Flags);
        #endregion
        #region Certificate Functions
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern void CertAddRefServerOcspResponseContext(HCERT_SERVER_OCSP_RESPONSE ServerOcspResponseContext);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern void CertCloseServerOcspResponse(HCERT_SERVER_OCSP_RESPONSE ServerOcspResponse,Int32 Flags);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertAddCertificateContextToStore(HCERTSTORE CertStore,IntPtr CertContext,Int32 AddDisposition,IntPtr Zero);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertAddCertificateLinkToStore(HCERTSTORE CertStore,IntPtr CertContext,Int32 AddDisposition,out IntPtr StoreContext);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertAddEncodedCertificateToStore(HCERTSTORE CertStore,Int32 CertEncodingType,[MarshalAs(UnmanagedType.LPArray)] Byte[] CertEncodedData,Int32 CertEncodedLength,Int32 AddDisposition,out IntPtr CertContext);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertDeleteCertificateFromStore(IntPtr CertContext);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertFreeCertificateContext(IntPtr CertContext);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertRetrieveLogoOrBiometricInfo(IntPtr CertContext,String LogoOrBiometricType,Int32 RetrievalFlags,Int32 Timeout,Int32 Flags,IntPtr Reserved,out IntPtr ppbData,out Int32 pcbData,out IntPtr ppwszMimeType);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertSerializeCertificateStoreElement(IntPtr CertContext,Int32 Flags,[MarshalAs(UnmanagedType.LPArray)]Byte[] pbElement,ref Int32 pcbElement);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertVerifySubjectCertificateContext(IntPtr Subject,IntPtr Issuer,ref Int32 Flags);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertCreateCertificateContext(Int32 CertEncodingType,[MarshalAs(UnmanagedType.LPArray)]Byte[] CertEncodedBytes,Int32 CertEncodedLength);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertDuplicateCertificateContext(IntPtr CertContext);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertEnumCertificatesInStore(HCERTSTORE CertStore,IntPtr PrevCertContext);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertFindCertificateInStore(HCERTSTORE CertStore,Int32 CertEncodingType,Int32 FindFlags,Int32 FindType,IntPtr FindPara,IntPtr PrevCertContext);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertGetIssuerCertificateFromStore(HCERTSTORE CertStore,IntPtr SubjectContext,IntPtr PrevIssuerContext,ref Int32 Flags);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern unsafe HCERT_SERVER_OCSP_RESPONSE CertAddRefServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern unsafe HCERT_SERVER_OCSP_RESPONSE CertOpenServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern unsafe CERT_SERVER_OCSP_RESPONSE_CONTEXT* CertGetServerOcspResponseContext(HCERT_SERVER_OCSP_RESPONSE ServerOcspResponse,Int32 Flags,IntPtr Reserved);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern unsafe IntPtr CertCreateSelfSignCertificate(HCRYPTPROV_OR_NCRYPT_KEY_HANDLE CryptProvOrNCryptKey,ref CERT_NAME_BLOB SubjectIssuerBlob,Int32 Flags,CRYPT_KEY_PROV_INFO* KeyProvInfo,CRYPT_ALGORITHM_IDENTIFIER* SignatureAlgorithm,SYSTEMTIME* StartTime,SYSTEMTIME* EndTime,CERT_EXTENSIONS* Extensions);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern unsafe IntPtr CertGetSubjectCertificateFromStore(HCERTSTORE CertStore,Int32 CertEncodingType,CERT_INFO* CertId);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern unsafe Boolean CertGetValidUsages(Int32 cCerts,CERT_CONTEXT* rghCerts,Int32* cNumOIDs,IntPtr rghOIDs,Int32* pcbOIDs);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern unsafe Boolean CertSelectCertificateChains(ref Guid SelectionContext,Int32 Flags,CERT_SELECT_CHAIN_PARA* ChainParameters,Int32 cCriteria,CERT_SELECT_CRITERIA* rgpCriteria,HCERTSTORE Store,out Int32 pcSelection,out CERT_CHAIN_CONTEXT* pprgpSelection);
        #endregion
        #region Data Conversion Functions
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern ALG_ID CertOIDToAlgId([MarshalAs(UnmanagedType.LPStr)] String Id);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern IntPtr CertAlgIdToOID(ALG_ID Id);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern Int32 CertNameToStrA(Int32 CertEncodingType, ref CERT_NAME_BLOB Name, Int32 StrType, IntPtr psz, Int32 csz);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern Int32 CertNameToStrW(Int32 CertEncodingType, ref CERT_NAME_BLOB Name, Int32 StrType, IntPtr psz, Int32 csz);
        [DllImport("libcrypt32", CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CertStrToNameA(Int32 CertEncodingType,[MarshalAs(UnmanagedType.LPStr)] String Name,Int32 StrType,IntPtr Reserved,[MarshalAs(UnmanagedType.LPArray)] Byte[] EncodedBytes,ref Int32 EncodedLength,IntPtr Error);
        #endregion

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
        }
    }
