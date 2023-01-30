using System;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

namespace BinaryStudio.Security.Cryptography
    {
    using CERT_NAME_BLOB = CRYPT_BLOB;
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean PFN_CERT_ENUM_SYSTEM_STORE_LOCATION([MarshalAs(UnmanagedType.LPWStr)] String Name,CERT_SYSTEM_STORE_FLAGS Flags,IntPtr Reserved,IntPtr Arg);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean CertEnumSystemStoreCallbackIntPtr(IntPtr SystemStore, CERT_SYSTEM_STORE_FLAGS Flags, ref CERT_SYSTEM_STORE_INFO StoreInfo, IntPtr Reserved, IntPtr Arg);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean CertEnumSystemStoreCallbackString(String SystemStoreName, CERT_SYSTEM_STORE_FLAGS Flags, ref CERT_SYSTEM_STORE_INFO StoreInfo, IntPtr Reserved, IntPtr Arg);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean PFN_CERT_ENUM_PHYSICAL_STORE(IntPtr SystemStore, CERT_SYSTEM_STORE_FLAGS Flags, [MarshalAs(UnmanagedType.LPWStr)] String Name, ref CERT_PHYSICAL_STORE_INFO StoreInfo, IntPtr Reserved, IntPtr Arg);

    internal interface ICryptoAPI
        {
        Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r);
        Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, out IntPtr r);
        Boolean CertFreeCertificateContext(IntPtr CertContext);
        Boolean CertCloseStore(IntPtr handle, UInt32 Flags);
        Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS Flags, IntPtr SystemStoreLocationPara, IntPtr Arg, CertEnumSystemStoreCallbackString Callback);
        Boolean CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus);
        IntPtr CertDuplicateCertificateContext([In] IntPtr pCertContext);
        IntPtr CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, [In] String Para);
        IntPtr CertOpenStore(IntPtr StoreProvider, UInt32 MsgAndCertEncodingType, IntPtr CryptProv, UInt32 Flags, [In] IntPtr Para);
        IntPtr CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext);
        IntPtr CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext);
        IntPtr CertCreateCertificateContext(UInt32 CertEncodingType, [MarshalAs(UnmanagedType.LPArray)] Byte[] blob, Int32 size);
        unsafe Boolean CertGetCertificateChain(IntPtr ChainEngine, IntPtr Context, ref FILETIME time, IntPtr AdditionalStore, ref CERT_CHAIN_PARA ChainPara, CERT_CHAIN_FLAGS Flags, IntPtr Reserved, CERT_CHAIN_CONTEXT** ChainContext);
        unsafe IntPtr CertGetSubjectCertificateFromStore(IntPtr CertStore, Int32 MsgAndCertEncodingType, CERT_INFO* CertId);
        IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, ref CMSG_STREAM_INFO si);
        IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS Flags, CMSG_TYPE Type, IntPtr CryptProv, IntPtr RecipientInfo, IntPtr si);
        Boolean CryptMsgClose(IntPtr Message);
        Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, IntPtr CtrlPara);
        Boolean CryptMsgControl(IntPtr Message, CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, ref CMSG_CTRL_DECRYPT_PARA CtrlPara);
        Boolean CryptMsgUpdate(IntPtr Message, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, Int32 Size, Boolean Final);
        Boolean CryptMsgUpdate(IntPtr Message, IntPtr Data, Int32 Size, Boolean Final);
        Boolean CryptMsgGetParam(IntPtr Message, CMSG_PARAM Parameter, Int32 SignerIndex, [MarshalAs(UnmanagedType.LPArray)] Byte[] Data, ref Int32 Size);
        Int32 CertNameToStr(Int32 CertEncodingType, ref CERT_NAME_BLOB Name, Int32 StrType, IntPtr psz, Int32 csz);
        Boolean CryptEnumProviders(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, StringBuilder name, ref Int32 sz);
        }
    }
