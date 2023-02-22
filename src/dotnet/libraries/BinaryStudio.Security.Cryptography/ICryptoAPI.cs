﻿using System;
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
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] public delegate Boolean CryptEnumOidInfoCallback(IntPtr Info,IntPtr Arg);

    internal interface ICryptoAPI : ILastErrorProvider
        {
        Encoding UnicodeEncoding { get; }
        Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, IntPtr r);
        Boolean CertAddCertificateContextToStore(IntPtr store, IntPtr context, CERT_STORE_ADD disposition, out IntPtr r);
        Boolean CertFreeCertificateContext(IntPtr CertContext);
        Boolean CertCloseStore(IntPtr handle, UInt32 Flags);
        Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS Flags, IntPtr SystemStoreLocationPara, IntPtr Arg, CertEnumSystemStoreCallbackString Callback);
        Boolean CertVerifyCertificateChainPolicy(IntPtr Policy, IntPtr ChainContext, ref CERT_CHAIN_POLICY_PARA PolicyPara, ref CERT_CHAIN_POLICY_STATUS PolicyStatus);
        IntPtr CertDuplicateCertificateContext([In] IntPtr pCertContext);
        IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, IntPtr Para);
        IntPtr CertOpenStoreA(IntPtr StoreProvider, Int32 MsgAndCertEncodingType, IntPtr CryptProv, Int32 Flags, String Para);
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
        Boolean CryptEnumProviderTypes(Int32 index, IntPtr reserved, Int32 flags, out Int32 type, StringBuilder name, ref Int32 sz);
        Boolean CryptAcquireContext(out IntPtr CryptProv, String Container, String Provider, Int32 ProvType, Int32 Flags);
        Boolean CryptGetProvParam(IntPtr Provider, CRYPT_PARAM Parameter, IntPtr Data, ref Int32 DataSize, Int32 Flags);
        Boolean CryptGetProvParam(IntPtr Provider, CRYPT_PARAM Parameter, Byte[] Data, ref Int32 DataSize, Int32 Flags);
        Boolean CryptEnumOIDInfo(CRYPT_ALG_OID_GROUP_ID GroupId,IntPtr Arg,CryptEnumOidInfoCallback Callback);
        Boolean CryptGetKeyParam(IntPtr Key, KEY_PARAM Param, Byte[] Data, ref Int32 DataSize, Int32 Flags);
        Boolean CryptGetUserKey(IntPtr Provider, KEY_SPEC_TYPE KeySpec, out IntPtr Key);
        ALG_ID CertOIDToAlgId(String Id);
        IntPtr CryptFindOIDInfo(CRYPT_OID_INFO_KEY_TYPE KeyType,IntPtr Key,Int32 GroupId);
        Boolean CryptAcquireCertificatePrivateKey(IntPtr Certificate, CRYPT_ACQUIRE_FLAGS Flags, IntPtr Parameters,out IntPtr CryptProvOrNCryptKey, out KEY_SPEC_TYPE KeySpec, out Boolean CallerFreeProvOrNCryptKey);
        Boolean CertGetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Byte[] Data, ref Int32 Size);
        Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, ref CRYPT_KEY_PROV_INFO Data);
        Boolean CertSetCertificateContextProperty(IntPtr Context, CERT_PROP_ID PropertyIndex, Int32 Flags, IntPtr Data);
        }
    }
