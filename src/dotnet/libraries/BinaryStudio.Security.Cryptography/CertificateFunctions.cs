using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    using HCERT_SERVER_OCSP_RESPONSE=IntPtr;
    using HCRYPTPROV_OR_NCRYPT_KEY_HANDLE=IntPtr;
    using CERT_NAME_BLOB=CRYPT_BLOB;
    using HCERTSTORE=IntPtr;

    internal interface CertificateFunctions
        {
        Boolean CertAddCertificateContextToStore(HCERTSTORE CertStore,IntPtr CertContext,Int32 AddDisposition,IntPtr StoreContext);
        Boolean CertAddCertificateLinkToStore(HCERTSTORE CertStore,IntPtr CertContext,Int32 AddDisposition,out IntPtr StoreContext);
        Boolean CertAddEncodedCertificateToStore(HCERTSTORE CertStore,Int32 CertEncodingType,Byte[] CertEncodedData,Int32 CertEncodedLength,Int32 AddDisposition,out IntPtr CertContext);
        unsafe HCERT_SERVER_OCSP_RESPONSE CertAddRefServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara);
        void CertAddRefServerOcspResponseContext(HCERT_SERVER_OCSP_RESPONSE ServerOcspResponseContext);
        void CertCloseServerOcspResponse(HCERT_SERVER_OCSP_RESPONSE ServerOcspResponse,Int32 Flags);
        IntPtr CertCreateCertificateContext(Int32 CertEncodingType,Byte[] CertEncodedBytes,Int32 CertEncodedLength);
        unsafe IntPtr CertCreateSelfSignCertificate(HCRYPTPROV_OR_NCRYPT_KEY_HANDLE CryptProvOrNCryptKey,ref CERT_NAME_BLOB SubjectIssuerBlob,Int32 Flags,CRYPT_KEY_PROV_INFO* KeyProvInfo,CRYPT_ALGORITHM_IDENTIFIER* SignatureAlgorithm,SYSTEMTIME* StartTime,SYSTEMTIME* EndTime,CERT_EXTENSIONS* Extensions);
        Boolean CertDeleteCertificateFromStore(IntPtr CertContext);
        IntPtr CertDuplicateCertificateContext(IntPtr CertContext);
        IntPtr CertEnumCertificatesInStore(HCERTSTORE CertStore,IntPtr PrevCertContext);
        IntPtr CertFindCertificateInStore(HCERTSTORE CertStore,Int32 CertEncodingType,Int32 FindFlags,Int32 FindType,IntPtr FindPara,IntPtr PrevCertContext);
        Boolean CertFreeCertificateContext(IntPtr CertContext);
        unsafe IntPtr CertGetIssuerCertificateFromStore(HCERTSTORE CertStore,IntPtr SubjectContext,IntPtr PrevIssuerContext,ref Int32 Flags);
        unsafe CERT_SERVER_OCSP_RESPONSE_CONTEXT* CertGetServerOcspResponseContext(HCERT_SERVER_OCSP_RESPONSE ServerOcspResponse,Int32 Flags,IntPtr Reserved);
        unsafe IntPtr CertGetSubjectCertificateFromStore(HCERTSTORE CertStore,Int32 CertEncodingType,CERT_INFO* CertId);
        unsafe Boolean CertGetValidUsages(Int32 cCerts,CERT_CONTEXT* rghCerts,Int32* cNumOIDs,IntPtr rghOIDs,Int32* pcbOIDs);
        unsafe HCERT_SERVER_OCSP_RESPONSE CertOpenServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara);
        Boolean CertRetrieveLogoOrBiometricInfo(IntPtr CertContext,String LogoOrBiometricType,Int32 RetrievalFlags,Int32 Timeout,Int32 Flags,IntPtr Reserved,out IntPtr ppbData,out Int32 pcbData,out IntPtr ppwszMimeType);
        unsafe Boolean CertSelectCertificateChains(ref Guid SelectionContext,Int32 Flags,CERT_SELECT_CHAIN_PARA* ChainParameters,Int32 cCriteria,CERT_SELECT_CRITERIA* rgpCriteria,HCERTSTORE Store,out Int32 pcSelection,out CERT_CHAIN_CONTEXT* pprgpSelection);
        Boolean CertSerializeCertificateStoreElement(IntPtr CertContext,Int32 Flags,Byte[] pbElement,ref Int32 pcbElement);
        Boolean CertVerifySubjectCertificateContext(IntPtr Subject,IntPtr Issuer,ref Int32 Flags);
        //GetFriendlyNameOfCert;
        //RKeyCloseKeyService;
        //RKeyOpenKeyService;
        //RKeyPFXInstall;
        //CertSelectCertificate;
        //CertSelectionGetSerializedBlob;
        //CryptUIDlgCertMgr;
        //CryptUIDlgSelectCertificate;
        //CryptUIDlgSelectCertificateFromStore;
        //CryptUIDlgViewCertificate;
        //CryptUIDlgViewContext;
        //CryptUIDlgViewSignerInfo;
        }
    }