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

    public interface CryptographicFunctions : LastErrorService
        {
        Encoding UnicodeEncoding { get; }
        ALG_ID CertOIDToAlgId(String Id);
        /// <summary>
        /// B
        /// </summary>
        /// <param name="Store"></param>
        /// <param name="InputContext"></param>
        /// <param name="Disposition"></param>
        /// <returns></returns>
        Boolean CertAddCertificateContextToStore(IntPtr Store,IntPtr InputContext,CERT_STORE_ADD Disposition);
        /// <summary>
        /// The function adds a certificate context to the certificate store.
        /// </summary>
        /// <param name="Store">Handle of a certificate store (HCERTSTORE).</param>
        /// <param name="InputContext">A pointer to the <see cref="CERT_CONTEXT"/> structure to be added to the store.</param>
        /// <param name="Disposition">
        /// Specifies the action to take if a matching certificate or a link to a matching certificate already exists
        /// in the store. Currently defined disposition values and their uses are as follows:
        /// <p>
        ///   <table style="font-family: Consolas;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_STORE_ADD_ALWAYS
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         The function makes no check for an existing matching certificate or link to a matching certificate. A new certificate is always added to the store. This can lead to duplicates in a store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_NEW
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching certificate or a link to a matching certificate exists, the operation fails. <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_NEWER
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching certificate or a link to a matching certificate exists and the <b>NotBefore</b> time of the existing context is equal to or greater than the <b>NotBefore</b> time of the new context being added, the operation fails and <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.<br/>
        ///         If the <b>NotBefore</b> time of the existing context is less than the <b>NotBefore</b> time of the new context being added, the existing certificate or link is deleted and a new certificate is created and added to the store. If a matching certificate or a link to a matching certificate does not exist, a new link is added.<br/>
        ///         If <a href="https://learn.microsoft.com/en-us/windows/win32/secgloss/c-gly#:~:text=a%20CERT_REQUEST_INFO%20structure.-,certificate%20revocation%20list,-(CRL)%20A%20document">certificate revocation lists</a> (CRLs) or <a href="https://learn.microsoft.com/en-us/windows/win32/secgloss/c-gly#:~:text=and%20Code%20Signing.-,certificate%20trust%20list,-(CTL)%20A%20predefined">certificate trust list</a> (CTLs) are being compared, the <b>ThisUpdate</b> time is used.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching certificate or a link to a matching certificate exists and the NotBefore time of the existing context is equal to or greater than the NotBefore time of the new context being added, the operation fails and <see cref="LastErrorService.GetLastError"/> returns the CRYPT_E_EXISTS code.<br/>
        ///         If the NotBefore time of the existing context is less than the NotBefore time of the new context being added, the existing context is deleted before creating and adding the new context. The new added context inherits properties from the existing certificate.<br/>
        ///         If CRLs or CTLs are being compared, the ThisUpdate time is used.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_REPLACE_EXISTING
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a link to a matching certificate exists, that existing certificate or link is deleted and a new certificate is created and added to the store. If a matching certificate or a link to a matching certificate does not exist, a new link is added.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching certificate exists in the store, the existing context is not replaced. The existing context inherits properties from the new certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_USE_EXISTING
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching certificate or a link to a matching certificate exists, that existing certificate or link is used and properties from the new certificate are added. The function does not fail, but it does not add a new context. If <paramref name="InputContext"/> is not <see cref="IntPtr.Zero"/>, the existing context is duplicated.<br/>
        ///         If a matching certificate or a link to a matching certificate does not exist, a new certificate is added.
        ///       </td>
        ///     </tr>
        ///   </table>
        /// </p>
        /// </param>
        /// <param name="OutputContext">A pointer to a pointer to the copy to be made of the certificate that was added to the store. Value must be freed by using <see cref="CertFreeCertificateContext"/>.</param>
        /// <returns>
        /// If the function succeeds, the return value is <see langword="true"/>. If the function fails, the return value is <see langword="false"/>.
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. Some possible error codes follow:
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        <see cref="HRESULT.CRYPT_E_EXISTS"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This value is returned if <see cref="CERT_STORE_ADD.CERT_STORE_ADD_NEW"/> is set and the certificate already exists in the store, or if <see cref="CERT_STORE_ADD.CERT_STORE_ADD_NEWER"/> is set and a certificate exists in the store with a <b>NotBefore</b> date greater than or equal to the <b>NotBefore</b> date on the certificate to be added.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        A disposition value that is not valid was specified in the <paramref name="Disposition"/> parameter.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </returns>
        /// <remarks>
        ///   <a href="https://learn.microsoft.com/en-us/windows/win32/api/wincrypt/nf-wincrypt-certaddcertificatecontexttostore">CertAddCertificateContextToStore</a>
        /// </remarks>
        Boolean CertAddCertificateContextToStore(IntPtr Store,IntPtr InputContext,CERT_STORE_ADD Disposition,out IntPtr OutputContext);
        Boolean CertAddCertificateLinkToStore(IntPtr Store,IntPtr CertContext,Int32 AddDisposition,out IntPtr OutputContext);
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
        IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType,CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,IntPtr CryptProv,IntPtr RecipientInfo,IntPtr si);
        IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE MsgEncodingType,CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,IntPtr CryptProv,IntPtr RecipientInfo,ref CMSG_STREAM_INFO si);
        IntPtr CryptMsgOpenToEncode(CRYPT_MSG_TYPE EncodingType,CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,CMSG_SIGNED_ENCODE_INFO EncodeInfo,ref CMSG_STREAM_INFO StreamInfo);
        IntPtr CryptMsgOpenToEncode(CRYPT_MSG_TYPE EncodingType,CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,ref CMSG_ENVELOPED_ENCODE_INFO EncodeInfo,ref CMSG_STREAM_INFO StreamInfo);
        unsafe Boolean CertGetCertificateChain(IntPtr ChainEngine,IntPtr Context,ref FILETIME time, IntPtr AdditionalStore, ref CERT_CHAIN_PARA ChainPara, CERT_CHAIN_FLAGS Flags, IntPtr Reserved, CERT_CHAIN_CONTEXT** ChainContext);
        unsafe Boolean CertGetValidUsages(Int32 cCerts,CERT_CONTEXT* rghCerts,Int32* cNumOIDs,IntPtr rghOIDs,Int32* pcbOIDs);
        unsafe Boolean CertSelectCertificateChains(ref Guid SelectionContext,Int32 Flags,CERT_SELECT_CHAIN_PARA* ChainParameters,Int32 cCriteria,CERT_SELECT_CRITERIA* rgpCriteria,IntPtr Store,out Int32 pcSelection,out CERT_CHAIN_CONTEXT* pprgpSelection);
        unsafe CERT_SERVER_OCSP_RESPONSE_CONTEXT* CertGetServerOcspResponseContext(IntPtr ServerOcspResponse,Int32 Flags,IntPtr Reserved);
        unsafe IntPtr CertAddRefServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara);
        unsafe IntPtr CertCreateSelfSignCertificate(IntPtr CryptProvOrNCryptKey,ref CERT_NAME_BLOB SubjectIssuerBlob,Int32 Flags,CRYPT_KEY_PROV_INFO* KeyProvInfo,CRYPT_ALGORITHM_IDENTIFIER* SignatureAlgorithm,SYSTEMTIME* StartTime,SYSTEMTIME* EndTime,CERT_EXTENSIONS* Extensions);

        /**
         * <summary>
         * The function returns from a certificate store a subject certificate context uniquely identified by its issuer and serial number.
         * </summary>
         * <param name="Store">A handle of a certificate store (HCERTSTORE).</param>
         * <param name="EncodingType">
         *   The type of encoding used. It is always acceptable to specify both the certificate and message encoding
         *   types by combining them with a bitwise-<b>OR</b> operation as shown in the following example:
         *   <p>
         *     X509_ASN_ENCODING | PKCS_7_ASN_ENCODING currently defined encoding types are:
         *     <list type="bullet">
         *       <item>X509_ASN_ENCODING</item>
         *       <item>PKCS_7_ASN_ENCODING</item>
         *     </list>
         *   </p>
         * </param>
         * <param name="CertId">A pointer to a <see cref="CERT_INFO"/> structure. Only the <see cref="CERT_INFO.Issuer"/> and <see cref="CERT_INFO.SerialNumber"/> members are used.</param>
         * <returns>
         *   If the function succeeds, the function returns a pointer to a read-only <see cref="CERT_CONTEXT"/>. The <see cref="CERT_CONTEXT"/> must be freed by calling <see cref="CertFreeCertificateContext"/>.<br/>
         *   The returned certificate might not be valid. Usually, it is verified when getting its issuer certificate (<see cref="CertGetIssuerCertificateFromStore"/>).<br/>
         *   For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following:<br/>
         *   <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
         *     <tr>
         *       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
         *         <see cref="HRESULT.CRYPT_E_NOT_FOUND"/>
         *       </td>
         *       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
         *         The subject certificate was not found in the store.
         *       </td>
         *     </tr>
         *   </table>
         * </returns>
         * <remarks>
         *   <a href="https://learn.microsoft.com/en-us/windows/win32/api/wincrypt/nf-wincrypt-certgetsubjectcertificatefromstore">CertGetIssuerCertificateFromStore</a>
         * </remarks>
         */
        unsafe IntPtr CertGetSubjectCertificateFromStore(IntPtr Store,Int32 EncodingType,CERT_INFO* CertId);
        unsafe IntPtr CertOpenServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara);
        unsafe IntPtr CryptFindOIDInfo(CRYPT_OID_INFO_KEY_TYPE KeyType,void* Key,Int32 GroupId);
        void CertAddRefServerOcspResponseContext(IntPtr ServerOcspResponseContext);
        void CertCloseServerOcspResponse(IntPtr ServerOcspResponse,Int32 Flags);
        }
    }
