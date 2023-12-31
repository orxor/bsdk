﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Services;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

// ReSharper disable ParameterHidesMember

namespace BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    using CERT_NAME_BLOB = CRYPT_BLOB;
    using CRYPT_DATA_BLOB=CRYPT_BLOB;

    internal class CryptographicContextP : CryptographicContext, CryptographicFunctions
        {
        public override IntPtr Handle { get { return IntPtr.Zero; }}
        public Encoding UnicodeEncoding { get { return Encoding.UTF32; }}
        public override CRYPT_PROVIDER_TYPE ProviderType { get { return CRYPT_PROVIDER_TYPE.VPN_PROV_TYPE_2012_1024; }}

        Int32 LastErrorService.GetLastError() { return GetLastWin32Error(); }
        ALG_ID CryptographicFunctions.CertOIDToAlgId(String Id) { return CertOIDToAlgId(Id); }
        Boolean CryptographicFunctions.CertAddCertificateContextToStore(IntPtr Store,IntPtr InputContext,CERT_STORE_ADD Disposition) { return CertAddCertificateContextToStore(Store,InputContext,Disposition,IntPtr.Zero); }
        Boolean CryptographicFunctions.CertAddCertificateContextToStore(IntPtr Store,IntPtr InputContext,CERT_STORE_ADD Disposition,out IntPtr OutputContext) { return CertAddCertificateContextToStore(Store,InputContext,Disposition,out OutputContext); }
        Boolean CryptographicFunctions.CertAddCertificateLinkToStore(IntPtr Store,IntPtr CertContext,Int32 AddDisposition,out IntPtr OutputContext) { return CertAddCertificateLinkToStore(Store,CertContext,AddDisposition,out OutputContext); }
        Boolean CryptographicFunctions.CertAddCRLContextToStore(IntPtr Store,IntPtr Context,CERT_STORE_ADD Disposition) { return CertAddCRLContextToStore(Store,Context,Disposition,IntPtr.Zero); }
        Boolean CryptographicFunctions.CertAddCRLContextToStore(IntPtr Store,IntPtr Context,CERT_STORE_ADD Disposition, out IntPtr StoreContext) { return CertAddCRLContextToStore(Store,Context,Disposition,out StoreContext); }
        Boolean CryptographicFunctions.CertAddEncodedCertificateToStore(IntPtr CertStore,Int32 CertEncodingType,Byte[] CertEncodedData,Int32 CertEncodedLength,Int32 AddDisposition,out IntPtr CertContext) { return CertAddEncodedCertificateToStore(CertStore,CertEncodingType,CertEncodedData,CertEncodedLength,AddDisposition,out CertContext); }
        Boolean CryptographicFunctions.CertCloseStore(IntPtr handle, Int32 flags) { return CertCloseStore(handle, flags); }
        Boolean CryptographicFunctions.CertDeleteCertificateFromStore(IntPtr CertContext) { return CertDeleteCertificateFromStore(CertContext); }
        Boolean CryptographicFunctions.CertDeleteCRLFromStore(IntPtr Context) { return CertDeleteCRLFromStore(Context); }
        Boolean CryptographicFunctions.CertExportCertStore(IntPtr Store,ref CRYPT_DATA_BLOB PFX,IntPtr Password,IntPtr Para,Int32 Flags){ throw new NotSupportedException(); }
        Boolean CryptographicFunctions.CertExportCertStore(IntPtr Store,ref CRYPT_DATA_BLOB PFX,IntPtr Password,Int32 Flags){ throw new NotSupportedException(); }
        Boolean CryptographicFunctions.CertFreeCertificateContext(IntPtr CertContext) { return CertFreeCertificateContext(CertContext); }
        Boolean CryptographicFunctions.CertFreeCRLContext(IntPtr CrlContext) { return CertFreeCRLContext(CrlContext); }
        Boolean CryptographicFunctions.CertGetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Byte[] Data, ref Int32 Size) { return CertGetCertificateContextProperty(Context,PropertyIndex,Data,ref Size); }
        Boolean CryptographicFunctions.CertRetrieveLogoOrBiometricInfo(IntPtr CertContext,String LogoOrBiometricType,Int32 RetrievalFlags,Int32 Timeout,Int32 Flags,IntPtr Reserved,out IntPtr ppbData,out Int32 pcbData,out IntPtr ppwszMimeType) { return CertRetrieveLogoOrBiometricInfo(CertContext,LogoOrBiometricType,RetrievalFlags,Timeout,Flags,Reserved,out ppbData,out pcbData,out ppwszMimeType); }
        Boolean CryptographicFunctions.CertSerializeCertificateStoreElement(IntPtr CertContext,Int32 Flags,Byte[] pbElement,ref Int32 pcbElement) { return CertSerializeCertificateStoreElement(CertContext,Flags,pbElement,ref pcbElement); }
        Boolean CryptographicFunctions.CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, IntPtr Data) { return CertSetCertificateContextProperty(Context,PropertyIndex,Flags,Data); }
        Boolean CryptographicFunctions.CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, ref CRYPT_KEY_PROV_INFO Data) { return CertSetCertificateContextProperty(Context,PropertyIndex,Flags,ref Data); }
        Boolean CryptographicFunctions.CertStrToName(Int32 CertEncodingType,String Name,Int32 StrType,IntPtr Reserved,Byte[] EncodedBytes,ref Int32 EncodedLength,IntPtr Error) { return CertStrToNameA(CertEncodingType,Name,StrType,Reserved,EncodedBytes,ref EncodedLength,Error); }
        Boolean CryptographicFunctions.CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus) { return CertVerifyCertificateChainPolicy(Policy,ChainContext,ref PolicyPara,ref PolicyStatus); }
        Boolean CryptographicFunctions.CertVerifySubjectCertificateContext(IntPtr Subject,IntPtr Issuer,ref Int32 Flags) { return CertVerifySubjectCertificateContext(Subject,Issuer,ref Flags); }
        Boolean CryptographicFunctions.CryptAcquireCertificatePrivateKey(IntPtr Certificate, CRYPT_ACQUIRE_FLAGS Flags, IntPtr Parameters,out IntPtr CryptProvOrNCryptKey, out KEY_SPEC_TYPE KeySpec, out Boolean CallerFreeProvOrNCryptKey) { return CryptAcquireCertificatePrivateKey(Certificate,Flags,Parameters,out CryptProvOrNCryptKey,out KeySpec,out CallerFreeProvOrNCryptKey); }
        Boolean CryptographicFunctions.CryptAcquireContext(out IntPtr CryptProv, String Container, String Provider, Int32 ProvType, Int32 Flags) { return CryptAcquireContext(out CryptProv,Container,Provider,ProvType,Flags); }
        Boolean CryptographicFunctions.CryptCreateHash(IntPtr Provider, ALG_ID Algorithm, IntPtr Key, out IntPtr Handle) { return CryptCreateHash(Provider,Algorithm,Key,0,out Handle); }
        Boolean CryptographicFunctions.CryptDeriveKey(IntPtr Context,ALG_ID AlgId,IntPtr BaseData,Int32 Flags,out IntPtr Key) { return CryptDeriveKey(Context,AlgId,BaseData,Flags,out Key); }
        Boolean CryptographicFunctions.CryptDestroyHash(IntPtr Handle) { return CryptDestroyHash(Handle); }
        Boolean CryptographicFunctions.CryptDestroyKey(IntPtr Key) { return CryptDestroyKey(Key); }
        Boolean CryptographicFunctions.CryptDuplicateHash(IntPtr Hash,out IntPtr Output) { return CryptDuplicateHash(Hash,IntPtr.Zero,0,out Output); }
        Boolean CryptographicFunctions.CryptDuplicateKey(IntPtr Key,out IntPtr Output) { return CryptDuplicateKey(Key,IntPtr.Zero,0,out Output); }
        Boolean CryptographicFunctions.CryptEnumOIDInfo(CRYPT_ALG_OID_GROUP_ID GroupId,IntPtr Arg,CryptEnumOidInfoCallback Callback) { return CryptEnumOIDInfo((Int32)GroupId,0,Arg,Callback); }
        Boolean CryptographicFunctions.CryptEnumProviders(Int32 Index,out Int32 Type, StringBuilder Name,ref Int32 Size) { return CryptEnumProviders(Index,IntPtr.Zero,0,out Type,Name,ref Size); }
        Boolean CryptographicFunctions.CryptEnumProviderTypes(Int32 Index,out Int32 Type,StringBuilder Name,ref Int32 Size) { return CryptEnumProviderTypes(Index,IntPtr.Zero,0,out Type,Name,ref Size); }
        Boolean CryptographicFunctions.CryptExportKey(IntPtr Key,IntPtr ExpKey,Int32 BlobType,Int32 Flags,Byte[] Data,ref Int32 DataLen) { return CryptExportKey(Key,ExpKey,BlobType,Flags,Data,ref DataLen); }
        Boolean CryptographicFunctions.CryptGenKey(IntPtr Context,ALG_ID AlgId,Int32 Flags,out IntPtr r) { return CryptGenKey(Context,AlgId,Flags,out r); }
        Boolean CryptographicFunctions.CryptGenRandom(IntPtr Context,Int32 Length,Byte[] Buffer) { return CryptGenRandom(Context,Length,Buffer); }
        Boolean CryptographicFunctions.CryptGetHashParam(IntPtr Handle, Int32 Parameter, Byte[] Block, ref Int32 BlockSize) { return CryptGetHashParam(Handle,Parameter,Block,ref BlockSize,0); }
        Boolean CryptographicFunctions.CryptGetKeyParam(IntPtr Key,KEY_PARAM Param,Byte[] Data,ref Int32 DataSize, Int32 Flags) { return CryptGetKeyParam(Key,Param,Data,ref DataSize,Flags); }
        Boolean CryptographicFunctions.CryptGetKeyParam(IntPtr Key,KEY_PARAM Param,IntPtr Data,ref Int32 DataSize, Int32 Flags) { return CryptGetKeyParam(Key,Param,Data,ref DataSize,Flags); }
        Boolean CryptographicFunctions.CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,Byte[] Data,ref Int32 DataSize,Int32 Flags) { return CryptGetProvParam(Context,Parameter,Data,ref DataSize,Flags); }
        Boolean CryptographicFunctions.CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,ref Int32 DataSize,Int32 Flags) { return CryptGetProvParam(Context,Parameter,Data,ref DataSize,Flags); }
        Boolean CryptographicFunctions.CryptGetUserKey(IntPtr Context,KEY_SPEC_TYPE KeySpec,out IntPtr r) { return CryptGetUserKey(Context,KeySpec,out r); }
        Boolean CryptographicFunctions.CryptHashData(IntPtr Handle, Byte[] Data, Int32 DataSize) { return CryptHashData(Handle,Data,DataSize,0); }
        Boolean CryptographicFunctions.CryptImportKey(IntPtr Context,Byte[] Data,Int32 DataLen,IntPtr PubKey,Int32 Flags,out IntPtr r) { return CryptImportKey(Context,Data,DataLen,PubKey,Flags,out r); }
        Boolean CryptographicFunctions.CryptMsgClose(IntPtr Message) { return CryptMsgClose(Message); }
        Boolean CryptographicFunctions.CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, IntPtr CtrlPara) { return CryptMsgControl(Message,Flags,CtrlType,CtrlPara); }
        Boolean CryptographicFunctions.CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, ref CMSG_CTRL_DECRYPT_PARA CtrlPara) { return CryptMsgControl(Message,Flags,CtrlType,ref CtrlPara); }
        Boolean CryptographicFunctions.CryptMsgGetParam(IntPtr Message, CMSG_PARAM Parameter, Int32 SignerIndex, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, ref Int32 Size) { return CryptMsgGetParam(Message,Parameter,SignerIndex,Data,ref Size); }
        Boolean CryptographicFunctions.CryptMsgUpdate(IntPtr Message, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, Int32 Size, Boolean Final) { return CryptMsgUpdate(Message,Data,Size,Final); }
        Boolean CryptographicFunctions.CryptMsgUpdate(IntPtr Message, IntPtr Data, Int32 Size, Boolean Final) { return CryptMsgUpdate(Message,Data,Size,Final); }
        Boolean CryptographicFunctions.CryptSetHashParam(IntPtr Hash,Int32 Param,Byte[] Data,Int32 Flags) { return CryptSetHashParam(Hash,Param,Data,Flags); }
        Boolean CryptographicFunctions.CryptSetHashParam(IntPtr Hash,Int32 Param,ref CRYPT_DATA_BLOB Data,Int32 Flags) { return CryptSetHashParam(Hash,Param,ref Data,Flags); }
        Boolean CryptographicFunctions.CryptSetKeyParam(IntPtr Key,KEY_PARAM Param,Byte[] Data,Int32 Flags) { return CryptSetKeyParam(Key,Param,Data,Flags); }
        Boolean CryptographicFunctions.CryptSetKeyParam(IntPtr Key,KEY_PARAM Param,IntPtr Data,Int32 Flags) { return CryptSetKeyParam(Key,Param,Data,Flags); }
        Boolean CryptographicFunctions.CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,Byte[] Data,Int32 Flags) { return CryptSetProvParam(Context,Parameter,Data,Flags); }
        Boolean CryptographicFunctions.CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,Int32 Flags) { return CryptSetProvParam(Context,Parameter,Data,Flags); }
        Boolean CryptographicFunctions.CryptSignHash(IntPtr Handle, KEY_SPEC_TYPE KeySpec, Byte[] Signature, ref Int32 Length) { return CryptSignHash(Handle,KeySpec,IntPtr.Zero,0,Signature,ref Length); }
        Boolean CryptographicFunctions.CryptVerifyCertificateSignature(IntPtr Context,Int32 SubjectType,IntPtr Subject,Int32 IssuerType,IntPtr Issuer,Int32 Flags) { return CryptVerifyCertificateSignatureEx(Context,X509_ASN_ENCODING,SubjectType,Subject,IssuerType,Issuer,Flags,IntPtr.Zero); }
        Boolean CryptographicFunctions.CryptVerifySignature(IntPtr Handle, Byte[] Signature, Int32 SignatureSize, IntPtr Key) { return CryptVerifySignature(Handle,Signature,SignatureSize,Key,IntPtr.Zero,0); }
        Int32 CryptographicFunctions.CertNameToStrA(ref CERT_NAME_BLOB Name, Int32 StrType, IntPtr psz, Int32 csz) { return CertNameToStrA(X509_ASN_ENCODING,ref Name,StrType,psz,csz); }
        Int32 CryptographicFunctions.CertNameToStrW(ref CERT_NAME_BLOB Name, Int32 StrType, IntPtr psz, Int32 csz) { return CertNameToStrW(X509_ASN_ENCODING,ref Name,StrType,psz,csz); }
        IntPtr CryptographicFunctions.CertAlgIdToOID(ALG_ID Id) { return CertAlgIdToOID(Id); }
        IntPtr CryptographicFunctions.CertCreateCertificateContext(Byte[] Source) { return CertCreateCertificateContext(X509_ASN_ENCODING|PKCS_7_ASN_ENCODING,Source,Source.Length); }
        IntPtr CryptographicFunctions.CertCreateCRLContext(Byte[] Source) { return CertCreateCRLContext(X509_ASN_ENCODING|PKCS_7_ASN_ENCODING,Source,Source.Length); }
        IntPtr CryptographicFunctions.CertDuplicateCertificateContext(IntPtr CertContext) { return CertDuplicateCertificateContext(CertContext); }
        IntPtr CryptographicFunctions.CertDuplicateCRLContext(IntPtr Context) { return CertDuplicateCRLContext(Context); }
        IntPtr CryptographicFunctions.CertEnumCertificatesInStore(IntPtr CertStore,IntPtr PrevCertContext) { return CertEnumCertificatesInStore(CertStore,PrevCertContext); }
        IntPtr CryptographicFunctions.CertEnumCRLsInStore(IntPtr CertStore,IntPtr PrevCrlContext) { return CertEnumCRLsInStore(CertStore,PrevCrlContext); }
        IntPtr CryptographicFunctions.CertFindCertificateInStore(IntPtr CertStore,Int32 FindFlags,Int32 FindType,IntPtr FindPara,IntPtr PrevCertContext) { return CertFindCertificateInStore(CertStore,X509_ASN_ENCODING|PKCS_7_ASN_ENCODING,FindFlags,FindType,FindPara,PrevCertContext); }
        IntPtr CryptographicFunctions.CertGetIssuerCertificateFromStore(IntPtr CertStore,IntPtr SubjectContext,IntPtr PrevIssuerContext,ref Int32 Flags) { return CertGetIssuerCertificateFromStore(CertStore,SubjectContext,PrevIssuerContext,ref Flags); }
        IntPtr CryptographicFunctions.CertOpenStore(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, IntPtr Para) { return CertOpenStoreA(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para); }
        IntPtr CryptographicFunctions.CertOpenStore(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, String Para) { return CertOpenStoreA(StoreProvider, MsgAndCertEncodingType,CryptProv,Flags,Para); }
        IntPtr CryptographicFunctions.CryptMsgOpenToDecode(CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,IntPtr CryptProv,IntPtr StreamInfo) { return CryptMsgOpenToDecode(X509_ASN_ENCODING|PKCS_7_ASN_ENCODING,Flags,Type,CryptProv,IntPtr.Zero,StreamInfo); }
        IntPtr CryptographicFunctions.CryptMsgOpenToDecode(CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,IntPtr CryptProv,ref CMSG_STREAM_INFO StreamInfo) { return CryptMsgOpenToDecode(X509_ASN_ENCODING|PKCS_7_ASN_ENCODING,Flags,Type,CryptProv,IntPtr.Zero,ref StreamInfo); }
        IntPtr CryptographicFunctions.CryptMsgOpenToEncode(CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,ref CMSG_ENVELOPED_ENCODE_INFO EncodeInfo,ref CMSG_STREAM_INFO StreamInfo) { return CryptMsgOpenToEncode(X509_ASN_ENCODING|PKCS_7_ASN_ENCODING,Flags,Type,ref EncodeInfo,IntPtr.Zero,ref StreamInfo); }
        unsafe Boolean CryptographicFunctions.CertGetCertificateChain(IntPtr ChainEngine,IntPtr Context,ref FILETIME Time,IntPtr AdditionalStore,ref CERT_CHAIN_PARA ChainPara,CERT_CHAIN_FLAGS Flags,CERT_CHAIN_CONTEXT** ChainContext) { return CertGetCertificateChain(ChainEngine,Context,ref Time,AdditionalStore,ref ChainPara,Flags,IntPtr.Zero,ChainContext); }
        unsafe Boolean CryptographicFunctions.CertGetValidUsages(Int32 cCerts,CERT_CONTEXT* rghCerts,Int32* cNumOIDs,IntPtr rghOIDs,Int32* pcbOIDs) { return CertGetValidUsages(cCerts,rghCerts,cNumOIDs,rghOIDs,pcbOIDs); }
        unsafe IntPtr CryptographicFunctions.CertCreateSelfSignCertificate(IntPtr CryptProvOrNCryptKey,ref CERT_NAME_BLOB SubjectIssuerBlob,Int32 Flags,CRYPT_KEY_PROV_INFO* KeyProvInfo,CRYPT_ALGORITHM_IDENTIFIER* SignatureAlgorithm,SYSTEMTIME* StartTime,SYSTEMTIME* EndTime,CERT_EXTENSIONS* Extensions) { return CertCreateSelfSignCertificate(CryptProvOrNCryptKey,ref SubjectIssuerBlob,Flags,KeyProvInfo,SignatureAlgorithm,StartTime,EndTime,Extensions); }
        unsafe IntPtr CryptographicFunctions.CertGetSubjectCertificateFromStore(IntPtr Store,Int32 EncodingType,CERT_INFO* CertId) { return CertGetSubjectCertificateFromStore(Store,EncodingType,CertId); }
        unsafe IntPtr CryptographicFunctions.CryptFindOIDInfo(CRYPT_OID_INFO_KEY_TYPE KeyType,void* Key,Int32 GroupId) { return CryptFindOIDInfo((Int32)KeyType,Key,GroupId); }

        [DllImport("libadvapi32", SetLastError = true)] private static extern Boolean CertAddCRLContextToStore(IntPtr Store,IntPtr Context,CERT_STORE_ADD Disposition, IntPtr Zero);
        [DllImport("libadvapi32", SetLastError = true)] private static extern Boolean CertAddCRLContextToStore(IntPtr Store,IntPtr Context,CERT_STORE_ADD Disposition, out IntPtr StoreContext);
        [DllImport("libadvapi32", SetLastError = true)] private static extern Boolean CertDeleteCRLFromStore(IntPtr Context);
        [DllImport("libadvapi32", SetLastError = true)] private static extern Boolean CryptCreateHash(IntPtr Provider, ALG_ID Algorithm, IntPtr Key, Int32 Flags, out IntPtr Handle);
        [DllImport("libadvapi32", SetLastError = true)] private static extern Boolean CryptDestroyHash(IntPtr Handle);
        [DllImport("libadvapi32", SetLastError = true)] private static extern Boolean CryptGetHashParam(IntPtr Handle, Int32 Parameter, [MarshalAs(UnmanagedType.LPArray)] Byte[] Block, ref Int32 BlockSize, Int32 Flags);
        [DllImport("libadvapi32", SetLastError = true)] private static extern Boolean CryptGetHashParam(IntPtr Handle, Int32 Parameter, out Int32 Block, ref Int32 BlockSize, Int32 Flags);
        [DllImport("libadvapi32", SetLastError = true)] private static extern Boolean CryptHashData(IntPtr Handle, [MarshalAs(UnmanagedType.LPArray)]Byte[] Data, Int32 DataSize, Int32 Flags);
        [DllImport("libadvapi32", SetLastError = true)] private static extern IntPtr CertCreateCRLContext(Int32 CertEncodingType,[MarshalAs(UnmanagedType.LPArray)] Byte[] CrlEncodedBytes,Int32 CrlEncodedLength);
        [DllImport("libadvapi32", SetLastError = true)] private static extern IntPtr CertDuplicateCRLContext(IntPtr Context);
        [DllImport("libadvapi32", SetLastError = true, CharSet = CharSet.Auto)] private static extern Boolean CryptSignHash(IntPtr Handle, KEY_SPEC_TYPE KeySpec, IntPtr Sescription, Int32 Flags, [MarshalAs(UnmanagedType.LPArray)] Byte[] Signature, ref Int32 Length);
        [DllImport("libadvapi32", SetLastError = true, CharSet = CharSet.Auto)] private static extern Boolean CryptVerifySignature(IntPtr Handle, [MarshalAs(UnmanagedType.LPArray)] Byte[] Signature, Int32 SignatureSize, IntPtr Key, IntPtr Description, Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private new static extern Boolean CryptGetKeyParam(IntPtr Key,KEY_PARAM Param,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,ref Int32 DataLen,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private new static extern Boolean CryptGetKeyParam(IntPtr Key,KEY_PARAM Param,IntPtr Data,ref Int32 DataLen,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern ALG_ID CertOIDToAlgId([MarshalAs(UnmanagedType.LPStr)] String Id);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertAddCertificateContextToStore(IntPtr Store,IntPtr InputContext,CERT_STORE_ADD Disposition,IntPtr Zero);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertAddCertificateContextToStore(IntPtr Store,IntPtr InputContext,CERT_STORE_ADD Disposition,out IntPtr OutputContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertAddCertificateLinkToStore(IntPtr Store,IntPtr CertContext,Int32 AddDisposition,out IntPtr OutputContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertAddEncodedCertificateToStore(IntPtr CertStore,Int32 CertEncodingType,[MarshalAs(UnmanagedType.LPArray)] Byte[] CertEncodedData,Int32 CertEncodedLength,Int32 AddDisposition,out IntPtr CertContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertCloseStore(IntPtr handle,Int32 flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertDeleteCertificateFromStore(IntPtr CertContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertFreeCertificateContext(IntPtr CertContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertFreeCRLContext(IntPtr CrlContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertGetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, [MarshalAs(UnmanagedType.LPArray)]Byte[] Data, ref Int32 Size);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertRetrieveLogoOrBiometricInfo(IntPtr CertContext,String LogoOrBiometricType,Int32 RetrievalFlags,Int32 Timeout,Int32 Flags,IntPtr Reserved,out IntPtr ppbData,out Int32 pcbData,out IntPtr ppwszMimeType);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertSerializeCertificateStoreElement(IntPtr CertContext,Int32 Flags,[MarshalAs(UnmanagedType.LPArray)]Byte[] pbElement,ref Int32 pcbElement);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, IntPtr Data);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, ref CRYPT_KEY_PROV_INFO Data);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertStrToNameA(Int32 CertEncodingType,[MarshalAs(UnmanagedType.LPStr)] String Name,Int32 StrType,IntPtr Reserved,[MarshalAs(UnmanagedType.LPArray)] Byte[] EncodedBytes,ref Int32 EncodedLength,IntPtr Error);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CertVerifySubjectCertificateContext(IntPtr Subject,IntPtr Issuer,ref Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptAcquireCertificatePrivateKey(IntPtr Certificate, CRYPT_ACQUIRE_FLAGS Flags, IntPtr Parameters,out IntPtr CryptProvOrNCryptKey, out KEY_SPEC_TYPE KeySpec, out Boolean CallerFreeProvOrNCryptKey);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptDeriveKey(IntPtr Context,ALG_ID AlgId,IntPtr BaseData,Int32 Flags,out IntPtr Key);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptDestroyKey(IntPtr Key);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptDuplicateHash(IntPtr Hash,IntPtr Reserved,Int32 Flags,out IntPtr Output);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptDuplicateKey(IntPtr Key,IntPtr Reserved,Int32 Flags,out IntPtr Output);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptEnumOIDInfo(Int32 GroupId,Int32 Flags,IntPtr Arg,CryptEnumOidInfoCallback Callback);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptExportKey(IntPtr Key,IntPtr ExpKey,Int32 BlobType,Int32 Flags, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data,ref Int32 DataLen);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptGenKey(IntPtr Context,ALG_ID AlgId,Int32 Flags,out IntPtr r);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptGenRandom(IntPtr Context,Int32 Length,[MarshalAs(UnmanagedType.LPArray)] Byte[] Buffer);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,ref Int32 DataSize,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,ref Int32 DataSize,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptGetUserKey(IntPtr Context,KEY_SPEC_TYPE KeySpec,out IntPtr UserKey);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptImportKey(IntPtr Context,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,Int32 DataLen,IntPtr PubKey,Int32 Flags,out IntPtr r);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptMsgClose(IntPtr Message);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, IntPtr CtrlPara);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, ref CMSG_CTRL_DECRYPT_PARA CtrlPara);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptMsgGetParam(IntPtr Message, CMSG_PARAM Parameter, Int32 SignerIndex, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, ref Int32 Size);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptMsgUpdate(IntPtr Message, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, Int32 Size, Boolean Final);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptMsgUpdate(IntPtr Message, IntPtr Data, Int32 Size, Boolean Final);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptSetHashParam(IntPtr Hash,Int32 Param,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptSetHashParam(IntPtr Hash,Int32 Param,ref CRYPT_DATA_BLOB Data,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptSetKeyParam(IntPtr Key,KEY_PARAM Param,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptSetKeyParam(IntPtr Key,KEY_PARAM Param,IntPtr Data,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,[MarshalAs(UnmanagedType.LPArray)] Byte[] Data,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Boolean CryptVerifyCertificateSignatureEx(IntPtr Context,Int32 CertEncodingType,Int32 SubjectType,IntPtr Subject,Int32 IssuerType,IntPtr Issuer,Int32 Flags,IntPtr Extra);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Int32 CertNameToStrA(Int32 CertEncodingType, ref CERT_NAME_BLOB Name, Int32 StrType, IntPtr psz, Int32 csz);
        [DllImport("libcrypt32", SetLastError = true)] private static extern Int32 CertNameToStrW(Int32 CertEncodingType, ref CERT_NAME_BLOB Name, Int32 StrType, IntPtr psz, Int32 csz);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CertAlgIdToOID(ALG_ID Id);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CertCreateCertificateContext(Int32 CertEncodingType,[MarshalAs(UnmanagedType.LPArray)]Byte[] CertEncodedBytes,Int32 CertEncodedLength);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CertCreateCRLContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CertDuplicateCertificateContext(IntPtr CertContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CertEnumCertificatesInStore(IntPtr CertStore,IntPtr PrevCertContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CertFindCertificateInStore(IntPtr CertStore,Int32 CertEncodingType,Int32 FindFlags,Int32 FindType,IntPtr FindPara,IntPtr PrevCertContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CertGetIssuerCertificateFromStore(IntPtr CertStore,IntPtr SubjectContext,IntPtr PrevIssuerContext,ref Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CryptMsgOpenToDecode(Int32 EncodingType,CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,IntPtr CryptProv,IntPtr RecipientInfo,IntPtr StreamInfo);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CryptMsgOpenToDecode(Int32 EncodingType,CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,IntPtr CryptProv,IntPtr RecipientInfo,ref CMSG_STREAM_INFO StreamInfo);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CryptMsgOpenToEncode(Int32 EncodingType,CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,ref CMSG_SIGNED_ENCODE_INFO32 EncodeInfo,IntPtr InnerContentObjId,ref CMSG_STREAM_INFO StreamInfo);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CryptMsgOpenToEncode(Int32 EncodingType,CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,ref CMSG_SIGNED_ENCODE_INFO64 EncodeInfo,IntPtr InnerContentObjId,ref CMSG_STREAM_INFO StreamInfo);
        [DllImport("libcrypt32", SetLastError = true)] private static extern IntPtr CryptMsgOpenToEncode(Int32 EncodingType,CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,ref CMSG_ENVELOPED_ENCODE_INFO EncodeInfo,IntPtr InnerContentObjId,ref CMSG_STREAM_INFO StreamInfo);
        [DllImport("libcrypt32", SetLastError = true)] private static extern unsafe Boolean CertGetCertificateChain(IntPtr ChainEngine, IntPtr Context, ref FILETIME time, IntPtr AdditionalStore, ref CERT_CHAIN_PARA ChainPara, CERT_CHAIN_FLAGS Flags, IntPtr Reserved, CERT_CHAIN_CONTEXT** ChainContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern unsafe Boolean CertGetValidUsages(Int32 cCerts,CERT_CONTEXT* rghCerts,Int32* cNumOIDs,IntPtr rghOIDs,Int32* pcbOIDs);
        [DllImport("libcrypt32", SetLastError = true)] private static extern unsafe Boolean CertSelectCertificateChains(ref Guid SelectionContext,Int32 Flags,CERT_SELECT_CHAIN_PARA* ChainParameters,Int32 cCriteria,CERT_SELECT_CRITERIA* rgpCriteria,IntPtr Store,out Int32 pcSelection,out CERT_CHAIN_CONTEXT* pprgpSelection);
        [DllImport("libcrypt32", SetLastError = true)] private static extern unsafe CERT_SERVER_OCSP_RESPONSE_CONTEXT* CertGetServerOcspResponseContext(IntPtr ServerOcspResponse,Int32 Flags,IntPtr Reserved);
        [DllImport("libcrypt32", SetLastError = true)] private static extern unsafe IntPtr CertAddRefServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara);
        [DllImport("libcrypt32", SetLastError = true)] private static extern unsafe IntPtr CertCreateSelfSignCertificate(IntPtr CryptProvOrNCryptKey,ref CERT_NAME_BLOB SubjectIssuerBlob,Int32 Flags,CRYPT_KEY_PROV_INFO* KeyProvInfo,CRYPT_ALGORITHM_IDENTIFIER* SignatureAlgorithm,SYSTEMTIME* StartTime,SYSTEMTIME* EndTime,CERT_EXTENSIONS* Extensions);
        [DllImport("libcrypt32", SetLastError = true)] private static extern unsafe IntPtr CertGetSubjectCertificateFromStore(IntPtr Store,Int32 EncodingType,CERT_INFO* CertId);
        [DllImport("libcrypt32", SetLastError = true)] private static extern unsafe IntPtr CertOpenServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara);
        [DllImport("libcrypt32", SetLastError = true)] private static extern unsafe IntPtr CryptFindOIDInfo(Int32 KeyType,void* Key,Int32 GroupId);
        [DllImport("libcrypt32", SetLastError = true)] private static extern void CertAddRefServerOcspResponseContext(IntPtr ServerOcspResponseContext);
        [DllImport("libcrypt32", SetLastError = true)] private static extern void CertCloseServerOcspResponse(IntPtr ServerOcspResponse,Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true, CharSet = CharSet.Auto)] private static extern bool CertControlStore([In] IntPtr CertStore, [In] uint Flags, [In] uint CtrlType, [In] IntPtr CtrlPara);
        [DllImport("libcrypt32", SetLastError = true, CharSet = CharSet.Auto)] private static extern Boolean CryptEnumProviders(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, [In][Out] StringBuilder name, ref Int32 sz);
        [DllImport("libcrypt32", SetLastError = true, CharSet = CharSet.Auto)] private static extern Boolean CryptEnumProviderTypes(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, [In][Out] StringBuilder name, ref Int32 sz);
        [DllImport("libcrypt32", SetLastError = true, CharSet = CharSet.Auto)] private static extern IntPtr CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext);
        [DllImport("libcrypt32", SetLastError = true, CharSet = CharSet.Unicode)] private static extern Boolean CertEnumPhysicalStore(IntPtr SystemStore, CERT_SYSTEM_STORE_FLAGS Flags, IntPtr Arg, PFN_CERT_ENUM_PHYSICAL_STORE Callback);
        [DllImport("libcrypt32", SetLastError = true, CharSet = CharSet.Unicode)] private static extern Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS Flags, IntPtr SystemStoreLocationPara, IntPtr Arg, CertEnumSystemStoreCallbackIntPtr Callback);
        [DllImport("libcrypt32", SetLastError = true, CharSet = CharSet.Unicode)] private static extern Boolean CertEnumSystemStoreLocation(Int32 flags, IntPtr args, PFN_CERT_ENUM_SYSTEM_STORE_LOCATION pfn);
        [DllImport("libcrypt32", SetLastError = true, EntryPoint = "CryptAcquireContextA")] private static extern Boolean CryptAcquireContext(out IntPtr CryptProv, [MarshalAs(UnmanagedType.LPStr)] String Container, [MarshalAs(UnmanagedType.LPStr)]String Provider, Int32 ProvType, Int32 Flags);
        [DllImport("libcrypt32", SetLastError = true, EntryPoint="CertOpenStore")] private static extern IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, [MarshalAs(UnmanagedType.LPStr)] String Para);
        [DllImport("libcrypt32", SetLastError = true, EntryPoint="CertOpenStore")] private static extern IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, IntPtr Para);
        [DllImport("libkernel32")] private static extern Int32 GetLastError();
        [DllImport("libkernel32")] private static extern void SetLastError(Int32 code);

        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="service">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="service"/>.
        /// -or-
        /// <see langword="null"/> if there is no service object of type <paramref name="service"/>.</returns>
        public override Object GetService(Type service) {
            if (service == typeof(CryptographicFunctions)) { return this; }
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

        Boolean CryptographicFunctions.CryptGetHashParam(IntPtr Handle, Int32 Parameter, out Int32 Value) {
            var Size = sizeof(Int32);
            return CryptGetHashParam(Handle,Parameter, out Value,ref Size,0);
            }

        IntPtr CryptographicFunctions.CryptMsgOpenToEncode(CRYPT_OPEN_MESSAGE_FLAGS Flags,
            CMSG_TYPE Type, CMSG_SIGNED_ENCODE_INFO EncodeInfo,
            ref CMSG_STREAM_INFO StreamInfo) {
            if (Environment.Is64BitProcess) {
                var e = (CMSG_SIGNED_ENCODE_INFO64)EncodeInfo;
                return CryptMsgOpenToEncode(X509_ASN_ENCODING|PKCS_7_ASN_ENCODING,Flags,Type,
                    ref e,IntPtr.Zero,ref StreamInfo);
                }
            else
                {
                var e = (CMSG_SIGNED_ENCODE_INFO32)EncodeInfo;
                return CryptMsgOpenToEncode(X509_ASN_ENCODING|PKCS_7_ASN_ENCODING,Flags,Type,
                    ref e,IntPtr.Zero,ref StreamInfo);
                }
            }
        }
    }
