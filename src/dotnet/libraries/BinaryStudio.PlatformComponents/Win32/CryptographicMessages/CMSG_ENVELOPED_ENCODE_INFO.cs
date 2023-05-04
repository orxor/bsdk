using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    using CERT_BLOB = CRYPT_BLOB;
    using CRL_BLOB  = CRYPT_BLOB;

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CMSG_ENVELOPED_ENCODE_INFO
        {
        public Int32 Size;
        public IntPtr CryptProv;
        public CRYPT_ALGORITHM_IDENTIFIER ContentEncryptionAlgorithm;
        public IntPtr EncryptionAuxInfo;
        public Int32 RecipientsCount;
        public unsafe CERT_INFO** Recipients;
        #if CMSG_ENVELOPED_ENCODE_INFO_HAS_CMS_FIELDS
        public IntPtr rgCmsRecipients;
        public Int32 cCertEncoded;
        public unsafe CERT_BLOB* rgCertEncoded;
        public Int32 cCrlEncoded;
        public unsafe CRL_BLOB* rgCrlEncoded;
        public Int32 cAttrCertEncoded;
        public unsafe CERT_BLOB* rgAttrCertEncoded;
        public Int32 cUnprotectedAttr;
        public unsafe CRYPT_ATTRIBUTE* rgUnprotectedAttr;
        #endif
        }
    }