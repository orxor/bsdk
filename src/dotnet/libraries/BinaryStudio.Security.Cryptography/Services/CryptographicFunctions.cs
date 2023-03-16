using System;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

namespace BinaryStudio.Security.Cryptography
    {
    using CERT_NAME_BLOB = CRYPT_BLOB;
    using CRYPT_DATA_BLOB = CRYPT_BLOB;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean PFN_CERT_ENUM_SYSTEM_STORE_LOCATION([MarshalAs(UnmanagedType.LPWStr)] String Name,CERT_SYSTEM_STORE_FLAGS Flags,IntPtr Reserved,IntPtr Arg);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean CertEnumSystemStoreCallbackIntPtr(IntPtr SystemStore, CERT_SYSTEM_STORE_FLAGS Flags, ref CERT_SYSTEM_STORE_INFO StoreInfo, IntPtr Reserved, IntPtr Arg);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean CertEnumSystemStoreCallbackString(String SystemStoreName, CERT_SYSTEM_STORE_FLAGS Flags, ref CERT_SYSTEM_STORE_INFO StoreInfo, IntPtr Reserved, IntPtr Arg);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean PFN_CERT_ENUM_PHYSICAL_STORE(IntPtr SystemStore, CERT_SYSTEM_STORE_FLAGS Flags, [MarshalAs(UnmanagedType.LPWStr)] String Name, ref CERT_PHYSICAL_STORE_INFO StoreInfo, IntPtr Reserved, IntPtr Arg);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean CryptEnumOidInfoCallback(IntPtr Info,IntPtr Arg);

    internal interface CryptographicFunctions : ILastErrorProvider
        {
        Encoding UnicodeEncoding { get; }
        ALG_ID CertOIDToAlgId(String Id);
        Boolean CertAddCertificateContextToStore(IntPtr CertStore,IntPtr CertContext,CERT_STORE_ADD AddDisposition,IntPtr Zero);
        Boolean CertAddCertificateContextToStore(IntPtr CertStore,IntPtr CertContext,CERT_STORE_ADD AddDisposition,out IntPtr StoreContext);
        Boolean CertAddCertificateLinkToStore(IntPtr CertStore,IntPtr CertContext,Int32 AddDisposition,out IntPtr StoreContext);
        Boolean CertAddCRLContextToStore(IntPtr Store,IntPtr Context,CERT_STORE_ADD Disposition, IntPtr Zero);
        Boolean CertAddCRLContextToStore(IntPtr Store,IntPtr Context,CERT_STORE_ADD Disposition, out IntPtr StoreContext);
        Boolean CertAddEncodedCertificateToStore(IntPtr CertStore,Int32 CertEncodingType,Byte[] CertEncodedData,Int32 CertEncodedLength,Int32 AddDisposition,out IntPtr CertContext);
        Boolean CertCloseStore(IntPtr handle, UInt32 Flags);
        Boolean CertDeleteCertificateFromStore(IntPtr CertContext);
        Boolean CertDeleteCRLFromStore(IntPtr Context);
        Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS Flags, IntPtr SystemStoreLocationPara, IntPtr Arg, CertEnumSystemStoreCallbackString Callback);
        Boolean CertExportCertStore(IntPtr Store,ref CRYPT_DATA_BLOB PFX,IntPtr Password,IntPtr Para,Int32 Flags);
        Boolean CertFreeCertificateContext(IntPtr CertContext);
        Boolean CertGetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Byte[] Data, ref Int32 Size);
        Boolean CertRetrieveLogoOrBiometricInfo(IntPtr CertContext,String LogoOrBiometricType,Int32 RetrievalFlags,Int32 Timeout,Int32 Flags,IntPtr Reserved,out IntPtr ppbData,out Int32 pcbData,out IntPtr ppwszMimeType);
        Boolean CertSerializeCertificateStoreElement(IntPtr CertContext,Int32 Flags,Byte[] pbElement,ref Int32 pcbElement);
        Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, IntPtr Data);
        Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, ref CRYPT_KEY_PROV_INFO Data);
        Boolean CertStrToName(Int32 CertEncodingType,String Name,Int32 StrType,IntPtr Reserved,Byte[] EncodedBytes,ref Int32 EncodedLength,IntPtr Error);
        Boolean CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus);
        Boolean CertVerifySubjectCertificateContext(IntPtr Subject,IntPtr Issuer,ref Int32 Flags);
        Boolean CryptAcquireCertificatePrivateKey(IntPtr Certificate, CRYPT_ACQUIRE_FLAGS Flags, IntPtr Parameters,out IntPtr CryptProvOrNCryptKey, out KEY_SPEC_TYPE KeySpec, out Boolean CallerFreeProvOrNCryptKey);
        Boolean CryptAcquireContext(out IntPtr CryptProv, String Container, String Provider, Int32 ProvType, Int32 Flags);
        Boolean CryptCreateHash(IntPtr Provider, ALG_ID Algorithm, IntPtr Key, out IntPtr Handle);
        Boolean CryptDeriveKey(IntPtr Context,ALG_ID AlgId,Int32 Flags,out IntPtr r);
        Boolean CryptDestroyHash(IntPtr Handle);
        Boolean CryptDestroyKey(IntPtr Key);
        Boolean CryptDuplicateKey(IntPtr Key,IntPtr Reserved,Int32 Flags,out IntPtr r);
        Boolean CryptEnumOIDInfo(CRYPT_ALG_OID_GROUP_ID GroupId,IntPtr Arg,CryptEnumOidInfoCallback Callback);
        Boolean CryptEnumProviders(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, StringBuilder name, ref Int32 sz);
        Boolean CryptEnumProviderTypes(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, StringBuilder name, ref Int32 sz);
        Boolean CryptExportKey(IntPtr Key,IntPtr ExpKey,Int32 BlobType,Int32 Flags, Byte[] Data,ref Int32 DataLen);
        Boolean CryptGenKey(IntPtr Context,ALG_ID AlgId,Int32 Flags,out IntPtr r);
        Boolean CryptGenRandom(IntPtr Context,Int32 Length,Byte[] Buffer);
        Boolean CryptGetHashParam(IntPtr Handle, Int32 Parameter, Byte[] Block, ref Int32 BlockSize);
        Boolean CryptGetHashParam(IntPtr Handle, Int32 Parameter, out Int32 Block, ref Int32 BlockSize);
        Boolean CryptGetKeyParam(IntPtr Key,KEY_PARAM Param,Byte[] Data,ref Int32 DataLen,Int32 Flags);
        Boolean CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,Byte[] Data,ref Int32 DataSize,Int32 Flags);
        Boolean CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,ref Int32 DataSize,Int32 Flags);
        Boolean CryptGetUserKey(IntPtr Context,KEY_SPEC_TYPE KeySpec,out IntPtr UserKey);
        Boolean CryptHashData(IntPtr Handle, Byte[] Data, Int32 DataSize);
        Boolean CryptImportKey(IntPtr Context,Byte[] Data,Int32 DataLen,IntPtr PubKey,Int32 Flags,out IntPtr r);
        Boolean CryptMsgClose(IntPtr Message);
        Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, IntPtr CtrlPara);
        Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, ref CMSG_CTRL_DECRYPT_PARA CtrlPara);
        Boolean CryptMsgGetParam(IntPtr Message, CMSG_PARAM Parameter, Int32 SignerIndex,Byte[] Data, ref Int32 Size);
        Boolean CryptMsgUpdate(IntPtr Message, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, Int32 Size, Boolean Final);
        Boolean CryptMsgUpdate(IntPtr Message, IntPtr Data, Int32 Size, Boolean Final);
        Boolean CryptSetKeyParam(IntPtr Key,KEY_PARAM Param,Byte[] Data,Int32 Flags);
        Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,Byte[] Data,Int32 Flags);
        Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,Int32 Flags);
        Boolean CryptSignHash(IntPtr Handle, KEY_SPEC_TYPE KeySpec, Byte[] Signature, ref Int32 Length);
        Boolean CryptVerifySignature(IntPtr Handle, Byte[] Signature, Int32 SignatureSize, IntPtr Key);
        Int32 CertNameToStrA(Int32 CertEncodingType,ref CRYPT_BLOB Name,Int32 StrType,IntPtr psz,Int32 csz);
        Int32 CertNameToStrW(Int32 CertEncodingType,ref CRYPT_BLOB Name,Int32 StrType,IntPtr psz,Int32 csz);
        IntPtr CertAlgIdToOID(ALG_ID Id);
        IntPtr CertCreateCertificateContext(Int32 CertEncodingType,Byte[] CertEncodedBytes,Int32 CertEncodedLength);
        IntPtr CertCreateCRLContext(Int32 CertEncodingType,Byte[] CrlEncodedBytes,Int32 CrlEncodedLength);
        IntPtr CertDuplicateCertificateContext(IntPtr CertContext);
        IntPtr CertDuplicateCRLContext(IntPtr Context);
        IntPtr CertEnumCertificatesInStore(IntPtr CertStore,IntPtr PrevCertContext);
        IntPtr CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext);
        IntPtr CertFindCertificateInStore(IntPtr CertStore,Int32 CertEncodingType,Int32 FindFlags,Int32 FindType,IntPtr FindPara,IntPtr PrevCertContext);
        IntPtr CertGetIssuerCertificateFromStore(IntPtr CertStore,IntPtr SubjectContext,IntPtr PrevIssuerContext,ref Int32 Flags);
        IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, IntPtr Para);
        IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, String Para);
        IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, IntPtr si);
        IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, ref CMSG_STREAM_INFO si);
        unsafe Boolean CertGetCertificateChain(IntPtr ChainEngine, IntPtr Context, ref FILETIME time, IntPtr AdditionalStore, ref CERT_CHAIN_PARA ChainPara, CERT_CHAIN_FLAGS Flags, IntPtr Reserved, CERT_CHAIN_CONTEXT** ChainContext);
        unsafe Boolean CertGetValidUsages(Int32 cCerts,CERT_CONTEXT* rghCerts,Int32* cNumOIDs,IntPtr rghOIDs,Int32* pcbOIDs);
        unsafe Boolean CertSelectCertificateChains(ref Guid SelectionContext,Int32 Flags,CERT_SELECT_CHAIN_PARA* ChainParameters,Int32 cCriteria,CERT_SELECT_CRITERIA* rgpCriteria,IntPtr Store,out Int32 pcSelection,out CERT_CHAIN_CONTEXT* pprgpSelection);
        unsafe CERT_SERVER_OCSP_RESPONSE_CONTEXT* CertGetServerOcspResponseContext(IntPtr ServerOcspResponse,Int32 Flags,IntPtr Reserved);
        unsafe IntPtr CertAddRefServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara);
        unsafe IntPtr CertCreateSelfSignCertificate(IntPtr CryptProvOrNCryptKey,ref CERT_NAME_BLOB SubjectIssuerBlob,Int32 Flags,CRYPT_KEY_PROV_INFO* KeyProvInfo,CRYPT_ALGORITHM_IDENTIFIER* SignatureAlgorithm,SYSTEMTIME* StartTime,SYSTEMTIME* EndTime,CERT_EXTENSIONS* Extensions);
        unsafe IntPtr CertGetSubjectCertificateFromStore(IntPtr CertStore,Int32 CertEncodingType,CERT_INFO* CertId);
        unsafe IntPtr CertOpenServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara);
        unsafe IntPtr CryptFindOIDInfo(CRYPT_OID_INFO_KEY_TYPE KeyType,void* Key,Int32 GroupId);
        void CertAddRefServerOcspResponseContext(IntPtr ServerOcspResponseContext);
        void CertCloseServerOcspResponse(IntPtr ServerOcspResponse,Int32 Flags);
        }
    }
