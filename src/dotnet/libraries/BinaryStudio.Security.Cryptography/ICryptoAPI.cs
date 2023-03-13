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

    internal interface ICryptoAPI : ILastErrorProvider,
        KeyGenerationAndExchangeFunctions,CertificateFunctions,
        DataConversionFunctions
        {
        Encoding UnicodeEncoding { get; }
        unsafe Boolean CertGetCertificateChain(IntPtr ChainEngine, IntPtr Context, ref FILETIME time, IntPtr AdditionalStore, ref CERT_CHAIN_PARA ChainPara, CERT_CHAIN_FLAGS Flags, IntPtr Reserved, CERT_CHAIN_CONTEXT** ChainContext);
        unsafe IntPtr CryptFindOIDInfo(CRYPT_OID_INFO_KEY_TYPE KeyType,void* Key,Int32 GroupId);
        Boolean CertCloseStore(IntPtr handle, UInt32 Flags);
        Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS Flags, IntPtr SystemStoreLocationPara, IntPtr Arg, CertEnumSystemStoreCallbackString Callback);
        Boolean CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus);
        IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, IntPtr Para);
        IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, String Para);
        IntPtr CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext);
        IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, ref CMSG_STREAM_INFO si);
        IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, IntPtr si);
        Boolean CryptMsgClose(IntPtr Message);
        Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, IntPtr CtrlPara);
        Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, ref CMSG_CTRL_DECRYPT_PARA CtrlPara);
        Boolean CryptMsgUpdate(IntPtr Message, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, Int32 Size, Boolean Final);
        Boolean CryptMsgUpdate(IntPtr Message, IntPtr Data, Int32 Size, Boolean Final);
        Boolean CryptMsgGetParam(IntPtr Message, CMSG_PARAM Parameter, Int32 SignerIndex, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, ref Int32 Size);
        Boolean CryptEnumProviders(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, StringBuilder name, ref Int32 sz);
        Boolean CryptEnumProviderTypes(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, StringBuilder name, ref Int32 sz);
        Boolean CryptAcquireContext(out IntPtr CryptProv, String Container, String Provider, Int32 ProvType, Int32 Flags);
        Boolean CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,ref Int32 DataSize,Int32 Flags);
        Boolean CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,Byte[] Data,ref Int32 DataSize,Int32 Flags);
        Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,Byte[] Data,Int32 Flags);
        Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,Int32 Flags);
        Boolean CryptEnumOIDInfo(CRYPT_ALG_OID_GROUP_ID GroupId,IntPtr Arg,CryptEnumOidInfoCallback Callback);
        Boolean CryptAcquireCertificatePrivateKey(IntPtr Certificate, CRYPT_ACQUIRE_FLAGS Flags, IntPtr Parameters,out IntPtr CryptProvOrNCryptKey, out KEY_SPEC_TYPE KeySpec, out Boolean CallerFreeProvOrNCryptKey);
        Boolean CertGetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Byte[] Data, ref Int32 Size);
        Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, ref CRYPT_KEY_PROV_INFO Data);
        Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, IntPtr Data);
        Boolean CryptHashData(IntPtr Handle, Byte[] Data, Int32 DataSize);
        Boolean CryptGetHashParam(IntPtr Handle, Int32 Parameter, Byte[] Block, ref Int32 BlockSize);
        Boolean CryptGetHashParam(IntPtr Handle, Int32 Parameter, out Int32 Block, ref Int32 BlockSize);
        Boolean CryptDestroyHash(IntPtr Handle);
        Boolean CryptVerifySignature(IntPtr Handle, Byte[] Signature, Int32 SignatureSize, IntPtr Key);
        Boolean CryptSignHash(IntPtr Handle, KEY_SPEC_TYPE KeySpec, Byte[] Signature, ref Int32 Length);
        Boolean CryptCreateHash(IntPtr Provider, ALG_ID Algorithm, IntPtr Key, out IntPtr Handle);
        Boolean CertExportCertStore(IntPtr Store,ref CRYPT_DATA_BLOB PFX,String Password,IntPtr Para,Int32 Flags);
        Boolean CertAddCRLContextToStore(IntPtr Store,IntPtr Context,CERT_STORE_ADD Disposition, IntPtr Zero);
        Boolean CertAddCRLContextToStore(IntPtr Store,IntPtr Context,CERT_STORE_ADD Disposition, out IntPtr StoreContext);
        Boolean CertDeleteCRLFromStore(IntPtr Context);
        }
    }
