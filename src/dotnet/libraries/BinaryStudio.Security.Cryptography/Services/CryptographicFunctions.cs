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
        #region M:CertAddCertificateContextToStore(IntPtr,IntPtr,CERT_STORE_ADD):Boolean
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
        Boolean CertAddCertificateContextToStore(IntPtr Store,IntPtr InputContext,CERT_STORE_ADD Disposition);
        #endregion
        #region M:CertAddCertificateContextToStore(IntPtr,IntPtr,CERT_STORE_ADD,{out}IntPtr)
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
        #endregion
        #region M:CertAddCertificateLinkToStore(IntPtr,IntPtr,Int32,{out}IntPtr):Boolean
        /// <summary>
        /// The function adds a link in a certificate store to a certificate context in a different store. Instead of creating and adding a duplicate of the certificate context, this function adds a link to the original certificate.
        /// </summary>
        /// <param name="Store">A handle to the certificate store where the link is to be added.</param>
        /// <param name="CertContext">A pointer to the <see cref="CERT_CONTEXT"/> structure to be linked.</param>
        /// <param name="Disposition">Specifies the action if a matching certificate or a link to a matching certificate already exists in the store. Currently defined disposition values and their uses are as follows:
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
        ///         CERT_STORE_ADD_REPLACE_EXISTING
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a link to a matching certificate exists, that existing link is deleted and a new link is created and added to the store. If no matching certificate or link to a matching certificate exists, one is added.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_USE_EXISTING
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching certificate or a link to a matching certificate exists, the existing certificate is used. The function does not fail, but no new link is added. If no matching certificate or link to a matching certificate exists, a new link is added.
        ///       </td>
        ///     </tr>
        ///   </table>
        /// </p>
        /// </param>
        /// <param name="OutputContext">
        /// A pointer to a pointer to a copy of the link created. If a copy of the link is created, that copy must be freed using the <see cref="CertFreeCertificateContext"/> function.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is <see langword="true"/>.<br/>
        /// If the function fails, the return value is <see langword="false"/>. For extended error information, call GetLastError. Some possible error codes follow.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        <see cref="HRESULT.CRYPT_E_EXISTS"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        For a <paramref name="Disposition"/> parameter of <see cref="CERT_STORE_ADD.CERT_STORE_ADD_NEW"/>, the certificate already exists in the store.
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
        Boolean CertAddCertificateLinkToStore(IntPtr Store,IntPtr CertContext,Int32 Disposition,out IntPtr OutputContext);
        #endregion
        #region M:CertAddCRLContextToStore(IntPtr,IntPtr,CERT_STORE_ADD):Boolean
        /// <summary>
        /// The function adds a certificate revocation list (CRL) context to the specified certificate store.
        /// </summary>
        /// <param name="Store">Handle of a certificate store.</param>
        /// <param name="Context">A pointer to the <see cref="CRL_CONTEXT"/> structure to be added.</param>
        /// <param name="Disposition">Specifies the action to take if a matching CRL or a link to a matching CRL already exists in the store. Currently defined disposition values and their uses are as follows:
        /// <p>
        ///   <table style="font-family: Consolas;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_STORE_ADD_ALWAYS
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Makes no check for an existing matching CRL or link to a matching CRL. A new CRL is always added to the store. This can lead to duplicates in a store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_NEW
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching CRL or a link to a matching CRL exists, the operation fails. <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_NEWER
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching CRL or a link to a matching CRL exists, the function compares the <see cref="CRL_INFO.ThisUpdate"/> times on the CRLs. If the existing CRL has a <see cref="CRL_INFO.ThisUpdate"/> time less than the <see cref="CRL_INFO.ThisUpdate"/> time on the new CRL, the old CRL or link is replaced just as with CERT_STORE_ADD_REPLACE_EXISTING.<br/>
        ///         If the existing CRL has a <see cref="CRL_INFO.ThisUpdate"/> time greater than or equal to the <see cref="CRL_INFO.ThisUpdate"/> time on the CRL to be added, the function fails with <see cref="LastErrorService.GetLastError"/> returning the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.<br/>
        ///         If a matching CRL or a link to a matching CRL is not found in the store, a new CRL is added to the store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         The action is the same as for CERT_STORE_ADD_NEWER, except that if an older CRL is replaced, the properties of the older CRL are incorporated into the replacement CRL.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_REPLACE_EXISTING
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching CRL or a link to a matching CRL exists, the existing CRL or link is deleted and a new CRL is created and added to the store.<br/>
        ///         If a matching CRL or a link to a matching CRL does not exist, one is added.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching CRL exists in the store, the existing context is deleted before creating and adding the new context. The added context inherits properties from the existing CRL.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_USE_EXISTING
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching CRL or a link to a matching CRL exists, that existing CRL is used and properties from the new CRL are added. The function does not fail, but no new CRL is added. The existing context is duplicated.<br/>
        ///         If a matching CRL or a link to a matching CRL does not exist, a new CRL is added.
        ///       </td>
        ///     </tr>
        ///   </table>
        /// </p>
        /// <returns>
        /// If the function succeeds, the return value is TRUE.<br/>
        /// If the function fails, the return value is FALSE. Errors from the called functions <see cref="CertAddEncodedCRLToStore"/> and <see cref="CertSetCRLContextProperty"/> can be propagated to this function.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. Some possible error codes follow.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        <see cref="HRESULT.CRYPT_E_EXISTS"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This error is returned if CERT_STORE_ADD_NEW is set and the CRL already exists in the store or if CERT_STORE_ADD_NEWER is set and a CRL exists in the store with a <see cref="CRL_INFO.ThisUpdate"/> date greater than or equal to the <see cref="CRL_INFO.ThisUpdate"/> date on the CRL to be added.
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
        Boolean CertAddCRLContextToStore(IntPtr Store,IntPtr Context,CERT_STORE_ADD Disposition);
        #endregion
        #region M:CertAddCRLContextToStore(IntPtr,IntPtr,CERT_STORE_ADD,{out}IntPtr):Boolean
        /// <summary>
        /// The function adds a certificate revocation list (CRL) context to the specified certificate store.
        /// </summary>
        /// <param name="Store">Handle of a certificate store.</param>
        /// <param name="Context">A pointer to the <see cref="CRL_CONTEXT"/> structure to be added.</param>
        /// <param name="Disposition">Specifies the action to take if a matching CRL or a link to a matching CRL already exists in the store. Currently defined disposition values and their uses are as follows:
        /// <p>
        ///   <table style="font-family: Consolas;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_STORE_ADD_ALWAYS
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Makes no check for an existing matching CRL or link to a matching CRL. A new CRL is always added to the store. This can lead to duplicates in a store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_NEW
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching CRL or a link to a matching CRL exists, the operation fails. <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_NEWER
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching CRL or a link to a matching CRL exists, the function compares the <see cref="CRL_INFO.ThisUpdate"/> times on the CRLs. If the existing CRL has a <see cref="CRL_INFO.ThisUpdate"/> time less than the <see cref="CRL_INFO.ThisUpdate"/> time on the new CRL, the old CRL or link is replaced just as with CERT_STORE_ADD_REPLACE_EXISTING.<br/>
        ///         If the existing CRL has a <see cref="CRL_INFO.ThisUpdate"/> time greater than or equal to the <see cref="CRL_INFO.ThisUpdate"/> time on the CRL to be added, the function fails with <see cref="LastErrorService.GetLastError"/> returning the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.<br/>
        ///         If a matching CRL or a link to a matching CRL is not found in the store, a new CRL is added to the store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         The action is the same as for CERT_STORE_ADD_NEWER, except that if an older CRL is replaced, the properties of the older CRL are incorporated into the replacement CRL.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_REPLACE_EXISTING
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching CRL or a link to a matching CRL exists, the existing CRL or link is deleted and a new CRL is created and added to the store.<br/>
        ///         If a matching CRL or a link to a matching CRL does not exist, one is added.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching CRL exists in the store, the existing context is deleted before creating and adding the new context. The added context inherits properties from the existing CRL.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_USE_EXISTING
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching CRL or a link to a matching CRL exists, that existing CRL is used and properties from the new CRL are added. The function does not fail, but no new CRL is added. The existing context is duplicated.<br/>
        ///         If a matching CRL or a link to a matching CRL does not exist, a new CRL is added.
        ///       </td>
        ///     </tr>
        ///   </table>
        /// </p>
        /// </param>
        /// <param name="StoreContext">A pointer to a pointer to the decoded CRL context. If a copy is made, that context must be freed by using <see cref="CertFreeCRLContext"/>.</param>
        /// <returns>
        /// If the function succeeds, the return value is TRUE.<br/>
        /// If the function fails, the return value is FALSE. Errors from the called functions <see cref="CertAddEncodedCRLToStore"/> and <see cref="CertSetCRLContextProperty"/> can be propagated to this function.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. Some possible error codes follow.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        <see cref="HRESULT.CRYPT_E_EXISTS"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This error is returned if CERT_STORE_ADD_NEW is set and the CRL already exists in the store or if CERT_STORE_ADD_NEWER is set and a CRL exists in the store with a <see cref="CRL_INFO.ThisUpdate"/> date greater than or equal to the <see cref="CRL_INFO.ThisUpdate"/> date on the CRL to be added.
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
        Boolean CertAddCRLContextToStore(IntPtr Store,IntPtr Context,CERT_STORE_ADD Disposition, out IntPtr StoreContext);
        #endregion
        #region M:CertAddEncodedCertificateToStore(IntPtr,Int32,Byte[],Int32,Int32,{out}IntPtr):Boolean
        /// <summary>
        /// The function creates a certificate context from an encoded certificate and adds it to the certificate store. The context created does not include any extended properties.<br/>
        /// The function also makes a copy of the encoded certificate before adding the certificate to the store.
        /// </summary>
        /// <param name="Store">A handle to the certificate store.</param>
        /// <param name="CertEncodingType">
        /// Specifies the type of encoding used. It is always acceptable to specify both the certificate and message encoding types by combining them with a bitwise-<b>OR</b> operation as shown in the following example:
        ///  <p>
        ///    X509_ASN_ENCODING | PKCS_7_ASN_ENCODING currently defined encoding types are:
        ///    <list type="bullet">
        ///      <item>X509_ASN_ENCODING</item>
        ///      <item>PKCS_7_ASN_ENCODING</item>
        ///    </list>
        ///  </p>
        /// </param>
        /// <param name="CertEncodedData">A pointer to a buffer containing the encoded certificate that is to be added to the certificate store.</param>
        /// <param name="CertEncodedLength">The size, in bytes, of the <paramref name="CertEncodedData"/> buffer.</param>
        /// <param name="Disposition">Specifies the action to take if a matching certificate or link to a matching certificate exists in the store. Currently defined disposition values and their uses are as follows.
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
        ///         If a matching certificate or a link to a matching certificate exists in the store, the operation fails. <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_REPLACE_EXISTING
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching certificate or link to a matching certificate exists in the store, the existing certificate or link is deleted and a new certificate is created and added to the store. If a matching certificate or link to a matching certificate does not exist, a new certificate is created and added to the store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching certificate exists in the store, that existing context is deleted before creating and adding the new context. The new context inherits properties from the existing certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_STORE_ADD_USE_EXISTING
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a matching certificate or a link to a matching certificate exists, that existing certificate or link is used and properties from the new certificate are added. The function does not fail, but it does not add a new context. The existing context is duplicated.<br/>
        ///         If a matching certificate or link to a matching certificate does not exist, a new certificate is added.
        ///       </td>
        ///     </tr>
        ///   </table>
        /// </p>
        /// </param>
        /// <param name="CertContext">
        /// A pointer to a pointer to the decoded certificate context. When a copy is made, its context must be freed by using <see cref="CertFreeCertificateContext"/>.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is TRUE.<br/>
        /// If the function fails, the return value is FALSE. For extended error information, call <see cref="LastErrorService.GetLastError"/>. Some possible error codes follow:
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        <see cref="HRESULT.CRYPT_E_EXISTS"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This code is returned if CERT_STORE_ADD_NEW is set and the certificate already exists in the store, or if CERT_STORE_ADD_NEWER is set and there is a certificate in the store with a <see cref="CERT_INFO.NotBefore"/> date greater than or equal to the <see cref="CERT_INFO.NotBefore"/> date on the certificate to be added.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        A disposition value that is not valid was specified in the <paramref name="Disposition"/> parameter, or a certificate encoding type that is not valid was specified. Currently, only the X509_ASN_ENCODING type is supported.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </returns>
        Boolean CertAddEncodedCertificateToStore(IntPtr Store,Int32 CertEncodingType,Byte[] CertEncodedData,Int32 CertEncodedLength,Int32 Disposition,out IntPtr CertContext);
        #endregion
        #region M:CertCloseStore(IntPtr,Int32):Boolean
        /// <summary>
        /// The function closes a certificate store handle and reduces the reference count on the store. There needs to be a corresponding call to <see cref="CertCloseStore"/> for each successful call to the <see cref="CertOpenStore"/> or <see cref="CertDuplicateStore"/> functions.
        /// </summary>
        /// <param name="Store">Handle of the certificate store to be closed.</param>
        /// <param name="Flags">
        /// Typically, this parameter uses the default value zero. The default is to close the store with memory remaining allocated for contexts that have not been freed. In this case, no check is made to determine whether memory for contexts remains allocated.<br/>
        /// Set flags can force the freeing of memory for all of a store's certificate, certificate revocation list (CRL), and certificate trust list (CTL) contexts when the store is closed. Flags can also be set that check whether all of the store's certificate, CRL, and CTL contexts have been freed. The following values are defined.
        ///   <table style="font-family: Consolas;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_CLOSE_STORE_CHECK_FLAG
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Checks for nonfreed certificate, CRL, and CTL contexts. A returned error code indicates that one or more store elements is still in use. This flag should only be used as a diagnostic tool in the development of applications.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         CERT_CLOSE_STORE_FORCE_FLAG
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Forces the freeing of memory for all contexts associated with the store. This flag can be safely used only when the store is opened in a function and neither the store handle nor any of its contexts are passed to any called functions.
        ///       </td>
        ///     </tr>
        ///   </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is TRUE.<br/>
        /// If the function fails, the return value is FALSE. For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// If CERT_CLOSE_STORE_CHECK_FLAG is not set or if it is set and all contexts associated with the store have been freed, the return value is TRUE.<br/>
        /// If CERT_CLOSE_STORE_CHECK_FLAG is set and memory for one or more contexts associated with the store remains allocated, the return value is FALSE. The store is always closed even when the function returns FALSE.<br/>
        /// GetLastError is set to <see cref="HRESULT.CRYPT_E_PENDING_CLOSE"/> if memory for contexts associated with the store remains allocated. Any existing value returned by <see cref="LastErrorService.GetLastError"/> is preserved unless CERT_CLOSE_STORE_CHECK_FLAG is set.
        /// </returns>
        Boolean CertCloseStore(IntPtr Store,Int32 Flags);
        #endregion
        #region M:CertDeleteCertificateFromStore(IntPtr):Boolean
        /// <summary>
        /// The function deletes the specified certificate context from the certificate store.
        /// </summary>
        /// <param name="Context">A pointer to the <see cref="CERT_CONTEXT"/> structure to be deleted.</param>
        /// <returns>
        /// If the function succeeds, the return value is TRUE.<br/>
        /// If the function fails, the return value is FALSE. For extended error information, call GetLastError. One possible error code is the following.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        <see cref="HRESULT.E_ACCESSDENIED"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Indicates the store was opened as read-only and a delete operation is not allowed.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </returns>
        Boolean CertDeleteCertificateFromStore(IntPtr Context);
        #endregion
        #region M:CertDeleteCRLFromStore(IntPtr):Boolean
        /// <summary>
        /// The function deletes the specified certificate revocation list (CRL) context from the certificate store.
        /// </summary>
        /// <param name="Context">A pointer to the <see cref="CRL_CONTEXT"/> structure to be deleted.</param>
        /// <returns>
        /// If the function succeeds, the return value is TRUE.<br/>
        /// If the function fails, the return value is FALSE. For extended error information, call GetLastError. One possible error code is the following.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        <see cref="HRESULT.E_ACCESSDENIED"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The store was opened read-only, and a delete operation is not allowed.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </returns>
        Boolean CertDeleteCRLFromStore(IntPtr Context);
        #endregion
        #region M:CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS,IntPtr,IntPtr,CertEnumSystemStoreCallbackString):Boolean
        /// <summary>
        /// The function retrieves the system stores available. The function calls the provided callback function for each system store found.
        /// </summary>
        /// <param name="Flags">
        /// Specifies the location of the system store. This parameter can be one of the following flags:
        ///    <list type="bullet">
        ///      <item>CERT_SYSTEM_STORE_CURRENT_USER</item>
        ///      <item>CERT_SYSTEM_STORE_CURRENT_SERVICE</item>
        ///      <item>CERT_SYSTEM_STORE_LOCAL_MACHINE</item>
        ///      <item>CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY</item>
        ///      <item>CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY</item>
        ///      <item>CERT_SYSTEM_STORE_SERVICES</item>
        ///      <item>CERT_SYSTEM_STORE_USERS</item>
        ///      <item>CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE</item>
        ///    </list>
        /// In addition, the CERT_SYSTEM_STORE_RELOCATE_FLAG can be combined, by using a bitwise-OR operation, with any of the high-word location flags.
        /// </param>
        /// <param name="SystemStoreLocationPara">
        /// If CERT_SYSTEM_STORE_RELOCATE_FLAG is set in the <paramref name="Flags"/> parameter, <paramref name="SystemStoreLocationPara"/> points to a CERT_SYSTEM_STORE_RELOCATE_PARA structure that indicates both the name and the location of the system store. Otherwise, <paramref name="SystemStoreLocationPara"/> is a pointer to a Unicode string that names the system store.<br/>
        /// For CERT_SYSTEM_STORE_LOCAL_MACHINE or CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY, <paramref name="SystemStoreLocationPara"/> can optionally be set to a Unicode computer name for enumerating local computer stores on a remote computer, for example "\\<i>computer_name</i>" or "<i>computer_name</i>". The leading backslashes (\) are optional in the <i>computer_name</i>.<br/>
        /// For CERT_SYSTEM_STORE_SERVICES or CERT_SYSTEM_STORE_USERS, if <paramref name="SystemStoreLocationPara"/> is NULL, the function enumerates both the service/user names and the stores for each service/user name. Otherwise, <paramref name="SystemStoreLocationPara"/> is a Unicode string that contains a remote computer name and, if available, a service/user name, for example, "<i>service_name</i>", "\\<i>computer_name</i>", or "<i>computer_name</i>".<br/>
        /// If only the <i>computer_name</i> is specified, it must have either the leading backslashes (\) or a trailing backslash (\). Otherwise, it is interpreted as the <i>service_name</i> or <i>user_name</i>.
        /// </param>
        /// <param name="Arg">A pointer to a void that allows the application to declare, define, and initialize a structure to hold any information to be passed to the callback enumeration function.</param>
        /// <param name="Callback">A pointer to the callback function used to show the details for each system store. This callback function determines the content and format for the presentation of information on each system store. The application must provide the <see cref="CertEnumSystemStoreCallbackString"/> callback function.</param>
        /// <returns>
        /// If the function succeeds, the function returns TRUE.<br/>
        /// If the function fails, it returns FALSE.
        /// </returns>
        Boolean CertEnumSystemStore(CERT_SYSTEM_STORE_FLAGS Flags,IntPtr SystemStoreLocationPara,IntPtr Arg,CertEnumSystemStoreCallbackString Callback);
        #endregion
        #region M:CertExportCertStore(IntPtr,{ref}CRYPT_DATA_BLOB,IntPtr,IntPtr,Int32):Boolean
        /// <summary>
        /// The function exports the certificates and, if available, their associated private keys from the referenced certificate store. This function replaces the older <see cref="M:BinaryStudio.Security.Cryptography.CryptographicFunctions.CertExportCertStore(System.IntPtr,BinaryStudio.PlatformComponents.Win32.CRYPT_BLOB@,System.IntPtr,System.Int32)"/> function. It should be used for its enhanced private key security. The PFX BLOB created by this function is protected by a password.
        /// </summary>
        /// <param name="Store">Handle of the certificate store containing the certificates to be exported.</param>
        /// <param name="PFX">A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure to contain the PFX packet with the exported certificates and keys. If <paramref name="PFX"/>.<see cref="CRYPT_BLOB.Data"/> is NULL, the function calculates the number of bytes needed for the encoded BLOB and returns this in <paramref name="PFX"/>.<see cref="CRYPT_BLOB.Size"/>. When the function is called with <paramref name="PFX"/>.<see cref="CRYPT_BLOB.Data"/> pointing to an allocated buffer of the needed size, the function copies the encoded bytes into the buffer and updates <paramref name="PFX"/>.<see cref="CRYPT_BLOB.Size"/> with the encode byte length.</param>
        /// <param name="Password">String password used to encrypt and verify the PFX packet.</param>
        /// <param name="Para">
        /// This parameter must be NULL if the <paramref name="Flags"/> parameter does not contain PKCS12_PROTECT_TO_DOMAIN_SIDS or PKCS12_EXPORT_PBES2_PARAMS. Prior to Windows 8 and Windows Server 2012, therefore, this parameter must be NULL.<br/>
        /// Beginning with Windows 8 and Windows Server 2012, if the <paramref name="Flags"/> parameter contains PKCS12_PROTECT_TO_DOMAIN_SIDS, you can set the <paramref name="Para"/> parameter to point to an NCRYPT_DESCRIPTOR_HANDLE value to identify which Active Directory principal the PFX password will be protected to inside of the PFX BLOB. Currently, the password can be protected to an Active Directory user, computer, or group.<br/>
        /// Beginning with Windows 10 1709 (Fall Creators update) and Windows Server 2019, if the dwFlags parameter contains PKCS12_EXPORT_PBES2_PARAMS, you should set the pvPara to an PKCS12_EXPORT_PBES2_PARAMS value to select the password-based encryption algorithm to use.
        /// </param>
        /// <param name="Flags">
        /// Flag values can be set to any combination of the following.
        ///   <table style="font-family: Consolas;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         EXPORT_PRIVATE_KEYS
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Private keys are exported as well as the certificates.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         REPORT_NO_PRIVATE_KEY
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a certificate is encountered that has no associated private key, the function returns FALSE with the last error set to either <see cref="HRESULT.CRYPT_E_NOT_FOUND"/> or <see cref="HRESULT.NTE_NO_KEY"/>.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a certificate is encountered that has a non-exportable private key, the function returns FALSE and the last error set to <see cref="HRESULT.NTE_BAD_KEY"/>, <see cref="HRESULT.NTE_BAD_KEY_STATE"/>, or <see cref="HRESULT.NTE_PERM"/>.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         PKCS12_INCLUDE_EXTENDED_PROPERTIES
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Export all extended properties on the certificate.<br/>
        ///         <b>Windows Server 2003 and Windows XP</b>: This value is not supported.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         PKCS12_PROTECT_TO_DOMAIN_SIDS
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         The PFX BLOB contains an embedded password that will be protected to the Active Directory (AD) protection descriptor pointed to by the <paramref name="Para"/> parameter. If the <paramref name="Password"/> parameter is not NULL or empty, the specified password is protected. If, however, the <paramref name="Password"/> parameter is NULL or an empty string, a random forty (40) character password is created and protected.<br/>
        ///         <see cref="ImportCertStore"/> uses the specified protection descriptor to decrypt the embedded password, whether specified by the user or randomly generated, and then uses the password to decrypt the PFX BLOB.<br/>
        ///         <b>Windows 8 and Windows Server 2012</b>: Support for this flag begins.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         PKCS12_EXPORT_PBES2_PARAMS
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Export using the passowrd-based encryption algorithm specified by the PKCS12_EXPORT_PBES2_PARAMS value passed as <paramref name="Para"/>.<br/>
        ///         <b>Windows 10 1709 (Fall Creators update) and Windows Server 2019</b>: Support for this flag begins.
        ///       </td>
        ///     </tr>
        ///   </table>
        /// </param>
        /// <returns>
        /// Returns TRUE (nonzero) if the function succeeds, and FALSE (zero) if the function fails.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>.
        /// </returns>
        Boolean CertExportCertStore(IntPtr Store,ref CRYPT_DATA_BLOB PFX,IntPtr Password,IntPtr Para,Int32 Flags);
        #endregion
        #region M:CertExportCertStore(IntPtr,{ref}CRYPT_DATA_BLOB,IntPtr,Int32):Boolean
        /// <summary>
        /// The function exports the certificates and, if available, their associated private keys from the referenced certificate store. This function replaces the older <see cref="M:BinaryStudio.Security.Cryptography.CryptographicFunctions.CertExportCertStore(System.IntPtr,BinaryStudio.PlatformComponents.Win32.CRYPT_BLOB@,System.IntPtr,System.Int32)"/> function. It should be used for its enhanced private key security. The PFX BLOB created by this function is protected by a password.
        /// </summary>
        /// <param name="Store">Handle of the certificate store containing the certificates to be exported.</param>
        /// <param name="PFX">A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure to contain the PFX packet with the exported certificates and keys. If <paramref name="PFX"/>.<see cref="CRYPT_BLOB.Data"/> is NULL, the function calculates the number of bytes needed for the encoded BLOB and returns this in <paramref name="PFX"/>.<see cref="CRYPT_BLOB.Size"/>. When the function is called with <paramref name="PFX"/>.<see cref="CRYPT_BLOB.Data"/> pointing to an allocated buffer of the needed size, the function copies the encoded bytes into the buffer and updates <paramref name="PFX"/>.<see cref="CRYPT_BLOB.Size"/> with the encode byte length.</param>
        /// <param name="Password">String password used to encrypt and verify the PFX packet.</param>
        /// <param name="Flags">
        /// Flag values can be set to any combination of the following.
        ///   <table style="font-family: Consolas;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         EXPORT_PRIVATE_KEYS
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Private keys are exported as well as the certificates.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         REPORT_NO_PRIVATE_KEY
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a certificate is encountered that has no associated private key, the function returns FALSE with the last error set to either <see cref="HRESULT.CRYPT_E_NOT_FOUND"/> or <see cref="HRESULT.NTE_NO_KEY"/>.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         If a certificate is encountered that has a non-exportable private key, the function returns FALSE and the last error set to <see cref="HRESULT.NTE_BAD_KEY"/>, <see cref="HRESULT.NTE_BAD_KEY_STATE"/>, or <see cref="HRESULT.NTE_PERM"/>.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         PKCS12_INCLUDE_EXTENDED_PROPERTIES
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Export all extended properties on the certificate.<br/>
        ///         <b>Windows Server 2003 and Windows XP</b>: This value is not supported.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         PKCS12_PROTECT_TO_DOMAIN_SIDS
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         The PFX BLOB contains an embedded password that will be protected to the Active Directory (AD) protection descriptor pointed to by the <paramref name="Para"/> parameter. If the <paramref name="Password"/> parameter is not NULL or empty, the specified password is protected. If, however, the <paramref name="Password"/> parameter is NULL or an empty string, a random forty (40) character password is created and protected.<br/>
        ///         <see cref="ImportCertStore"/> uses the specified protection descriptor to decrypt the embedded password, whether specified by the user or randomly generated, and then uses the password to decrypt the PFX BLOB.<br/>
        ///         <b>Windows 8 and Windows Server 2012</b>: Support for this flag begins.
        ///       </td>
        ///     </tr>
        ///   </table>
        /// </param>
        /// <returns>
        /// Returns TRUE (nonzero) if the function succeeds, and FALSE (zero) if the function fails.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>.
        /// </returns>
        Boolean CertExportCertStore(IntPtr Store,ref CRYPT_DATA_BLOB PFX,IntPtr Password,Int32 Flags);
        #endregion
        #region M:CertFreeCertificateContext(IntPtr):Boolean
        /// <summary>
        /// The function frees a certificate context by decrementing its reference count. When the reference count goes to zero, <see cref="CertFreeCertificateContext"/> frees the memory used by a certificate context.<br/>
        /// To free a context obtained by a get, duplicate, or create function, call the appropriate free function. To free a context obtained by a find or enumerate function, either pass it in as the previous context parameter to a subsequent invocation of the function, or call the appropriate free function. For more information, see the reference topic for the function that obtains the context.
        /// </summary>
        /// <param name="Context">A pointer to the <see cref="CERT_CONTEXT"/> to be freed.</param>
        /// <returns>The function always returns TRUE.</returns>
        Boolean CertFreeCertificateContext(IntPtr Context);
        #endregion
        #region M:CertGetCertificateContextProperty(IntPtr,CERT_PROP_ID,Byte[],{ref}Int32):Boolean
        /// <summary>
        /// The function retrieves the information contained in an extended property of a certificate context.
        /// </summary>
        /// <param name="Context">A pointer to the <see cref="CERT_CONTEXT"/> structure of the certificate that contains the property to be retrieved.</param>
        /// <param name="PropertyId">The property to be retrieved. Currently defined identifiers and the data type to be returned in <paramref name="Data"/> are listed in the following table.
        ///   <table style="font-family: Consolas;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_ACCESS_STATE_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to a <see cref="Int32"/> value.
        ///         Returns a <see cref="Int32"/> value that indicates whether write operations to the certificate are persisted. The <see cref="Int32"/> value is not set if the certificate is in a memory store or in a registry-based store that is opened as read-only.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_AIA_URL_RETRIEVED_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_ARCHIVED_KEY_HASH_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a previously saved encrypted key hash for the certificate context.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_ARCHIVED_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: NULL. If the <see cref="CertGetCertificateContextProperty"/> function returns true, then the specified property ID exists for the <see cref="CERT_CONTEXT"/>.<br/>
        ///         Indicates the certificate is skipped during enumerations. A certificate with this property set is found with explicit search operations, such as those used to find a certificate with a specific hash or a serial number. No data in <paramref name="Data"/> is associated with this property.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_AUTHORITY_INFO_ACCESS_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_AUTO_ENROLL_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a null-terminated Unicode string that names the certificate type for which the certificate has been auto enrolled.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_AUTO_ENROLL_RETRY_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_BACKED_UP_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_CA_DISABLE_CRL_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Disables certificate revocation list (CRL) retrieval for certificates used by the certification authority (CA). If the CA certificate contains this property, it must also include the CERT_CA_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID property.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_CA_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Contains the list of online certificate status protocol (OCSP) URLs to use for certificates issued by the CA certificate. The array contents are the Abstract Syntax Notation One (ASN.1)-encoded bytes of an X509_AUTHORITY_INFO_ACCESS structure where pszAccessMethod is set to szOID_PKIX_OCSP.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_CROSS_CERT_DIST_POINTS_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Location of the cross certificates. Currently, this identifier is only applicable to certificates and not to CRLs or certificate trust lists (CTLs).<br/>
        ///         The BYTE array contains an ASN.1-encoded CROSS_CERT_DIST_POINTS_INFO structure decoded by using the CryptDecodeObject function with a X509_CROSS_CERT_DIST_POINTS value for the lpszStuctType parameter.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_CTL_USAGE_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns an array of bytes that contain an ASN.1-encoded CTL_USAGE structure.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_DATE_STAMP_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to a <see cref="FILETIME"/> structure.<br/>
        ///         Time when the certificate was added to the store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_DESCRIPTION_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the property displayed by the certificate UI. This property allows the user to describe the certificate's use.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_EFS_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_ENHKEY_USAGE_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns an array of bytes that contain an ASN.1-encoded CERT_ENHKEY_USAGE structure. This structure contains an array of Enhanced Key Usage object identifiers (OIDs), each of which specifies a valid use of the certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_ENROLLMENT_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Enrollment information of the pending request that contains RequestID, CADNSName, CAName, and DisplayName. The data format is defined as follows.
        ///         <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///           <tr>
        ///             <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///               <b>Bytes</b>
        ///             </td>
        ///             <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///               <b>Contents</b>
        ///             </td>
        ///           </tr>
        ///           <tr>
        ///             <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///               First 4 bytes
        ///             </td>
        ///             <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///               Pending request ID
        ///             </td>
        ///           </tr>
        ///           <tr>
        ///             <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///               Next 4 bytes
        ///             </td>
        ///             <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///               CADNSName size, in characters, including the terminating null character, followed by CADNSName string with terminating null character
        ///             </td>
        ///           </tr>
        ///           <tr>
        ///             <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///               Next 4 bytes
        ///             </td>
        ///             <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///               CAName size, in characters, including the terminating null character, followed by CAName string with terminating null character
        ///             </td>
        ///           </tr>
        ///           <tr>
        ///             <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///               Next 4 bytes
        ///             </td>
        ///             <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///               DisplayName size, in characters, including the terminating null character, followed by DisplayName string with terminating null character
        ///             </td>
        ///           </tr>
        ///         </table>
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_EXTENDED_ERROR_INFO_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a null-terminated Unicode character string that contains extended error information for the certificate context.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_FORTEZZA_DATA_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_FRIENDLY_NAME_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a null-terminated Unicode character string that contains the display name for the certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_HASH_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the SHA1 hash. If the hash does not exist, it is computed by using the CryptHashCertificate function.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_HCRYPTPROV_OR_NCRYPT_KEY_HANDLE_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an HCRYPTPROV_OR_NCRYPT_KEY_HANDLE data type.<br/>
        ///         Returns either the HCRYPTPROV or NCRYPT_KEY_HANDLE choice.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_HCRYPTPROV_TRANSFER_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Returns the Cryptography API (CAPI) key handle associated with the certificate. The caller is responsible for freeing the handle. It will not be freed when the context is freed. The property value is removed after after it is returned. If you call this property on a context that has a CNG key, <see cref="HRESULT.CRYPT_E_NOT_FOUND"/> is returned.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_IE30_RESERVED_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_ISSUER_PUBLIC_KEY_MD5_HASH_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         MD5 hash of the public key associated with the private key used to sign this certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_ISSUER_SERIAL_NUMBER_MD5_HASH_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         MD5 hash of the issuer name and serial number from this certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_KEY_CONTEXT_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to a CERT_KEY_CONTEXT structure.<br/>
        ///         Returns a CERT_KEY_CONTEXT structure.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_KEY_IDENTIFIER_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         If nonexistent, searches for the szOID_SUBJECT_KEY_IDENTIFIER extension. If that fails, a SHA1 hash is done on the certificate's SubjectPublicKeyInfo member to produce the identifier values.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_KEY_PROV_HANDLE_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an HCRYPTPROV value.<br/>
        ///         Returns the provider handle obtained from CERT_KEY_CONTEXT_PROP_ID.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_KEY_PROV_INFO_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_KEY_PROV_INFO"/> structure.<br/>
        ///         Returns a pointer to a <see cref="CRYPT_KEY_PROV_INFO"/> structure.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_KEY_SPEC_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to a <see cref="UInt32"/> value.<br/>
        ///         Returns a <see cref="Int32"/> value that specifies the private key obtained from CERT_KEY_CONTEXT_PROP_ID if it exists. Otherwise, if CERT_KEY_PROV_INFO_PROP_ID exists, it is the source of the dwKeySpec.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_MD5_HASH_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the MD5 hash. If the hash does not exist, it is computed by using the CryptHashCertificate function.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_NCRYPT_KEY_HANDLE_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an NCRYPT_KEY_HANDLE data type.<br/>
        ///         Returns a CERT_NCRYPT_KEY_SPEC choice where applicable.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_NCRYPT_KEY_HANDLE_TRANSFER_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Returns the CNG key handle associated with the certificate. The caller is responsible for freeing the handle. It will not be freed when the context is freed. The property value is removed after after it is returned. If you call this property on a context that has a legacy (CAPI) key, <see cref="HRESULT.CRYPT_E_NOT_FOUND"/> is returned.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_NEW_KEY_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_NEXT_UPDATE_LOCATION_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the ASN.1-encoded CERT_ALT_NAME_INFO structure.<br/>
        ///         CERT_NEXT_UPDATE_LOCATION_PROP_ID is currently used only with CTLs.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_NO_AUTO_EXPIRE_CHECK_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_OCSP_CACHE_PREFIX_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_OCSP_RESPONSE_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns an encoded OCSP response for this certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_PUB_KEY_CNG_ALG_BIT_LENGTH_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: Pointer to a null-terminated Unicode string.<br/>
        ///         Returns an L”&lt;PUBKEY&gt;/&lt;BITLENGTH&gt;” string representing the certificate’s public key algorithm and bit length. The following &lt;PUBKEY&gt; algorithms are supported:
        ///         <list type="bullet">
        ///           <item>"RSA" (BCRYPT_RSA_ALGORITHM)</item>
        ///           <item>"DSA" (BCRYPT_DSA_ALGORITHM)</item>
        ///           <item>"ECDSA" (SSL_ECDSA_ALGORITHM)</item>
        ///         </list>
        ///         <b>Windows 8 and Windows Server 2012</b>: Support for this property begins.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_PUBKEY_ALG_PARA_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         For public keys that support algorithm parameter inheritance, returns the ASN.1-encoded PublicKey Algorithm parameters. For Digital Signature Standard (DSS), returns the parameters encoded by using the CryptEncodeObject function. This property is used only if CMS_PKCS7 is defined.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_PUBKEY_HASH_RESERVED_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_PVK_FILE_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a null-terminated Unicode wide character string that contains the file name that contains the private key associated with the certificate's public key.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_RENEWAL_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         DData type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the hash of the renewed certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_REQUEST_ORIGINATOR_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a null-terminated Unicode string that contains the DNS computer name for the origination of the certificate context request.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_ROOT_PROGRAM_CERT_POLICIES_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a pointer to an encoded CERT_POLICIES_INFO structure that contains the application policies of the root certificate for the context. This property can be decoded by using the CryptDecodeObject function with the lpszStructType parameter set to X509_CERT_POLICIES and the dwCertEncodingType parameter set to a combination of X509_ASN_ENCODING bitwise OR PKCS_7_ASN_ENCODING.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_ROOT_PROGRAM_NAME_CONSTRAINTS_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SHA1_HASH_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the SHA1 hash. If the hash does not exist, it is computed by using the CryptHashCertificate function.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SIGN_HASH_CNG_ALG_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: Pointer to a null-terminated Unicode string.<br/>
        ///         Returns the L”&lt;SIGNATURE&gt;/&lt;HASH&gt;” string representing the certificate signature. The &lt;SIGNATURE&gt; value identifies the CNG public key algorithm. The following algorithms are supported:
        ///         <list type="bullet">
        ///           <item>"RSA" (BCRYPT_RSA_ALGORITHM)</item>
        ///           <item>"DSA" (BCRYPT_DSA_ALGORITHM)</item>
        ///           <item>"ECDSA" (SSL_ECDSA_ALGORITHM)</item>
        ///         </list>
        ///         The &lt;HASH&gt; value identifies the CNG hash algorithm. The following algorithms are supported:
        ///         <list type="bullet">
        ///           <item>"MD5" (BCRYPT_MD5_ALGORITHM)</item>
        ///           <item>"SHA1" (BCRYPT_SHA1_ALGORITHM)</item>
        ///           <item>"SHA256" (BCRYPT_SHA256_ALGORITHM)</item>
        ///           <item>"SHA384" (BCRYPT_SHA384_ALGORITHM)</item>
        ///           <item>"SHA512" (BCRYPT_SHA512_ALGORITHM)</item>
        ///         </list>
        ///         The following are common examples:
        ///         <list type="bullet">
        ///           <item>"RSA/SHA1"</item>
        ///           <item>"RSA/SHA256"</item>
        ///           <item>"ECDSA/SHA256"</item>
        ///         </list>
        ///         <b>Windows 7 and Windows Server 2008 R2</b>: Support for this property begins.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SIGNATURE_HASH_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the signature hash. If the hash does not exist, it is computed by using the CryptHashToBeSigned function. The length of the hash is 20 bytes for SHA and 16 for MD5.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SMART_CARD_DATA_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a pointer to encoded smart card data. Prior to calling <see cref="CertGetCertificateContextProperty"/>, you can use this constant to retrieve a smart card certificate by using the <see cref="CertFindCertificateInStore"/> function with the pvFindPara parameter set to CERT_SMART_CARD_DATA_PROP_ID and the dwFindType parameter set to CERT_FIND_PROPERTY.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SMART_CARD_ROOT_INFO_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a pointer to an encoded CRYPT_SMART_CARD_ROOT_INFO structure.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SOURCE_LOCATION_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SOURCE_URL_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SUBJECT_DISABLE_CRL_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SUBJECT_INFO_ACCESS_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the subject information access extension of the certificate context as an encoded CERT_SUBJECT_INFO_ACCESS structure.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SUBJECT_NAME_MD5_HASH_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns an MD5 hash of the encoded subject name of the certificate context.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SUBJECT_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SUBJECT_PUB_KEY_BIT_LENGTH_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: Pointer to a <see cref="Int32"/> value.<br/>
        ///         Returns the length, in bits, of the public key in the certificate.<br/>
        ///         <b>Windows 8 and Windows Server 2012</b>: Support for this property begins.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///         CERT_SUBJECT_PUBLIC_KEY_MD5_HASH_PROP_ID
        ///       </td>
        ///       <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the MD5 hash of this certificate's public key.<br/>
        ///         For all user-defined property identifiers, <paramref name="Data"/> points to an array of <see cref="Byte"/> values.<br/>
        ///         For more information about each property identifier, see the documentation on the dwPropId parameter in CertSetCertificateContextProperty.
        ///       </td>
        ///     </tr>
        ///   </table>
        /// </param>
        /// <param name="Data">
        /// A pointer to a buffer to receive the data as determined by <paramref name="PropertyId"/>. Structures pointed to by members of a structure returned are also returned following the base structure. Therefore, the size contained in <paramref name="Size"/> often exceeds the size of the base structure.<br/>
        /// This parameter can be NULL to set the size of the information for memory allocation purposes.
        /// </param>
        /// <param name="Size">
        /// A pointer to a <see cref="Int32"/> value that specifies the size, in bytes, of the buffer pointed to by the <paramref name="Data"/> parameter. When the function returns, the <see cref="Int32"/> value contains the number of bytes to be stored in the buffer.<br/>
        /// To obtain the required size of a buffer at run time, pass NULL for the <paramref name="Data"/> parameter, and set the value pointed to by this parameter to zero. If the <paramref name="Data"/> parameter is not NULL and the size specified in <paramref name="Size"/> is less than the number of bytes required to contain the data, the function fails, <see cref="LastErrorService.GetLastError"/> returns <see cref="Win32ErrorCode.ERROR_MORE_DATA"/>, and the required size is placed in the variable pointed to by the <paramref name="Size"/> parameter.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> When processing the data returned in the buffer, applications must use the actual size of the data returned. The actual size can be slightly smaller than the size of the buffer specified on input. (On input, buffer sizes are usually specified large enough to ensure that the largest possible output data fits in the buffer.) On output, the variable pointed to by this parameter is updated to reflect the actual size of the data copied to the buffer.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns TRUE.<br/>
        /// If the function fails, it returns FALSE. For extended error information, call <see cref="LastErrorService.GetLastError"/>.
        /// Some possible error codes follow.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        <see cref="HRESULT.CRYPT_E_NOT_FOUND"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The certificate does not have the specified property.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <see cref="Win32ErrorCode.ERROR_MORE_DATA"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        If the buffer specified by the <paramref name="Data"/> parameter is not large enough to hold the returned data, the function sets the <see cref="Win32ErrorCode.ERROR_MORE_DATA"/> code and stores the required buffer size, in bytes, in the variable pointed to by <paramref name="Size"/>.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </returns>
        Boolean CertGetCertificateContextProperty(IntPtr Context,CERT_PROP_ID PropertyId,Byte[] Data, ref Int32 Size);
        #endregion
        #region M:CertRetrieveLogoOrBiometricInfo(IntPtr,String,Int32,Int32,Int32,IntPtr,{out}IntPtr,{out}Int32,{out}IntPtr);
        /// <summary>
        /// The function performs a URL retrieval of logo or biometric information specified in either the szOID_LOGOTYPE_EXT or szOID_BIOMETRIC_EXT certificate extension. The szOID_BIOMETRIC_EXT extension (IETF RFC 3739) supports the addition of a signature or a pictorial representation of the human holder of the certificate. The szOID_LOGOTYPE_EXT extension (IETF RFC 3709) supports the addition of organizational pictorial representations in certificates.
        /// </summary>
        /// <param name="Context">The address of a <see cref="CERT_CONTEXT"/> structure that contains the certificate.</param>
        /// <param name="LogoOrBiometricType">
        /// The address of a null-terminated ANSI string that contains an object identifier (OID) string that identifies the type of information to retrieve.<br/>
        /// This parameter may also contain one of the following predefined values.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_RETRIEVE_ISSUER_LOGO
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Retrieve the certificate issuer logotype.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_RETRIEVE_SUBJECT_LOGO
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Retrieve the certificate subject logotype.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_RETRIEVE_COMMUNITY_LOGO
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Retrieve the certificate community logotype.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_RETRIEVE_BIOMETRIC_PICTURE_TYPE
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Retrieve the picture associated with the certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_RETRIEVE_BIOMETRIC_SIGNATURE_TYPE
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Retrieve the signature associated with the certificate.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="RetrievalFlags">A set of flags that specify how the information should be retrieved. This parameter is passed as the dwRetrievalFlags in the CryptRetrieveObjectByUrl function.</param>
        /// <param name="Timeout">The maximum amount of time, in milliseconds, to wait for the retrieval.</param>
        /// <param name="Flags">This parameter is not used and must be zero.</param>
        /// <param name="Reserved">This parameter is not used and must be <see cref="IntPtr.Zero"/>.</param>
        /// <param name="Data">The address of a BYTE pointer that receives the logotype or biometric data. This memory must be freed when it is no longer needed by passing this pointer to the CryptMemFree function.</param>
        /// <param name="DataSize">The address of a <see cref="Int32"/> variable that receives the number of bytes in the <paramref name="Data"/> buffer.</param>
        /// <param name="MimeType">
        /// The address of a pointer to a null-terminated Unicode string that receives the Multipurpose Internet Mail Extensions (MIME) type of the data. This memory must be freed when it is no longer needed by passing this pointer to the CryptMemFree function.<br/>
        /// This address always receives <see cref="IntPtr.Zero"/> for biometric types. You must always ensure that this parameter contains a valid memory address before attempting to access the memory.
        /// </param>
        /// <returns>
        /// Returns TRUE if successful or FALSE otherwise.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. Possible error codes returned by the <see cref="LastErrorService.GetLastError"/> function include, but are not limited to, the following.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        <see cref="HRESULT.CRYPT_E_HASH_VALUE"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The computed hash value does not match the hash value in the certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <see cref="HRESULT.CRYPT_E_NOT_FOUND"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The certificate does not contain the szOID_LOGOTYPE_EXT or szOID_BIOMETRIC_EXT extension, or the specified lpszLogoOrBiometricType was not found.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        One or more parameters are not valid.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <see cref="Win32ErrorCode.ERROR_INVALID_DATA"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        No data could be retrieved from the URL specified by the certificate extension.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <see cref="Win32ErrorCode.ERROR_NOT_SUPPORTED"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The certificate does not support the required extension.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <see cref="HRESULT.NTE_BAD_ALGID"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The hash algorithm OID is unknown.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </returns>
        Boolean CertRetrieveLogoOrBiometricInfo(IntPtr Context,String LogoOrBiometricType,Int32 RetrievalFlags,Int32 Timeout,Int32 Flags,IntPtr Reserved,out IntPtr Data,out Int32 DataSize,out IntPtr MimeType);
        #endregion
        #region M:CertSerializeCertificateStoreElement(IntPtr,Int32,Byte[],{ref}Int32):Boolean
        /// <summary>
        /// The function serializes a certificate context's encoded certificate and its encoded properties. The result can be persisted to storage so that the certificate and properties can be retrieved at a later time.
        /// </summary>
        /// <param name="Context">A pointer to the <see cref="CERT_CONTEXT"/> to be serialized.</param>
        /// <param name="Flags">Reserved for future use and must be zero.</param>
        /// <param name="Element">
        /// A pointer to a buffer that receives the serialized output, including the encoded certificate and possibly its properties.<br/>
        /// This parameter can be NULL to set the size of this information for memory allocation purposes.
        /// </param>
        /// <param name="ElementSize">A pointer to a <see cref="Int32"/> value specifying the size, in bytes, of the buffer pointed to by the pbElement parameter. When the function returns, <see cref="Int32"/> value contains the number of bytes stored in the buffer.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> When processing the data returned in the buffer, applications must use the actual size of the data returned. The actual size can be slightly smaller than the size of the buffer specified on input. (On input, buffer sizes are usually specified large enough to ensure that the largest possible output data fits in the buffer.) On output, the variable pointed to by this parameter is updated to reflect the actual size of the data copied to the buffer.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is TRUE.<br/>
        /// If the function fails, the return value is FALSE. For extended error information, call <see cref="LastErrorService.GetLastError"/>.
        /// </returns>
        Boolean CertSerializeCertificateStoreElement(IntPtr Context,Int32 Flags,Byte[] Element,ref Int32 ElementSize);
        #endregion
        #region M:CertSetCertificateContextProperty(IntPtr,CERT_PROP_ID,Int32,IntPtr):Boolean
        /// <summary>
        /// The function sets an extended property for a specified certificate context.
        /// </summary>
        /// <param name="Context">A pointer to a <see cref="CERT_CONTEXT"/> structure.</param>
        /// <param name="PropertyIndex">The property to be set. The value of <paramref name="PropertyIndex"/> determines the type and content of the <paramref name="Data"/> parameter. Currently defined identifiers and their related <paramref name="Data"/> types are as follows.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_ACCESS_STATE_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="Int32"/> value.<br/>
        ///        Returns a <see cref="Int32"/> value that indicates whether write operations to the certificate are persisted. The <see cref="Int32"/> value is not set if the certificate is in a memory store or in a registry-based store that is opened as read-only.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_AIA_URL_RETRIEVED_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_ARCHIVED_KEY_HASH_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_HASH_BLOB structure.<br/>
        ///        This property saves an encrypted key hash for the certificate context.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_ARCHIVED_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Indicates that the certificate is skipped during enumerations. A certificate with this property set is still found with explicit search operations, such as finding a certificate with a specific hash or a specific serial number. This property can be set to the empty BLOB, {0,NULL}.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_AUTHORITY_INFO_ACCESS_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_AUTO_ENROLL_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        A property that is set after a certificate has been enrolled by using Auto Enroll. The <see cref="CRYPT_DATA_BLOB"/> structure pointed to by <paramref name="Data"/> includes a null-terminated Unicode name of the certificate type for which the certificate has been auto enrolled. Any subsequent calls to Auto Enroll for the certificate checks for this property to determine whether the certificate has been enrolled.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_AUTO_ENROLL_RETRY_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_BACKED_UP_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_CA_DISABLE_CRL_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Disables certificate revocation list (CRL) retrieval for certificates used by the certification authority (CA). If the CA certificate contains this property, it must also include the CERT_CA_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID property.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_CA_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Contains the list of online certificate status protocol (OCSP) URLs to use for certificates issued by the CA certificate. The array contents are the Abstract Syntax Notation One (ASN.1)-encoded bytes of an X509_AUTHORITY_INFO_ACCESS structure where pszAccessMethod is set to szOID_PKIX_OCSP.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_CROSS_CERT_DIST_POINTS_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Sets the location of the cross certificates. This value is only applicable to certificates and not to certificate revocation lists (CRLs) or certificate trust lists (CTLs). The <see cref="CRYPT_DATA_BLOB"/> structure contains an Abstract Syntax Notation One (ASN.1)-encoded CROSS_CERT_DIST_POINTS_INFO structure that is encoded by using the CryptEncodeObject function with a X509_CROSS_CERT_DIST_POINTS value for the lpszStuctType parameter.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_CTL_USAGE_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure contains an ASN.1-encoded CTL_USAGE structure. This structure is encoded by using the CryptEncodeObject function with the X509_ENHANCED_KEY_USAGE value set.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_DATE_STAMP_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a FILETIME structure.<br/>
        ///        This property sets the time that the certificate was added to the store.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_DESCRIPTION_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        A property that is set and displayed by the certificate UI. This property allows the user to describe the certificate's use.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_EFS_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_ENHKEY_USAGE_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        A property that indicates that the <paramref name="Data"/> parameter points to a <see cref="CRYPT_DATA_BLOB"/> structure that contains an ASN.1-encoded CERT_ENHKEY_USAGE structure. This structure is encoded by using the CryptEncodeObject function with the X509_ENHANCED_KEY_USAGE value set.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_ENROLLMENT_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Enrollment information of the pending request that contains RequestID, CADNSName, CAName, and DisplayName. The data format is defined as follows.
        ///        <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///          <tr>
        ///            <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///              <b>Bytes</b>
        ///            </td>
        ///            <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///              <b>Contents</b>
        ///            </td>
        ///          </tr>
        ///          <tr>
        ///            <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///              First 4 bytes
        ///            </td>
        ///            <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///              Pending request ID
        ///            </td>
        ///          </tr>
        ///          <tr>
        ///            <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///              Next 4 bytes
        ///            </td>
        ///            <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///              CADNSName size, in characters, including the terminating null character, followed by CADNSName string with terminating null character
        ///            </td>
        ///          </tr>
        ///          <tr>
        ///            <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///              Next 4 bytes
        ///            </td>
        ///            <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///              CAName size, in characters, including the terminating null character, followed by CAName string with terminating null character
        ///            </td>
        ///          </tr>
        ///          <tr>
        ///            <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///              Next 4 bytes
        ///            </td>
        ///            <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///              DisplayName size, in characters, including the terminating null character, followed by DisplayName string with terminating null character
        ///            </td>
        ///          </tr>
        ///        </table>
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_EXTENDED_ERROR_INFO_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets a string that contains extended error information for the certificate context.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_FORTEZZA_DATA_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_FRIENDLY_NAME_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure contains the display name of the certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_HASH_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property is implicitly set by a call to the <see cref="CertGetCertificateContextProperty"/> function.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_HCRYPTPROV_OR_NCRYPT_KEY_HANDLE_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to an HCRYPTPROV_OR_NCRYPT_KEY_HANDLE data type.<br/>
        ///        This property calls NCryptIsKeyHandle to determine whether this is an NCRYPT_KEY_HANDLE. For an NCRYPT_KEY_HANDLE, sets CERT_NCRYPT_KEY_HANDLE_PROP_ID; otherwise, it sets CERT_KEY_PROV_HANDLE_PROP_ID.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_HCRYPTPROV_TRANSFER_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Sets the handle of the CAPI key associated with the certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_IE30_RESERVED_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_ISSUER_PUBLIC_KEY_MD5_HASH_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets the MD5 hash of the public key associated with the private key used to sign this certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_ISSUER_SERIAL_NUMBER_MD5_HASH_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure contains the MD5 hash of the issuer name and serial number from this certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_KEY_CONTEXT_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a CERT_KEY_CONTEXT structure.<br/>
        ///        The structure specifies the certificate's private key. It contains both the HCRYPTPROV and key specification for the private key. For more information about the hCryptProv member and dwFlags settings, see CERT_KEY_PROV_HANDLE_PROP_ID, later in this topic.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_KEY_IDENTIFIER_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property is typically implicitly set by a call to the <see cref="CertGetCertificateContextProperty"/> function.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_KEY_PROV_HANDLE_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A HCRYPTPROV value.<br/>
        ///        The HCRYPTPROV handle for the certificate's private key is set. The hCryptProv member of the CERT_KEY_CONTEXT structure is updated if it exists. If it does not exist, it is created with dwKeySpec and initialized by CERT_KEY_PROV_INFO_PROP_ID. If CERT_STORE_NO_CRYPT_RELEASE_FLAG is not set, the hCryptProv value is implicitly released either when the property is set to NULL or on the final freeing of the CERT_CONTEXT structure.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_KEY_PROV_INFO_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_KEY_PROV_INFO structure.<br/>
        ///        The structure specifies the certificate's private key.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_KEY_SPEC_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="Int32"/> value.<br/>
        ///        The <see cref="Int32"/> value that specifies the private key. The dwKeySpec member of the CERT_KEY_CONTEXT structure is updated if it exists. If it does not, it is created with hCryptProv set to zero.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_MD5_HASH_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_HASH_BLOB structure.<br/>
        ///        This property is implicitly set by a call to the <see cref="CertGetCertificateContextProperty"/> function.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_NCRYPT_KEY_HANDLE_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to an NCRYPT_KEY_HANDLE data type.<br/>
        ///        This property sets the NCRYPT_KEY_HANDLE for the certificate private key and sets the dwKeySpec to CERT_NCRYPT_KEY_SPEC.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_NCRYPT_KEY_HANDLE_TRANSFER_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Sets the handle of the CNG key associated with the certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_NEW_KEY_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_NEXT_UPDATE_LOCATION_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure contains an ASN.1-encoded CERT_ALT_NAME_INFO structure that is encoded by using the CryptEncodeObject function with the X509_ALTERNATE_NAME value set.<br/>
        ///        CERT_NEXT_UPDATE_LOCATION_PROP_ID is currently used only with CTLs.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_NO_AUTO_EXPIRE_CHECK_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_OCSP_CACHE_PREFIX_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_OCSP_RESPONSE_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets the encoded online certificate status protocol (OCSP) response from a CERT_SERVER_OCSP_RESPONSE_CONTEXT for this certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_PUB_KEY_CNG_ALG_BIT_LENGTH_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: Pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property is implicitly set by calling the <see cref="CertGetCertificateContextProperty"/> function.<br/>
        ///        <b>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This identifier is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_PUBKEY_ALG_PARA_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_DATA_BLOB structure.<br/>
        ///        This property is used with public keys that support algorithm parameter inheritance. The data BLOB contains the ASN.1-encoded PublicKey Algorithm parameters. For DSS, these are parameters encoded by using the CryptEncodeObject function. This is used only if CMS_PKCS7 is defined.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_PUBKEY_HASH_RESERVED_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_PVK_FILE_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure specifies the name of a file that contains the private key associated with the certificate's public key. Inside the <see cref="CRYPT_DATA_BLOB"/> structure, the pbData member is a pointer to a null-terminated Unicode wide-character string, and the cbData member indicates the length of the string.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_RENEWAL_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property specifies the hash of the renewed certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_REQUEST_ORIGINATOR_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure contains a null-terminated Unicode string that contains the DNS computer name for the origination of the certificate context request.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_ROOT_PROGRAM_CERT_POLICIES_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Returns a pointer to an encoded CERT_POLICIES_INFO structure that contains the application policies of the root certificate for the context. This property can be decoded by using the CryptDecodeObject function with the lpszStructType parameter set to X509_CERT_POLICIES and the dwCertEncodingType parameter set to a combination of X509_ASN_ENCODING bitwise OR PKCS_7_ASN_ENCODING.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_ROOT_PROGRAM_NAME_CONSTRAINTS_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SIGN_HASH_CNG_ALG_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: Pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property is implicitly set by calling the <see cref="CertGetCertificateContextProperty"/> function.<br/>
        ///        <b>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This identifier is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SHA1_HASH_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_HASH_BLOB structure.<br/>
        ///        This property is implicitly set by a call to the <see cref="CertGetCertificateContextProperty"/> function.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SIGNATURE_HASH_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_HASH_BLOB structure.<br/>
        ///        If a signature hash does not exist, it is computed by using the CryptHashToBeSigned function. <paramref name="Data"/> points to an existing or computed hash. Usually, the length of the hash is 20 bytes for SHA and 16 for MD5.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SMART_CARD_DATA_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets the smart card data property of a smart card certificate context.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SMART_CARD_ROOT_INFO_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets the information property of a smart card root certificate context.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SOURCE_LOCATION_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SOURCE_URL_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SUBJECT_DISABLE_CRL_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SUBJECT_INFO_ACCESS_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets the subject information access extension of the certificate context as an encoded CERT_SUBJECT_INFO_ACCESS structure.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SUBJECT_NAME_MD5_HASH_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Returns an MD5 hash of the encoded subject name of the certificate context.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SUBJECT_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SUBJECT_PUB_KEY_BIT_LENGTH_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: Pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property is implicitly set by calling the <see cref="CertGetCertificateContextProperty"/> function.<br/>
        ///        <b>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This identifier is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SUBJECT_PUBLIC_KEY_MD5_HASH_PROP_ID
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets the MD5 hash of this certificate's public key.<br/>
        ///        <paramref name="Data"/> is a pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The user can define additional <paramref name="PropertyIndex"/> types by using <see cref="Int32"/> values from CERT_FIRST_USER_PROP_ID to CERT_LAST_USER_PROP_ID. For all user-defined dwPropId types, <paramref name="Data"/> points to an encoded <see cref="CRYPT_DATA_BLOB"/> structure.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="Flags">
        /// CERT_STORE_NO_CRYPT_RELEASE_FLAG can be set for the CERT_KEY_PROV_HANDLE_PROP_ID or CERT_KEY_CONTEXT_PROP_ID <paramref name="PropertyIndex"/> properties.<br/>
        /// If the CERT_SET_PROPERTY_IGNORE_PERSIST_ERROR_FLAG value is set, any provider-write errors are ignored and the cached context's properties are always set.<br/>
        /// If CERT_SET_PROPERTY_INHIBIT_PERSIST_FLAG is set, any context property set is not persisted.
        /// </param>
        /// <param name="Data">A pointer to a data type determined by the value of <paramref name="PropertyIndex"/>.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> For any <paramref name="PropertyIndex"/>, setting <paramref name="Data"/> to NULL deletes the property.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns TRUE.<br/>
        /// If the function fails, the function returns FALSE. For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The property is not valid. The identifier specified was greater than 0x0000FFFF, or, for the CERT_KEY_CONTEXT_PROP_ID property, a cbSize member that is not valid was specified in the CERT_KEY_CONTEXT structure.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </returns>
        Boolean CertSetCertificateContextProperty(IntPtr Context,CERT_PROP_ID PropertyIndex,Int32 Flags,IntPtr Data);
        #endregion
        #region M:CertSetCertificateContextProperty(IntPtr,CERT_PROP_ID,Int32,{ref}CRYPT_KEY_PROV_INFO):Boolean
        /// <summary>
        /// The function sets an extended property for a specified certificate context.
        /// </summary>
        /// <param name="Context">A pointer to a <see cref="CERT_CONTEXT"/> structure.</param>
        /// <param name="PropertyIndex">The property to be set:
        ///   <list type="bullet">
        ///    <item>CERT_KEY_PROV_INFO_PROP_ID</item>
        ///   </list>
        /// </param>
        /// <param name="Flags">
        /// If the CERT_SET_PROPERTY_IGNORE_PERSIST_ERROR_FLAG value is set, any provider-write errors are ignored and the cached context's properties are always set.<br/>
        /// If CERT_SET_PROPERTY_INHIBIT_PERSIST_FLAG is set, any context property set is not persisted.
        /// </param>
        /// <param name="Value">A reference to <see cref="CRYPT_KEY_PROV_INFO"/>.</param>
        /// <returns>
        /// If the function succeeds, the function returns TRUE.<br/>
        /// If the function fails, the function returns FALSE. For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The property is not valid.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </returns>
        Boolean CertSetCertificateContextProperty(IntPtr Context,CERT_PROP_ID PropertyIndex,Int32 Flags,ref CRYPT_KEY_PROV_INFO Value);
        #endregion
        #region M:CertStrToName(Int32,String,Int32,IntPtr,Byte[],{ref}Int32,IntPtr):Boolean
        /// <summary>
        /// The function converts a null-terminated X.500 string to an encoded certificate name.
        /// </summary>
        /// <param name="CertEncodingType">
        /// The certificate encoding type that was used to encode the string. The message encoding type identifier, contained in the high <see cref="Int16"/> of this value, is ignored by this function.<br/>
        /// This parameter can be the following currently defined certificate encoding type.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        X509_ASN_ENCODING
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Specifies X.509 certificate encoding.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="Name">
        /// A pointer to the null-terminated X.500 string to be converted. The format of this string is specified by the <paramref name="StrType"/> parameter.<br/>
        /// This string is expected to be formatted the same as the output from the CertNameToStr function.
        /// </param>
        /// <param name="StrType">
        /// This parameter specifies the type of the string. This parameter also specifies other options for the contents of the string.<br/>
        /// If no flags are combined with the string type specifier, the string can contain a comma (,) or a semicolon (;) as separators in the relative distinguished name (RDN) and a plus sign (+) as the separator in multiple RDN values.<br/>
        /// Quotation marks ("") are supported. A quotation can be included in a quoted value by using two sets of quotation marks, for example, CN="User ""one""".<br/>
        /// A value that starts with a number sign (#) is treated as ASCII hexadecimal and converted to a CERT_RDN_OCTET_STRING. Embedded white space is ignored. For example, 1.2.3 = # AB CD 01 is the same as 1.2.3=#ABCD01.<br/>
        /// White space that surrounds the keys, object identifiers, and values is ignored.<br/>
        /// This parameter can be one of the following values.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_SIMPLE_NAME_STR
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        This string type is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_OID_NAME_STR
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Validates that the string type is supported. The string can be either an object identifier (OID) or an X.500 name.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_X500_NAME_STR
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Identical to CERT_OID_NAME_STR. Validates that the string type is supported. The string can be either an object identifier (OID) or an X.500 name.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// The following options can also be combined with the value above to specify additional options for the string.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_NAME_STR_COMMA_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Only a comma (,) is supported as the RDN separator.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_NAME_STR_SEMICOLON_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Only a semicolon (;) is supported as the RDN separator.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_NAME_STR_CRLF_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Only a backslash r (\r) or backslash n (\n) is supported as the RDN separator.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_NAME_STR_NO_PLUS_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The plus sign (+) is ignored as a separator, and multiple values per RDN are not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_NAME_STR_NO_QUOTING_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Quoting is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_NAME_STR_REVERSE_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The order of the RDNs in a distinguished name is reversed before encoding. This flag is not set by default.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_NAME_STR_ENABLE_T61_UNICODE_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The CERT_RDN_T61_STRING encoded value type is used instead of CERT_RDN_UNICODE_STRING. This flag can be used if all the Unicode characters are less than or equal to 0xFF.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_NAME_STR_ENABLE_UTF8_UNICODE_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        The CERT_RDN_UTF8_STRING encoded value type is used instead of CERT_RDN_UNICODE_STRING.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_NAME_STR_FORCE_UTF8_DIR_STR_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Forces the X.500 key to be encoded as a UTF-8 (CERT_RDN_UTF8_STRING) string rather than as a printable Unicode (CERT_RDN_PRINTABLE_STRING) string. This is the default value for Microsoft certification authorities beginning with Windows Server 2003.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_NAME_STR_DISABLE_UTF8_DIR_STR_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Prevents forcing a printable Unicode (CERT_RDN_PRINTABLE_STRING) X.500 key to be encoded by using UTF-8 (CERT_RDN_UTF8_STRING). Use to enable encoding of X.500 keys as Unicode values when CERT_NAME_STR_FORCE_UTF8_DIR_STR_FLAG is set.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_NAME_STR_ENABLE_PUNYCODE_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        If the string contains an email RDN value, and the email address contains Unicode characters outside of the ASCII character set, the host name portion of the email address is encoded in Punycode. The resultant email address is then encoded as an IA5String string. The Punycode encoding of the host name is performed on a label-by-label basis.<br/>
        ///        <b>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This value is not supported.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="Reserved">Reserved for future use and must be NULL.</param>
        /// <param name="EncodedBytes">
        /// A pointer to a buffer that receives the encoded structure.<br/>
        /// The size of this buffer is specified in the pcbEncoded parameter.<br/>
        /// This parameter can be NULL to obtain the required size of the buffer for memory allocation purposes.
        /// </param>
        /// <param name="EncodedLength">
        /// A pointer to a <see cref="Int32"/> that, before calling the function, contains the size, in bytes, of the buffer pointed to by the pbEncoded parameter. When the function returns, the <see cref="Int32"/> contains the number of bytes stored in the buffer.<br/>
        /// If <paramref name="EncodedBytes"/> is NULL, the <see cref="Int32"/> receives the size, in bytes, required for the buffer.
        /// </param>
        /// <param name="Error">
        /// A pointer to a string pointer that receives additional error information about an input string that is not valid.<br/>
        /// If the <paramref name="Name"/> string is not valid, <paramref name="Error"/> is updated by this function to point to the beginning of the character sequence that is not valid. If no errors are detected in the input string, <paramref name="Error"/> is set to NULL.<br/>
        /// If this information is not required, pass NULL for this parameter.<br/>
        /// This parameter is updated for the following error codes returned from <see cref="LastErrorService.GetLastError"/>.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CRYPT_E_INVALID_X500_STRING
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CRYPT_E_INVALID_NUMERIC_STRING
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CRYPT_E_INVALID_PRINTABLE_STRING
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CRYPT_E_INVALID_IA5_STRING
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns TRUE.<br/>
        /// If the function fails, the function returns FALSE. For extended error information, call <see cref="LastErrorService.GetLastError"/>.
        /// </returns>
        Boolean CertStrToName(Int32 CertEncodingType,String Name,Int32 StrType,IntPtr Reserved,Byte[] EncodedBytes,ref Int32 EncodedLength,IntPtr Error);
        #endregion
        #region M:CertVerifyCertificateChainPolicy(IntPtr,IntPtr,{ref}CERT_CHAIN_POLICY_PARA,{ref}CERT_CHAIN_POLICY_STATUS):Boolean
        /// <summary>
        /// The function checks a certificate chain to verify its validity, including its compliance with any specified validity policy criteria.
        /// </summary>
        /// <param name="Policy">
        /// Current predefined verify chain policy structures are listed in the following table.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_CHAIN_POLICY_BASE
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Implements the base chain policy verification checks. The <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> member of the structure pointed to by <see cref="CERT_CHAIN_POLICY_PARA.ExtraPolicyPara"/> can be set to alter the default policy checking behavior.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_CHAIN_POLICY_AUTHENTICODE
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Implements the Authenticode chain policy verification checks. The <see cref="CERT_CHAIN_POLICY_PARA.ExtraPolicyPara"/> member of the structure pointed to by <paramref name="PolicyPara"/> can be set to point to an AUTHENTICODE_EXTRA_CERT_CHAIN_POLICY_PARA structure.<br/>
        ///        The <see cref="CERT_CHAIN_POLICY_STATUS.ExtraPolicyStatus"/> member of the structure pointed to by <paramref name="PolicyStatus"/> can be set to point to an AUTHENTICODE_EXTRA_CERT_CHAIN_POLICY_STATUS structure.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_CHAIN_POLICY_AUTHENTICODE_TS
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Implements Authenticode Time Stamp chain policy verification checks. The <see cref="CERT_CHAIN_POLICY_PARA.ExtraPolicyPara"/> member of the data structure pointed to by <paramref name="PolicyPara"/> can be set to point to an AUTHENTICODE_TS_EXTRA_CERT_CHAIN_POLICY_PARA structure.<br/>
        ///        The <see cref="CERT_CHAIN_POLICY_STATUS.ExtraPolicyStatus"/> member of the data structure pointed to by <paramref name="PolicyStatus"/> is not used and must be set to NULL.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_CHAIN_POLICY_SSL
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Implements the SSL client/server chain policy verification checks. The <see cref="CERT_CHAIN_POLICY_PARA.ExtraPolicyPara"/> member in the data structure pointed to by <paramref name="PolicyPara"/> can be set to point to an SSL_EXTRA_CERT_CHAIN_POLICY_PARA structure initialized with additional policy criteria.<br/>
        ///        The <see cref="CERT_CHAIN_POLICY_STATUS.ExtraPolicyStatus"/> member of the data structure pointed to by <paramref name="PolicyStatus"/> is not used and must be set to NULL.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_CHAIN_POLICY_BASIC_CONSTRAINTS
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Implements the basic constraints chain policy. Iterates through all the certificates in the chain checking for either a szOID_BASIC_CONSTRAINTS or a szOID_BASIC_CONSTRAINTS2 extension. If neither extension is present, the certificate is assumed to have valid policy. Otherwise, for the first certificate element, checks if it matches the expected CA_FLAG or END_ENTITY_FLAG specified in the <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> member of the CERT_CHAIN_POLICY_PARA structure pointed to by the <paramref name="PolicyPara"/> parameter. If neither or both flags are set, then, the first element can be either a CA or END_ENTITY. All other elements must be a certification authority (CA). If the PathLenConstraint is present in the extension, it is checked.<br/>
        ///        The first elements in the remaining simple chains (that is, the certificates used to sign the CTL) are checked to be an END_ENTITY. If this verification fails, dwError will be set to TRUST_E_BASIC_CONSTRAINTS.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_CHAIN_POLICY_NT_AUTH
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Implements the Windows NT Authentication chain policy, which consists of three distinct chain verifications in the following order:<br/>
        ///        <list type="number">
        ///          <item>CERT_CHAIN_POLICY_BASE—Implements the base chain policy verification checks. The LOWORD of <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> can be set in <paramref name="PolicyPara"/> to alter the default policy checking behavior. For more information, see CERT_CHAIN_POLICY_BASE.</item>
        ///          <item>CERT_CHAIN_POLICY_BASIC_CONSTRAINTS—Implements the basic constraints chain policy. The HIWORD of <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> can be set to specify if the first element must be either a CA or END_ENTITY. For more information, see CERT_CHAIN_POLICY_BASIC_CONSTRAINTS.</item>
        ///          <item>Checks if the second element in the chain, the CA that issued the end certificate, is a trusted CA for Windows NT Authentication. A CA is considered to be trusted if it exists in the "NTAuth" system registry store found in the CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE store location. If this verification fails, the CA is untrusted, and dwError is set to CERT_E_UNTRUSTEDCA. If CERT_PROT_ROOT_DISABLE_NT_AUTH_REQUIRED_FLAG is set in the Flags value of the HKEY_LOCAL_MACHINE policy ProtectedRoots subkey, defined by CERT_PROT_ROOT_FLAGS_REGPATH and the above check fails, the chain is checked for CERT_TRUST_HAS_VALID_NAME_CONSTRAINTS set in dwInfoStatus. This is set if there was a valid name constraint for all namespaces including UPN. If the chain does not have this info status set, dwError is set to CERT_E_UNTRUSTEDCA.</item>
        ///        </list>
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_CHAIN_POLICY_MICROSOFT_ROOT
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Checks the last element of the first simple chain for a Microsoft root public key. If that element does not contain a Microsoft root public key, the dwError member of the CERT_CHAIN_POLICY_STATUS structure pointed to by the <paramref name="PolicyStatus"/> parameter is set to CERT_E_UNTRUSTEDROOT.<br/>
        ///        The <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> member of the CERT_CHAIN_POLICY_PARA structure pointed to by the <paramref name="PolicyStatus"/> parameter can contain the MICROSOFT_ROOT_CERT_CHAIN_POLICY_CHECK_APPLICATION_ROOT_FLAG flag, which causes this function to instead check for the Microsoft application root "Microsoft Root Certificate Authority 2011".<br/>
        ///        The <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> member of the CERT_CHAIN_POLICY_PARA structure pointed to by the <paramref name="PolicyPara"/> parameter can contain the MICROSOFT_ROOT_CERT_CHAIN_POLICY_ENABLE_TEST_ROOT_FLAG flag, which causes this function to also check for the Microsoft test roots.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_CHAIN_POLICY_EV
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Specifies that extended validation of certificates is performed.<br/>
        ///        <b>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This value is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_CHAIN_POLICY_SSL_F12
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Checks if any certificates in the chain have weak crypto or if third party root certificate compliance and provide an error string. The <see cref="CERT_CHAIN_POLICY_STATUS.ExtraPolicyStatus"/> member of the <see cref="CERT_CHAIN_POLICY_STATUS"/> structure pointed to by the <paramref name="PolicyStatus"/> parameter must point to SSL_F12_EXTRA_CERT_CHAIN_POLICY_STATUS, which is updated with the results of the weak crypto and root program compliance checks.<br/>
        ///        Before calling, the <see cref="CERT_CHAIN_POLICY_STATUS.Size"/> member of the <see cref="CERT_CHAIN_POLICY_STATUS"/> structure pointed to by the <paramref name="PolicyStatus"/> parameter must be set to a value greater than or equal to sizeof(SSL_F12_EXTRA_CERT_CHAIN_POLICY_STATUS).<br/>
        ///        The dwError member in <see cref="CERT_CHAIN_POLICY_STATUS"/> structure pointed to by the <paramref name="PolicyStatus"/> parameter will be set to <see cref="HRESULT.TRUST_E_CERT_SIGNATURE"/> for potential weak crypto and set to <see cref="HRESULT.CERT_E_UNTRUSTEDROOT"/> for Third Party Roots not in compliance with the Microsoft Root Program.<br/>
        ///        <b>Windows 10, version 1607, Windows Server 2016, Windows 10, version 1511 with KB3172985, Windows 10 RTM with KB3163912, Windows 8.1 and Windows Server 2012 R2 with KB3163912, and Windows 7 with SP1 and Windows Server 2008 R2 SP1 with KB3161029</b>
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="ChainContext">A pointer to a <see cref="CERT_CHAIN_CONTEXT"/> structure that contains a chain to be verified.</param>
        /// <param name="PolicyPara">
        /// A pointer to a <see cref="CERT_CHAIN_POLICY_PARA"/> structure that provides the policy verification criteria for the chain. The <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> member of that structure can be set to change the default policy checking behavior.<br/>
        /// In addition, policy-specific parameters can also be passed in the <see cref="CERT_CHAIN_POLICY_PARA.ExtraPolicyPara"/> member of the structure.
        /// </param>
        /// <param name="PolicyStatus">A pointer to a <see cref="CERT_CHAIN_POLICY_STATUS"/> structure where status information on the chain is returned. OID-specific extra status can be returned in the <see cref="CERT_CHAIN_POLICY_STATUS.ExtraPolicyStatus"/> member of this structure.</param>
        /// <returns>
        /// The return value indicates whether the function was able to check for the policy, it does not indicate whether the policy check failed or passed.<br/>
        /// If the chain can be verified for the specified policy, TRUE is returned and the <see cref="CERT_CHAIN_POLICY_STATUS.Error"/> member of the <paramref name="PolicyStatus"/> is updated. A <see cref="CERT_CHAIN_POLICY_STATUS.Error"/> of 0 (<see cref="Win32ErrorCode.ERROR_SUCCESS"/> or <see cref="HRESULT.S_OK"/>) indicates the chain satisfies the specified policy.<br/>
        /// If the chain cannot be validated, the return value is TRUE and you need to verify the <paramref name="PolicyStatus"/> parameter for the actual error.<br/>
        /// A value of FALSE indicates that the function wasn't able to check for the policy.
        /// </returns>
        Boolean CertVerifyCertificateChainPolicy(IntPtr Policy,IntPtr ChainContext,ref CERT_CHAIN_POLICY_PARA PolicyPara,ref CERT_CHAIN_POLICY_STATUS PolicyStatus);
        #endregion
        #region M:CertVerifySubjectCertificateContext(IntPtr,IntPtr,{ref}Int32):Boolean
        /// <summary>
        /// The function performs the enabled verification checks on a certificate by checking the validity of the certificate's issuer.
        /// </summary>
        /// <param name="Subject">A pointer to a <see cref="CERT_CONTEXT"/> structure containing the subject's certificate.</param>
        /// <param name="Issuer">A pointer to a <see cref="CERT_CONTEXT"/> containing the issuer's certificate. When checking just CERT_STORE_TIME_VALIDITY_FLAG, pIssuer can be NULL.</param>
        /// <param name="Flags">
        /// A pointer to a DWORD value contain verification check flags. The following flags can be set to enable verification checks on the subject certificate. They can be combined using a bitwise-OR operation to enable multiple verifications.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        CERT_STORE_REVOCATION_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Checks whether the subject certificate is on the issuer's revocation list.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_STORE_SIGNATURE_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Uses the public key in the issuer's certificate to verify the signature on the subject certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        CERT_STORE_TIME_VALIDITY_FLAG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        Gets the current time and verifies that it is within the subject certificate's validity period.
        ///      </td>
        ///    </tr>
        /// </table>
        /// If an enabled verification check succeeds, its flag is set to zero. If it fails, then its flag is set upon return.<br/>
        /// If CERT_STORE_REVOCATION_FLAG was enabled and the issuer does not have a CRL in the store, then CERT_STORE_NO_CRL_FLAG is set in addition to CERT_STORE_REVOCATION_FLAG.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is TRUE.<br/>
        /// If the function fails, the return value is FALSE.<br/>
        /// For a verification check failure, TRUE is still returned. FALSE is returned only when a bad parameter is passed in.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        ///  <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:40%">
        ///        E_INVALIDARG
        ///      </td>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
        ///        An unsupported bit was set in <paramref name="Flags"/>. Any combination of CERT_STORE_SIGNATURE_FLAG, CERT_STORE_TIME_VALIDITY_FLAG, and CERT_STORE_REVOCATION_FLAG can be set. If <paramref name="Issuer"/> is NULL, only CERT_STORE_TIME_VALIDITY_FLAG can be set.
        ///      </td>
        ///    </tr>
        ///   </table>
        /// </returns>
        Boolean CertVerifySubjectCertificateContext(IntPtr Subject,IntPtr Issuer,ref Int32 Flags);
        #endregion
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
        Boolean CryptSetKeyParam(IntPtr Key,KEY_PARAM Param,IntPtr Data,Int32 Flags);
        Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,Byte[] Data,Int32 Flags);
        Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,Int32 Flags);
        Boolean CryptSignHash(IntPtr Handle, KEY_SPEC_TYPE KeySpec, Byte[] Signature, ref Int32 Length);
        Boolean CryptVerifyCertificateSignature(IntPtr Context,Int32 SubjectType,IntPtr Subject,Int32 IssuerType,IntPtr Issuer,Int32 Flags);
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

        #region M:CertGetSubjectCertificateFromStore(IntPtr,Int32,CERT_INFO*):IntPtr
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
        #endregion
        unsafe IntPtr CertOpenServerOcspResponse(CERT_CHAIN_CONTEXT* ChainContext,Int32 Flags,CERT_SERVER_OCSP_RESPONSE_OPEN_PARA* OpenPara);
        unsafe IntPtr CryptFindOIDInfo(CRYPT_OID_INFO_KEY_TYPE KeyType,void* Key,Int32 GroupId);
        void CertAddRefServerOcspResponseContext(IntPtr ServerOcspResponseContext);
        void CertCloseServerOcspResponse(IntPtr ServerOcspResponse,Int32 Flags);
        }
    }
