using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    using CERT_BLOB = CRYPT_BLOB;
    using CRL_BLOB  = CRYPT_BLOB;

    public interface CMSG_SIGNED_ENCODE_INFO
        {
        Int32 SignerCount { get;set; }
        Int32 CertificateCount { get;set; }
        unsafe CERT_BLOB* Certificates { get;set; }
        IntPtr Signers { get;set; }
        unsafe void Setup(Int32 SignerCount,void* Signers);
        unsafe void Setup(Int32 SignerIndex,KEY_SPEC_TYPE KeySpec,IntPtr CryptProvOrKey,CERT_INFO* CertInfo,IntPtr HashAlgorithm);
        #if CMSG_SIGNER_ENCODE_INFO_HAS_CMS_FIELDS
        unsafe void Setup(Int32 SignerIndex,KEY_SPEC_TYPE KeySpec,IntPtr CryptProvOrKey,CERT_INFO* CertInfo,IntPtr HashAlgorithm,IntPtr HashEncryptionAlgorithm);
        #endif
        }

    [StructLayout(LayoutKind.Sequential)]
    public struct CMSG_SIGNED_ENCODE_INFO64 : CMSG_SIGNED_ENCODE_INFO
        {
        public Int32 Size;
        public Int32 SignerCount;
        public unsafe CMSG_SIGNER_ENCODE_INFO64* Signers;
        public Int32 CertificateCount;
        public unsafe CERT_BLOB* Certificates;
        public Int32 cCrlEncoded;
        public unsafe CRL_BLOB* rgCrlEncoded;
        #if CMSG_SIGNED_ENCODE_INFO_HAS_CMS_FIELDS
        public Int32  cAttrCertEncoded;
        public unsafe CERT_BLOB *rgAttrCertEncoded;
        #endif

        #region P:CMSG_SIGNED_ENCODE_INFO.SignerCount:Int32
        Int32 CMSG_SIGNED_ENCODE_INFO.SignerCount {
            get { return SignerCount;  }
            set { SignerCount = value; }
            }
        #endregion
        #region P:CMSG_SIGNED_ENCODE_INFO.Signers:IntPtr
        unsafe IntPtr CMSG_SIGNED_ENCODE_INFO.Signers {
            get { return (IntPtr)Signers; }
            set { Signers = (CMSG_SIGNER_ENCODE_INFO64*)value; }
            }
        #endregion
        #region P:CMSG_SIGNED_ENCODE_INFO.CertificateCount:Int32
        Int32 CMSG_SIGNED_ENCODE_INFO.CertificateCount {
            get { return CertificateCount; }
            set { CertificateCount = value; }
            }
        #endregion
        #region P:CMSG_SIGNED_ENCODE_INFO.Certificates:CERT_BLOB*
        unsafe CERT_BLOB* CMSG_SIGNED_ENCODE_INFO.Certificates {
            get { return Certificates; }
            set { Certificates = value; }
            }
        #endregion

        unsafe void CMSG_SIGNED_ENCODE_INFO.Setup(Int32 SignerCount,void* Signers) {
            this.SignerCount = SignerCount;
            this.Signers = (CMSG_SIGNER_ENCODE_INFO64*)Signers;
            for (var i = 0; i < SignerCount; ++i) {
                this.Signers[i].Size = sizeof(CMSG_SIGNER_ENCODE_INFO64);
                }
            }

        unsafe void CMSG_SIGNED_ENCODE_INFO.Setup(Int32 SignerIndex,KEY_SPEC_TYPE KeySpec,IntPtr CryptProvOrKey,CERT_INFO* CertInfo,IntPtr HashAlgorithm) {
            Signers[SignerIndex].KeySpec = KeySpec;
            Signers[SignerIndex].CryptProvOrKey = CryptProvOrKey;
            Signers[SignerIndex].CertInfo = CertInfo;
            Signers[SignerIndex].HashAlgorithm.ObjectId = HashAlgorithm;
            }

        #if CMSG_SIGNER_ENCODE_INFO_HAS_CMS_FIELDS
        unsafe void CMSG_SIGNED_ENCODE_INFO.Setup(Int32 SignerIndex,KEY_SPEC_TYPE KeySpec,IntPtr CryptProvOrKey,CERT_INFO* CertInfo,IntPtr HashAlgorithm,IntPtr HashEncryptionAlgorithm) {
            Signers[SignerIndex].KeySpec = KeySpec;
            Signers[SignerIndex].CryptProvOrKey = CryptProvOrKey;
            Signers[SignerIndex].CertInfo = CertInfo;
            Signers[SignerIndex].HashAlgorithm.ObjectId = HashAlgorithm;
            Signers[SignerIndex].HashEncryptionAlgorithm.ObjectId = HashEncryptionAlgorithm;
            }
        #endif
        }

    [StructLayout(LayoutKind.Sequential)]
    public struct CMSG_SIGNED_ENCODE_INFO32 : CMSG_SIGNED_ENCODE_INFO
        {
        public Int32 Size;
        public Int32 SignerCount;
        public unsafe CMSG_SIGNER_ENCODE_INFO32* Signers;
        public Int32 CertificateCount;
        public unsafe CERT_BLOB* Certificates;
        public Int32 cCrlEncoded;
        public unsafe CRL_BLOB* rgCrlEncoded;
        #if CMSG_SIGNED_ENCODE_INFO_HAS_CMS_FIELDS
        public Int32  cAttrCertEncoded;
        public unsafe CERT_BLOB *rgAttrCertEncoded;
        #endif

        #region P:CMSG_SIGNED_ENCODE_INFO.SignerCount:Int32
        Int32 CMSG_SIGNED_ENCODE_INFO.SignerCount {
            get { return SignerCount;  }
            set { SignerCount = value; }
            }
        #endregion
        #region P:CMSG_SIGNED_ENCODE_INFO.Signers:IntPtr
        unsafe IntPtr CMSG_SIGNED_ENCODE_INFO.Signers {
            get { return (IntPtr)Signers; }
            set { Signers = (CMSG_SIGNER_ENCODE_INFO32*)value; }
            }
        #endregion
        #region P:CMSG_SIGNED_ENCODE_INFO.CertificateCount:Int32
        Int32 CMSG_SIGNED_ENCODE_INFO.CertificateCount {
            get { return CertificateCount; }
            set { CertificateCount = value; }
            }
        #endregion
        #region P:CMSG_SIGNED_ENCODE_INFO.Certificates:CERT_BLOB*
        unsafe CERT_BLOB* CMSG_SIGNED_ENCODE_INFO.Certificates {
            get { return Certificates; }
            set { Certificates = value; }
            }
        #endregion

        unsafe void CMSG_SIGNED_ENCODE_INFO.Setup(Int32 SignerCount,void* Signers) {
            this.SignerCount = SignerCount;
            this.Signers = (CMSG_SIGNER_ENCODE_INFO32*)Signers;
            for (var i = 0; i < SignerCount; ++i) {
                this.Signers[i].Size = sizeof(CMSG_SIGNER_ENCODE_INFO32);
                }
            }

        unsafe void CMSG_SIGNED_ENCODE_INFO.Setup(Int32 SignerIndex,KEY_SPEC_TYPE KeySpec,IntPtr CryptProvOrKey,CERT_INFO* CertInfo,IntPtr HashAlgorithm) {
            Signers[SignerIndex].KeySpec = KeySpec;
            Signers[SignerIndex].CryptProvOrKey = CryptProvOrKey;
            Signers[SignerIndex].CertInfo = CertInfo;
            Signers[SignerIndex].HashAlgorithm.ObjectId = HashAlgorithm;
            }

        #if CMSG_SIGNER_ENCODE_INFO_HAS_CMS_FIELDS
        unsafe void CMSG_SIGNED_ENCODE_INFO.Setup(Int32 SignerIndex,KEY_SPEC_TYPE KeySpec,IntPtr CryptProvOrKey,CERT_INFO* CertInfo,IntPtr HashAlgorithm,IntPtr HashEncryptionAlgorithm) {
            Signers[SignerIndex].KeySpec = KeySpec;
            Signers[SignerIndex].CryptProvOrKey = CryptProvOrKey;
            Signers[SignerIndex].CertInfo = CertInfo;
            Signers[SignerIndex].HashAlgorithm.ObjectId = HashAlgorithm;
            Signers[SignerIndex].HashEncryptionAlgorithm.ObjectId = HashEncryptionAlgorithm;
            }
        #endif
        }
    }