using System;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

namespace BinaryStudio.Services
    {
    using CERT_NAME_BLOB = CRYPT_BLOB;
    using CRYPT_DATA_BLOB = CRYPT_BLOB;
    using CRYPT_INTEGER_BLOB = CRYPT_BLOB;
    using DATA_BLOB = CRYPT_BLOB;
    using CRYPT_HASH_BLOB=CRYPT_BLOB;

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
        ///   <table class="table_value_meaning">
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_ALWAYS
        ///       </td>
        ///       <td>
        ///         The function makes no check for an existing matching certificate or link to a matching certificate. A new certificate is always added to the store. This can lead to duplicates in a store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_NEW
        ///       </td>
        ///       <td>
        ///         If a matching certificate or a link to a matching certificate exists, the operation fails. <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_NEWER
        ///       </td>
        ///       <td>
        ///         If a matching certificate or a link to a matching certificate exists and the <b>NotBefore</b> time of the existing context is equal to or greater than the <b>NotBefore</b> time of the new context being added, the operation fails and <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.<br/>
        ///         If the <b>NotBefore</b> time of the existing context is less than the <b>NotBefore</b> time of the new context being added, the existing certificate or link is deleted and a new certificate is created and added to the store. If a matching certificate or a link to a matching certificate does not exist, a new link is added.<br/>
        ///         If <a href="https://learn.microsoft.com/en-us/windows/win32/secgloss/c-gly#:~:text=a%20CERT_REQUEST_INFO%20structure.-,certificate%20revocation%20list,-(CRL)%20A%20document">certificate revocation lists</a> (CRLs) or <a href="https://learn.microsoft.com/en-us/windows/win32/secgloss/c-gly#:~:text=and%20Code%20Signing.-,certificate%20trust%20list,-(CTL)%20A%20predefined">certificate trust list</a> (CTLs) are being compared, the <b>ThisUpdate</b> time is used.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES
        ///       </td>
        ///       <td>
        ///         If a matching certificate or a link to a matching certificate exists and the NotBefore time of the existing context is equal to or greater than the NotBefore time of the new context being added, the operation fails and <see cref="LastErrorService.GetLastError"/> returns the CRYPT_E_EXISTS code.<br/>
        ///         If the NotBefore time of the existing context is less than the NotBefore time of the new context being added, the existing context is deleted before creating and adding the new context. The new added context inherits properties from the existing certificate.<br/>
        ///         If CRLs or CTLs are being compared, the ThisUpdate time is used.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_REPLACE_EXISTING
        ///       </td>
        ///       <td>
        ///         If a link to a matching certificate exists, that existing certificate or link is deleted and a new certificate is created and added to the store. If a matching certificate or a link to a matching certificate does not exist, a new link is added.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES
        ///       </td>
        ///       <td>
        ///         If a matching certificate exists in the store, the existing context is not replaced. The existing context inherits properties from the new certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_USE_EXISTING
        ///       </td>
        ///       <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.CRYPT_E_EXISTS"/>
        ///      </td>
        ///      <td>
        ///        This value is returned if <see cref="CERT_STORE_ADD.CERT_STORE_ADD_NEW"/> is set and the certificate already exists in the store, or if <see cref="CERT_STORE_ADD.CERT_STORE_ADD_NEWER"/> is set and a certificate exists in the store with a <b>NotBefore</b> date greater than or equal to the <b>NotBefore</b> date on the certificate to be added.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td>
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
        ///   <table class="table_value_meaning">
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_ALWAYS
        ///       </td>
        ///       <td>
        ///         The function makes no check for an existing matching certificate or link to a matching certificate. A new certificate is always added to the store. This can lead to duplicates in a store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_NEW
        ///       </td>
        ///       <td>
        ///         If a matching certificate or a link to a matching certificate exists, the operation fails. <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_NEWER
        ///       </td>
        ///       <td>
        ///         If a matching certificate or a link to a matching certificate exists and the <b>NotBefore</b> time of the existing context is equal to or greater than the <b>NotBefore</b> time of the new context being added, the operation fails and <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.<br/>
        ///         If the <b>NotBefore</b> time of the existing context is less than the <b>NotBefore</b> time of the new context being added, the existing certificate or link is deleted and a new certificate is created and added to the store. If a matching certificate or a link to a matching certificate does not exist, a new link is added.<br/>
        ///         If <a href="https://learn.microsoft.com/en-us/windows/win32/secgloss/c-gly#:~:text=a%20CERT_REQUEST_INFO%20structure.-,certificate%20revocation%20list,-(CRL)%20A%20document">certificate revocation lists</a> (CRLs) or <a href="https://learn.microsoft.com/en-us/windows/win32/secgloss/c-gly#:~:text=and%20Code%20Signing.-,certificate%20trust%20list,-(CTL)%20A%20predefined">certificate trust list</a> (CTLs) are being compared, the <b>ThisUpdate</b> time is used.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES
        ///       </td>
        ///       <td>
        ///         If a matching certificate or a link to a matching certificate exists and the NotBefore time of the existing context is equal to or greater than the NotBefore time of the new context being added, the operation fails and <see cref="LastErrorService.GetLastError"/> returns the CRYPT_E_EXISTS code.<br/>
        ///         If the NotBefore time of the existing context is less than the NotBefore time of the new context being added, the existing context is deleted before creating and adding the new context. The new added context inherits properties from the existing certificate.<br/>
        ///         If CRLs or CTLs are being compared, the ThisUpdate time is used.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_REPLACE_EXISTING
        ///       </td>
        ///       <td>
        ///         If a link to a matching certificate exists, that existing certificate or link is deleted and a new certificate is created and added to the store. If a matching certificate or a link to a matching certificate does not exist, a new link is added.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES
        ///       </td>
        ///       <td>
        ///         If a matching certificate exists in the store, the existing context is not replaced. The existing context inherits properties from the new certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_USE_EXISTING
        ///       </td>
        ///       <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.CRYPT_E_EXISTS"/>
        ///      </td>
        ///      <td>
        ///        This value is returned if <see cref="CERT_STORE_ADD.CERT_STORE_ADD_NEW"/> is set and the certificate already exists in the store, or if <see cref="CERT_STORE_ADD.CERT_STORE_ADD_NEWER"/> is set and a certificate exists in the store with a <b>NotBefore</b> date greater than or equal to the <b>NotBefore</b> date on the certificate to be added.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td>
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
        ///   <table class="table_value_meaning">
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_ALWAYS
        ///       </td>
        ///       <td>
        ///         The function makes no check for an existing matching certificate or link to a matching certificate. A new certificate is always added to the store. This can lead to duplicates in a store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_NEW
        ///       </td>
        ///       <td>
        ///         If a matching certificate or a link to a matching certificate exists, the operation fails. <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_REPLACE_EXISTING
        ///       </td>
        ///       <td>
        ///         If a link to a matching certificate exists, that existing link is deleted and a new link is created and added to the store. If no matching certificate or link to a matching certificate exists, one is added.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_USE_EXISTING
        ///       </td>
        ///       <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.CRYPT_E_EXISTS"/>
        ///      </td>
        ///      <td>
        ///        For a <paramref name="Disposition"/> parameter of <see cref="CERT_STORE_ADD.CERT_STORE_ADD_NEW"/>, the certificate already exists in the store.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td>
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
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_ADD_ALWAYS
        ///     </td>
        ///     <td>
        ///       Makes no check for an existing matching CRL or link to a matching CRL.
        ///       A new CRL is always added to the store.
        ///       This can lead to duplicates in a store.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_ADD_NEW
        ///     </td>
        ///     <td>
        ///       If a matching CRL or a link to a matching CRL exists, the operation fails.
        ///       <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_ADD_NEWER
        ///     </td>
        ///     <td>
        ///       If a matching CRL or a link to a matching CRL exists, the function compares the <see cref="CRL_INFO.ThisUpdate"/> times on the CRLs.
        ///       If the existing CRL has a <see cref="CRL_INFO.ThisUpdate"/> time less than the <see cref="CRL_INFO.ThisUpdate"/> time on the new CRL, the old CRL or link is replaced just as with <b>CERT_STORE_ADD_REPLACE_EXISTING</b>.<br/>
        ///       If the existing CRL has a <see cref="CRL_INFO.ThisUpdate"/> time greater than or equal to the <see cref="CRL_INFO.ThisUpdate"/> time on the CRL to be added, the function fails with <see cref="LastErrorService.GetLastError"/> returning the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.<br/>
        ///       If a matching CRL or a link to a matching CRL is not found in the store, a new CRL is added to the store.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES
        ///     </td>
        ///     <td>
        ///       The action is the same as for CERT_STORE_ADD_NEWER, except that if an older CRL is replaced, the properties of the older CRL are incorporated into the replacement CRL.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_ADD_REPLACE_EXISTING
        ///     </td>
        ///     <td>
        ///       If a matching CRL or a link to a matching CRL exists, the existing CRL or link is deleted and a new CRL is created and added to the store.<br/>
        ///       If a matching CRL or a link to a matching CRL does not exist, one is added.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES
        ///     </td>
        ///     <td>
        ///       If a matching CRL exists in the store, the existing context is deleted before creating and adding the new context.
        ///       The added context inherits properties from the existing CRL.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_ADD_USE_EXISTING
        ///     </td>
        ///     <td>
        ///       If a matching CRL or a link to a matching CRL exists, that existing CRL is used and properties from the new CRL are added.
        ///       The function does not fail, but no new CRL is added. The existing context is duplicated.<br/>
        ///       If a matching CRL or a link to a matching CRL does not exist, a new CRL is added.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is TRUE.<br/>
        /// If the function fails, the return value is FALSE. Errors from the called functions <see cref="CertAddEncodedCRLToStore"/> and <see cref="CertSetCRLContextProperty"/> can be propagated to this function.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. Some possible error codes follow.
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.CRYPT_E_EXISTS"/>
        ///      </td>
        ///      <td>
        ///        This error is returned if CERT_STORE_ADD_NEW is set and the CRL already exists in the store or if CERT_STORE_ADD_NEWER is set and a CRL exists in the store with a <see cref="CRL_INFO.ThisUpdate"/> date greater than or equal to the <see cref="CRL_INFO.ThisUpdate"/> date on the CRL to be added.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td>
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
        ///   <table class="table_value_meaning">
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_ALWAYS
        ///       </td>
        ///       <td>
        ///         Makes no check for an existing matching CRL or link to a matching CRL. A new CRL is always added to the store. This can lead to duplicates in a store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_NEW
        ///       </td>
        ///       <td>
        ///         If a matching CRL or a link to a matching CRL exists, the operation fails. <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_NEWER
        ///       </td>
        ///       <td>
        ///         If a matching CRL or a link to a matching CRL exists, the function compares the <see cref="CRL_INFO.ThisUpdate"/> times on the CRLs. If the existing CRL has a <see cref="CRL_INFO.ThisUpdate"/> time less than the <see cref="CRL_INFO.ThisUpdate"/> time on the new CRL, the old CRL or link is replaced just as with CERT_STORE_ADD_REPLACE_EXISTING.<br/>
        ///         If the existing CRL has a <see cref="CRL_INFO.ThisUpdate"/> time greater than or equal to the <see cref="CRL_INFO.ThisUpdate"/> time on the CRL to be added, the function fails with <see cref="LastErrorService.GetLastError"/> returning the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.<br/>
        ///         If a matching CRL or a link to a matching CRL is not found in the store, a new CRL is added to the store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES
        ///       </td>
        ///       <td>
        ///         The action is the same as for CERT_STORE_ADD_NEWER, except that if an older CRL is replaced, the properties of the older CRL are incorporated into the replacement CRL.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_REPLACE_EXISTING
        ///       </td>
        ///       <td>
        ///         If a matching CRL or a link to a matching CRL exists, the existing CRL or link is deleted and a new CRL is created and added to the store.<br/>
        ///         If a matching CRL or a link to a matching CRL does not exist, one is added.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES
        ///       </td>
        ///       <td>
        ///         If a matching CRL exists in the store, the existing context is deleted before creating and adding the new context. The added context inherits properties from the existing CRL.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_USE_EXISTING
        ///       </td>
        ///       <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.CRYPT_E_EXISTS"/>
        ///      </td>
        ///      <td>
        ///        This error is returned if CERT_STORE_ADD_NEW is set and the CRL already exists in the store or if CERT_STORE_ADD_NEWER is set and a CRL exists in the store with a <see cref="CRL_INFO.ThisUpdate"/> date greater than or equal to the <see cref="CRL_INFO.ThisUpdate"/> date on the CRL to be added.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td>
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
        ///   <table class="table_value_meaning">
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_ALWAYS
        ///       </td>
        ///       <td>
        ///         The function makes no check for an existing matching certificate or link to a matching certificate. A new certificate is always added to the store. This can lead to duplicates in a store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_NEW
        ///       </td>
        ///       <td>
        ///         If a matching certificate or a link to a matching certificate exists in the store, the operation fails. <see cref="LastErrorService.GetLastError"/> returns the <see cref="HRESULT.CRYPT_E_EXISTS"/> code.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_REPLACE_EXISTING
        ///       </td>
        ///       <td>
        ///         If a matching certificate or link to a matching certificate exists in the store, the existing certificate or link is deleted and a new certificate is created and added to the store. If a matching certificate or link to a matching certificate does not exist, a new certificate is created and added to the store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES
        ///       </td>
        ///       <td>
        ///         If a matching certificate exists in the store, that existing context is deleted before creating and adding the new context. The new context inherits properties from the existing certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_STORE_ADD_USE_EXISTING
        ///       </td>
        ///       <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.CRYPT_E_EXISTS"/>
        ///      </td>
        ///      <td>
        ///        This code is returned if CERT_STORE_ADD_NEW is set and the certificate already exists in the store, or if CERT_STORE_ADD_NEWER is set and there is a certificate in the store with a <see cref="CERT_INFO.NotBefore"/> date greater than or equal to the <see cref="CERT_INFO.NotBefore"/> date on the certificate to be added.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td>
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
        ///   <table class="table_value_meaning">
        ///     <tr>
        ///       <td>
        ///         CERT_CLOSE_STORE_CHECK_FLAG
        ///       </td>
        ///       <td>
        ///         Checks for nonfreed certificate, CRL, and CTL contexts. A returned error code indicates that one or more store elements is still in use. This flag should only be used as a diagnostic tool in the development of applications.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_CLOSE_STORE_FORCE_FLAG
        ///       </td>
        ///       <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.E_ACCESSDENIED"/>
        ///      </td>
        ///      <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.E_ACCESSDENIED"/>
        ///      </td>
        ///      <td>
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
        /// Beginning with Windows 10 1709 (Fall Creators update) and Windows Server 2019, if the <paramref name="Flags"/> parameter contains PKCS12_EXPORT_PBES2_PARAMS, you should set the pvPara to an PKCS12_EXPORT_PBES2_PARAMS value to select the password-based encryption algorithm to use.
        /// </param>
        /// <param name="Flags">
        /// Flag values can be set to any combination of the following.
        ///   <table class="table_value_meaning">
        ///     <tr>
        ///       <td>
        ///         EXPORT_PRIVATE_KEYS
        ///       </td>
        ///       <td>
        ///         Private keys are exported as well as the certificates.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         REPORT_NO_PRIVATE_KEY
        ///       </td>
        ///       <td>
        ///         If a certificate is encountered that has no associated private key, the function returns FALSE with the last error set to either <see cref="HRESULT.CRYPT_E_NOT_FOUND"/> or <see cref="HRESULT.NTE_NO_KEY"/>.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY
        ///       </td>
        ///       <td>
        ///         If a certificate is encountered that has a non-exportable private key, the function returns FALSE and the last error set to <see cref="HRESULT.NTE_BAD_KEY"/>, <see cref="HRESULT.NTE_BAD_KEY_STATE"/>, or <see cref="HRESULT.NTE_PERM"/>.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         PKCS12_INCLUDE_EXTENDED_PROPERTIES
        ///       </td>
        ///       <td>
        ///         Export all extended properties on the certificate.<br/>
        ///         <b>Windows Server 2003 and Windows XP</b>: This value is not supported.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         PKCS12_PROTECT_TO_DOMAIN_SIDS
        ///       </td>
        ///       <td>
        ///         The PFX BLOB contains an embedded password that will be protected to the Active Directory (AD) protection descriptor pointed to by the <paramref name="Para"/> parameter. If the <paramref name="Password"/> parameter is not NULL or empty, the specified password is protected. If, however, the <paramref name="Password"/> parameter is NULL or an empty string, a random forty (40) character password is created and protected.<br/>
        ///         <see cref="ImportCertStore"/> uses the specified protection descriptor to decrypt the embedded password, whether specified by the user or randomly generated, and then uses the password to decrypt the PFX BLOB.<br/>
        ///         <b>Windows 8 and Windows Server 2012</b>: Support for this flag begins.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         PKCS12_EXPORT_PBES2_PARAMS
        ///       </td>
        ///       <td>
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
        ///   <table class="table_value_meaning">
        ///     <tr>
        ///       <td>
        ///         EXPORT_PRIVATE_KEYS
        ///       </td>
        ///       <td>
        ///         Private keys are exported as well as the certificates.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         REPORT_NO_PRIVATE_KEY
        ///       </td>
        ///       <td>
        ///         If a certificate is encountered that has no associated private key, the function returns FALSE with the last error set to either <see cref="HRESULT.CRYPT_E_NOT_FOUND"/> or <see cref="HRESULT.NTE_NO_KEY"/>.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY
        ///       </td>
        ///       <td>
        ///         If a certificate is encountered that has a non-exportable private key, the function returns FALSE and the last error set to <see cref="HRESULT.NTE_BAD_KEY"/>, <see cref="HRESULT.NTE_BAD_KEY_STATE"/>, or <see cref="HRESULT.NTE_PERM"/>.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         PKCS12_INCLUDE_EXTENDED_PROPERTIES
        ///       </td>
        ///       <td>
        ///         Export all extended properties on the certificate.<br/>
        ///         <b>Windows Server 2003 and Windows XP</b>: This value is not supported.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         PKCS12_PROTECT_TO_DOMAIN_SIDS
        ///       </td>
        ///       <td>
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
        #region M:CertFreeCRLContext(IntPtr):Boolean
        /// <summary>
        /// This function frees a certificate revocation list (CRL) context by decrementing its reference count.
        /// When the reference count goes to zero, <b>CertFreeCRLContext</b> frees the memory used by a CRL context.<br/>
        /// To free a context obtained by a get, duplicate, or create function, call the appropriate free function.
        /// To free a context obtained by a find or enumerate function, either pass it in as the previous context parameter to a subsequent invocation of the function, or call the appropriate free function.
        /// For more information, see the reference topic for the function that obtains the context.
        /// </summary>
        /// <param name="Context">A pointer to the <see cref="CRL_CONTEXT"/> to be freed.</param>
        /// <returns>The function always returns TRUE.</returns>
        Boolean CertFreeCRLContext(IntPtr Context);
        #endregion
        #region M:CertGetCertificateContextProperty(IntPtr,CERT_PROP_ID,Byte[],{ref}Int32):Boolean
        /// <summary>
        /// The function retrieves the information contained in an extended property of a certificate context.
        /// </summary>
        /// <param name="Context">A pointer to the <see cref="CERT_CONTEXT"/> structure of the certificate that contains the property to be retrieved.</param>
        /// <param name="PropertyId">The property to be retrieved. Currently defined identifiers and the data type to be returned in <paramref name="Data"/> are listed in the following table.
        ///   <table class="table_value_meaning">
        ///     <tr>
        ///       <td>
        ///         CERT_ACCESS_STATE_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to a <see cref="Int32"/> value.
        ///         Returns a <see cref="Int32"/> value that indicates whether write operations to the certificate are persisted. The <see cref="Int32"/> value is not set if the certificate is in a memory store or in a registry-based store that is opened as read-only.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_AIA_URL_RETRIEVED_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_ARCHIVED_KEY_HASH_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a previously saved encrypted key hash for the certificate context.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_ARCHIVED_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: NULL. If the <see cref="CertGetCertificateContextProperty"/> function returns true, then the specified property ID exists for the <see cref="CERT_CONTEXT"/>.<br/>
        ///         Indicates the certificate is skipped during enumerations. A certificate with this property set is found with explicit search operations, such as those used to find a certificate with a specific hash or a serial number. No data in <paramref name="Data"/> is associated with this property.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_AUTHORITY_INFO_ACCESS_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_AUTO_ENROLL_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a null-terminated Unicode string that names the certificate type for which the certificate has been auto enrolled.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_AUTO_ENROLL_RETRY_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_BACKED_UP_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_CA_DISABLE_CRL_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Disables certificate revocation list (CRL) retrieval for certificates used by the certification authority (CA). If the CA certificate contains this property, it must also include the CERT_CA_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID property.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_CA_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Contains the list of online certificate status protocol (OCSP) URLs to use for certificates issued by the CA certificate. The array contents are the Abstract Syntax Notation One (ASN.1)-encoded bytes of an X509_AUTHORITY_INFO_ACCESS structure where pszAccessMethod is set to szOID_PKIX_OCSP.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_CROSS_CERT_DIST_POINTS_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Location of the cross certificates. Currently, this identifier is only applicable to certificates and not to CRLs or certificate trust lists (CTLs).<br/>
        ///         The <b>BYTE</b> array contains an ASN.1-encoded CROSS_CERT_DIST_POINTS_INFO structure decoded by using the CryptDecodeObject function with a X509_CROSS_CERT_DIST_POINTS value for the lpszStuctType parameter.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_CTL_USAGE_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns an array of bytes that contain an ASN.1-encoded CTL_USAGE structure.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_DATE_STAMP_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to a <see cref="FILETIME"/> structure.<br/>
        ///         Time when the certificate was added to the store.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_DESCRIPTION_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the property displayed by the certificate UI. This property allows the user to describe the certificate's use.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_EFS_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_ENHKEY_USAGE_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns an array of bytes that contain an ASN.1-encoded CERT_ENHKEY_USAGE structure. This structure contains an array of Enhanced Key Usage object identifiers (OIDs), each of which specifies a valid use of the certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_ENROLLMENT_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Enrollment information of the pending request that contains RequestID, CADNSName, CAName, and DisplayName. The data format is defined as follows.
        ///         <table class="table_value_meaning">
        ///           <tr>
        ///             <td>
        ///               <b>Bytes</b>
        ///             </td>
        ///             <td>
        ///               <b>Contents</b>
        ///             </td>
        ///           </tr>
        ///           <tr>
        ///             <td>
        ///               First 4 bytes
        ///             </td>
        ///             <td>
        ///               Pending request ID
        ///             </td>
        ///           </tr>
        ///           <tr>
        ///             <td>
        ///               Next 4 bytes
        ///             </td>
        ///             <td>
        ///               CADNSName size, in characters, including the terminating null character, followed by CADNSName string with terminating null character
        ///             </td>
        ///           </tr>
        ///           <tr>
        ///             <td>
        ///               Next 4 bytes
        ///             </td>
        ///             <td>
        ///               CAName size, in characters, including the terminating null character, followed by CAName string with terminating null character
        ///             </td>
        ///           </tr>
        ///           <tr>
        ///             <td>
        ///               Next 4 bytes
        ///             </td>
        ///             <td>
        ///               DisplayName size, in characters, including the terminating null character, followed by DisplayName string with terminating null character
        ///             </td>
        ///           </tr>
        ///         </table>
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_EXTENDED_ERROR_INFO_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a null-terminated Unicode character string that contains extended error information for the certificate context.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_FORTEZZA_DATA_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_FRIENDLY_NAME_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a null-terminated Unicode character string that contains the display name for the certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_HASH_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the SHA1 hash. If the hash does not exist, it is computed by using the CryptHashCertificate function.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_HCRYPTPROV_OR_NCRYPT_KEY_HANDLE_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an HCRYPTPROV_OR_NCRYPT_KEY_HANDLE data type.<br/>
        ///         Returns either the HCRYPTPROV or NCRYPT_KEY_HANDLE choice.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_HCRYPTPROV_TRANSFER_PROP_ID
        ///       </td>
        ///       <td>
        ///         Returns the Cryptography API (CAPI) key handle associated with the certificate. The caller is responsible for freeing the handle. It will not be freed when the context is freed. The property value is removed after after it is returned. If you call this property on a context that has a CNG key, <see cref="HRESULT.CRYPT_E_NOT_FOUND"/> is returned.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_IE30_RESERVED_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_ISSUER_PUBLIC_KEY_MD5_HASH_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         MD5 hash of the public key associated with the private key used to sign this certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_ISSUER_SERIAL_NUMBER_MD5_HASH_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         MD5 hash of the issuer name and serial number from this certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_KEY_CONTEXT_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to a CERT_KEY_CONTEXT structure.<br/>
        ///         Returns a CERT_KEY_CONTEXT structure.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_KEY_IDENTIFIER_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         If nonexistent, searches for the szOID_SUBJECT_KEY_IDENTIFIER extension. If that fails, a SHA1 hash is done on the certificate's SubjectPublicKeyInfo member to produce the identifier values.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_KEY_PROV_HANDLE_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an HCRYPTPROV value.<br/>
        ///         Returns the provider handle obtained from CERT_KEY_CONTEXT_PROP_ID.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_KEY_PROV_INFO_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_KEY_PROV_INFO"/> structure.<br/>
        ///         Returns a pointer to a <see cref="CRYPT_KEY_PROV_INFO"/> structure.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_KEY_SPEC_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to a <see cref="UInt32"/> value.<br/>
        ///         Returns a <see cref="Int32"/> value that specifies the private key obtained from CERT_KEY_CONTEXT_PROP_ID if it exists. Otherwise, if CERT_KEY_PROV_INFO_PROP_ID exists, it is the source of the dwKeySpec.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_MD5_HASH_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the MD5 hash. If the hash does not exist, it is computed by using the CryptHashCertificate function.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_NCRYPT_KEY_HANDLE_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an NCRYPT_KEY_HANDLE data type.<br/>
        ///         Returns a CERT_NCRYPT_KEY_SPEC choice where applicable.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_NCRYPT_KEY_HANDLE_TRANSFER_PROP_ID
        ///       </td>
        ///       <td>
        ///         Returns the CNG key handle associated with the certificate. The caller is responsible for freeing the handle. It will not be freed when the context is freed. The property value is removed after after it is returned. If you call this property on a context that has a legacy (CAPI) key, <see cref="HRESULT.CRYPT_E_NOT_FOUND"/> is returned.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_NEW_KEY_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_NEXT_UPDATE_LOCATION_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the ASN.1-encoded CERT_ALT_NAME_INFO structure.<br/>
        ///         CERT_NEXT_UPDATE_LOCATION_PROP_ID is currently used only with CTLs.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_NO_AUTO_EXPIRE_CHECK_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_OCSP_CACHE_PREFIX_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_OCSP_RESPONSE_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns an encoded OCSP response for this certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_PUB_KEY_CNG_ALG_BIT_LENGTH_PROP_ID
        ///       </td>
        ///       <td>
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
        ///       <td>
        ///         CERT_PUBKEY_ALG_PARA_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         For public keys that support algorithm parameter inheritance, returns the ASN.1-encoded PublicKey Algorithm parameters. For Digital Signature Standard (DSS), returns the parameters encoded by using the CryptEncodeObject function. This property is used only if CMS_PKCS7 is defined.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_PUBKEY_HASH_RESERVED_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_PVK_FILE_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a null-terminated Unicode wide character string that contains the file name that contains the private key associated with the certificate's public key.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_RENEWAL_PROP_ID
        ///       </td>
        ///       <td>
        ///         DData type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the hash of the renewed certificate.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_REQUEST_ORIGINATOR_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a null-terminated Unicode string that contains the DNS computer name for the origination of the certificate context request.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_ROOT_PROGRAM_CERT_POLICIES_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a pointer to an encoded CERT_POLICIES_INFO structure that contains the application policies of the root certificate for the context. This property can be decoded by using the CryptDecodeObject function with the lpszStructType parameter set to X509_CERT_POLICIES and the dwCertEncodingType parameter set to a combination of X509_ASN_ENCODING bitwise OR PKCS_7_ASN_ENCODING.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_ROOT_PROGRAM_NAME_CONSTRAINTS_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SHA1_HASH_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the SHA1 hash. If the hash does not exist, it is computed by using the CryptHashCertificate function.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SIGN_HASH_CNG_ALG_PROP_ID
        ///       </td>
        ///       <td>
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
        ///       <td>
        ///         CERT_SIGNATURE_HASH_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the signature hash. If the hash does not exist, it is computed by using the CryptHashToBeSigned function. The length of the hash is 20 bytes for SHA and 16 for MD5.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SMART_CARD_DATA_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a pointer to encoded smart card data. Prior to calling <see cref="CertGetCertificateContextProperty"/>, you can use this constant to retrieve a smart card certificate by using the <see cref="CertFindCertificateInStore"/> function with the pvFindPara parameter set to CERT_SMART_CARD_DATA_PROP_ID and the dwFindType parameter set to CERT_FIND_PROPERTY.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SMART_CARD_ROOT_INFO_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns a pointer to an encoded CRYPT_SMART_CARD_ROOT_INFO structure.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SOURCE_LOCATION_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SOURCE_URL_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SUBJECT_DISABLE_CRL_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SUBJECT_INFO_ACCESS_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns the subject information access extension of the certificate context as an encoded CERT_SUBJECT_INFO_ACCESS structure.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SUBJECT_NAME_MD5_HASH_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: A pointer to an array of <see cref="Byte"/> values. The size of this array is specified in the <paramref name="Size"/> parameter.<br/>
        ///         Returns an MD5 hash of the encoded subject name of the certificate context.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SUBJECT_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID
        ///       </td>
        ///       <td>
        ///         This identifier is reserved.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SUBJECT_PUB_KEY_BIT_LENGTH_PROP_ID
        ///       </td>
        ///       <td>
        ///         Data type of <paramref name="Data"/>: Pointer to a <see cref="Int32"/> value.<br/>
        ///         Returns the length, in bits, of the public key in the certificate.<br/>
        ///         <b>Windows 8 and Windows Server 2012</b>: Support for this property begins.
        ///       </td>
        ///     </tr>
        ///     <tr>
        ///       <td>
        ///         CERT_SUBJECT_PUBLIC_KEY_MD5_HASH_PROP_ID
        ///       </td>
        ///       <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.CRYPT_E_NOT_FOUND"/>
        ///      </td>
        ///      <td>
        ///        The certificate does not have the specified property.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="Win32ErrorCode.ERROR_MORE_DATA"/>
        ///      </td>
        ///      <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        CERT_RETRIEVE_ISSUER_LOGO
        ///      </td>
        ///      <td>
        ///        Retrieve the certificate issuer logotype.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_RETRIEVE_SUBJECT_LOGO
        ///      </td>
        ///      <td>
        ///        Retrieve the certificate subject logotype.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_RETRIEVE_COMMUNITY_LOGO
        ///      </td>
        ///      <td>
        ///        Retrieve the certificate community logotype.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_RETRIEVE_BIOMETRIC_PICTURE_TYPE
        ///      </td>
        ///      <td>
        ///        Retrieve the picture associated with the certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_RETRIEVE_BIOMETRIC_SIGNATURE_TYPE
        ///      </td>
        ///      <td>
        ///        Retrieve the signature associated with the certificate.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="RetrievalFlags">A set of flags that specify how the information should be retrieved. This parameter is passed as the dwRetrievalFlags in the CryptRetrieveObjectByUrl function.</param>
        /// <param name="Timeout">The maximum amount of time, in milliseconds, to wait for the retrieval.</param>
        /// <param name="Flags">This parameter is not used and must be zero.</param>
        /// <param name="Reserved">This parameter is not used and must be <see cref="IntPtr.Zero"/>.</param>
        /// <param name="Data">The address of a <b>BYTE</b> pointer that receives the logotype or biometric data. This memory must be freed when it is no longer needed by passing this pointer to the CryptMemFree function.</param>
        /// <param name="DataSize">The address of a <see cref="Int32"/> variable that receives the number of bytes in the <paramref name="Data"/> buffer.</param>
        /// <param name="MimeType">
        /// The address of a pointer to a null-terminated Unicode string that receives the Multipurpose Internet Mail Extensions (MIME) type of the data. This memory must be freed when it is no longer needed by passing this pointer to the CryptMemFree function.<br/>
        /// This address always receives <see cref="IntPtr.Zero"/> for biometric types. You must always ensure that this parameter contains a valid memory address before attempting to access the memory.
        /// </param>
        /// <returns>
        /// Returns TRUE if successful or FALSE otherwise.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. Possible error codes returned by the <see cref="LastErrorService.GetLastError"/> function include, but are not limited to, the following.
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.CRYPT_E_HASH_VALUE"/>
        ///      </td>
        ///      <td>
        ///        The computed hash value does not match the hash value in the certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.CRYPT_E_NOT_FOUND"/>
        ///      </td>
        ///      <td>
        ///        The certificate does not contain the szOID_LOGOTYPE_EXT or szOID_BIOMETRIC_EXT extension, or the specified lpszLogoOrBiometricType was not found.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td>
        ///        One or more parameters are not valid.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="Win32ErrorCode.ERROR_INVALID_DATA"/>
        ///      </td>
        ///      <td>
        ///        No data could be retrieved from the URL specified by the certificate extension.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="Win32ErrorCode.ERROR_NOT_SUPPORTED"/>
        ///      </td>
        ///      <td>
        ///        The certificate does not support the required extension.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.NTE_BAD_ALGID"/>
        ///      </td>
        ///      <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        CERT_ACCESS_STATE_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="Int32"/> value.<br/>
        ///        Returns a <see cref="Int32"/> value that indicates whether write operations to the certificate are persisted. The <see cref="Int32"/> value is not set if the certificate is in a memory store or in a registry-based store that is opened as read-only.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_AIA_URL_RETRIEVED_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_ARCHIVED_KEY_HASH_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_HASH_BLOB structure.<br/>
        ///        This property saves an encrypted key hash for the certificate context.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_ARCHIVED_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Indicates that the certificate is skipped during enumerations. A certificate with this property set is still found with explicit search operations, such as finding a certificate with a specific hash or a specific serial number. This property can be set to the empty BLOB, {0,NULL}.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_AUTHORITY_INFO_ACCESS_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_AUTO_ENROLL_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        A property that is set after a certificate has been enrolled by using Auto Enroll. The <see cref="CRYPT_DATA_BLOB"/> structure pointed to by <paramref name="Data"/> includes a null-terminated Unicode name of the certificate type for which the certificate has been auto enrolled. Any subsequent calls to Auto Enroll for the certificate checks for this property to determine whether the certificate has been enrolled.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_AUTO_ENROLL_RETRY_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_BACKED_UP_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CA_DISABLE_CRL_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Disables certificate revocation list (CRL) retrieval for certificates used by the certification authority (CA). If the CA certificate contains this property, it must also include the CERT_CA_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID property.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CA_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Contains the list of online certificate status protocol (OCSP) URLs to use for certificates issued by the CA certificate. The array contents are the Abstract Syntax Notation One (ASN.1)-encoded bytes of an X509_AUTHORITY_INFO_ACCESS structure where pszAccessMethod is set to szOID_PKIX_OCSP.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CROSS_CERT_DIST_POINTS_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Sets the location of the cross certificates. This value is only applicable to certificates and not to certificate revocation lists (CRLs) or certificate trust lists (CTLs). The <see cref="CRYPT_DATA_BLOB"/> structure contains an Abstract Syntax Notation One (ASN.1)-encoded CROSS_CERT_DIST_POINTS_INFO structure that is encoded by using the CryptEncodeObject function with a X509_CROSS_CERT_DIST_POINTS value for the lpszStuctType parameter.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CTL_USAGE_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure contains an ASN.1-encoded CTL_USAGE structure. This structure is encoded by using the CryptEncodeObject function with the X509_ENHANCED_KEY_USAGE value set.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_DATE_STAMP_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a FILETIME structure.<br/>
        ///        This property sets the time that the certificate was added to the store.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_DESCRIPTION_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        A property that is set and displayed by the certificate UI. This property allows the user to describe the certificate's use.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_EFS_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_ENHKEY_USAGE_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        A property that indicates that the <paramref name="Data"/> parameter points to a <see cref="CRYPT_DATA_BLOB"/> structure that contains an ASN.1-encoded CERT_ENHKEY_USAGE structure. This structure is encoded by using the CryptEncodeObject function with the X509_ENHANCED_KEY_USAGE value set.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_ENROLLMENT_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Enrollment information of the pending request that contains RequestID, CADNSName, CAName, and DisplayName. The data format is defined as follows.
        ///        <table class="table_value_meaning">
        ///          <tr>
        ///            <td>
        ///              <b>Bytes</b>
        ///            </td>
        ///            <td>
        ///              <b>Contents</b>
        ///            </td>
        ///          </tr>
        ///          <tr>
        ///            <td>
        ///              First 4 bytes
        ///            </td>
        ///            <td>
        ///              Pending request ID
        ///            </td>
        ///          </tr>
        ///          <tr>
        ///            <td>
        ///              Next 4 bytes
        ///            </td>
        ///            <td>
        ///              CADNSName size, in characters, including the terminating null character, followed by CADNSName string with terminating null character
        ///            </td>
        ///          </tr>
        ///          <tr>
        ///            <td>
        ///              Next 4 bytes
        ///            </td>
        ///            <td>
        ///              CAName size, in characters, including the terminating null character, followed by CAName string with terminating null character
        ///            </td>
        ///          </tr>
        ///          <tr>
        ///            <td>
        ///              Next 4 bytes
        ///            </td>
        ///            <td>
        ///              DisplayName size, in characters, including the terminating null character, followed by DisplayName string with terminating null character
        ///            </td>
        ///          </tr>
        ///        </table>
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_EXTENDED_ERROR_INFO_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets a string that contains extended error information for the certificate context.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_FORTEZZA_DATA_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_FRIENDLY_NAME_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure contains the display name of the certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_HASH_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property is implicitly set by a call to the <see cref="CertGetCertificateContextProperty"/> function.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_HCRYPTPROV_OR_NCRYPT_KEY_HANDLE_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to an HCRYPTPROV_OR_NCRYPT_KEY_HANDLE data type.<br/>
        ///        This property calls NCryptIsKeyHandle to determine whether this is an NCRYPT_KEY_HANDLE. For an NCRYPT_KEY_HANDLE, sets CERT_NCRYPT_KEY_HANDLE_PROP_ID; otherwise, it sets CERT_KEY_PROV_HANDLE_PROP_ID.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_HCRYPTPROV_TRANSFER_PROP_ID
        ///      </td>
        ///      <td>
        ///        Sets the handle of the CAPI key associated with the certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_IE30_RESERVED_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_ISSUER_PUBLIC_KEY_MD5_HASH_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets the MD5 hash of the public key associated with the private key used to sign this certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_ISSUER_SERIAL_NUMBER_MD5_HASH_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure contains the MD5 hash of the issuer name and serial number from this certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_KEY_CONTEXT_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a CERT_KEY_CONTEXT structure.<br/>
        ///        The structure specifies the certificate's private key. It contains both the HCRYPTPROV and key specification for the private key. For more information about the hCryptProv member and <paramref name="Flags"/> settings, see CERT_KEY_PROV_HANDLE_PROP_ID, later in this topic.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_KEY_IDENTIFIER_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property is typically implicitly set by a call to the <see cref="CertGetCertificateContextProperty"/> function.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_KEY_PROV_HANDLE_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A HCRYPTPROV value.<br/>
        ///        The HCRYPTPROV handle for the certificate's private key is set. The hCryptProv member of the CERT_KEY_CONTEXT structure is updated if it exists. If it does not exist, it is created with dwKeySpec and initialized by CERT_KEY_PROV_INFO_PROP_ID. If CERT_STORE_NO_CRYPT_RELEASE_FLAG is not set, the hCryptProv value is implicitly released either when the property is set to NULL or on the final freeing of the CERT_CONTEXT structure.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_KEY_PROV_INFO_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_KEY_PROV_INFO structure.<br/>
        ///        The structure specifies the certificate's private key.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_KEY_SPEC_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="Int32"/> value.<br/>
        ///        The <see cref="Int32"/> value that specifies the private key. The dwKeySpec member of the CERT_KEY_CONTEXT structure is updated if it exists. If it does not, it is created with hCryptProv set to zero.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_MD5_HASH_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_HASH_BLOB structure.<br/>
        ///        This property is implicitly set by a call to the <see cref="CertGetCertificateContextProperty"/> function.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NCRYPT_KEY_HANDLE_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to an NCRYPT_KEY_HANDLE data type.<br/>
        ///        This property sets the NCRYPT_KEY_HANDLE for the certificate private key and sets the dwKeySpec to CERT_NCRYPT_KEY_SPEC.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NCRYPT_KEY_HANDLE_TRANSFER_PROP_ID
        ///      </td>
        ///      <td>
        ///        Sets the handle of the CNG key associated with the certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NEW_KEY_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NEXT_UPDATE_LOCATION_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure contains an ASN.1-encoded CERT_ALT_NAME_INFO structure that is encoded by using the CryptEncodeObject function with the X509_ALTERNATE_NAME value set.<br/>
        ///        CERT_NEXT_UPDATE_LOCATION_PROP_ID is currently used only with CTLs.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NO_AUTO_EXPIRE_CHECK_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_OCSP_CACHE_PREFIX_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_OCSP_RESPONSE_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets the encoded online certificate status protocol (OCSP) response from a CERT_SERVER_OCSP_RESPONSE_CONTEXT for this certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_PUB_KEY_CNG_ALG_BIT_LENGTH_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: Pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property is implicitly set by calling the <see cref="CertGetCertificateContextProperty"/> function.<br/>
        ///        <b>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This identifier is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_PUBKEY_ALG_PARA_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_DATA_BLOB structure.<br/>
        ///        This property is used with public keys that support algorithm parameter inheritance. The data BLOB contains the ASN.1-encoded PublicKey Algorithm parameters. For DSS, these are parameters encoded by using the CryptEncodeObject function. This is used only if CMS_PKCS7 is defined.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_PUBKEY_HASH_RESERVED_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_PVK_FILE_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure specifies the name of a file that contains the private key associated with the certificate's public key. Inside the <see cref="CRYPT_DATA_BLOB"/> structure, the <paramref name="Data"/> member is a pointer to a null-terminated Unicode wide-character string, and the cbData member indicates the length of the string.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_RENEWAL_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property specifies the hash of the renewed certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_REQUEST_ORIGINATOR_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        The <see cref="CRYPT_DATA_BLOB"/> structure contains a null-terminated Unicode string that contains the DNS computer name for the origination of the certificate context request.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_ROOT_PROGRAM_CERT_POLICIES_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Returns a pointer to an encoded CERT_POLICIES_INFO structure that contains the application policies of the root certificate for the context. This property can be decoded by using the CryptDecodeObject function with the lpszStructType parameter set to X509_CERT_POLICIES and the dwCertEncodingType parameter set to a combination of X509_ASN_ENCODING bitwise OR PKCS_7_ASN_ENCODING.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_ROOT_PROGRAM_NAME_CONSTRAINTS_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SIGN_HASH_CNG_ALG_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: Pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property is implicitly set by calling the <see cref="CertGetCertificateContextProperty"/> function.<br/>
        ///        <b>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This identifier is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SHA1_HASH_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_HASH_BLOB structure.<br/>
        ///        This property is implicitly set by a call to the <see cref="CertGetCertificateContextProperty"/> function.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SIGNATURE_HASH_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a CRYPT_HASH_BLOB structure.<br/>
        ///        If a signature hash does not exist, it is computed by using the CryptHashToBeSigned function. <paramref name="Data"/> points to an existing or computed hash. Usually, the length of the hash is 20 bytes for SHA and 16 for MD5.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SMART_CARD_DATA_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets the smart card data property of a smart card certificate context.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SMART_CARD_ROOT_INFO_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets the information property of a smart card root certificate context.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SOURCE_LOCATION_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SOURCE_URL_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SUBJECT_DISABLE_CRL_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SUBJECT_INFO_ACCESS_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property sets the subject information access extension of the certificate context as an encoded CERT_SUBJECT_INFO_ACCESS structure.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SUBJECT_NAME_MD5_HASH_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: A pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        Returns an MD5 hash of the encoded subject name of the certificate context.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SUBJECT_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID
        ///      </td>
        ///      <td>
        ///        This identifier is reserved.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SUBJECT_PUB_KEY_BIT_LENGTH_PROP_ID
        ///      </td>
        ///      <td>
        ///        Data type of <paramref name="Data"/>: Pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.<br/>
        ///        This property is implicitly set by calling the <see cref="CertGetCertificateContextProperty"/> function.<br/>
        ///        <b>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This identifier is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_SUBJECT_PUBLIC_KEY_MD5_HASH_PROP_ID
        ///      </td>
        ///      <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        <see cref="HRESULT.E_INVALIDARG"/>
        ///      </td>
        ///      <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        X509_ASN_ENCODING
        ///      </td>
        ///      <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        CERT_SIMPLE_NAME_STR
        ///      </td>
        ///      <td>
        ///        This string type is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_OID_NAME_STR
        ///      </td>
        ///      <td>
        ///        Validates that the string type is supported. The string can be either an object identifier (OID) or an X.500 name.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_X500_NAME_STR
        ///      </td>
        ///      <td>
        ///        Identical to CERT_OID_NAME_STR. Validates that the string type is supported. The string can be either an object identifier (OID) or an X.500 name.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// The following options can also be combined with the value above to specify additional options for the string.
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        CERT_NAME_STR_COMMA_FLAG
        ///      </td>
        ///      <td>
        ///        Only a comma (,) is supported as the RDN separator.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NAME_STR_SEMICOLON_FLAG
        ///      </td>
        ///      <td>
        ///        Only a semicolon (;) is supported as the RDN separator.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NAME_STR_CRLF_FLAG
        ///      </td>
        ///      <td>
        ///        Only a backslash r (\r) or backslash n (\n) is supported as the RDN separator.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NAME_STR_NO_PLUS_FLAG
        ///      </td>
        ///      <td>
        ///        The plus sign (+) is ignored as a separator, and multiple values per RDN are not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NAME_STR_NO_QUOTING_FLAG
        ///      </td>
        ///      <td>
        ///        Quoting is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NAME_STR_REVERSE_FLAG
        ///      </td>
        ///      <td>
        ///        The order of the RDNs in a distinguished name is reversed before encoding. This flag is not set by default.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NAME_STR_ENABLE_T61_UNICODE_FLAG
        ///      </td>
        ///      <td>
        ///        The CERT_RDN_T61_STRING encoded value type is used instead of CERT_RDN_UNICODE_STRING. This flag can be used if all the Unicode characters are less than or equal to 0xFF.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NAME_STR_ENABLE_UTF8_UNICODE_FLAG
        ///      </td>
        ///      <td>
        ///        The CERT_RDN_UTF8_STRING encoded value type is used instead of CERT_RDN_UNICODE_STRING.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NAME_STR_FORCE_UTF8_DIR_STR_FLAG
        ///      </td>
        ///      <td>
        ///        Forces the X.500 key to be encoded as a UTF-8 (CERT_RDN_UTF8_STRING) string rather than as a printable Unicode (CERT_RDN_PRINTABLE_STRING) string. This is the default value for Microsoft certification authorities beginning with Windows Server 2003.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NAME_STR_DISABLE_UTF8_DIR_STR_FLAG
        ///      </td>
        ///      <td>
        ///        Prevents forcing a printable Unicode (CERT_RDN_PRINTABLE_STRING) X.500 key to be encoded by using UTF-8 (CERT_RDN_UTF8_STRING). Use to enable encoding of X.500 keys as Unicode values when CERT_NAME_STR_FORCE_UTF8_DIR_STR_FLAG is set.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_NAME_STR_ENABLE_PUNYCODE_FLAG
        ///      </td>
        ///      <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        CRYPT_E_INVALID_X500_STRING
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CRYPT_E_INVALID_NUMERIC_STRING
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CRYPT_E_INVALID_PRINTABLE_STRING
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
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
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        CERT_CHAIN_POLICY_BASE
        ///      </td>
        ///      <td>
        ///        Implements the base chain policy verification checks. The <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> member of the structure pointed to by <see cref="CERT_CHAIN_POLICY_PARA.ExtraPolicyPara"/> can be set to alter the default policy checking behavior.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CHAIN_POLICY_AUTHENTICODE
        ///      </td>
        ///      <td>
        ///        Implements the Authenticode chain policy verification checks. The <see cref="CERT_CHAIN_POLICY_PARA.ExtraPolicyPara"/> member of the structure pointed to by <paramref name="PolicyPara"/> can be set to point to an AUTHENTICODE_EXTRA_CERT_CHAIN_POLICY_PARA structure.<br/>
        ///        The <see cref="CERT_CHAIN_POLICY_STATUS.ExtraPolicyStatus"/> member of the structure pointed to by <paramref name="PolicyStatus"/> can be set to point to an AUTHENTICODE_EXTRA_CERT_CHAIN_POLICY_STATUS structure.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CHAIN_POLICY_AUTHENTICODE_TS
        ///      </td>
        ///      <td>
        ///        Implements Authenticode Time Stamp chain policy verification checks. The <see cref="CERT_CHAIN_POLICY_PARA.ExtraPolicyPara"/> member of the data structure pointed to by <paramref name="PolicyPara"/> can be set to point to an AUTHENTICODE_TS_EXTRA_CERT_CHAIN_POLICY_PARA structure.<br/>
        ///        The <see cref="CERT_CHAIN_POLICY_STATUS.ExtraPolicyStatus"/> member of the data structure pointed to by <paramref name="PolicyStatus"/> is not used and must be set to NULL.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CHAIN_POLICY_SSL
        ///      </td>
        ///      <td>
        ///        Implements the SSL client/server chain policy verification checks. The <see cref="CERT_CHAIN_POLICY_PARA.ExtraPolicyPara"/> member in the data structure pointed to by <paramref name="PolicyPara"/> can be set to point to an SSL_EXTRA_CERT_CHAIN_POLICY_PARA structure initialized with additional policy criteria.<br/>
        ///        The <see cref="CERT_CHAIN_POLICY_STATUS.ExtraPolicyStatus"/> member of the data structure pointed to by <paramref name="PolicyStatus"/> is not used and must be set to NULL.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CHAIN_POLICY_BASIC_CONSTRAINTS
        ///      </td>
        ///      <td>
        ///        Implements the basic constraints chain policy. Iterates through all the certificates in the chain checking for either a szOID_BASIC_CONSTRAINTS or a szOID_BASIC_CONSTRAINTS2 extension. If neither extension is present, the certificate is assumed to have valid policy. Otherwise, for the first certificate element, checks if it matches the expected CA_FLAG or END_ENTITY_FLAG specified in the <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> member of the CERT_CHAIN_POLICY_PARA structure pointed to by the <paramref name="PolicyPara"/> parameter. If neither or both flags are set, then, the first element can be either a CA or END_ENTITY. All other elements must be a certification authority (CA). If the PathLenConstraint is present in the extension, it is checked.<br/>
        ///        The first elements in the remaining simple chains (that is, the certificates used to sign the CTL) are checked to be an END_ENTITY. If this verification fails, dwError will be set to TRUST_E_BASIC_CONSTRAINTS.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CHAIN_POLICY_NT_AUTH
        ///      </td>
        ///      <td>
        ///        Implements the Windows NT Authentication chain policy, which consists of three distinct chain verifications in the following order:<br/>
        ///        <list type="number">
        ///          <item>CERT_CHAIN_POLICY_BASE—Implements the base chain policy verification checks. The LOWORD of <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> can be set in <paramref name="PolicyPara"/> to alter the default policy checking behavior. For more information, see CERT_CHAIN_POLICY_BASE.</item>
        ///          <item>CERT_CHAIN_POLICY_BASIC_CONSTRAINTS—Implements the basic constraints chain policy. The HIWORD of <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> can be set to specify if the first element must be either a CA or END_ENTITY. For more information, see CERT_CHAIN_POLICY_BASIC_CONSTRAINTS.</item>
        ///          <item>Checks if the second element in the chain, the CA that issued the end certificate, is a trusted CA for Windows NT Authentication. A CA is considered to be trusted if it exists in the "NTAuth" system registry store found in the CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE store location. If this verification fails, the CA is untrusted, and dwError is set to CERT_E_UNTRUSTEDCA. If CERT_PROT_ROOT_DISABLE_NT_AUTH_REQUIRED_FLAG is set in the Flags value of the HKEY_LOCAL_MACHINE policy ProtectedRoots subkey, defined by CERT_PROT_ROOT_FLAGS_REGPATH and the above check fails, the chain is checked for CERT_TRUST_HAS_VALID_NAME_CONSTRAINTS set in dwInfoStatus. This is set if there was a valid name constraint for all namespaces including UPN. If the chain does not have this info status set, dwError is set to CERT_E_UNTRUSTEDCA.</item>
        ///        </list>
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CHAIN_POLICY_MICROSOFT_ROOT
        ///      </td>
        ///      <td>
        ///        Checks the last element of the first simple chain for a Microsoft root public key. If that element does not contain a Microsoft root public key, the dwError member of the CERT_CHAIN_POLICY_STATUS structure pointed to by the <paramref name="PolicyStatus"/> parameter is set to CERT_E_UNTRUSTEDROOT.<br/>
        ///        The <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> member of the CERT_CHAIN_POLICY_PARA structure pointed to by the <paramref name="PolicyStatus"/> parameter can contain the MICROSOFT_ROOT_CERT_CHAIN_POLICY_CHECK_APPLICATION_ROOT_FLAG flag, which causes this function to instead check for the Microsoft application root "Microsoft Root Certificate Authority 2011".<br/>
        ///        The <see cref="CERT_CHAIN_POLICY_PARA.Flags"/> member of the CERT_CHAIN_POLICY_PARA structure pointed to by the <paramref name="PolicyPara"/> parameter can contain the MICROSOFT_ROOT_CERT_CHAIN_POLICY_ENABLE_TEST_ROOT_FLAG flag, which causes this function to also check for the Microsoft test roots.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CHAIN_POLICY_EV
        ///      </td>
        ///      <td>
        ///        Specifies that extended validation of certificates is performed.<br/>
        ///        <b>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This value is not supported.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_CHAIN_POLICY_SSL_F12
        ///      </td>
        ///      <td>
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
        /// A pointer to a <see cref="Int32"/> value contain verification check flags. The following flags can be set to enable verification checks on the subject certificate. They can be combined using a bitwise-OR operation to enable multiple verifications.
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        CERT_STORE_REVOCATION_FLAG
        ///      </td>
        ///      <td>
        ///        Checks whether the subject certificate is on the issuer's revocation list.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_STORE_SIGNATURE_FLAG
        ///      </td>
        ///      <td>
        ///        Uses the public key in the issuer's certificate to verify the signature on the subject certificate.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        CERT_STORE_TIME_VALIDITY_FLAG
        ///      </td>
        ///      <td>
        ///        Gets the current time and verifies that it is within the subject certificate's validity period.
        ///      </td>
        ///    </tr>
        /// </table>
        /// If an enabled verification check succeeds, its flag is set to zero. If it fails, then its flag is set upon return.<br/>
        /// If CERT_STORE_REVOCATION_FLAG was enabled and the issuer does not have a CRL in the store, then CERT_STORE_NO_CRL_FLAG is set in addition to CERT_STORE_REVOCATION_FLAG.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is <see langword="true"/>.<br/>
        /// If the function fails, the return value is <see langword="false"/>.<br/>
        /// For a verification check failure, <see langword="true"/> is still returned. <see langword="false"/> is returned only when a bad parameter is passed in.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        ///  <table class="table_value_meaning">
        ///    <tr>
        ///      <td>
        ///        E_INVALIDARG
        ///      </td>
        ///      <td>
        ///        An unsupported bit was set in <paramref name="Flags"/>. Any combination of CERT_STORE_SIGNATURE_FLAG, CERT_STORE_TIME_VALIDITY_FLAG, and CERT_STORE_REVOCATION_FLAG can be set. If <paramref name="Issuer"/> is NULL, only CERT_STORE_TIME_VALIDITY_FLAG can be set.
        ///      </td>
        ///    </tr>
        ///   </table>
        /// </returns>
        Boolean CertVerifySubjectCertificateContext(IntPtr Subject,IntPtr Issuer,ref Int32 Flags);
        #endregion
        #region M:CryptAcquireCertificatePrivateKey(IntPtr,CRYPT_ACQUIRE_FLAGS,IntPtr,{out}IntPtr,{out}KEY_SPEC_TYPE,{out}Boolean):Boolean
        /// <summary>
        /// The <b>CryptAcquireCertificatePrivateKey</b> function obtains the private key for a certificate. This function is used to obtain access to a user's private key when the user's certificate is available, but the handle of the user's key container is not available. This function can only be used by the owner of a private key and not by any other user.<br/>
        /// If a CSP handle and the key container containing a user's private key are available, the <see cref="CryptGetUserKey"/> function should be used instead.
        /// </summary>
        /// <param name="Certificate">The address of a <see cref="CERT_CONTEXT"/> structure that contains the certificate context for which a private key will be obtained.</param>
        /// <param name="Flags">
        /// A set of flags that modify the behavior of this function. This can be zero or a combination of one or more of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_ACQUIRE_CACHE_FLAG
        ///     </td>
        ///     <td>
        ///       If a handle is already acquired and cached, that same handle is returned. Otherwise, a new handle is acquired and cached by using the certificate's <b>CERT_KEY_CONTEXT_PROP_ID</b> property.<br/>
        ///       When this flag is set, the <paramref name="CallerFreeProvOrNCryptKey"/> parameter receives <b>FALSE</b> and the calling application must not release the handle. The handle is freed when the certificate context is freed; however, you must retain the certificate context referenced by the <paramref name="Certificate"/> parameter as long as the key is in use, otherwise operations that rely on the key will fail.
        ///     </td>
        ///    </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_ACQUIRE_COMPARE_KEY_FLAG
        ///     </td>
        ///     <td>
        ///       The public key in the certificate is compared with the public key returned by the cryptographic service provider (CSP). If the keys do not match, the acquisition operation fails and the last error code is set to <b>NTE_BAD_PUBLIC_KEY</b>. If a cached handle is returned, no comparison is made.
        ///     </td>
        ///    </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_ACQUIRE_NO_HEALING
        ///     </td>
        ///     <td>
        ///       This function will not attempt to re-create the <b>CERT_KEY_PROV_INFO_PROP_ID</b> property in the certificate context if this property cannot be retrieved.
        ///     </td>
        ///    </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_ACQUIRE_SILENT_FLAG
        ///     </td>
        ///     <td>
        ///       The CSP should not display any user interface (UI) for this context. If the CSP must display UI to operate, the call fails and the <b>NTE_SILENT_CONTEXT</b> error code is set as the last error.
        ///     </td>
        ///    </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_ACQUIRE_USE_PROV_INFO_FLAG
        ///     </td>
        ///     <td>
        ///       Uses the certificate's <b>CERT_KEY_PROV_INFO_PROP_ID</b> property to determine whether caching should be accomplished. For more information about the <b>CERT_KEY_PROV_INFO_PROP_ID</b> property, see <see cref="CertSetCertificateContextProperty"/>.<br/>
        ///       This function will only use caching if during a previous call, the <see cref="CRYPT_KEY_PROV_INFO.ProviderFlags"/> member of the <see cref="CRYPT_KEY_PROV_INFO"/> structure contained <b>CERT_SET_KEY_CONTEXT_PROP</b>.
        ///     </td>
        ///    </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_ACQUIRE_WINDOW_HANDLE_FLAG
        ///     </td>
        ///     <td>
        ///       Any UI that is needed by the CSP or KSP will be a child of the <b>HWND</b> that is supplied in the <paramref name="Parameters"/> parameter. For a CSP key, using this flag will cause the <see cref="CryptSetProvParam"/> function with the flag <b>PP_CLIENT_HWND</b> using this <b>HWND</b> to be called with <b>NULL</b> for <b>HCRYPTPROV</b>. For a KSP key, using this flag will cause the <b>NCryptSetProperty</b> function with the <b>NCRYPT_WINDOW_HANDLE_PROPERTY</b> flag to be called using the <b>HWND</b>.<br/>
        ///       Do not use this flag with <b>CRYPT_ACQUIRE_SILENT_FLAG</b>.
        ///     </td>
        ///    </tr>
        /// </table>
        /// The following flags determine which technology is used to obtain the key. If none of these flags is present, this function will only attempt to obtain the key by using CryptoAPI.<br/>
        /// <b>Windows Server 2003 and Windows XP</b>: These flags are not supported.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_ACQUIRE_ALLOW_NCRYPT_KEY_FLAG
        ///     </td>
        ///     <td>
        ///       This function will attempt to obtain the key by using CryptoAPI. If that fails, this function will attempt to obtain the key by using the Cryptography API: Next Generation (CNG).<br/>
        ///       The <paramref name="KeySpec"/> variable receives the <b>CERT_NCRYPT_KEY_SPEC</b> flag if CNG is used to obtain the key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_ACQUIRE_ONLY_NCRYPT_KEY_FLAG
        ///     </td>
        ///     <td>
        ///       This function will only attempt to obtain the key by using CNG and will not use CryptoAPI to obtain the key.<br/>
        ///       The <paramref name="KeySpec"/> variable receives the <b>CERT_NCRYPT_KEY_SPEC</b> flag if CNG is used to obtain the key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_ACQUIRE_PREFER_NCRYPT_KEY_FLAG
        ///     </td>
        ///     <td>
        ///       This function will attempt to obtain the key by using CNG. If that fails, this function will attempt to obtain the key by using CryptoAPI.<br/>
        ///       The <paramref name="KeySpec"/> variable receives the <b>CERT_NCRYPT_KEY_SPEC</b> flag if CNG is used to obtain the key.
        ///       <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///         <tr>
        ///           <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///             <b>Note:</b> CryptoAPI does not support the CNG Diffie-Hellman or DSA asymmetric algorithms. CryptoAPI only supports Diffie-Hellman and DSA public keys through the legacy CSPs. If this flag is set for a certificate that contains a Diffie-Hellman or DSA public key, this function will implicitly change this flag to <b>CRYPT_ACQUIRE_ALLOW_NCRYPT_KEY_FLAG</b> to first attempt to use CryptoAPI to obtain the key.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Parameters">
        /// If the <b>CRYPT_ACQUIRE_WINDOW_HANDLE_FLAG</b> is set, then this is the address of an HWND. If the <b>CRYPT_ACQUIRE_WINDOW_HANDLE_FLAG</b> is not set, then this parameter must be <b>NULL</b>.<br/>
        /// <b>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This parameter reserved for future use and must be <b>NULL</b>.
        /// </param>
        /// <param name="CryptProvOrNCryptKey">
        /// The address of an <b>HCRYPTPROV_OR_NCRYPT_KEY_HANDLE</b> variable that receives the handle of either the CryptoAPI provider or the CNG key. If the <paramref name="KeySpec"/> variable receives the <b>CERT_NCRYPT_KEY_SPEC</b> flag, this is a CNG key handle of type <b>NCRYPT_KEY_HANDLE</b>; otherwise, this is a CryptoAPI provider handle of type <b>HCRYPTPROV</b>.<br/>
        /// For more information about when and how to release this handle, see the description of the <paramref name="CallerFreeProvOrNCryptKey"/> parameter.
        /// </param>
        /// <param name="KeySpec">
        /// The address of a <see cref="Int32"/> variable that receives additional information about the key. This can be one of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       AT_KEYEXCHANGE
        ///     </td>
        ///     <td>
        ///       The key pair is a key exchange pair.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       AT_SIGNATURE
        ///     </td>
        ///     <td>
        ///       The key pair is a signature pair.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NCRYPT_KEY_SPEC
        ///     </td>
        ///     <td>
        ///       The key is a CNG key.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This value is not supported.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="CallerFreeProvOrNCryptKey">
        /// The address of a <see cref="Boolean"/> variable that receives a value that indicates whether the caller must free the handle returned in the <paramref name="CryptProvOrNCryptKey"/> variable. This receives <b>FALSE</b> if any of the following is true:
        /// <list type="bullet">
        ///   <item>Public key acquisition or comparison fails.</item>
        ///   <item>The <paramref name="Flags"/> parameter contains the <b>CRYPT_ACQUIRE_CACHE_FLAG</b> flag.</item>
        ///   <item>The <paramref name="Flags"/> parameter contains the <b>CRYPT_ACQUIRE_USE_PROV_INFO_FLAG</b> flag, the certificate context property is set to <b>CERT_KEY_PROV_INFO_PROP_ID</b> with the <b>CRYPT_KEY_PROV_INFO</b> structure, and the <paramref name="Flags"/> member of the <b>CRYPT_KEY_PROV_INFO</b> structure is set to <b>CERT_SET_KEY_CONTEXT_PROP_ID</b>.</item>
        /// </list>
        /// If this variable receives <b>FALSE</b>, the calling application must not release the handle returned in the <paramref name="CryptProvOrNCryptKey"/> variable. The handle will be released on the last free action of the certificate context.<br/>
        /// If this variable receives <b>TRUE</b>, the caller is responsible for releasing the handle returned in the <paramref name="CryptProvOrNCryptKey"/> variable. If the <paramref name="KeySpec"/> variable receives the <b>CERT_NCRYPT_KEY_SPEC</b> value, the handle must be released by passing it to the <b>NCryptFreeObject</b> function; otherwise, the handle is released by passing it to the <see cref="CryptReleaseContext"/> function.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       NTE_BAD_PUBLIC_KEY
        ///     </td>
        ///     <td>
        ///       The public key in the certificate does not match the public key returned by the CSP. This error code is returned if the <b>CRYPT_ACQUIRE_COMPARE_KEY_FLAG</b> is set and the public key in the certificate does not match the public key returned by the cryptographic provider.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_SILENT_CONTEXT
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter contained the <b>CRYPT_ACQUIRE_SILENT_FLAG</b> flag and the CSP could not continue an operation without displaying a user interface.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptAcquireCertificatePrivateKey(IntPtr Certificate,CRYPT_ACQUIRE_FLAGS Flags,IntPtr Parameters,out IntPtr CryptProvOrNCryptKey,out KEY_SPEC_TYPE KeySpec, out Boolean CallerFreeProvOrNCryptKey);
        #endregion
        #region M:CryptAcquireContext({out}IntPtr,String,String,Int32,Int32):Boolean
        /// <summary>
        /// The <b>CryptAcquireContext</b> function is used to acquire a handle to a particular key container within a particular cryptographic service provider (CSP). This returned handle is used in calls to CryptoAPI functions that use the selected CSP.<br/>
        /// This function first attempts to find a CSP with the characteristics described in the <paramref name="ProvType"/> and <paramref name="Provider"/> parameters. If the CSP is found, the function attempts to find a key container within the CSP that matches the name specified by the <paramref name="Container"/> parameter. To acquire the context and the key container of a private key associated with the public key of a certificate, use <see cref="CryptAcquireCertificatePrivateKey"/>.<br/>
        /// With the appropriate setting of <paramref name="Flags"/>, this function can also create and destroy key containers and can provide access to a CSP with a temporary key container if access to a private key is not required.
        /// </summary>
        /// <param name="CryptProv">A pointer to a handle of a CSP. When you have finished using the CSP, release the handle by calling the <see cref="CryptReleaseContext"/> function.</param>
        /// <param name="Container">The key container name. This is a null-terminated string that identifies the key container to the CSP. This name is independent of the method used to store the keys. Some CSPs store their key containers internally (in hardware), some use the system registry, and others use the file system. In most cases, when <paramref name="Flags"/> is set to <b>CRYPT_VERIFYCONTEXT</b>, <paramref name="Container"/> must be set to <b>NULL</b>. However, for hardware-based CSPs, such as a smart card CSP, can be access publicly available information in the specfied container.</param>
        /// <param name="Provider">
        /// A null-terminated string that contains the name of the CSP to be used.<br/>
        /// If this parameter is <b>NULL</b>, the user default provider is used.<br/>
        /// An application can obtain the name of the CSP in use by using the <see cref="CryptGetProvParam"/> function to read the <b>PP_NAME</b> CSP value in the <b>Param</b> parameter.<br/>
        /// The default CSP can change between operating system releases. To ensure interoperability on different operating system platforms, the CSP should be explicitly set by using this parameter instead of using the default CSP.
        /// </param>
        /// <param name="ProvType">Specifies the type of provider to acquire.</param>
        /// <param name="Flags">
        /// One or more of the following flags. Note, most applications should set the <b>CRYPT_VERIFYCONTEXT</b> flag unless they need to create digital signatures or decrypt messages.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_VERIFYCONTEXT
        ///     </td>
        ///     <td>
        ///       The caller does not need access to persisted private keys. Apps that use ephemeral keys, or that perform only hashing, encryption, and digital signature verification should set this flag. Only applications that create signatures or decrypt messages need access to a private key (and should not set this flag).<br/>
        ///       For file-based CSPs, when this flag is set, the <paramref name="Container"/> parameter must be set to <b>NULL</b>. The application has no access to the persisted private keys of public/private key pairs. When this flag is set, temporary public/private key pairs can be created, but they are not persisted.<br/>
        ///       For hardware-based CSPs, such as a smart card CSP, if the <paramref name="Container"/> parameter is <b>NULL</b> or blank, this flag implies that no access to any keys is required, and that no UI should be presented to the user. This form is used to connect to the CSP to query its capabilities but not to actually use its keys. If the <paramref name="Container"/> parameter is not <b>NULL</b> and not blank, then this flag implies that access to only the publicly available information within the specified container is required. The CSP should not ask for a PIN. Attempts to access private information (for example, the <see cref="CryptSignHash"/> function) will fail.<br/>
        ///       When <b>CryptAcquireContext</b> is called, many CSPs require input from the owning user before granting access to the private keys in the key container. For example, the private keys can be encrypted, requiring a password from the user before they can be used. However, if the <b>CRYPT_VERIFYCONTEXT</b> flag is specified, access to the private keys is not required and the user interface can be bypassed.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_NEWKEYSET
        ///     </td>
        ///     <td>
        ///       Creates a new key container with the name specified by <paramref name="Container"/>. If <paramref name="Container"/> is <b>NULL</b>, a key container with the default name is created.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_MACHINE_KEYSET
        ///     </td>
        ///     <td>
        ///       By default, keys and key containers are stored as user keys. For Base Providers, this means that user key containers are stored in the user's profile. A key container created without this flag by an administrator can be accessed only by the user creating the key container and a user with administration privileges. <b>Windows XP</b>: A key container created without this flag by an administrator can be accessed only by the user creating the key container and the local system account.<br/>
        ///       A key container created without this flag by a user that is not an administrator can be accessed only by the user creating the key container and the local system account.<br/>
        ///       The <b>CRYPT_MACHINE_KEYSET</b> flag can be combined with all of the other flags to indicate that the key container of interest is a computer key container and the CSP treats it as such. For Base Providers, this means that the keys are stored locally on the computer that created the key container. If a key container is to be a computer container, the <b>CRYPT_MACHINE_KEYSET</b> flag must be used with all calls to <b>CryptAcquireContext</b> that reference the computer container. The key container created with <b>CRYPT_MACHINE_KEYSET</b> by an administrator can be accessed only by its creator and by a user with administrator privileges unless access rights to the container are granted using <see cref="CryptSetProvParam"/>.<br/>
        ///       <b>Windows XP</b>: The key container created with <b>CRYPT_MACHINE_KEYSET</b> by an administrator can be accessed only by its creator and by the local system account unless access rights to the container are granted using <see cref="CryptSetProvParam"/>.<br/>
        ///       The key container created with <b>CRYPT_MACHINE_KEYSET</b> by a user that is not an administrator can be accessed only by its creator and by the local system account unless access rights to the container are granted using <see cref="CryptSetProvParam"/>.<br/>
        ///       The <b>CRYPT_MACHINE_KEYSET</b> flag is useful when the user is accessing from a service or user account that did not log on interactively. When key containers are created, most CSPs do not automatically create any public/private key pairs. These keys must be created as a separate step with the <see cref="CryptGenKey"/> function.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_DELETEKEYSET
        ///     </td>
        ///     <td>
        ///       Delete the key container specified by pszContainer. If <paramref name="Container"/> is <b>NULL</b>, the key container with the default name is deleted. All key pairs in the key container are also destroyed.<br/>
        ///       When this flag is set, the value returned in <paramref name="Provider"/> is undefined, and thus, the <see cref="CryptReleaseContext"/> function need not be called afterward.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_SILENT
        ///     </td>
        ///     <td>
        ///       The application requests that the CSP not display any user interface (UI) for this context. If the CSP must display the UI to operate, the call fails and the <b>NTE_SILENT_CONTEXT</b> error code is set as the last error. In addition, if calls are made to <see cref="CryptGenKey"/> with the <b>CRYPT_USER_PROTECTED</b> flag with a context that has been acquired with the <b>CRYPT_SILENT</b> flag, the calls fail and the CSP sets <b>NTE_SILENT_CONTEXT</b>.<br/>
        ///       <b>CRYPT_SILENT</b> is intended for use with applications for which the UI cannot be displayed by the CSP.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_DEFAULT_CONTAINER_OPTIONAL
        ///     </td>
        ///     <td>
        ///       Obtains a context for a smart card CSP that can be used for hashing and symmetric key operations but cannot be used for any operation that requires authentication to a smart card using a PIN. This type of context is most often used to perform operations on an empty smart card, such as setting the PIN by using <see cref="CryptSetProvParam"/>. This flag can only be used with smart card CSPs.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This flag is not supported.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_BUSY
        ///     </td>
        ///     <td>
        ///       Some CSPs set this error if the <b>CRYPT_DELETEKEYSET</b> flag value is set and another thread or process is using this key container.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_FILE_NOT_FOUND
        ///     </td>
        ///     <td>
        ///       The profile of the user is not loaded and cannot be found. This happens when the application impersonates a user, for example, the IUSR<i>_ComputerName</i> account.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_NOT_ENOUGH_MEMORY
        ///     </td>
        ///     <td>
        ///       The operating system ran out of memory during the operation.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter has a value that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEY_STATE
        ///     </td>
        ///     <td>
        ///       The user password has changed since the private keys were encrypted.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEYSET
        ///     </td>
        ///     <td>
        ///       The key container could not be opened. A common cause of this error is that the key container does not exist. To create a key container, call <b>CryptAcquireContext</b> using the <b>CRYPT_NEWKEYSET</b> flag. This error code can also indicate that access to an existing key container is denied. Access rights to the container can be granted by the key set creator by using <see cref="CryptSetProvParam"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEYSET_PARAM
        ///     </td>
        ///     <td>
        ///       The <paramref name="Container"/> or <paramref name="Provider"/> parameter is set to a value that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_PROV_TYPE
        ///     </td>
        ///     <td>
        ///       The value of the <paramref name="ProvType"/> parameter is out of range. All provider types must be from 1 through 999, inclusive.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_SIGNATURE
        ///     </td>
        ///     <td>
        ///       The provider DLL signature could not be verified. Either the DLL or the digital signature has been tampered with.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_EXISTS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter is <b>CRYPT_NEWKEYSET</b>, but the key container already exists.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_KEYSET_ENTRY_BAD
        ///     </td>
        ///     <td>
        ///       The <paramref name="Container"/> key container was found but is corrupt.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_KEYSET_NOT_DEF
        ///     </td>
        ///     <td>
        ///       The requested provider does not exist.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_NO_MEMORY
        ///     </td>
        ///     <td>
        ///       The CSP ran out of memory during the operation.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_PROV_DLL_NOT_FOUND
        ///     </td>
        ///     <td>
        ///       The provider DLL file does not exist or is not on the current path.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_PROV_TYPE_ENTRY_BAD
        ///     </td>
        ///     <td>
        ///       The provider type specified by <paramref name="ProvType"/> is corrupt. This error can relate to either the user default CSP list or the computer default CSP list.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_PROV_TYPE_NO_MATCH
        ///     </td>
        ///     <td>
        ///       The provider type specified by <paramref name="ProvType"/> does not match the provider type found. Note that this error can only occur when <paramref name="Provider"/> specifies an actual CSP name.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_PROV_TYPE_NOT_DEF
        ///     </td>
        ///     <td>
        ///       No entry exists for the provider type specified by <paramref name="ProvType"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_PROVIDER_DLL_FAIL
        ///     </td>
        ///     <td>
        ///       The provider DLL file could not be loaded or failed to initialize.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_SIGNATURE_FILE_BAD
        ///     </td>
        ///     <td>
        ///       An error occurred while loading the DLL file image, prior to verifying its signature.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptAcquireContext(out IntPtr CryptProv,String Container,String Provider,Int32 ProvType,Int32 Flags);
        #endregion
        #region M:CryptCreateHash(IntPtr,ALG_ID,IntPtr,{out}IntPtr):Boolean
        /// <summary>
        /// The <b>CryptCreateHash</b> function initiates the hashing of a stream of data. It creates and returns to the calling application a handle to a cryptographic service provider (CSP) hash object. This handle is used in subsequent calls to <see cref="CryptHashData"/> and <see cref="CryptHashSessionKey"/> to hash session keys and other streams of data.
        /// </summary>
        /// <param name="Provider">A handle to a CSP created by a call to <see cref="CryptAcquireContext"/>.</param>
        /// <param name="Algorithm">
        /// An <see cref="ALG_ID"/> value that identifies the hash algorithm to use.<br/>
        /// Valid values for this parameter vary, depending on the CSP that is used.
        /// </param>
        /// <param name="Key">
        /// If the type of hash algorithm is a keyed hash, such as the Hash-Based Message Authentication Code (HMAC) or Message Authentication Code (MAC) algorithm, the key for the hash is passed in this parameter. For nonkeyed algorithms, this parameter must be set to zero.<br/>
        /// For keyed algorithms, the key must be to a block cipher key, such as RC2, that has a cipher mode of Cipher Block Chaining (CBC).
        /// </param>
        /// <param name="Handle">The reference to which the function copies a handle to the new hash object. When you have finished using the hash object, release the handle by calling the <see cref="CryptDestroyHash"/> function.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_NOT_ENOUGH_MEMORY
        ///     </td>
        ///     <td>
        ///       The operating system ran out of memory during the operation.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_ALGID
        ///     </td>
        ///     <td>
        ///       The <paramref name="Algorithm"/> parameter specifies an algorithm that this CSP does not support.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEY
        ///     </td>
        ///     <td>
        ///       A keyed hash algorithm, such as CALG_MAC, is specified by <paramref name="Algorithm"/>, and the <paramref name="Key"/> parameter is either zero or it specifies a key handle that is not valid. This error code is also returned if the key is to a stream cipher or if the cipher mode is anything other than CBC.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_NO_MEMORY
        ///     </td>
        ///     <td>
        ///       The CSP ran out of memory during the operation
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptCreateHash(IntPtr Provider,ALG_ID Algorithm,IntPtr Key,out IntPtr Handle);
        #endregion
        #region M:CryptDeriveKey(IntPtr,ALG_ID,IntPtr,Int32,{out}IntPtr):Boolean
        /// <summary>
        /// The <b>CryptDeriveKey</b> function generates cryptographic session keys derived from a base data value. This function guarantees that when the same cryptographic service provider (CSP) and algorithms are used, the keys generated from the same base data are identical. The base data can be a password or any other user data.<br/>
        /// This function is the same as <see cref="CryptGenKey"/>, except that the generated session keys are derived from base data instead of being random. <see cref="CryptDeriveKey"/> can only be used to generate session keys. It cannot generate public/private key pairs.<br/>
        /// A handle to the session key is returned in the <paramref name="Key"/> parameter. This handle can be used with any CryptoAPI function that requires a key handle.
        /// </summary>
        /// <param name="Context">A <b>HCRYPTPROV</b> handle of a CSP created by a call to <see cref="CryptAcquireContext"/>.</param>
        /// <param name="AlgId">An <see cref="ALG_ID"/> structure that identifies the symmetric encryption algorithm for which the key is to be generated. The algorithms available will most likely be different for each CSP. For more information about which algorithm identifier is used by the different providers for the key specs AT_KEYEXCHANGE and AT_SIGNATURE, see ALG_ID.</param>
        /// <param name="BaseData">
        /// A handle to a hash object that has been fed the exact base data.<br/>
        /// To obtain this handle, an application must first create a hash object with <see cref="CryptCreateHash"/> and then add the base data to the hash object with CryptHashData.
        /// </param>
        /// <param name="Flags">
        /// Specifies the type of key generated.<br/>
        /// The sizes of a session key can be set when the key is generated. The key size, representing the length of the key modulus in bits, is set with the upper 16 bits of this parameter. Thus, if a 128-bit RC4 session key is to be generated, the value 0x00800000 is combined with any other <paramref name="Flags"/> predefined value with a bitwise-OR operation. Due to changing export control restrictions, the default CSP and default key length may change between operating system releases. It is important that both the encryption and decryption use the same CSP and that the key length be explicitly set using the <paramref name="Flags"/> parameter to ensure interoperability on different operating system platforms.<br/>
        /// The lower 16 bits of this parameter can be zero or you can specify one or more of the following flags by using the bitwise-OR operator to combine them.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_CREATE_SALT
        ///     </td>
        ///     <td>
        ///       Typically, when a session key is made from a hash value, there are a number of leftover bits. For example, if the hash value is 128 bits and the session key is 40 bits, there will be 88 bits left over.<br/>
        ///       If this flag is set, then the key is assigned a salt value based on the unused hash value bits. You can retrieve this salt value by using the <see cref="CryptGetKeyParam"/> function with the <b>Param</b> parameter set to <b>KP_SALT</b>.<br/>
        ///       If this flag is not set, then the key is given a salt value of zero.<br/>
        ///       When keys with nonzero salt values are exported (by using <see cref="CryptExportKey"/>), the salt value must also be obtained and kept with the key BLOB.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_EXPORTABLE
        ///     </td>
        ///     <td>
        ///       If this flag is set, the session key can be transferred out of the CSP into a key BLOB through the <see cref="CryptExportKey"/> function. Because keys generally must be exportable, this flag should usually be set.<br/>
        ///       If this flag is not set, then the session key is not exportable. This means the key is available only within the current session and only the application that created it is able to use it.<br/>
        ///       This flag does not apply to public/private key pairs.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_NO_SALT
        ///     </td>
        ///     <td>
        ///       This flag specifies that a no salt value gets allocated for a 40-bit symmetric key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_UPDATE_KEY
        ///     </td>
        ///     <td>
        ///       Some CSPs use session keys that are derived from multiple hash values. When this is the case, <see cref="CryptDeriveKey"/> must be called multiple times.<br/>
        ///       If this flag is set, a new session key is not generated. Instead, the key specified by <paramref name="Key"/> is modified. The precise behavior of this flag is dependent on the type of key being generated and on the particular CSP being used.<br/>
        ///       Microsoft cryptographic service providers ignore this flag.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_SERVER
        ///     </td>
        ///     <td>
        ///       This flag is used only with <a href="https://learn.microsoft.com/en-us/windows/desktop/SecGloss/s-gly">schannel</a> providers. If this flag is set, the key to be generated is a server-write key; otherwise, it is a client-write key.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Key">A reference to a <b>HCRYPTKEY</b> variable to receive the address of the handle of the newly generated key. When you have finished using the key, release the handle by calling the <see cref="CryptDestroyKey"/> function.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_ALGID
        ///     </td>
        ///     <td>
        ///       The <paramref name="AlgId"/> parameter specifies an algorithm that this CSP does not support.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter contains a value that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_HASH
        ///     </td>
        ///     <td>
        ///       The <paramref name="BaseData"/> parameter does not contain a valid handle to a hash object.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_HASH_STATE
        ///     </td>
        ///     <td>
        ///       An attempt was made to add data to a hash object that is already marked "finished."
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The <paramref name="Context"/> parameter does not contain a valid context handle.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_FAIL
        ///     </td>
        ///     <td>
        ///       The function failed in some unexpected way.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_SILENT_CONTEXT
        ///     </td>
        ///     <td>
        ///       The provider could not perform the action because the context was acquired as silent.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptDeriveKey(IntPtr Context,ALG_ID AlgId,IntPtr BaseData,Int32 Flags,out IntPtr Key);
        #endregion
        #region M:CryptDestroyHash(IntPtr):Boolean
        /// <summary>
        /// The <b>CryptDestroyHash</b> function destroys the hash object referenced by the <paramref name="Hash"/> parameter. After a hash object has been destroyed, it can no longer be used.<br/>
        /// To help ensure security, we recommend that hash objects be destroyed after they have been used.
        /// </summary>
        /// <param name="Hash">The handle of the hash object to be destroyed.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_BUSY
        ///     </td>
        ///     <td>
        ///       The hash object specified by <paramref name="Hash"/> is currently being used and cannot be destroyed.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Hash"/> parameter specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       The <paramref name="Hash"/> parameter contains a value that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_ALGID
        ///     </td>
        ///     <td>
        ///       The <paramref name="Hash"/> handle specifies an algorithm that this CSP does not support.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_HASH
        ///     </td>
        ///     <td>
        ///       The hash object specified by the <paramref name="Hash"/> parameter is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the hash object was created cannot be found.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptDestroyHash(IntPtr Hash);
        #endregion
        #region M:CryptDestroyKey(IntPtr):Boolean
        /// <summary>
        /// The <b>CryptDestroyKey</b> function releases the handle referenced by the <paramref name="Key"/> parameter. After a key handle has been released, it is no longer valid and cannot be used again.<br/>
        /// If the handle refers to a session key, or to a public key that has been imported into the cryptographic service provider (CSP) through <see cref="CryptImportKey"/>, this function destroys the key and frees the memory that the key used. Many CSPs overwrite the memory where the key was held before freeing it. However, the underlying public/private key pair is not destroyed by this function. Only the handle is destroyed.
        /// </summary>
        /// <param name="Key">The handle of the key to be destroyed.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_BUSY
        ///     </td>
        ///     <td>
        ///       The key object specified by <paramref name="Key"/> is currently being used and cannot be destroyed.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Key"/> parameter specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       The <paramref name="Key"/> parameter contains a value that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEY
        ///     </td>
        ///     <td>
        ///       The <paramref name="Key"/> parameter does not contain a valid handle to a key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the key was created cannot be found.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptDestroyKey(IntPtr Key);
        #endregion
        #region M:CryptDuplicateHash(IntPtr,{out}IntPtr):Boolean
        /// <summary>
        /// The <b>CryptDuplicateHash</b> function makes an exact copy of a hash to the point when the duplication is done. The duplicate hash includes the state of the hash.<br/>
        /// A hash can be created in a piece-by-piece way. The <see cref="CryptDuplicateHash"/> function can be used to create separate hashes of two different contents that begin with the same content.
        /// </summary>
        /// <param name="Hash">Handle of the hash to be duplicated.</param>
        /// <param name="Output">Reference of the handle of the duplicated hash. When you have finished using the hash, release the handle by calling the <see cref="CryptDestroyHash"/> function.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_CALL_NOT_IMPLEMENTED
        ///     </td>
        ///     <td>
        ///       Because this is a new function, existing CSPs cannot implement it. This error is returned if the CSP does not support this function.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_HASH
        ///     </td>
        ///     <td>
        ///       A handle to the original hash is not valid.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptDuplicateHash(IntPtr Hash,out IntPtr Output);
        #endregion
        #region M:CryptDuplicateKey(IntPtr,{out}IntPtr):Boolean
        /// <summary>
        /// The <b>CryptDuplicateKey</b> function makes an exact copy of a key and the state of the key.
        /// </summary>
        /// <param name="Key">A handle to the key to be duplicated.</param>
        /// <param name="Output">Reference of the handle to the duplicated key. When you have finished using the key, release the handle by calling the <see cref="CryptDestroyKey"/> function</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_CALL_NOT_IMPLEMENTED
        ///     </td>
        ///     <td>
        ///       Because this is a new function, existing CSPs cannot implement it. This error is returned if the CSP does not support this function.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEY
        ///     </td>
        ///     <td>
        ///       A handle to the original key is not valid.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptDuplicateKey(IntPtr Key,out IntPtr Output);
        #endregion
        #region M:CryptEnumOIDInfo(CRYPT_ALG_OID_GROUP_ID,IntPtr,CryptEnumOidInfoCallback):Boolean
        /// <summary>
        /// The <b>CryptEnumOIDInfo</b> function enumerates predefined and registered object identifier (OID) <see cref="CRYPT_OID_INFO"/> structures. This function enumerates either all of the predefined and registered structures or only structures identified by a selected OID group. For each OID information structure enumerated, an application provided callback function, <paramref name="Callback"/>, is called.
        /// </summary>
        /// <param name="GroupId">
        /// Indicates which OID groups to be matched. Setting <paramref name="GroupId"/> to zero matches all groups. If <paramref name="GroupId"/> is greater than zero, only the OID entries in the specified group are enumerated.<br/>
        /// The currently defined OID group IDs are:
        /// <list type="bullet">
        ///   <item>CRYPT_HASH_ALG_OID_GROUP_ID</item>
        ///   <item>CRYPT_ENCRYPT_ALG_OID_GROUP_ID</item>
        ///   <item>CRYPT_PUBKEY_ALG_OID_GROUP_ID</item>
        ///   <item>CRYPT_SIGN_ALG_OID_GROUP_ID</item>
        ///   <item>CRYPT_RDN_ATTR_OID_GROUP_ID</item>
        ///   <item>CRYPT_EXT_OR_ATTR_OID_GROUP_ID</item>
        ///   <item>CRYPT_ENHKEY_USAGE_OID_GROUP_ID</item>
        ///   <item>CRYPT_POLICY_OID_GROUP_ID</item>
        ///   <item>CRYPT_TEMPLATE_OID_GROUP_ID</item>
        ///   <item>CRYPT_KDF_OID_GROUP_ID<br/><b>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: The CRYPT_KDF_OID_GROUP_ID value is not supported.</item>
        ///   <item>CRYPT_LAST_OID_GROUP_ID</item>
        ///   <item>CRYPT_FIRST_ALG_OID_GROUP_ID</item>
        ///   <item>CRYPT_LAST_ALG_OID_GROUP_ID</item>
        /// </list>
        /// </param>
        /// <param name="Arg">A pointer to arguments to be passed through to the callback function.</param>
        /// <param name="Callback">A pointer to the callback function that is executed for each OID information entry enumerated.</param>
        /// <returns>
        /// If the callback function completes the enumeration, this function returns <b>TRUE</b>.<br/>
        /// If the callback function has stopped the enumeration, this function returns <b>FALSE</b>.
        /// </returns>
        Boolean CryptEnumOIDInfo(CRYPT_ALG_OID_GROUP_ID GroupId,IntPtr Arg,CryptEnumOidInfoCallback Callback);
        #endregion
        #region M:CryptEnumProviders(Int32,{out}Int32,StringBuilder,{ref}Int32):Boolean
        /// <summary>
        /// The <b>CryptEnumProviders</b> function retrieves the first or next available cryptographic service providers (CSPs). Used in a loop, this function can retrieve in sequence all of the CSPs available on a computer.
        /// </summary>
        /// <param name="Index">Index of the next provider to be enumerated.</param>
        /// <param name="Type">Reference of the <see cref="Int32"/> value designating the type of the enumerated provider.</param>
        /// <param name="Name">
        /// A pointer to a buffer that receives the data from the enumerated provider. This is a string including the terminating null character.<br/>
        /// This parameter can be <b>NULL</b> to set the size of the name for memory allocation purposes.
        /// </param>
        /// <param name="Size">
        /// A reference to a <see cref="Int32"/> value specifying the size, in bytes, of the buffer pointed to by the <paramref name="Name"/> parameter. When the function returns, the <see cref="Int32"/> value contains the number of bytes stored in the buffer.
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> When processing the data returned in the buffer, applications must use the actual size of the data returned. The actual size can be slightly smaller than the size of the buffer specified on input. (On input, buffer sizes are usually specified large enough to ensure that the largest possible output data fits in the buffer.) On output, the variable pointed to by this parameter is updated to reflect the actual size of the data copied to the buffer.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_MORE_DATA
        ///     </td>
        ///     <td>
        ///       The <paramref name="Name"/> buffer was not large enough to hold the provider name.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_NO_MORE_ITEMS
        ///     </td>
        ///     <td>
        ///       There are no more items to enumerate.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_NOT_ENOUGH_MEMORY
        ///     </td>
        ///     <td>
        ///       The operating system ran out of memory.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_FAIL
        ///     </td>
        ///     <td>
        ///       Something was wrong with the type registration.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptEnumProviders(Int32 Index,out Int32 Type,StringBuilder Name,ref Int32 Size);
        #endregion
        #region M:CryptEnumProviderTypes(Int32,{out}Int32,StringBuilder,{ref}Int32):Boolean
        /// <summary>
        /// The <b>CryptEnumProviderTypes</b> function retrieves the first or next types of cryptographic service provider (CSP) supported on the computer. Used in a loop, this function retrieves in sequence all of the CSP types available on a computer.<br/>
        /// Provider types include <b>PROV_RSA_FULL</b>, <b>PROV_RSA_SCHANNEL</b>, and <b>PROV_DSS</b>.
        /// </summary>
        /// <param name="Index">Index of the next provider type to be enumerated.</param>
        /// <param name="Type">Reference of the <see cref="Int32"/> value designating the enumerated provider type.</param>
        /// <param name="Name">
        /// A pointer to a buffer that receives the data from the enumerated provider type. This is a string including the terminating <b>NULL</b> character. Some provider types do not have display names, and in this case no name is returned and the returned value pointed to by <paramref name="Size"/> is zero.<br/>
        /// This parameter can be <b>NULL</b> to get the size of the name for memory allocation purposes.
        /// </param>
        /// <param name="Size">
        /// A pointer to a <see cref="Int32"/> value specifying the size, in bytes, of the buffer pointed to by the <paramref name="Name"/> parameter. When the function returns, the <see cref="Int32"/> value contains the number of bytes stored or to be stored in the buffer. Some provider types do not have display names, and in this case no name is returned and the returned value pointed to by <paramref name="Size"/> is zero.
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> When processing the data returned in the buffer, applications must use the actual size of the data returned. The actual size can be slightly smaller than the size of the buffer specified on input. (On input, buffer sizes are usually specified large enough to ensure that the largest possible output data fits in the buffer.) On output, the variable pointed to by this parameter is updated to reflect the actual size of the data copied to the buffer.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_NO_MORE_ITEMS
        ///     </td>
        ///     <td>
        ///       There are no more items to enumerate.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_NOT_ENOUGH_MEMORY
        ///     </td>
        ///     <td>
        ///       The operating system ran out of memory.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_FAIL
        ///     </td>
        ///     <td>
        ///       Something was wrong with the type registration.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        /// <remarks>
        /// This function enumerates the provider types available on a computer. Providers for any specific provider type can be enumerated using <see cref="CryptEnumProviders"/>.
        /// </remarks>
        Boolean CryptEnumProviderTypes(Int32 Index,out Int32 Type,StringBuilder Name,ref Int32 Size);
        #endregion
        #region M:CryptExportKey(IntPtr,IntPtr,Int32,Int32,Byte[],{ref}Int32):Boolean
        /// <summary>
        /// The <b>CryptExportKey</b> function exports a cryptographic key or a key pair from a cryptographic service provider (CSP) in a secure manner.<br/>
        /// A handle to the key to be exported is passed to the function, and the function returns a key BLOB. This key BLOB can be sent over a nonsecure transport or stored in a nonsecure storage location. This function can export an <a href="https://learn.microsoft.com/en-us/windows/desktop/SecGloss/s-gly">schannel</a> session key, regular session key, public key, or public/private key pair. The key BLOB to export is useless until the intended recipient uses the <see cref="CryptImportKey"/> function on it to import the key or key pair into a recipient's CSP.
        /// </summary>
        /// <param name="Key">A handle to the key to be exported.</param>
        /// <param name="ExpKey">
        /// A handle to a cryptographic key of the destination user. The key data within the exported key BLOB is encrypted using this key. This ensures that only the destination user is able to make use of the key BLOB. Both <paramref name="ExpKey"/> and <paramref name="Key"/> must come from the same CSP.<br/>
        /// Most often, this is the key exchange public key of the destination user. However, certain protocols in some CSPs require that a session key belonging to the destination user be used for this purpose.<br/>
        /// If the key BLOB type specified by <paramref name="BlobType"/> is <b>PUBLICKEYBLOB</b>, this parameter is unused and must be set to zero.<br/>
        /// If the key BLOB type specified by <paramref name="BlobType"/> is <b>PRIVATEKEYBLOB</b>, this is typically a handle to a session key that is to be used to encrypt the key BLOB. Some CSPs allow this parameter to be zero, in which case the application must encrypt the private key BLOB manually so as to protect it.
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> Some CSPs may modify this parameter as a result of the operation. Applications that subsequently use this key for other purposes should call the <see cref="CryptDuplicateKey"/> function to create a duplicate key handle. When the application has finished using the handle, release it by calling the <see cref="CryptDestroyKey"/> function.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="BlobType">
        /// Specifies the type of key BLOB to be exported in <paramref name="Data"/>. This must be one of the following constants:
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       OPAQUEKEYBLOB
        ///     </td>
        ///     <td>
        ///       Used to store session keys in an <b>Schannel CSP</b> or any other vendor-specific format. <b>OPAQUEKEYBLOB</b>s are nontransferable and must be used within the CSP that generated the BLOB.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PRIVATEKEYBLOB
        ///     </td>
        ///     <td>
        ///       Used to transport public/private key pairs.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PUBLICKEYBLOB
        ///     </td>
        ///     <td>
        ///       Used to transport public keys.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SIMPLEBLOB
        ///     </td>
        ///     <td>
        ///       Used to transport session keys.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PLAINTEXTKEYBLOB
        ///     </td>
        ///     <td>
        ///       A <see cref="PLAINTEXTKEYBLOB"/> used to export any key supported by the CSP in use.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SYMMETRICWRAPKEYBLOB
        ///     </td>
        ///     <td>
        ///       Used to export and import a symmetric key wrapped with another symmetric key. The actual wrapped key is in the format specified in the IETF <a href="https://www.ietf.org/rfc/rfc3217.txt">RFC 3217</a> standard.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Flags">
        /// Specifies additional options for the function. This parameter can be zero or a combination of one or more of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_BLOB_VER3
        ///     </td>
        ///     <td>
        ///       This flag causes this function to export version 3 of a BLOB type.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_DESTROYKEY
        ///     </td>
        ///     <td>
        ///       This flag destroys the original key in the <b>OPAQUEKEYBLOB</b>. This flag is available in <b>Schannel CSP</b>s only.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_OAEP
        ///     </td>
        ///     <td>
        ///       This flag causes PKCS #1 version 2 formatting to be created with the RSA encryption and decryption when exporting SIMPLEBLOBs.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_SSL2_FALLBACK
        ///     </td>
        ///     <td>
        ///       The first eight bytes of the RSA encryption block padding must be set to 0x03 rather than to random data. This prevents version rollback attacks and is discussed in the SSL3 specification. This flag is available for Schannel CSPs only.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_Y_ONLY
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Data">
        /// A pointer to a buffer that receives the key BLOB data. The format of this BLOB varies depending on the BLOB type requested in the <paramref name="BlobType"/> parameter.<br/>
        /// If this parameter is <b>NULL</b>, the required buffer size is placed in the value pointed to by the <paramref name="DataLen"/> parameter.
        /// </param>
        /// <param name="DataLen">
        /// A pointer to a <see cref="Int32"/> value that, on entry, contains the size, in bytes, of the buffer pointed to by the <paramref name="Data"/> parameter. When the function returns, this value contains the number of bytes stored in the buffer.
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> When processing the data returned in the buffer, applications must use the actual size of the data returned. The actual size can be slightly smaller than the size of the buffer specified on input. On input, buffer sizes are usually specified large enough to ensure that the largest possible output data fits in the buffer. On output, the variable pointed to by this parameter is updated to reflect the actual size of the data copied to the buffer.
        ///      </td>
        ///    </tr>
        ///  </table>
        ///  To retrieve the required size of the <paramref name="Data"/> buffer, pass <b>NULL</b> for <paramref name="Data"/>. The required buffer size will be placed in the value pointed to by this parameter.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_MORE_DATA
        ///     </td>
        ///     <td>
        ///       If the buffer specified by the <paramref name="Data"/> parameter is not large enough to hold the returned data, the function sets the <b>ERROR_MORE_DATA</b> code and stores the required buffer size, in bytes, in the variable pointed to by <paramref name="DataLen"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_DATA
        ///     </td>
        ///     <td>
        ///       Either the algorithm that works with the public key to be exported is not supported by this CSP, or an attempt was made to export a session key that was encrypted with something other than one of your public keys.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter is invalid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEY
        ///     </td>
        ///     <td>
        ///       One or both of the keys specified by <paramref name="Key"/> and <paramref name="ExpKey"/> are not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEY_STATE
        ///     </td>
        ///     <td>
        ///       You do not have permission to export the key. That is, when the <paramref name="Key"/> key was created, the <b>CRYPT_EXPORTABLE</b> flag was not specified.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_PUBLIC_KEY
        ///     </td>
        ///     <td>
        ///       The key BLOB type specified by <paramref name="BlobType"/> is <b>PUBLICKEYBLOB</b>, but <paramref name="ExpKey"/> does not contain a public key handle.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The <paramref name="BlobType"/> parameter specifies an unknown BLOB type.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the <paramref name="Key"/> key was created cannot be found.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_NO_KEY
        ///     </td>
        ///     <td>
        ///       A session key is being exported, and the <paramref name="ExpKey"/> parameter does not specify a public key.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptExportKey(IntPtr Key,IntPtr ExpKey,Int32 BlobType,Int32 Flags,Byte[] Data,ref Int32 DataLen);
        #endregion
        #region M:CryptGenKey(IntPtr,ALG_ID,Int32,{out}IntPtr):Boolean
        /// <summary>
        /// The <b>CryptGenKey</b> function generates a random cryptographic session key or a public/private key pair. A handle to the key or key pair is returned in <paramref name="Key"/>. This handle can then be used as needed with any CryptoAPI function that requires a key handle.<br/>
        /// The calling application must specify the algorithm when calling this function. Because this algorithm type is kept bundled with the key, the application does not need to specify the algorithm later when the actual cryptographic operations are performed.
        /// </summary>
        /// <param name="Context">A handle to a cryptographic service provider (CSP) created by a call to <see cref="CryptAcquireContext"/>.</param>
        /// <param name="AlgId">
        /// An <see cref="ALG_ID"/> value that identifies the algorithm for which the key is to be generated. Values for this parameter vary depending on the CSP used.<br/>
        /// For a Diffie-Hellman CSP, use one of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CALG_DH_EPHEM
        ///     </td>
        ///     <td>
        ///       Specifies an "Ephemeral" Diffie-Hellman key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CALG_DH_SF
        ///     </td>
        ///     <td>
        ///       Specifies a "Store and Forward" Diffie-Hellman key.
        ///     </td>
        ///   </tr>
        /// </table>
        /// In addition to generating session keys for symmetric algorithms, this function can also generate public/private key pairs. Each CryptoAPI client generally possesses two public/private key pairs. To generate one of these key pairs, set the Algid parameter to one of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       AT_KEYEXCHANGE
        ///     </td>
        ///     <td>
        ///       Key exchange
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       AT_SIGNATURE
        ///     </td>
        ///     <td>
        ///       Digital signature
        ///     </td>
        ///   </tr>
        /// </table>
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> When key specifications <b>AT_KEYEXCHANGE</b> and <b>AT_SIGNATURE</b> are specified, the algorithm identifiers that are used to generate the key depend on the provider used. As a result, for these key specifications, the values returned from <see cref="CryptGetKeyParam"/> (when the <b>KP_ALGID</b> parameter is specified) depend on the provider used. To determine which algorithm identifier is used by the different providers for the key specs <b>AT_KEYEXCHANGE</b> and <b>AT_SIGNATURE</b>, see <see cref="ALG_ID"/>.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="Flags">
        /// Specifies the type of key generated. The sizes of a session key, RSA signature key, and RSA key exchange keys can be set when the key is generated. The key size, representing the length of the key modulus in bits, is set with the upper 16 bits of this parameter. Thus, if a 2,048-bit RSA signature key is to be generated, the value 0x08000000 is combined with any other <paramref name="Flags"/> predefined value with a bitwise-<b>OR</b> operation. The upper 16 bits of 0x08000000 is 0x0800, or decimal 2,048. The <b>RSA1024BIT_KEY</b> value can be used to specify a 1024-bit RSA key.<br/>
        /// Due to changing export control restrictions, the default CSP and default key length may change between operating system versions. It is important that both the encryption and decryption use the same CSP and that the key length be explicitly set using the <paramref name="Flags"/> parameter to ensure interoperability on different operating system platforms.<br/>
        /// In particular, the default RSA Full Cryptographic Service Provider is the Microsoft RSA Strong Cryptographic Provider. The default DSS Signature Diffie-Hellman Cryptographic Service Provider is the Microsoft Enhanced DSS Diffie-Hellman Cryptographic Provider. Each of these CSPs has a default 128-bit symmetric key length for RC2 and RC4 and a 1,024-bit default key length for public key algorithms.<br/>
        /// If the upper 16 bits is zero, the default key size is generated. If a key larger than the maximum or smaller than the minimum is specified, the call fails with the <b>ERROR_INVALID_PARAMETER</b> code.<br/>
        /// The following table lists minimum, default, and maximum signature and exchange key lengths beginning with Windows XP.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:60%">
        ///       <b>Key type and provider</b>
        ///     </td>
        ///     <td>
        ///       <b>Minimum length</b>
        ///     </td>
        ///     <td>
        ///       <b>Default length</b>
        ///     </td>
        ///     <td>
        ///       <b>Maximum length</b>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       RSA Base Provider Signature and ExchangeKeys
        ///     </td>
        ///     <td>
        ///       384
        ///     </td>
        ///     <td>
        ///       512
        ///     </td>
        ///     <td>
        ///       16,384
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       RSA Strong and Enhanced Providers Signature and Exchange Keys
        ///     </td>
        ///     <td>
        ///       384
        ///     </td>
        ///     <td>
        ///       1,024
        ///     </td>
        ///     <td>
        ///       16,384
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       DSS Base Providers Signature Keys
        ///     </td>
        ///     <td>
        ///       512
        ///     </td>
        ///     <td>
        ///       1,024
        ///     </td>
        ///     <td>
        ///       1,024
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       DSS Base Providers Exchange Keys
        ///     </td>
        ///     <td>
        ///       Not applicable
        ///     </td>
        ///     <td>
        ///       Not applicable
        ///     </td>
        ///     <td>
        ///       Not applicable
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       DSS/DH Base Providers Signature Keys
        ///     </td>
        ///     <td>
        ///       512
        ///     </td>
        ///     <td>
        ///       1,024
        ///     </td>
        ///     <td>
        ///       1,024
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       DSS/DH Base Providers Exchange Keys
        ///     </td>
        ///     <td>
        ///       512
        ///     </td>
        ///     <td>
        ///       512
        ///     </td>
        ///     <td>
        ///       1,024
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       DSS/DH Enhanced Providers Signature Keys
        ///     </td>
        ///     <td>
        ///       512
        ///     </td>
        ///     <td>
        ///       1,024
        ///     </td>
        ///     <td>
        ///       1,024
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       DSS/DH Enhanced Providers Exchange Keys
        ///     </td>
        ///     <td>
        ///       512
        ///     </td>
        ///     <td>
        ///       1,024
        ///     </td>
        ///     <td>
        ///       4,096
        ///     </td>
        ///   </tr>
        /// </table>
        /// For session key lengths, see <see cref="CryptDeriveKey"/>.<br/>
        /// The lower 16-bits of this parameter can be zero or a combination of one or more of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_ARCHIVABLE
        ///     </td>
        ///     <td>
        ///       If this flag is set, the key can be exported until its handle is closed by a call to <see cref="CryptDestroyKey"/>. This allows newly generated keys to be exported upon creation for archiving or key recovery. After the handle is closed, the key is no longer exportable.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_CREATE_IV
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_CREATE_SALT
        ///     </td>
        ///     <td>
        ///       If this flag is set, then the key is assigned a random salt value automatically. You can retrieve this salt value by using the <see cref="CryptGetKeyParam"/> function with the <b>Param</b> parameter set to <b>KP_SALT</b>.<br/>
        ///       If this flag is not set, then the key is given a salt value of zero.<br/>
        ///       When keys with nonzero salt values are exported (through <see cref="CryptExportKey"/>), then the salt value must also be obtained and kept with the key BLOB.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_DATA_KEY
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_EXPORTABLE
        ///     </td>
        ///     <td>
        ///       If this flag is set, then the key can be transferred out of the CSP into a key BLOB by using the <see cref="CryptExportKey"/> function. Because session keys generally must be exportable, this flag should usually be set when they are created.<br/>
        ///       If this flag is not set, then the key is not exportable. For a session key, this means that the key is available only within the current session and only the application that created it will be able to use it. For a public/private key pair, this means that the private key cannot be transported or backed up.<br/>
        ///       This flag applies only to session key and private key BLOBs. It does not apply to public keys, which are always exportable.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_FORCE_KEY_PROTECTION_HIGH
        ///     </td>
        ///     <td>
        ///       This flag specifies strong key protection. When this flag is set, the user is prompted to enter a password for the key when the key is created. The user will be prompted to enter the password whenever this key is used. This flag is only used by the CSPs that are provided by Microsoft. Third party CSPs will define their own behavior for strong key protection.<br/>
        ///       Specifying this flag causes the same result as calling this function with the <b>CRYPT_USER_PROTECTED</b> flag when strong key protection is specified in the system registry.<br/>
        ///       If this flag is specified and the provider handle in the <paramref name="Context"/> parameter was created by using the <b>CRYPT_VERIFYCONTEXT</b> or <b>CRYPT_SILENT</b> flag, this function will set the last error to <b>NTE_SILENT_CONTEXT</b> and return zero.
        ///       <b>Windows Server 2003 and Windows XP</b>: This flag is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_KEK
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_INITIATOR
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_NO_SALT
        ///     </td>
        ///     <td>
        ///       This flag specifies that a no salt value gets allocated for a forty-bit symmetric key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_ONLINE
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_PREGEN
        ///     </td>
        ///     <td>
        ///       This flag specifies an initial Diffie-Hellman or DSS key generation. This flag is useful only with Diffie-Hellman and DSS CSPs. When used, a default key length will be used unless a key length is specified in the upper 16 bits of the <paramref name="Flags"/> parameter. If parameters that involve key lengths are set on a PREGEN Diffie-Hellman or DSS key using <see cref="CryptSetKeyParam"/>, the key lengths must be compatible with the key length set here.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_RECIPIENT
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_SF
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_SGCKEY
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_USER_PROTECTED
        ///     </td>
        ///     <td>
        ///       If this flag is set, the user is notified through a dialog box or another method when certain actions are attempting to use this key. The precise behavior is specified by the CSP being used. If the provider context was opened with the <b>CRYPT_SILENT</b> flag set, using this flag causes a failure and the last error is set to <b>NTE_SILENT_CONTEXT</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_VOLATILE
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Key">Reference to which the function copies the handle of the newly generated key. When you have finished using the key, delete the handle to the key by calling the <see cref="CryptDestroyKey"/> function.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_ALGID
        ///     </td>
        ///     <td>
        ///       The <paramref name="AlgId"/> parameter specifies an algorithm that this CSP does not support.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter contains a value that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The <paramref name="Context"/> parameter does not contain a valid context handle.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_FAIL
        ///     </td>
        ///     <td>
        ///       The function failed in some unexpected way.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_SILENT_CONTEXT
        ///     </td>
        ///     <td>
        ///       The provider could not perform the action because the context was acquired as silent.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptGenKey(IntPtr Context,ALG_ID AlgId,Int32 Flags,out IntPtr Key);
        #endregion
        #region M:CryptGenRandom(IntPtr,Int32,Byte[]):Boolean
        /// <summary>
        /// The <b>CryptGenRandom</b> function fills a buffer with cryptographically random bytes.
        /// </summary>
        /// <param name="Context">Handle of a cryptographic service provider (CSP) created by a call to <see cref="CryptAcquireContext"/>.</param>
        /// <param name="Length">Number of bytes of random data to be generated.</param>
        /// <param name="Buffer">
        /// Buffer to receive the returned data. This buffer must be at least <paramref name="Length"/> bytes in length.<br/>
        /// Optionally, the application can fill this buffer with data to use as an auxiliary random seed.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptGenRandom(IntPtr Context,Int32 Length,Byte[] Buffer);
        #endregion
        #region M:CryptGetHashParam(IntPtr,Int32,Byte[],{ref}Int32):Boolean
        /// <summary>
        /// The <b>CryptGetHashParam</b> function retrieves data that governs the operations of a hash object. The actual hash value can be retrieved by using this function.
        /// </summary>
        /// <param name="Hash">Handle of the hash object to be queried (HCRYPTHASH).</param>
        /// <param name="Parameter">
        /// Query type. This parameter can be set to one of the following queries.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:15%">
        ///       HP_ALGID
        ///     </td>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:15%">
        ///       Hash algorithm
        ///     </td>
        ///     <td>
        ///       An <see cref="ALG_ID"/> that indicates the algorithm specified when the hash object was created. For a list of hash algorithms, see <see cref="CryptCreateHash"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       HP_HASHSIZE
        ///     </td>
        ///     <td>
        ///       Hash value size
        ///     </td>
        ///     <td>
        ///       <see cref="Int32"/> value indicating the number of bytes in the hash value. This value will vary depending on the hash algorithm. Applications must retrieve this value just before the <b>HP_HASHVAL</b> value so the correct amount of memory can be allocated.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       HP_HASHVAL
        ///     </td>
        ///     <td>
        ///       Hash value
        ///     </td>
        ///     <td>
        ///       The hash value or message hash for the hash object specified by <paramref name="Hash"/>. This value is generated based on the data supplied to the hash object earlier through the <see cref="CryptHashData"/> and <see cref="CryptHashSessionKey"/> functions.<br/>
        ///       The <b>CryptGetHashParam</b> function completes the hash. After <b>CryptGetHashParam</b> has been called, no more data can be added to the hash. Additional calls to <see cref="CryptHashData"/> or <see cref="CryptHashSessionKey"/> fail. After the application is done with the hash, <see cref="CryptDestroyHash"/> should be called to destroy the hash object.
        ///     </td>
        ///   </tr>
        /// </table>
        /// <b>CryptoPro CSP</b>:<br/>
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       HP_HASHVAL
        ///     </td>
        ///     <td>
        ///       The little-endian hash value according GostR3411-94-Digest CPCMS [RFC 4490]. Calculated hash value
        ///       returning through <paramref name="Block"/>. Hash value size returning by <paramref name="BlockSize"/>.<br/>
        ///       During this call hash object is closing.<br/>
        ///       GOST R 34.11-94 and GOST R 34.11-2012 (also HMAC) are padding to block size (if required), step functions are executed for the last block (if required), for the length and for the checksum.<br/>
        ///       GOST 28147-89 and GOST R 34.13-2015 HMAC ("magma" and "grasshopper") are padding to block size (if required), and executing last block step function (if required).<br/>
        ///       The hash function calculation can be continued with:
        ///       <list type="bullet">
        ///         <item>subsequent call of CryptSetHashParam(HP_OPEN,TRUE) for CSP 3.6 and above;</item>
        ///         <item>preliminary call <see cref="CryptDuplicateKey"/> (Microsoft);</item>
        ///       </list>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       HP_OID
        ///     </td>
        ///     <td>
        ///       Returns null-terminated OID string. For hash objects GOST R 34.11-94 and MAC GOST 28147-89 returns the identifier of the S-box.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       HP_OPAQUEBLOB
        ///     </td>
        ///     <td>
        ///       Returns a blob containing the internal state of the MAC GOST 28147-89 object.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       HP_HASHSTATEBLOB
        ///     </td>
        ///     <td>
        ///       Returns a blob containing the internal state of hash objects CALG_GR3411, CALG_GR3411_2012_256, CALG_GR3411_2012_512.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       HP_SHAREDKEYMODE
        ///     </td>
        ///     <td>
        ///       Returns the number of components into which the key is decomposed by the CALG_SHAREDKEY_HASH object, or from which the key is assembled by the CALG_FITTINGKEY_HASH object.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       HP_IKE_SPI_COOKIE
        ///     </td>
        ///     <td>
        ///       Returns 32-35 bytes of hash value (when numbering 0..63) hash objects CALG_GR3411_2012_512 and CALG_GR3411_2012_512_HMAC.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Block">
        /// A pointer to a buffer that receives the specified value data. The form of this data varies, depending on the value number.<br/>
        /// This parameter can be <b>NULL</b> to determine the memory size required.
        /// </param>
        /// <param name="BlockSize">
        /// A reference to a <see cref="Int32"/> value specifying the size, in bytes, of the <paramref name="Block"/> buffer. When the function returns, the <see cref="Int32"/> value contains the number of bytes stored in the buffer.<br/>
        /// If <paramref name="Block"/> is <b>NULL</b>, set the value of <paramref name="BlockSize"/> to zero.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns <see langword="true"/>.<br/>
        /// If the function fails, it returns <see langword="false"/>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// The error codes prefaced by "NTE" are generated by the particular CSP you are using. Some possible error codes follow.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_MORE_DATA
        ///     </td>
        ///     <td>
        ///       If the buffer specified by the <paramref name="Block"/> parameter is not large enough to hold the returned data, the function sets the ERROR_MORE_DATA code and stores the required buffer size, in bytes, in the variable pointed to by <paramref name="BlockSize"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_HASH
        ///     </td>
        ///     <td>
        ///       The hash object specified by the <paramref name="Hash"/> parameter is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Parameter"/> parameter specifies an unknown value number.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the hash was created cannot be found.
        ///     </td>
        ///   </tr>
        /// </table>
        /// <b>CryptoPro CSP</b>:<br/>
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       SCARD_W_CANCELLED_BY_USER
        ///     </td>
        ///     <td>
        ///       The requested operation was canceled by the user.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptGetHashParam(IntPtr Hash,Int32 Parameter,Byte[] Block,ref Int32 BlockSize);
        #endregion
        #region M:CryptGetHashParam(IntPtr,Int32,{out}Int32):Boolean
        /// <summary>
        /// The <b>CryptGetHashParam</b> function retrieves data that governs the operations of a hash object. The actual hash value can be retrieved by using this function.
        /// </summary>
        /// <param name="Hash">Handle of the hash object to be queried (HCRYPTHASH).</param>
        /// <param name="Parameter">
        /// Query type. This parameter can be set to one of the following queries.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:15%">
        ///       HP_ALGID
        ///     </td>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:15%">
        ///       Hash algorithm
        ///     </td>
        ///     <td>
        ///       An <see cref="ALG_ID"/> that indicates the algorithm specified when the hash object was created. For a list of hash algorithms, see <see cref="CryptCreateHash"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       HP_HASHSIZE
        ///     </td>
        ///     <td>
        ///       Hash value size
        ///     </td>
        ///     <td>
        ///       <see cref="Int32"/> value indicating the number of bytes in the hash value. This value will vary depending on the hash algorithm. Applications must retrieve this value just before the <b>HP_HASHVAL</b> value so the correct amount of memory can be allocated.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Value">A reference to a variable that receives the specified value.</param>
        /// <returns>
        /// If the function succeeds, the function returns <see langword="true"/>.<br/>
        /// If the function fails, it returns <see langword="false"/>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// The error codes prefaced by "NTE" are generated by the particular CSP you are using. Some possible error codes follow.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_HASH
        ///     </td>
        ///     <td>
        ///       The hash object specified by the <paramref name="Hash"/> parameter is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Parameter"/> parameter specifies an unknown value number.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the hash was created cannot be found.
        ///     </td>
        ///   </tr>
        /// </table>
        /// <b>CryptoPro CSP</b>:<br/>
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       SCARD_W_CANCELLED_BY_USER
        ///     </td>
        ///     <td>
        ///       The requested operation was canceled by the user.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptGetHashParam(IntPtr Hash,Int32 Parameter,out Int32 Value);
        #endregion
        #region M:CryptGetKeyParam(IntPtr,KEY_PARAM,Byte[],{ref}Int32,Int32):Boolean
        /// <summary>
        /// The <b>CryptGetKeyParam</b> function retrieves data that governs the operations of a key. If the <b>Microsoft Cryptographic Service Provider</b> is used, the base symmetric keying material is not obtainable by this or any other function.
        /// </summary>
        /// <param name="Key">The handle of the key being queried.</param>
        /// <param name="Parameter">
        /// Specifies the type of query being made.<br/>
        /// For all key types, this parameter can contain one of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       KP_ALGID
        ///     </td>
        ///     <td>
        ///       Retrieve the key algorithm. The <paramref name="Data"/> parameter is a pointer to an <see cref="ALG_ID"/> value that receives the identifier of the algorithm that was specified when the key was created.<br/>
        ///       When <b>AT_KEYEXCHANGE</b> or <b>AT_SIGNATURE</b> is specified for the <b>AlgId</b> parameter of the <see cref="CryptGenKey"/> function, the algorithm identifiers that are used to generate the key depend on the provider used. For more information, see <see cref="ALG_ID"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_BLOCKLEN
        ///     </td>
        ///     <td>
        ///       If a session key is specified by the <paramref name="Key"/> parameter, retrieve the block length of the key cipher. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the block length, in bits. For stream ciphers, this value is always zero.<br/>
        ///       If a public/private key pair is specified by <paramref name="Key"/>, retrieve the encryption granularity of the key pair. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the encryption granularity, in bits. For example, the Microsoft Base Cryptographic Provider generates 512-bit RSA key pairs, so a value of 512 is returned for these keys. If the public key algorithm does not support encryption, the value retrieved is undefined.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_CERTIFICATE
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> is the address of a buffer that receives the X.509 certificate that has been encoded by using Distinguished Encoding Rules (DER). The public key in the certificate must match the corresponding signature or exchange key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_GET_USE_COUNT
        ///     </td>
        ///     <td>
        ///       This value is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_KEYLEN
        ///     </td>
        ///     <td>
        ///       Retrieve the actual length of the key. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the key length, in bits. <b>KP_KEYLEN</b> can be used to get the length of any key type. Microsoft cryptographic service providers (CSPs) return a key length of 64 bits for <b>CALG_DES</b>, 128 bits for <b>CALG_3DES_112</b>, and 192 bits for <b>CALG_3DES</b>. These lengths are different from the lengths returned when you are enumerating algorithms with the <b>Param</b> value of the <see cref="CryptGetProvParam"/> function set to <b>PP_ENUMALGS</b>. The length returned by this call is the actual size of the key, including the parity bits included in the key.<br/>
        ///       Microsoft CSPs that support the <b>CALG_CYLINK_MEK</b> ALG_ID return 64 bits for that algorithm. <b>CALG_CYLINK_MEK</b> is a 40-bit key but has parity and zeroed key bits to make the key length 64 bits.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_SALT
        ///     </td>
        ///     <td>
        ///       Retrieve the salt value of the key. The <paramref name="Data"/> parameter is a pointer to a <b>BYTE</b> array that receives the salt value in little-endian form. The size of the salt value varies depending on the CSP and algorithm being used. Salt values do not apply to public/private key pairs.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_PERMISSIONS
        ///     </td>
        ///     <td>
        ///       Retrieve the key permissions. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the permission flags for the key.<br/>
        ///       The following permission identifiers are currently defined. The key permissions can be zero or a combination of one or more of the following values.
        ///       <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///         <tr>
        ///           <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///             CRYPT_ARCHIVE
        ///           </td>
        ///           <td>
        ///            Allow export during the lifetime of the handle of the key. This permission can be set only if it is already set in the internal permissions field of the key. Attempts to clear this permission are ignored.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_DECRYPT
        ///           </td>
        ///           <td>
        ///             Allow decryption.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_ENCRYPT
        ///           </td>
        ///           <td>
        ///             Allow encryption.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_EXPORT
        ///           </td>
        ///           <td>
        ///             Allow the key to be exported.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_EXPORT_KEY
        ///           </td>
        ///           <td>
        ///             Allow the key to be used for exporting keys.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_IMPORT_KEY
        ///           </td>
        ///           <td>
        ///             Allow the key to be used for importing keys.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_MAC
        ///           </td>
        ///           <td>
        ///             Allow Message Authentication Codes (MACs) to be used with key.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_READ
        ///           </td>
        ///           <td>
        ///             Allow values to be read.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_WRITE
        ///           </td>
        ///           <td>
        ///             Allow values to be set.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a Digital Signature Standard (DSS) key is specified by the <paramref name="Key"/> parameter, the <paramref name="Parameter"/> value can also be set to one of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       KP_P
        ///     </td>
        ///     <td>
        ///       	Retrieve the modulus prime number P of the DSS key. The <paramref name="Data"/> parameter is a pointer to a buffer that receives the value in little-endian form. The <paramref name="DataLen"/> parameter contains the size of the buffer, in bytes.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_Q
        ///     </td>
        ///     <td>
        ///       Retrieve the modulus prime number Q of the DSS key. The <paramref name="Data"/> parameter is a pointer to a buffer that receives the value in little-endian form. The <paramref name="DataLen"/> parameter contains the size of the buffer, in bytes.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_G
        ///     </td>
        ///     <td>
        ///       Retrieve the generator G of the DSS key. The <paramref name="Data"/> parameter is a pointer to a buffer that receives the value in little-endian form. The <paramref name="DataLen"/> parameter contains the size of the buffer, in bytes.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a block cipher session key is specified by the <paramref name="Key"/> parameter, the <paramref name="Parameter"/> value can also be set to one of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       KP_EFFECTIVE_KEYLEN
        ///     </td>
        ///     <td>
        ///       Retrieve the effective key length of an RC2 key. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the effective key length.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_IV
        ///     </td>
        ///     <td>
        ///       Retrieve the initialization vector of the key. The <paramref name="Data"/> parameter is a pointer to a <b>BYTE</b> array that receives the initialization vector. The size of this array is the block size, in bytes. For example, if the block length is 64 bits, the initialization vector consists of 8 bytes.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_PADDING
        ///     </td>
        ///     <td>
        ///       Retrieve the padding mode. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives a numeric identifier that identifies the padding method used by the cipher. This can be one of the following values.
        ///       <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///         <tr>
        ///           <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///             PKCS5_PADDING
        ///           </td>
        ///           <td>
        ///             Specifies the PKCS 5 (sec 6.2) padding method.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             RANDOM_PADDING
        ///           </td>
        ///           <td>
        ///             The padding uses random numbers. This padding method is not supported by the Microsoft supplied CSPs.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             ZERO_PADDING
        ///           </td>
        ///           <td>
        ///             The padding uses zeros. This padding method is not supported by the Microsoft supplied CSPs.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_MODE
        ///     </td>
        ///     <td>
        ///       Retrieve the cipher mode. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives a cipher mode identifier.
        ///       The following cipher mode identifiers are currently defined.
        ///       <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///         <tr>
        ///           <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///             CRYPT_MODE_CBC
        ///           </td>
        ///           <td>
        ///             The cipher mode is cipher block chaining.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_MODE_CFB
        ///           </td>
        ///           <td>
        ///             The cipher mode is cipher feedback (CFB). Microsoft CSPs currently support only 8-bit feedback in cipher feedback mode.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_MODE_ECB
        ///           </td>
        ///           <td>
        ///             The cipher mode is electronic codebook.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_MODE_OFB
        ///           </td>
        ///           <td>
        ///             The cipher mode is Output Feedback (OFB). Microsoft CSPs currently do not support Output Feedback Mode.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_MODE_CTS
        ///           </td>
        ///           <td>
        ///             The cipher mode is ciphertext stealing mode.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_MODE_BITS
        ///     </td>
        ///     <td>
        ///       Retrieve the number of bits to feed back. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the number of bits that are processed per cycle when the OFB or CFB cipher modes are used.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a Diffie-Hellman algorithm or Digital Signature Algorithm (DSA) key is specified by <paramref name="Key"/>, the <paramref name="Parameter"/> value can also be set to the following value.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       KP_VERIFY_PARAMS
        ///     </td>
        ///     <td>
        ///       Verifies the parameters of a Diffie-Hellman algorithm or DSA key. The <paramref name="Data"/> parameter is not used, and the value pointed to by <paramref name="DataLen"/> receives zero.<br/>
        ///       This function returns a nonzero value if the key parameters are valid or zero otherwise.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_KEYVAL
        ///     </td>
        ///     <td>
        ///       This value is not used.<br/>
        ///       <b>Windows Vista, Windows Server 2003 and Windows XP</b>: Retrieve the secret agreement value from an imported Diffie-Hellman algorithm key of type CALG_AGREEDKEY_ANY. The <paramref name="Data"/> parameter is the address of a buffer that receives the secret agreement value, in little-endian format. This buffer must be the same length as the key. The <paramref name="Flags"/> parameter must be set to 0xF42A19B6. This property can only be retrieved by a thread running under the local system account.This property is available for use in the operating systems listed above. It may be altered or unavailable in subsequent versions.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a certificate is specified by <paramref name="Key"/>, the <paramref name="Parameter"/> value can also be set to the following value.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       KP_CERTIFICATE
        ///     </td>
        ///     <td>
        ///       A buffer that contains the DER-encoded X.509 certificate. The <paramref name="Data"/> parameter is not used, and the value pointed to by <paramref name="DataLen"/> receives zero.<br/>
        ///       This function returns a nonzero value if the key parameters are valid or zero otherwise.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Data">
        /// A pointer to a buffer that receives the data. The form of this data depends on the value of dwParam.<br/>
        /// If the size of this buffer is not known, the required size can be retrieved at run time by passing NULL for this parameter and setting the value pointed to by <paramref name="DataLen"/> to zero. This function will place the required size of the buffer, in bytes, in the value pointed to by <paramref name="DataLen"/>.
        /// </param>
        /// <param name="DataLen">
        /// A pointer to a <see cref="Int32"/> value that, on entry, contains the size, in bytes, of the buffer pointed to by the <paramref name="Data"/> parameter. When the function returns, the <see cref="Int32"/> value contains the number of bytes stored in the buffer.
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> When processing the data returned in the buffer, applications must use the actual size of the data returned. The actual size may be slightly smaller than the size of the buffer specified on input. On input, buffer sizes are sometimes specified large enough to ensure that the largest possible output data fits in the buffer. On output, the variable pointed to by this parameter is updated to reflect the actual size of the data copied to the buffer.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="Flags">This parameter is reserved for future use and must be set to zero.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_MORE_DATA
        ///     </td>
        ///     <td>
        ///       If the buffer specified by the <paramref name="Data"/> parameter is not large enough to hold the returned data, the function sets the ERROR_MORE_DATA code and stores the required buffer size, in bytes, in the variable pointed to by <paramref name="DataLen"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter is nonzero.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEY<br/>NTE_NO_KEY
        ///     </td>
        ///     <td>
        ///       The key specified by the <paramref name="Key"/> parameter is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Parameter"/> parameter specifies an unknown value number.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the key was created cannot be found.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptGetKeyParam(IntPtr Key,KEY_PARAM Parameter,Byte[] Data,ref Int32 DataLen,Int32 Flags);
        #endregion
        #region M:CryptGetKeyParam(IntPtr,KEY_PARAM,IntPtr,{ref}Int32,Int32):Boolean
        /// <summary>
        /// The <b>CryptGetKeyParam</b> function retrieves data that governs the operations of a key. If the <b>Microsoft Cryptographic Service Provider</b> is used, the base symmetric keying material is not obtainable by this or any other function.
        /// </summary>
        /// <param name="Key">The handle of the key being queried.</param>
        /// <param name="Parameter">
        /// Specifies the type of query being made.<br/>
        /// For all key types, this parameter can contain one of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       KP_ALGID
        ///     </td>
        ///     <td>
        ///       Retrieve the key algorithm. The <paramref name="Data"/> parameter is a pointer to an <see cref="ALG_ID"/> value that receives the identifier of the algorithm that was specified when the key was created.<br/>
        ///       When <b>AT_KEYEXCHANGE</b> or <b>AT_SIGNATURE</b> is specified for the <b>AlgId</b> parameter of the <see cref="CryptGenKey"/> function, the algorithm identifiers that are used to generate the key depend on the provider used. For more information, see <see cref="ALG_ID"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_BLOCKLEN
        ///     </td>
        ///     <td>
        ///       If a session key is specified by the <paramref name="Key"/> parameter, retrieve the block length of the key cipher. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the block length, in bits. For stream ciphers, this value is always zero.<br/>
        ///       If a public/private key pair is specified by <paramref name="Key"/>, retrieve the encryption granularity of the key pair. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the encryption granularity, in bits. For example, the Microsoft Base Cryptographic Provider generates 512-bit RSA key pairs, so a value of 512 is returned for these keys. If the public key algorithm does not support encryption, the value retrieved is undefined.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_CERTIFICATE
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> is the address of a buffer that receives the X.509 certificate that has been encoded by using Distinguished Encoding Rules (DER). The public key in the certificate must match the corresponding signature or exchange key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_GET_USE_COUNT
        ///     </td>
        ///     <td>
        ///       This value is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_KEYLEN
        ///     </td>
        ///     <td>
        ///       Retrieve the actual length of the key. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the key length, in bits. <b>KP_KEYLEN</b> can be used to get the length of any key type. Microsoft cryptographic service providers (CSPs) return a key length of 64 bits for <b>CALG_DES</b>, 128 bits for <b>CALG_3DES_112</b>, and 192 bits for <b>CALG_3DES</b>. These lengths are different from the lengths returned when you are enumerating algorithms with the <b>Param</b> value of the <see cref="CryptGetProvParam"/> function set to <b>PP_ENUMALGS</b>. The length returned by this call is the actual size of the key, including the parity bits included in the key.<br/>
        ///       Microsoft CSPs that support the <b>CALG_CYLINK_MEK</b> ALG_ID return 64 bits for that algorithm. <b>CALG_CYLINK_MEK</b> is a 40-bit key but has parity and zeroed key bits to make the key length 64 bits.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_SALT
        ///     </td>
        ///     <td>
        ///       Retrieve the salt value of the key. The <paramref name="Data"/> parameter is a pointer to a <b>BYTE</b> array that receives the salt value in little-endian form. The size of the salt value varies depending on the CSP and algorithm being used. Salt values do not apply to public/private key pairs.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_PERMISSIONS
        ///     </td>
        ///     <td>
        ///       Retrieve the key permissions. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the permission flags for the key.<br/>
        ///       The following permission identifiers are currently defined. The key permissions can be zero or a combination of one or more of the following values.
        ///       <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///         <tr>
        ///           <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///             CRYPT_ARCHIVE
        ///           </td>
        ///           <td>
        ///            Allow export during the lifetime of the handle of the key. This permission can be set only if it is already set in the internal permissions field of the key. Attempts to clear this permission are ignored.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_DECRYPT
        ///           </td>
        ///           <td>
        ///             Allow decryption.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_ENCRYPT
        ///           </td>
        ///           <td>
        ///             Allow encryption.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_EXPORT
        ///           </td>
        ///           <td>
        ///             Allow the key to be exported.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_EXPORT_KEY
        ///           </td>
        ///           <td>
        ///             Allow the key to be used for exporting keys.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_IMPORT_KEY
        ///           </td>
        ///           <td>
        ///             Allow the key to be used for importing keys.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_MAC
        ///           </td>
        ///           <td>
        ///             Allow Message Authentication Codes (MACs) to be used with key.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_READ
        ///           </td>
        ///           <td>
        ///             Allow values to be read.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_WRITE
        ///           </td>
        ///           <td>
        ///             Allow values to be set.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a Digital Signature Standard (DSS) key is specified by the <paramref name="Key"/> parameter, the <paramref name="Parameter"/> value can also be set to one of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       KP_P
        ///     </td>
        ///     <td>
        ///       	Retrieve the modulus prime number P of the DSS key. The <paramref name="Data"/> parameter is a pointer to a buffer that receives the value in little-endian form. The <paramref name="DataLen"/> parameter contains the size of the buffer, in bytes.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_Q
        ///     </td>
        ///     <td>
        ///       Retrieve the modulus prime number Q of the DSS key. The <paramref name="Data"/> parameter is a pointer to a buffer that receives the value in little-endian form. The <paramref name="DataLen"/> parameter contains the size of the buffer, in bytes.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_G
        ///     </td>
        ///     <td>
        ///       Retrieve the generator G of the DSS key. The <paramref name="Data"/> parameter is a pointer to a buffer that receives the value in little-endian form. The <paramref name="DataLen"/> parameter contains the size of the buffer, in bytes.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a block cipher session key is specified by the <paramref name="Key"/> parameter, the <paramref name="Parameter"/> value can also be set to one of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       KP_EFFECTIVE_KEYLEN
        ///     </td>
        ///     <td>
        ///       Retrieve the effective key length of an RC2 key. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the effective key length.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_IV
        ///     </td>
        ///     <td>
        ///       Retrieve the initialization vector of the key. The <paramref name="Data"/> parameter is a pointer to a <b>BYTE</b> array that receives the initialization vector. The size of this array is the block size, in bytes. For example, if the block length is 64 bits, the initialization vector consists of 8 bytes.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_PADDING
        ///     </td>
        ///     <td>
        ///       Retrieve the padding mode. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives a numeric identifier that identifies the padding method used by the cipher. This can be one of the following values.
        ///       <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///         <tr>
        ///           <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///             PKCS5_PADDING
        ///           </td>
        ///           <td>
        ///             Specifies the PKCS 5 (sec 6.2) padding method.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             RANDOM_PADDING
        ///           </td>
        ///           <td>
        ///             The padding uses random numbers. This padding method is not supported by the Microsoft supplied CSPs.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             ZERO_PADDING
        ///           </td>
        ///           <td>
        ///             The padding uses zeros. This padding method is not supported by the Microsoft supplied CSPs.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_MODE
        ///     </td>
        ///     <td>
        ///       Retrieve the cipher mode. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives a cipher mode identifier.
        ///       The following cipher mode identifiers are currently defined.
        ///       <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///         <tr>
        ///           <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///             CRYPT_MODE_CBC
        ///           </td>
        ///           <td>
        ///             The cipher mode is cipher block chaining.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_MODE_CFB
        ///           </td>
        ///           <td>
        ///             The cipher mode is cipher feedback (CFB). Microsoft CSPs currently support only 8-bit feedback in cipher feedback mode.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_MODE_ECB
        ///           </td>
        ///           <td>
        ///             The cipher mode is electronic codebook.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_MODE_OFB
        ///           </td>
        ///           <td>
        ///             The cipher mode is Output Feedback (OFB). Microsoft CSPs currently do not support Output Feedback Mode.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_MODE_CTS
        ///           </td>
        ///           <td>
        ///             The cipher mode is ciphertext stealing mode.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_MODE_BITS
        ///     </td>
        ///     <td>
        ///       Retrieve the number of bits to feed back. The <paramref name="Data"/> parameter is a pointer to a <see cref="Int32"/> value that receives the number of bits that are processed per cycle when the OFB or CFB cipher modes are used.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a Diffie-Hellman algorithm or Digital Signature Algorithm (DSA) key is specified by <paramref name="Key"/>, the <paramref name="Parameter"/> value can also be set to the following value.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       KP_VERIFY_PARAMS
        ///     </td>
        ///     <td>
        ///       Verifies the parameters of a Diffie-Hellman algorithm or DSA key. The <paramref name="Data"/> parameter is not used, and the value pointed to by <paramref name="DataLen"/> receives zero.<br/>
        ///       This function returns a nonzero value if the key parameters are valid or zero otherwise.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_KEYVAL
        ///     </td>
        ///     <td>
        ///       This value is not used.<br/>
        ///       <b>Windows Vista, Windows Server 2003 and Windows XP</b>: Retrieve the secret agreement value from an imported Diffie-Hellman algorithm key of type CALG_AGREEDKEY_ANY. The <paramref name="Data"/> parameter is the address of a buffer that receives the secret agreement value, in little-endian format. This buffer must be the same length as the key. The <paramref name="Flags"/> parameter must be set to 0xF42A19B6. This property can only be retrieved by a thread running under the local system account.This property is available for use in the operating systems listed above. It may be altered or unavailable in subsequent versions.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a certificate is specified by <paramref name="Key"/>, the <paramref name="Parameter"/> value can also be set to the following value.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       KP_CERTIFICATE
        ///     </td>
        ///     <td>
        ///       A buffer that contains the DER-encoded X.509 certificate. The <paramref name="Data"/> parameter is not used, and the value pointed to by <paramref name="DataLen"/> receives zero.<br/>
        ///       This function returns a nonzero value if the key parameters are valid or zero otherwise.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Data">
        /// A pointer to a buffer that receives the data. The form of this data depends on the value of dwParam.<br/>
        /// If the size of this buffer is not known, the required size can be retrieved at run time by passing NULL for this parameter and setting the value pointed to by <paramref name="DataLen"/> to zero. This function will place the required size of the buffer, in bytes, in the value pointed to by <paramref name="DataLen"/>.
        /// </param>
        /// <param name="DataLen">
        /// A pointer to a <see cref="Int32"/> value that, on entry, contains the size, in bytes, of the buffer pointed to by the <paramref name="Data"/> parameter. When the function returns, the <see cref="Int32"/> value contains the number of bytes stored in the buffer.
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> When processing the data returned in the buffer, applications must use the actual size of the data returned. The actual size may be slightly smaller than the size of the buffer specified on input. On input, buffer sizes are sometimes specified large enough to ensure that the largest possible output data fits in the buffer. On output, the variable pointed to by this parameter is updated to reflect the actual size of the data copied to the buffer.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="Flags">This parameter is reserved for future use and must be set to zero.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_MORE_DATA
        ///     </td>
        ///     <td>
        ///       If the buffer specified by the <paramref name="Data"/> parameter is not large enough to hold the returned data, the function sets the ERROR_MORE_DATA code and stores the required buffer size, in bytes, in the variable pointed to by <paramref name="DataLen"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter is nonzero.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEY<br/>NTE_NO_KEY
        ///     </td>
        ///     <td>
        ///       The key specified by the <paramref name="Key"/> parameter is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Parameter"/> parameter specifies an unknown value number.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the key was created cannot be found.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptGetKeyParam(IntPtr Key,KEY_PARAM Parameter,IntPtr Data,ref Int32 DataLen,Int32 Flags);
        #endregion
        #region M:CryptGetProvParam(IntPtr,CRYPT_PARAM,Byte[],{ref}Int32,Int32):Boolean
        /// <summary>
        /// The <b>CryptGetProvParam</b> function retrieves parameters that govern the operations of a cryptographic service provider (CSP).
        /// </summary>
        /// <param name="Context">A handle of the CSP target of the query. This handle must have been created by using the <see cref="CryptAcquireContext"/> function.</param>
        /// <param name="Parameter">
        /// The nature of the query. The following queries are defined.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       PP_ADMIN_PIN
        ///     </td>
        ///     <td>
        ///       Returns the administrator personal identification number (PIN) in the <paramref name="Data"/> parameter as a <b>LPSTR</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_APPLI_CERT
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_CHANGE_PASSWORD
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_CERTCHAIN
        ///     </td>
        ///     <td>
        ///       Returns the certificate chain associated with the <paramref name="Context"/> handle. The returned certificate chain is <b>X509_ASN_ENCODING</b> encoded.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_CONTAINER
        ///     </td>
        ///     <td>
        ///       The name of the current key container as a null-terminated <b>CHAR</b> string. This string is exactly the same as the one passed in the <b>Container</b> parameter of the <see cref="CryptAcquireContext"/> function to specify the key container to use. The <b>Container</b> parameter can be read to determine the name of the default key container.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_CRYPT_COUNT_KEY_USE
        ///     </td>
        ///     <td>
        ///       Not implemented by Microsoft CSPs. This behavior may be implemented by other CSPs.
        ///       <b>Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMALGS
        ///     </td>
        ///     <td>
        ///       A <see cref="PROV_ENUMALGS"/> structure that contains information about one algorithm supported by the CSP being queried.<br/>
        ///       The first time this value is read, the dwFlags parameter must contain the <b>CRYPT_FIRST</b> flag. Doing so causes this function to retrieve the first element in the enumeration. The subsequent elements can then be retrieved by setting the <b>CRYPT_NEXT</b> flag in the <paramref name="Flags"/> parameter. When this function fails with the <b>ERROR_NO_MORE_ITEMS</b> error code, the end of the enumeration has been reached.<br/>
        ///       This function is not thread safe, and all of the available algorithms might not be enumerated if this function is used in a multithreaded context.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMALGS_EX
        ///     </td>
        ///     <td>
        ///       A <see cref="PROV_ENUMALGS_EX"/> structure that contains information about one algorithm supported by the CSP being queried. The structure returned contains more information about the algorithm than the structure returned for <b>PP_ENUMALGS</b>.<br/>
        ///       The first time this value is read, the <paramref name="Flags"/> parameter must contain the <b>CRYPT_FIRST</b> flag. Doing so causes this function to retrieve the first element in the enumeration. The subsequent elements can then be retrieved by setting the <b>CRYPT_NEXT</b> flag in the <paramref name="Flags"/> parameter. When this function fails with the <b>ERROR_NO_MORE_ITEMS</b> error code, the end of the enumeration has been reached.<br/>
        ///       This function is not thread safe and all of the available algorithms might not be enumerated if this function is used in a multithreaded context.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMCONTAINERS
        ///     </td>
        ///     <td>
        ///       The name of one of the key containers maintained by the CSP in the form of a null-terminated <b>CHAR</b> string.<br/>
        ///       The first time this value is read, the dwFlags parameter must contain the <b>CRYPT_FIRST</b> flag. Doing so causes this function to retrieve the first element in the enumeration. The subsequent elements can then be retrieved by setting the <b>CRYPT_NEXT</b> flag in the <paramref name="Flags"/> parameter. When this function fails with the <b>ERROR_NO_MORE_ITEMS</b> error code, the end of the enumeration has been reached.<br/>
        ///       To enumerate key containers associated with a computer, first call <see cref="CryptAcquireContext"/> using the <b>CRYPT_MACHINE_KEYSET</b> flag, and then use the handle returned from <see cref="CryptAcquireContext"/> as the <b>Context</b> parameter in the call to <see cref="CryptGetProvParam"/>.<br/>
        ///       This function is not thread safe and all of the available algorithms might not be enumerated if this function is used in a multithreaded context.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMELECTROOTS
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMEX_SIGNING_PROT
        ///     </td>
        ///     <td>
        ///       Indicates that the current CSP supports the <see cref="PROV_ENUMALGS_EX.Protocols"/> member of the <see cref="PROV_ENUMALGS_EX"/> structure. If this function succeeds, the CSP supports the <see cref="PROV_ENUMALGS_EX.Protocols"/> member of the <see cref="PROV_ENUMALGS_EX"/> structure. If this function fails with an <b>NTE_BAD_TYPE</b> error code, the CSP does not support the dwProtocols member.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMMANDROOTS
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_IMPTYPE
        ///     </td>
        ///     <td>
        ///       A <see cref="Int32"/> value that indicates how the CSP is implemented. 
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEY_TYPE_SUBTYPE
        ///     </td>
        ///     <td>
        ///       This query is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYEXCHANGE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies that the key exchange PIN is contained in <paramref name="Data"/>. The PIN is represented as a <b>null</b>-terminated ASCII string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYSET_SEC_DESCR
        ///     </td>
        ///     <td>
        ///       Retrieves the security descriptor for the key storage container. The <paramref name="Data"/> parameter is the address of a <see cref="SECURITY_DESCRIPTOR"/> structure that receives the security descriptor for the key storage container. The security descriptor is returned in self-relative format.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYSET_TYPE
        ///     </td>
        ///     <td>
        ///       Determines whether the <paramref name="Context"/> parameter is a computer key set. The <paramref name="Data"/> parameter must be a <see cref="Int32"/>; the <see cref="Int32"/> will be set to the <b>CRYPT_MACHINE_KEYSET</b> flag if that flag was passed to the <see cref="CryptAcquireContext"/> function.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYSPEC
        ///     </td>
        ///     <td>
        ///       Returns information about the key specifier values that the CSP supports. Key specifier values are joined in a logical <b>OR</b> and returned in the <paramref name="Data"/> parameter of the call as a <see cref="Int32"/>. For example, the Microsoft Base Cryptographic Provider version 1.0 returns a <see cref="Int32"/> value of AT_SIGNATURE | AT_KEYEXCHANGE.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYSTORAGE
        ///     </td>
        ///     <td>
        ///       Returns a <see cref="Int32"/> value of CRYPT_SEC_DESCR.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYX_KEYSIZE_INC
        ///     </td>
        ///     <td>
        ///       The number of bits for the increment length of <b>AT_KEYEXCHANGE</b>. This information is used with information returned in the <b>PP_ENUMALGS_EX</b> value. With the information returned when using <b>PP_ENUMALGS_EX</b> and <b>PP_KEYX_KEYSIZE_INC</b>, the valid key lengths for <b>AT_KEYEXCHANGE</b> can be determined. These key lengths can then be used with <see cref="CryptGenKey"/>. For example if a CSP enumerates <b>CALG_RSA_KEYX</b> (AT_KEYEXCHANGE) with a minimum key length of 512 bits and a maximum of 1024 bits, and returns the increment length as 64 bits, then valid key lengths are 512, 576, 640,… 1024.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_NAME
        ///     </td>
        ///     <td>
        ///       The name of the CSP in the form of a null-terminated <b>CHAR</b> string. This string is identical to the one passed in the <b>Provider</b> parameter of the <see cref="CryptAcquireContext"/> function to specify that the current CSP be used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_PROVTYPE
        ///     </td>
        ///     <td>
        ///       A <see cref="Int32"/> value that indicates the provider type of the CSP.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ROOT_CERTSTORE
        ///     </td>
        ///     <td>
        ///       Obtains the root certificate store for the smart card. This certificate store contains all of the root certificates that are stored on the smart card.<br/>
        ///       The <paramref name="Data"/> parameter is the address of an <b>HCERTSTORE</b> variable that receives the handle of the certificate store. When this handle is no longer needed, the caller must close it by using the <see cref="CertCloseStore"/> function.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SESSION_KEYSIZE
        ///     </td>
        ///     <td>
        ///       The size, in bits, of the session key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SGC_INFO
        ///     </td>
        ///     <td>
        ///       Used with server gated cryptography.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SIG_KEYSIZE_INC
        ///     </td>
        ///     <td>
        ///       The number of bits for the increment length of <b>AT_SIGNATURE</b>. This information is used with information returned in the <b>PP_ENUMALGS_EX</b> value. With the information returned when using <b>PP_ENUMALGS_EX</b> and <b>PP_SIG_KEYSIZE_INC</b>, the valid key lengths for <b>AT_SIGNATURE</b> can be determined. These key lengths can then be used with <see cref="CryptGenKey"/>.<br/>
        ///       For example, if a CSP enumerates <b>CALG_RSA_SIGN</b> (AT_SIGNATURE) with a minimum key length of 512 bits and a maximum of 1024 bits, and returns the increment length as 64 bits, then valid key lengths are 512, 576, 640,… 1024.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SIGNATURE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies that the key signature PIN is contained in <paramref name="Data"/>. The PIN is represented as a <b>null</b>-terminated ASCII string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SMARTCARD_GUID
        ///     </td>
        ///     <td>
        ///       Obtains the identifier of the smart card. The <paramref name="Data"/> parameter is the address of a <b>GUID</b> structure that receives the identifier of the smart card.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SMARTCARD_READER
        ///     </td>
        ///     <td>
        ///       Obtains the name of the smart card reader. The <paramref name="Data"/> parameter is the address of an ANSI character array that receives a <b>null</b>-terminated ANSI string that contains the name of the smart card reader. The size of this buffer, contained in the variable pointed to by the <paramref name="DataSize"/> parameter, must include the <b>NULL</b> terminator.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SYM_KEYSIZE
        ///     </td>
        ///     <td>
        ///       The size of the symmetric key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_UI_PROMPT
        ///     </td>
        ///     <td>
        ///       This query is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_UNIQUE_CONTAINER
        ///     </td>
        ///     <td>
        ///       The unique container name of the current key container in the form of a <b>null</b>-terminated <b>CHAR</b> string. For many CSPs, this name is the same name returned when the <b>PP_CONTAINER</b> value is used. The <see cref="CryptAcquireContext"/> function must work with this container name.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_USE_HARDWARE_RNG
        ///     </td>
        ///     <td>
        ///       Indicates whether a hardware random number generator (RNG) is supported. When <b>PP_USE_HARDWARE_RNG</b> is specified, the function succeeds and returns <b>TRUE</b> if a hardware RNG is supported. The function fails and returns <b>FALSE</b> if a hardware RNG is not supported. If a RNG is supported, <b>PP_USE_HARDWARE_RNG</b> can be set in <see cref="CryptSetProvParam"/> to indicate that the CSP must exclusively use the hardware RNG for this provider context. When <b>PP_USE_HARDWARE_RNG</b> is used, the <paramref name="Data"/> parameter must be <b>NULL</b> and <paramref name="Flags"/> must be zero.<br/>
        ///       None of the Microsoft CSPs currently support using a hardware RNG.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_USER_CERTSTORE
        ///     </td>
        ///     <td>
        ///       Obtains the user certificate store for the smart card. This certificate store contains all of the user certificates that are stored on the smart card. The certificates in this store are encoded by using PKCS_7_ASN_ENCODING or X509_ASN_ENCODING encoding and should contain the <b>CERT_KEY_PROV_INFO_PROP_ID</b> property.<br/>
        ///       The <paramref name="Data"/> parameter is the address of an <b>HCERTSTORE</b> variable that receives the handle of an in-memory certificate store. When this handle is no longer needed, the caller must close it by using the <see cref="CertCloseStore"/> function.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_VERSION
        ///     </td>
        ///     <td>
        ///       The version number of the CSP. The least significant byte contains the minor version number and the next most significant byte the major version number. Version 2.0 is represented as 0x00000200. To maintain backward compatibility with earlier versions of the Microsoft Base Cryptographic Provider and the Microsoft Enhanced Cryptographic Provider, the provider names retain the "v1.0" designation in later versions.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Data">
        /// A pointer to a buffer to receive the data. The form of this data varies depending on the value of <paramref name="Parameter"/>. When <paramref name="Parameter"/> is set to <b>PP_USE_HARDWARE_RNG</b>, <paramref name="Data"/> must be set to <b>NULL</b>.
        /// This parameter can be <b>NULL</b> to set the size of this information for memory allocation purposes.
        /// </param>
        /// <param name="DataSize">
        /// A pointer to a <see cref="Int32"/> value that specifies the size, in bytes, of the buffer pointed to by the <paramref name="Data"/> parameter. When the function returns, the <see cref="Int32"/> value contains the number of bytes stored or to be stored in the buffer.<br/>
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> When processing the data returned in the buffer, applications must use the actual size of the data returned. The actual size can be slightly smaller than the size of the buffer specified on input. (On input, buffer sizes are usually specified large enough to ensure that the largest possible output data fits in the buffer.) On output, the variable pointed to by this parameter is updated to reflect the actual size of the data copied to the buffer.<br/>
        ///        If <b>PP_ENUMALGS</b>, or <b>PP_ENUMALGS_EX</b> is set, the <paramref name="DataSize"/> parameter works somewhat differently. If <paramref name="Data"/> is <b>NULL</b> or the value pointed to by <paramref name="DataSize"/> is too small, the value returned in this parameter is the size of the largest item in the enumeration list instead of the size of the item currently being read.<br/>
        ///        If <b>PP_ENUMCONTAINERS</b> is set, the first call to the function returns the size of the maximum key-container allowed by the current provider. This is in contrast to other possible behaviors, like returning the length of the longest existing container, or the length of the current container. Subsequent enumerating calls will not change the <paramref name="DataSize"/> parameter. For each enumerated container, the caller can determine the length of the null-terminated string programmatically, if desired. If one of the enumeration values is read and the <paramref name="Data"/> parameter is <b>NULL</b>, the <b>CRYPT_FIRST</b> flag must be specified for the size information to be correctly retrieved.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="Flags">
        /// If <paramref name="Parameter"/> is <b>PP_KEYSET_SEC_DESCR</b>, the security descriptor on the key container where the keys are stored is retrieved. For this case, <paramref name="Flags"/> is used to pass in the <b>SECURITY_INFORMATION</b> bit flags that indicate the requested security information, as defined in the Platform SDK. <b>SECURITY_INFORMATION</b> bit flags can be combined with a bitwise-<b>OR</b> operation.<br/>
        /// The following values are defined for use with <b>PP_KEYSET_SEC_DESCR</b>.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       OWNER_SECURITY_INFORMATION
        ///     </td>
        ///     <td>
        ///       Owner identifier of the object is being referenced.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       GROUP_SECURITY_INFORMATION
        ///     </td>
        ///     <td>
        ///       Primary group identifier of the object is being referenced.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       DACL_SECURITY_INFORMATION
        ///     </td>
        ///     <td>
        ///       Discretionary ACL of the object is being referenced.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SACL_SECURITY_INFORMATION
        ///     </td>
        ///     <td>
        ///       System ACL of the object is being referenced.
        ///     </td>
        ///   </tr>
        /// </table>
        /// The following values are defined for use with <b>PP_ENUMALGS</b>, <b>PP_ENUMALGS_EX</b>, and <b>PP_ENUMCONTAINERS</b>.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_FIRST
        ///     </td>
        ///     <td>
        ///       Retrieve the first element in the enumeration. This has the same effect as resetting the enumerator.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_NEXT
        ///     </td>
        ///     <td>
        ///       Retrieve the next element in the enumeration. When there are no more elements to retrieve, this function will fail and set the last error to <b>ERROR_NO_MORE_ITEMS</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_SGC_ENUM
        ///     </td>
        ///     <td>
        ///       Retrieve server-gated cryptography (SGC) enabled certificates. SGC enabled certificates are no longer supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_SGC
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_FASTSGC
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_MORE_DATA
        ///     </td>
        ///     <td>
        ///       If the buffer specified by the <paramref name="Data"/> parameter is not large enough to hold the returned data, the function sets the <b>ERROR_MORE_DATA</b> code and stores the required buffer size, in bytes, in the variable pointed to by <paramref name="DataSize"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_NO_MORE_ITEMS
        ///     </td>
        ///     <td>
        ///       The end of the enumeration list has been reached. No valid data has been placed in the <paramref name="Data"/> buffer. This error code is returned only when <paramref name="Parameter"/> equals <b>PP_ENUMALGS</b> or <b>PP_ENUMCONTAINERS</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter specifies a flag that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Parameter"/> parameter specifies an unknown value number.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context specified by <paramref name="Context"/> is not valid.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,Byte[] Data,ref Int32 DataSize,Int32 Flags);
        #endregion
        #region M:CryptGetProvParam(IntPtr,CRYPT_PARAM,IntPtr,{ref}Int32,Int32):Boolean
        /// <summary>
        /// The <b>CryptGetProvParam</b> function retrieves parameters that govern the operations of a cryptographic service provider (CSP).
        /// </summary>
        /// <param name="Context">A handle of the CSP target of the query. This handle must have been created by using the <see cref="CryptAcquireContext"/> function.</param>
        /// <param name="Parameter">
        /// The nature of the query. The following queries are defined.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       PP_ADMIN_PIN
        ///     </td>
        ///     <td>
        ///       Returns the administrator personal identification number (PIN) in the <paramref name="Data"/> parameter as a <b>LPSTR</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_APPLI_CERT
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_CHANGE_PASSWORD
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_CERTCHAIN
        ///     </td>
        ///     <td>
        ///       Returns the certificate chain associated with the <paramref name="Context"/> handle. The returned certificate chain is <b>X509_ASN_ENCODING</b> encoded.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_CONTAINER
        ///     </td>
        ///     <td>
        ///       The name of the current key container as a null-terminated <b>CHAR</b> string. This string is exactly the same as the one passed in the <b>Container</b> parameter of the <see cref="CryptAcquireContext"/> function to specify the key container to use. The <b>Container</b> parameter can be read to determine the name of the default key container.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_CRYPT_COUNT_KEY_USE
        ///     </td>
        ///     <td>
        ///       Not implemented by Microsoft CSPs. This behavior may be implemented by other CSPs.
        ///       <b>Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMALGS
        ///     </td>
        ///     <td>
        ///       A <see cref="PROV_ENUMALGS"/> structure that contains information about one algorithm supported by the CSP being queried.<br/>
        ///       The first time this value is read, the dwFlags parameter must contain the <b>CRYPT_FIRST</b> flag. Doing so causes this function to retrieve the first element in the enumeration. The subsequent elements can then be retrieved by setting the <b>CRYPT_NEXT</b> flag in the <paramref name="Flags"/> parameter. When this function fails with the <b>ERROR_NO_MORE_ITEMS</b> error code, the end of the enumeration has been reached.<br/>
        ///       This function is not thread safe, and all of the available algorithms might not be enumerated if this function is used in a multithreaded context.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMALGS_EX
        ///     </td>
        ///     <td>
        ///       A <see cref="PROV_ENUMALGS_EX"/> structure that contains information about one algorithm supported by the CSP being queried. The structure returned contains more information about the algorithm than the structure returned for <b>PP_ENUMALGS</b>.<br/>
        ///       The first time this value is read, the <paramref name="Flags"/> parameter must contain the <b>CRYPT_FIRST</b> flag. Doing so causes this function to retrieve the first element in the enumeration. The subsequent elements can then be retrieved by setting the <b>CRYPT_NEXT</b> flag in the <paramref name="Flags"/> parameter. When this function fails with the <b>ERROR_NO_MORE_ITEMS</b> error code, the end of the enumeration has been reached.<br/>
        ///       This function is not thread safe and all of the available algorithms might not be enumerated if this function is used in a multithreaded context.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMCONTAINERS
        ///     </td>
        ///     <td>
        ///       The name of one of the key containers maintained by the CSP in the form of a null-terminated <b>CHAR</b> string.<br/>
        ///       The first time this value is read, the dwFlags parameter must contain the <b>CRYPT_FIRST</b> flag. Doing so causes this function to retrieve the first element in the enumeration. The subsequent elements can then be retrieved by setting the <b>CRYPT_NEXT</b> flag in the <paramref name="Flags"/> parameter. When this function fails with the <b>ERROR_NO_MORE_ITEMS</b> error code, the end of the enumeration has been reached.<br/>
        ///       To enumerate key containers associated with a computer, first call <see cref="CryptAcquireContext"/> using the <b>CRYPT_MACHINE_KEYSET</b> flag, and then use the handle returned from <see cref="CryptAcquireContext"/> as the <b>Context</b> parameter in the call to <see cref="CryptGetProvParam"/>.<br/>
        ///       This function is not thread safe and all of the available algorithms might not be enumerated if this function is used in a multithreaded context.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMELECTROOTS
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMEX_SIGNING_PROT
        ///     </td>
        ///     <td>
        ///       Indicates that the current CSP supports the <see cref="PROV_ENUMALGS_EX.Protocols"/> member of the <see cref="PROV_ENUMALGS_EX"/> structure. If this function succeeds, the CSP supports the <see cref="PROV_ENUMALGS_EX.Protocols"/> member of the <see cref="PROV_ENUMALGS_EX"/> structure. If this function fails with an <b>NTE_BAD_TYPE</b> error code, the CSP does not support the dwProtocols member.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ENUMMANDROOTS
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_IMPTYPE
        ///     </td>
        ///     <td>
        ///       A <see cref="Int32"/> value that indicates how the CSP is implemented. 
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEY_TYPE_SUBTYPE
        ///     </td>
        ///     <td>
        ///       This query is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYEXCHANGE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies that the key exchange PIN is contained in <paramref name="Data"/>. The PIN is represented as a <b>null</b>-terminated ASCII string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYSET_SEC_DESCR
        ///     </td>
        ///     <td>
        ///       Retrieves the security descriptor for the key storage container. The <paramref name="Data"/> parameter is the address of a <see cref="SECURITY_DESCRIPTOR"/> structure that receives the security descriptor for the key storage container. The security descriptor is returned in self-relative format.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYSET_TYPE
        ///     </td>
        ///     <td>
        ///       Determines whether the <paramref name="Context"/> parameter is a computer key set. The <paramref name="Data"/> parameter must be a <see cref="Int32"/>; the <see cref="Int32"/> will be set to the <b>CRYPT_MACHINE_KEYSET</b> flag if that flag was passed to the <see cref="CryptAcquireContext"/> function.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYSPEC
        ///     </td>
        ///     <td>
        ///       Returns information about the key specifier values that the CSP supports. Key specifier values are joined in a logical <b>OR</b> and returned in the <paramref name="Data"/> parameter of the call as a <see cref="Int32"/>. For example, the Microsoft Base Cryptographic Provider version 1.0 returns a <see cref="Int32"/> value of AT_SIGNATURE | AT_KEYEXCHANGE.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYSTORAGE
        ///     </td>
        ///     <td>
        ///       Returns a <see cref="Int32"/> value of CRYPT_SEC_DESCR.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYX_KEYSIZE_INC
        ///     </td>
        ///     <td>
        ///       The number of bits for the increment length of <b>AT_KEYEXCHANGE</b>. This information is used with information returned in the <b>PP_ENUMALGS_EX</b> value. With the information returned when using <b>PP_ENUMALGS_EX</b> and <b>PP_KEYX_KEYSIZE_INC</b>, the valid key lengths for <b>AT_KEYEXCHANGE</b> can be determined. These key lengths can then be used with <see cref="CryptGenKey"/>. For example if a CSP enumerates <b>CALG_RSA_KEYX</b> (AT_KEYEXCHANGE) with a minimum key length of 512 bits and a maximum of 1024 bits, and returns the increment length as 64 bits, then valid key lengths are 512, 576, 640,… 1024.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_NAME
        ///     </td>
        ///     <td>
        ///       The name of the CSP in the form of a null-terminated <b>CHAR</b> string. This string is identical to the one passed in the <b>Provider</b> parameter of the <see cref="CryptAcquireContext"/> function to specify that the current CSP be used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_PROVTYPE
        ///     </td>
        ///     <td>
        ///       A <see cref="Int32"/> value that indicates the provider type of the CSP.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ROOT_CERTSTORE
        ///     </td>
        ///     <td>
        ///       Obtains the root certificate store for the smart card. This certificate store contains all of the root certificates that are stored on the smart card.<br/>
        ///       The <paramref name="Data"/> parameter is the address of an <b>HCERTSTORE</b> variable that receives the handle of the certificate store. When this handle is no longer needed, the caller must close it by using the <see cref="CertCloseStore"/> function.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SESSION_KEYSIZE
        ///     </td>
        ///     <td>
        ///       The size, in bits, of the session key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SGC_INFO
        ///     </td>
        ///     <td>
        ///       Used with server gated cryptography.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SIG_KEYSIZE_INC
        ///     </td>
        ///     <td>
        ///       The number of bits for the increment length of <b>AT_SIGNATURE</b>. This information is used with information returned in the <b>PP_ENUMALGS_EX</b> value. With the information returned when using <b>PP_ENUMALGS_EX</b> and <b>PP_SIG_KEYSIZE_INC</b>, the valid key lengths for <b>AT_SIGNATURE</b> can be determined. These key lengths can then be used with <see cref="CryptGenKey"/>.<br/>
        ///       For example, if a CSP enumerates <b>CALG_RSA_SIGN</b> (AT_SIGNATURE) with a minimum key length of 512 bits and a maximum of 1024 bits, and returns the increment length as 64 bits, then valid key lengths are 512, 576, 640,… 1024.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SIGNATURE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies that the key signature PIN is contained in <paramref name="Data"/>. The PIN is represented as a <b>null</b>-terminated ASCII string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SMARTCARD_GUID
        ///     </td>
        ///     <td>
        ///       Obtains the identifier of the smart card. The <paramref name="Data"/> parameter is the address of a <b>GUID</b> structure that receives the identifier of the smart card.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SMARTCARD_READER
        ///     </td>
        ///     <td>
        ///       Obtains the name of the smart card reader. The <paramref name="Data"/> parameter is the address of an ANSI character array that receives a <b>null</b>-terminated ANSI string that contains the name of the smart card reader. The size of this buffer, contained in the variable pointed to by the <paramref name="DataSize"/> parameter, must include the <b>NULL</b> terminator.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SYM_KEYSIZE
        ///     </td>
        ///     <td>
        ///       The size of the symmetric key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_UI_PROMPT
        ///     </td>
        ///     <td>
        ///       This query is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_UNIQUE_CONTAINER
        ///     </td>
        ///     <td>
        ///       The unique container name of the current key container in the form of a <b>null</b>-terminated <b>CHAR</b> string. For many CSPs, this name is the same name returned when the <b>PP_CONTAINER</b> value is used. The <see cref="CryptAcquireContext"/> function must work with this container name.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_USE_HARDWARE_RNG
        ///     </td>
        ///     <td>
        ///       Indicates whether a hardware random number generator (RNG) is supported. When <b>PP_USE_HARDWARE_RNG</b> is specified, the function succeeds and returns <b>TRUE</b> if a hardware RNG is supported. The function fails and returns <b>FALSE</b> if a hardware RNG is not supported. If a RNG is supported, <b>PP_USE_HARDWARE_RNG</b> can be set in <see cref="CryptSetProvParam"/> to indicate that the CSP must exclusively use the hardware RNG for this provider context. When <b>PP_USE_HARDWARE_RNG</b> is used, the <paramref name="Data"/> parameter must be <b>NULL</b> and <paramref name="Flags"/> must be zero.<br/>
        ///       None of the Microsoft CSPs currently support using a hardware RNG.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_USER_CERTSTORE
        ///     </td>
        ///     <td>
        ///       Obtains the user certificate store for the smart card. This certificate store contains all of the user certificates that are stored on the smart card. The certificates in this store are encoded by using PKCS_7_ASN_ENCODING or X509_ASN_ENCODING encoding and should contain the <b>CERT_KEY_PROV_INFO_PROP_ID</b> property.<br/>
        ///       The <paramref name="Data"/> parameter is the address of an <b>HCERTSTORE</b> variable that receives the handle of an in-memory certificate store. When this handle is no longer needed, the caller must close it by using the <see cref="CertCloseStore"/> function.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_VERSION
        ///     </td>
        ///     <td>
        ///       The version number of the CSP. The least significant byte contains the minor version number and the next most significant byte the major version number. Version 2.0 is represented as 0x00000200. To maintain backward compatibility with earlier versions of the Microsoft Base Cryptographic Provider and the Microsoft Enhanced Cryptographic Provider, the provider names retain the "v1.0" designation in later versions.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Data">
        /// A pointer to a buffer to receive the data. The form of this data varies depending on the value of <paramref name="Parameter"/>. When <paramref name="Parameter"/> is set to <b>PP_USE_HARDWARE_RNG</b>, <paramref name="Data"/> must be set to <b>NULL</b>.
        /// This parameter can be <b>NULL</b> to set the size of this information for memory allocation purposes.
        /// </param>
        /// <param name="DataSize">
        /// A pointer to a <see cref="Int32"/> value that specifies the size, in bytes, of the buffer pointed to by the <paramref name="Data"/> parameter. When the function returns, the <see cref="Int32"/> value contains the number of bytes stored or to be stored in the buffer.<br/>
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> When processing the data returned in the buffer, applications must use the actual size of the data returned. The actual size can be slightly smaller than the size of the buffer specified on input. (On input, buffer sizes are usually specified large enough to ensure that the largest possible output data fits in the buffer.) On output, the variable pointed to by this parameter is updated to reflect the actual size of the data copied to the buffer.<br/>
        ///        If <b>PP_ENUMALGS</b>, or <b>PP_ENUMALGS_EX</b> is set, the <paramref name="DataSize"/> parameter works somewhat differently. If <paramref name="Data"/> is <b>NULL</b> or the value pointed to by <paramref name="DataSize"/> is too small, the value returned in this parameter is the size of the largest item in the enumeration list instead of the size of the item currently being read.<br/>
        ///        If <b>PP_ENUMCONTAINERS</b> is set, the first call to the function returns the size of the maximum key-container allowed by the current provider. This is in contrast to other possible behaviors, like returning the length of the longest existing container, or the length of the current container. Subsequent enumerating calls will not change the <paramref name="DataSize"/> parameter. For each enumerated container, the caller can determine the length of the null-terminated string programmatically, if desired. If one of the enumeration values is read and the <paramref name="Data"/> parameter is <b>NULL</b>, the <b>CRYPT_FIRST</b> flag must be specified for the size information to be correctly retrieved.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="Flags">
        /// If <paramref name="Parameter"/> is <b>PP_KEYSET_SEC_DESCR</b>, the security descriptor on the key container where the keys are stored is retrieved. For this case, <paramref name="Flags"/> is used to pass in the <b>SECURITY_INFORMATION</b> bit flags that indicate the requested security information, as defined in the Platform SDK. <b>SECURITY_INFORMATION</b> bit flags can be combined with a bitwise-<b>OR</b> operation.<br/>
        /// The following values are defined for use with <b>PP_KEYSET_SEC_DESCR</b>.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       OWNER_SECURITY_INFORMATION
        ///     </td>
        ///     <td>
        ///       Owner identifier of the object is being referenced.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       GROUP_SECURITY_INFORMATION
        ///     </td>
        ///     <td>
        ///       Primary group identifier of the object is being referenced.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       DACL_SECURITY_INFORMATION
        ///     </td>
        ///     <td>
        ///       Discretionary ACL of the object is being referenced.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SACL_SECURITY_INFORMATION
        ///     </td>
        ///     <td>
        ///       System ACL of the object is being referenced.
        ///     </td>
        ///   </tr>
        /// </table>
        /// The following values are defined for use with <b>PP_ENUMALGS</b>, <b>PP_ENUMALGS_EX</b>, and <b>PP_ENUMCONTAINERS</b>.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_FIRST
        ///     </td>
        ///     <td>
        ///       Retrieve the first element in the enumeration. This has the same effect as resetting the enumerator.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_NEXT
        ///     </td>
        ///     <td>
        ///       Retrieve the next element in the enumeration. When there are no more elements to retrieve, this function will fail and set the last error to <b>ERROR_NO_MORE_ITEMS</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_SGC_ENUM
        ///     </td>
        ///     <td>
        ///       Retrieve server-gated cryptography (SGC) enabled certificates. SGC enabled certificates are no longer supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_SGC
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_FASTSGC
        ///     </td>
        ///     <td>
        ///       This flag is not used.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_MORE_DATA
        ///     </td>
        ///     <td>
        ///       If the buffer specified by the <paramref name="Data"/> parameter is not large enough to hold the returned data, the function sets the <b>ERROR_MORE_DATA</b> code and stores the required buffer size, in bytes, in the variable pointed to by <paramref name="DataSize"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_NO_MORE_ITEMS
        ///     </td>
        ///     <td>
        ///       The end of the enumeration list has been reached. No valid data has been placed in the <paramref name="Data"/> buffer. This error code is returned only when <paramref name="Parameter"/> equals <b>PP_ENUMALGS</b> or <b>PP_ENUMCONTAINERS</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter specifies a flag that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Parameter"/> parameter specifies an unknown value number.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context specified by <paramref name="Context"/> is not valid.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptGetProvParam(IntPtr Context,CRYPT_PARAM Parameter,IntPtr Data,ref Int32 DataSize,Int32 Flags);
        #endregion
        #region M:CryptGetUserKey(IntPtr,KEY_SPEC_TYPE,{out}IntPtr):Boolean
        /// <summary>
        /// The <b>CryptGetUserKey</b> function retrieves a handle of one of a user's two public/private key pairs. This function is used only by the owner of the public/private key pairs and only when the handle of a cryptographic service provider (CSP) and its associated key container is available. If the CSP handle is not available and the user's certificate is, use <see cref="CryptAcquireCertificatePrivateKey"/>.
        /// </summary>
        /// <param name="Context"><b>HCRYPTPROV</b> handle of a cryptographic service provider (CSP) created by a call to <see cref="CryptAcquireContext"/>.</param>
        /// <param name="KeySpec">
        /// Identifies the private key to use from the key container. It can be <b>AT_KEYEXCHANGE</b> or <b>AT_SIGNATURE</b>.<br/>
        /// Additionally, some providers allow access to other user-specific keys through this function. For details, see the documentation on the specific provider.
        /// </param>
        /// <param name="UserKey">A pointer to the <b>HCRYPTKEY</b> handle of the retrieved keys. When you have finished using the key, delete the handle by calling the <see cref="CryptDestroyKey"/> function.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEY
        ///     </td>
        ///     <td>
        ///       The <paramref name="KeySpec"/> parameter contains a value that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The <paramref name="Context"/> parameter does not contain a valid context handle.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_NO_KEY
        ///     </td>
        ///     <td>
        ///       The key requested by the <paramref name="KeySpec"/> parameter does not exist.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptGetUserKey(IntPtr Context,KEY_SPEC_TYPE KeySpec,out IntPtr UserKey);
        #endregion
        #region M:CryptHashData(IntPtr,Byte[],Int32):Boolean
        /// <summary>
        /// The <b>CryptHashData</b> function adds data to a specified hash object. This function and <see cref="CryptHashSessionKey"/> can be called multiple times to compute the hash of long or discontinuous data streams.<br/>
        /// Before calling this function, <b>CryptCreateHash</b> must be called to create a handle of a hash object.
        /// </summary>
        /// <param name="Hash">Handle of the hash object.</param>
        /// <param name="Data">A pointer to a buffer that contains the data to be added to the hash object.</param>
        /// <param name="DataSize">Number of bytes of data to be added.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_ALGID
        ///     </td>
        ///     <td>
        ///       The <paramref name="Hash"/> handle specifies an algorithm that this CSP does not support.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_HASH
        ///     </td>
        ///     <td>
        ///       The hash object specified by the <paramref name="Hash"/> parameter is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_HASH_STATE
        ///     </td>
        ///     <td>
        ///       An attempt was made to add data to a hash object that is already marked "finished."
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEY
        ///     </td>
        ///     <td>
        ///       A keyed hash algorithm is being used, but the session key is no longer valid. This error is generated if the session key is destroyed before the hashing operation is complete.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the hash object was created cannot be found.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_FAIL
        ///     </td>
        ///     <td>
        ///       The function failed in some unexpected way.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_NO_MEMORY
        ///     </td>
        ///     <td>
        ///       The CSP ran out of memory during the operation.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptHashData(IntPtr Hash,Byte[] Data,Int32 DataSize);
        #endregion
        #region M:CryptImportKey(IntPtr,Byte[],Int32,IntPtr,Int32,{out}IntPtr):Boolean
        /// <summary>
        /// The <b>CryptImportKey</b> function transfers a cryptographic key from a key BLOB into a cryptographic service provider (CSP). This function can be used to import an <a href="https://learn.microsoft.com/en-us/windows/desktop/SecGloss/s-gly">Schannel</a> session key, regular session key, public key, or public/private key pair. For all but the public key, the key or key pair is encrypted.
        /// </summary>
        /// <param name="Context">The handle of a CSP obtained with the <see cref="CryptAcquireContext"/> function.</param>
        /// <param name="Data">A <b>BYTE</b> array that contains a <see cref="PUBLICKEYSTRUC"/> BLOB header followed by the encrypted key. This key BLOB is created by the <see cref="CryptExportKey"/> function, either in this application or by another application possibly running on a different computer.</param>
        /// <param name="DataLen">Contains the length, in bytes, of the key BLOB.</param>
        /// <param name="PubKey">
        /// A handle to the cryptographic key that decrypts the key stored in <paramref name="Data"/>. This key must come from the same CSP to which <paramref name="Context"/> refers. The meaning of this parameter differs depending on the CSP type and the type of key BLOB being imported:
        /// <list type="bullet">
        ///   <item>If the key BLOB is encrypted with the key exchange key pair, for example, a <b>SIMPLEBLOB</b>, this parameter can be the handle to the key exchange key.</item>
        ///   <item>If the key BLOB is encrypted with a session key, for example, an encrypted <b>PRIVATEKEYBLOB</b>, this parameter contains the handle of this session key.</item>
        ///   <item>If the key BLOB is not encrypted, for example, a <b>PUBLICKEYBLOB</b>, this parameter is not used and must be zero.</item>
        ///   <item>If the key BLOB is encrypted with a session key in an <b>Schannel CSP</b>, for example, an encrypted <b>OPAQUEKEYBLOB</b> or any other vendor-specific <b>OPAQUEKEYBLOB</b>, this parameter is not used and must be set to zero.</item>
        /// </list>
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///        <b>Note:</b> Some CSPs may modify this parameter as a result of the operation. Applications that subsequently use this key for other purposes should call the <see cref="CryptDuplicateKey"/> function to create a duplicate key handle. When the application has finished using the handle, release it by calling the <see cref="CryptDestroyKey"/> function.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <param name="Flags">
        /// Currently used only when a public/private key pair in the form of a <b>PRIVATEKEYBLOB</b> is imported into the CSP.<br/>
        /// This parameter can be one of the following values.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_EXPORTABLE
        ///     </td>
        ///     <td>
        ///       The key being imported is eventually to be reexported. If this flag is not used, then calls to <see cref="CryptExportKey"/> with the key handle fail.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_OAEP
        ///     </td>
        ///     <td>
        ///       This flag causes PKCS #1 version 2 formatting to be checked with RSA encryption and decryption when importing <b>SIMPLEBLOB</b>s.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_NO_SALT
        ///     </td>
        ///     <td>
        ///       A no-salt value gets allocated for a 40-bit symmetric key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_USER_PROTECTED
        ///     </td>
        ///     <td>
        ///       If this flag is set, the CSP notifies the user through a dialog box or some other method when certain actions are attempted using this key. The precise behavior is specified by the CSP or the CSP type used. If the provider context was acquired with <b>CRYPT_SILENT</b> set, using this flag causes a failure and the last error is set to <b>NTE_SILENT_CONTEXT</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_IPSEC_HMAC_KEY
        ///     </td>
        ///     <td>
        ///       Allows for the import of an RC2 key that is larger than 16 bytes. If this flag is not set, calls to the <see cref="CryptImportKey"/> function with RC2 keys that are greater than 16 bytes fail, and a call to <see cref="LastErrorService.GetLastError"/> will return <b>NTE_BAD_DATA</b>.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Key">A pointer to a <b>HCRYPTKEY</b> value that receives the handle of the imported key. When you have finished using the key, release the handle by calling the <see cref="CryptDestroyKey"/> function.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       ERROR_BUSY
        ///     </td>
        ///     <td>
        ///       Some CSPs set this error if a private key is imported into a container while another thread or process is using this key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_ALGID
        ///     </td>
        ///     <td>
        ///       The simple key BLOB to be imported is not encrypted with the expected key exchange algorithm.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_DATA
        ///     </td>
        ///     <td>
        ///       Either the algorithm that works with the public key to be imported is not supported by this CSP, or an attempt was made to import a session key that was encrypted with something other than one of your public keys.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter specified is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The key BLOB type is not supported by this CSP and is possibly not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The <paramref name="Context"/> parameter does not contain a valid context handle.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_VER
        ///     </td>
        ///     <td>
        ///       The version number of the key BLOB does not match the CSP version. This usually indicates that the CSP needs to be upgraded.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptImportKey(IntPtr Context,Byte[] Data,Int32 DataLen,IntPtr PubKey,Int32 Flags,out IntPtr Key);
        #endregion
        #region M:CryptMsgClose(IntPtr):Boolean
        /// <summary>
        /// The <b>CryptMsgClose</b> function closes a cryptographic message handle. At each call to this function, the reference count on the message is reduced by one. When the reference count reaches zero, the message is fully released.
        /// </summary>
        /// <param name="Message">Handle of the cryptographic message to be closed.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>.
        /// </returns>
        Boolean CryptMsgClose(IntPtr Message);
        #endregion
        #region M:CryptMsgControl(IntPtr,CRYPT_MESSAGE_FLAGS,CMSG_CTRL,IntPtr):Boolean
        /// <summary>
        /// The <b>CryptMsgControl</b> function performs a control operation after a message has been decoded by a final call to the <see cref="CryptMsgUpdate"/> function. The control operations provided by this function are used for decryption, signature and hash verification, and the addition and deletion of certificates, certificate revocation lists (CRLs), signers, and unauthenticated attributes.<br/>
        /// Important changes that affect the handling of enveloped messages have been made to CryptoAPI to support Secure/Multipurpose Internet Mail Extensions (S/MIME) email interoperability.
        /// </summary>
        /// <param name="Message">A handle of a cryptographic message for which a control is to be applied.</param>
        /// <param name="Flags">
        /// The following value is defined when the <paramref name="CtrlType"/> parameter is one of the following:
        /// <list type="bullet">
        ///   <item>CMSG_CTRL_DECRYPT</item>
        ///   <item>CMSG_CTRL_KEY_TRANS_DECRYPT</item>
        ///   <item>CMSG_CTRL_KEY_AGREE_DECRYPT</item>
        ///   <item>CMSG_CTRL_MAIL_LIST_DECRYPT</item>
        /// </list>
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CMSG_CRYPT_RELEASE_CONTEXT_FLAG
        ///     </td>
        ///     <td>
        ///       The handle to the cryptographic provider is released on the final call to the <see cref="CryptMsgClose"/> function. This handle is not released if the <see cref="CryptMsgControl"/> function fails.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If the <paramref name="CtrlType"/> parameter does not specify a decrypt operation, set this value to zero.
        /// </param>
        /// <param name="CtrlType">
        /// The type of operation to be performed. Currently defined message control types and the type of structure that should be passed to the <paramref name="CtrlPara"/> parameter are shown in the following table.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CMSG_CTRL_ADD_ATTR_CERT
        ///     </td>
        ///     <td>
        ///       A BLOB that contains the encoded bytes of attribute certificate.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_ADD_CERT
        ///     </td>
        ///     <td>
        ///       A <see cref="CRYPT_INTEGER_BLOB"/> structure that contains the encoded bytes of the certificate to be added to the message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_ADD_CMS_SIGNER_INFO
        ///     </td>
        ///     <td>
        ///       A <see cref="CMSG_CMS_SIGNER_INFO"/> structure that contains signer information. This operation differs from <b>CMSG_CTRL_ADD_SIGNER</b> because the signer information contains the signature.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_ADD_CRL
        ///     </td>
        ///     <td>
        ///       A BLOB that contains the encoded bytes of the CRL to be added to the message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_ADD_SIGNER
        ///     </td>
        ///     <td>
        ///       A <see cref="CMSG_SIGNER_ENCODE_INFO"/> structure that contains the signer information to be added to the message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR
        ///     </td>
        ///     <td>
        ///       A <see cref="CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA"/> structure that contains the index of the signer and a BLOB that contains the unauthenticated attribute information to be added to the message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DECRYPT
        ///     </td>
        ///     <td>
        ///       A <see cref="CMSG_CTRL_DECRYPT_PARA"/> structure used to decrypt the message for the specified key transport recipient. This value is applicable to RSA recipients. This operation specifies that the <see cref="CryptMsgControl"/> function search the recipient index to obtain the key transport recipient information. If the function fails, <see cref="LastErrorService.GetLastError"/> will return <b>CRYPT_E_INVALID_INDEX</b> if no key transport recipient is found.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DEL_ATTR_CERT
        ///     </td>
        ///     <td>
        ///       The index of the attribute certificate to be removed.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DEL_CERT
        ///     </td>
        ///     <td>
        ///       The index of the certificate to be deleted from the message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DEL_CRL
        ///     </td>
        ///     <td>
        ///       The index of the CRL to be deleted from the message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DEL_SIGNER
        ///     </td>
        ///     <td>
        ///       The index of the signer to be deleted.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR
        ///     </td>
        ///     <td>
        ///       A <see cref="CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA"/> structure that contains an index that specifies the signer and the index that specifies the signer's unauthenticated attribute to be deleted.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_ENABLE_STRONG_SIGNATURE
        ///     </td>
        ///     <td>
        ///       A <see cref="CERT_STRONG_SIGN_PARA"/> structure used to perform strong signature checking.<br/>
        ///       To check for a strong signature, specify this control type before calling <see cref="CryptMsgGetAndVerifySigner"/> or before calling <see cref="CryptMsgControl"/> with the following control types set:
        ///       <list type="bullet">
        ///         <item>CMSG_CTRL_VERIFY_SIGNATURE</item>
        ///         <item>CMSG_CTRL_VERIFY_SIGNATURE_EX</item>
        ///       </list>
        ///       After the signature is successfully verified, this function checks for a strong signature. If the signature is not strong, the operation will fail and the <see cref="LastErrorService.GetLastError"/> value will be set to <b>NTE_BAD_ALGID</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_KEY_AGREE_DECRYPT
        ///     </td>
        ///     <td>
        ///       A <see cref="CMSG_CTRL_KEY_AGREE_DECRYPT_PARA"/> structure used to decrypt the message for the specified key agreement session key. Key agreement is used with Diffie-Hellman encryption/decryption.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_KEY_TRANS_DECRYPT
        ///     </td>
        ///     <td>
        ///       A <see cref="CMSG_CTRL_KEY_TRANS_DECRYPT_PARA"/> structure used to decrypt the message for the specified key transport recipient. Key transport is used with RSA encryption/decryption.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_MAIL_LIST_DECRYPT
        ///     </td>
        ///     <td>
        ///       A <see cref="CMSG_CTRL_MAIL_LIST_DECRYPT_PARA"/> structure used to decrypt the message for the specified recipient using a previously distributed key-encryption key (KEK).
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_VERIFY_HASH
        ///     </td>
        ///     <td>
        ///       This value is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_VERIFY_SIGNATURE
        ///     </td>
        ///     <td>
        ///       A <see cref="CERT_INFO"/> structure that identifies the signer of the message whose signature is to be verified.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_VERIFY_SIGNATURE_EX
        ///     </td>
        ///     <td>
        ///       A <see cref="CMSG_CTRL_VERIFY_SIGNATURE_EX_PARA"/> structure that specifies the signer index and public key to verify the message signature. The signer public key can be a <see cref="CERT_PUBLIC_KEY_INFO"/> structure, a certificate context, or a certificate chain context.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="CtrlPara">
        /// A pointer to a structure determined by the value of <paramref name="CtrlType"/>.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       <b><paramref name="CtrlType"/> value</b>
        ///     </td>
        ///     <td>
        ///       <b>Meaning</b>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DECRYPT,<br/>CMSG_CTRL_KEY_TRANS_DECRYPT,<br/>CMSG_CTRL_KEY_AGREE_DECRYPT,<br/>or CMSG_CTRL_MAIL_LIST_DECRYPT,<br/>and the streamed enveloped message is being decoded
        ///     </td>
        ///     <td>
        ///       Decoding will be done as if the streamed content were being decrypted. If any encrypted streamed content has accumulated prior to this call, some or all of the plaintext that results from the decryption of the cipher text is passed back to the application through the callback function specified in the call to the <see cref="CryptMsgOpenToDecode"/> function.
        ///       <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///         <tr>
        ///           <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///             <b>Note:</b> When streaming an enveloped message, the <see cref="CryptMsgControl"/> function is not called until the polling for the availability of the <b>CMSG_ENVELOPE_ALGORITHM_PARAM</b> succeeds. If the polling does not succeed, an error results. For a description of that polling, see the <see cref="CryptMsgOpenToDecode"/> function.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_VERIFY_HASH
        ///     </td>
        ///     <td>
        ///       The hash computed from the content of the message is compared against the hash contained in the message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_ADD_SIGNER
        ///     </td>
        ///     <td>
        ///       <paramref name="CtrlPara"/> points to a <see cref="CMSG_SIGNER_ENCODE_INFO"/> structure that contains the signer information to be added to the message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DEL_SIGNER
        ///     </td>
        ///     <td>
        ///       After a deletion is made, any other signer indices in use for this message are no longer valid and must be reacquired by calling the <see cref="CryptMsgGetParam"/> function.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR
        ///     </td>
        ///     <td>
        ///       After a deletion is made, any other unauthenticated attribute indices in use for this signer are no longer valid and must be reacquired by calling the <see cref="CryptMsgGetParam"/> function.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DEL_CERT
        ///     </td>
        ///     <td>
        ///       After a deletion is made, any other certificate indices in use for this message are no longer valid and must be reacquired by calling the <see cref="CryptMsgGetParam"/> function.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DEL_CRL
        ///     </td>
        ///     <td>
        ///       After a deletion is made, any other CRL indices in use for this message are no longer valid and will need to be reacquired by calling the <see cref="CryptMsgGetParam"/> function.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.<br/>
        /// If the function fails, the return value is zero and the GetLastError function returns an Abstract Syntax Notation One (ASN.1) encoding/decoding error.<br/>
        /// When a streamed, enveloped message is being decoded, errors encountered in the application-defined callback function specified by the <b>StreamInfo</b> parameter of the
        /// <see cref="CryptMsgOpenToDecode"/> function might be propagated to the <see cref="CryptMsgControl"/> function. If this happens, the <b>SetLastError</b> function is not called by the <see cref="CryptMsgControl"/> function after the callback function returns. This preserves any errors encountered under the control of the application. It is the responsibility of the callback function (or one of the APIs that it calls) to call the <b>SetLastError</b> function if an error occurs while the application is processing the streamed data.<br/>
        /// Propagated errors might be encountered from the following functions:
        /// <list type="bullet">
        ///   <item><see cref="CryptCreateHash"/></item>
        ///   <item><see cref="CryptDecrypt"/></item>
        ///   <item><see cref="CryptGetHashParam"/></item>
        ///   <item><see cref="CryptGetUserKey"/></item>
        ///   <item><see cref="CryptHashData"/></item>
        ///   <item><see cref="CryptImportKey"/></item>
        ///   <item><see cref="CryptSignHash"/></item>
        ///   <item><see cref="CryptVerifySignature"/></item>
        /// </list>
        /// The following error codes are most commonly returned.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_E_ALREADY_DECRYPTED
        ///     </td>
        ///     <td>
        ///       The message content has already been decrypted. This error can be returned if the <paramref name="CtrlType"/> parameter is set to <b>CMSG_CTRL_DECRYPT</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_AUTH_ATTR_MISSING
        ///     </td>
        ///     <td>
        ///       The message does not contain an expected authenticated attribute. This error can be returned if the <paramref name="CtrlType"/> parameter is set to <b>CMSG_CTRL_VERIFY_SIGNATURE</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_BAD_ENCODE
        ///     </td>
        ///     <td>
        ///       An error was encountered while encoding or decoding. This error can be returned if the <paramref name="CtrlType"/> parameter is set to <b>CMSG_CTRL_VERIFY_SIGNATURE</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_CONTROL_TYPE
        ///     </td>
        ///     <td>
        ///       The control type is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_HASH_VALUE
        ///     </td>
        ///     <td>
        ///       The hash value is incorrect.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_INVALID_INDEX
        ///     </td>
        ///     <td>
        ///       The index value is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_INVALID_MSG_TYPE
        ///     </td>
        ///     <td>
        ///       The message type is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_OID_FORMAT
        ///     </td>
        ///     <td>
        ///       The object identifier is badly formatted. This error can be returned if the <paramref name="CtrlType"/> parameter is set to <b>CMSG_CTRL_ADD_SIGNER</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_RECIPIENT_NOT_FOUND
        ///     </td>
        ///     <td>
        ///       The enveloped data message does not contain the specified recipient. This error can be returned if the <paramref name="CtrlType"/> parameter is set to <b>CMSG_CTRL_DECRYPT</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_SIGNER_NOT_FOUND
        ///     </td>
        ///     <td>
        ///       The specified signer for the message was not found. This error can be returned if the <paramref name="CtrlType"/> parameter is set to <b>CMSG_CTRL_VERIFY_SIGNATURE</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_UNKNOWN_ALGO
        ///     </td>
        ///     <td>
        ///       The cryptographic algorithm is unknown.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_UNEXPECTED_ENCODING
        ///     </td>
        ///     <td>
        ///       The message is not encoded as expected. This error can be returned if the <paramref name="CtrlType"/> parameter is set to <b>CMSG_CTRL_VERIFY_SIGNATURE</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       One or more arguments are not valid. This error can be returned if the <paramref name="CtrlType"/> parameter is set to <b>CMSG_CTRL_DECRYPT</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_OUTOFMEMORY
        ///     </td>
        ///     <td>
        ///       Not enough memory was available to complete the operation.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptMsgControl(IntPtr Message,CRYPT_MESSAGE_FLAGS Flags,CMSG_CTRL CtrlType,IntPtr CtrlPara);
        #endregion
        #region M:CryptMsgControl(IntPtr,CRYPT_MESSAGE_FLAGS,CMSG_CTRL,{ref}CMSG_CTRL_DECRYPT_PARA):Boolean
        /// <summary>
        /// The <b>CryptMsgControl</b> function performs a control operation after a message has been decoded by a final call to the <see cref="CryptMsgUpdate"/> function. The control operations provided by this function are used for decryption, signature and hash verification, and the addition and deletion of certificates, certificate revocation lists (CRLs), signers, and unauthenticated attributes.<br/>
        /// Important changes that affect the handling of enveloped messages have been made to CryptoAPI to support Secure/Multipurpose Internet Mail Extensions (S/MIME) email interoperability.
        /// </summary>
        /// <param name="Message">A handle of a cryptographic message for which a control is to be applied.</param>
        /// <param name="Flags">
        /// The following value is defined when the <paramref name="CtrlType"/> parameter is one of the following:
        /// <list type="bullet">
        ///   <item>CMSG_CTRL_DECRYPT</item>
        /// </list>
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CMSG_CRYPT_RELEASE_CONTEXT_FLAG
        ///     </td>
        ///     <td>
        ///       The handle to the cryptographic provider is released on the final call to the <see cref="CryptMsgClose"/> function. This handle is not released if the <see cref="CryptMsgControl"/> function fails.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If the <paramref name="CtrlType"/> parameter does not specify a decrypt operation, set this value to zero.
        /// </param>
        /// <param name="CtrlType">
        /// The type of operation to be performed. Currently defined message control types and the type of structure that should be passed to the <paramref name="CtrlPara"/> parameter are shown in the following table.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DECRYPT
        ///     </td>
        ///     <td>
        ///       A <see cref="CMSG_CTRL_DECRYPT_PARA"/> structure used to decrypt the message for the specified key transport recipient. This value is applicable to RSA recipients. This operation specifies that the <see cref="CryptMsgControl"/> function search the recipient index to obtain the key transport recipient information. If the function fails, <see cref="LastErrorService.GetLastError"/> will return <b>CRYPT_E_INVALID_INDEX</b> if no key transport recipient is found.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="CtrlPara">
        /// A pointer to a structure determined by the value of <paramref name="CtrlType"/>.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       <b><paramref name="CtrlType"/> value</b>
        ///     </td>
        ///     <td>
        ///       <b>Meaning</b>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CTRL_DECRYPT and the streamed enveloped message is being decoded
        ///     </td>
        ///     <td>
        ///       Decoding will be done as if the streamed content were being decrypted. If any encrypted streamed content has accumulated prior to this call, some or all of the plaintext that results from the decryption of the cipher text is passed back to the application through the callback function specified in the call to the <see cref="CryptMsgOpenToDecode"/> function.
        ///       <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///         <tr>
        ///           <td style="windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt">
        ///             <b>Note:</b> When streaming an enveloped message, the <see cref="CryptMsgControl"/> function is not called until the polling for the availability of the <b>CMSG_ENVELOPE_ALGORITHM_PARAM</b> succeeds. If the polling does not succeed, an error results. For a description of that polling, see the <see cref="CryptMsgOpenToDecode"/> function.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.<br/>
        /// If the function fails, the return value is zero and the GetLastError function returns an Abstract Syntax Notation One (ASN.1) encoding/decoding error.<br/>
        /// When a streamed, enveloped message is being decoded, errors encountered in the application-defined callback function specified by the <b>StreamInfo</b> parameter of the
        /// <see cref="CryptMsgOpenToDecode"/> function might be propagated to the <see cref="CryptMsgControl"/> function. If this happens, the <b>SetLastError</b> function is not called by the <see cref="CryptMsgControl"/> function after the callback function returns. This preserves any errors encountered under the control of the application. It is the responsibility of the callback function (or one of the APIs that it calls) to call the <b>SetLastError</b> function if an error occurs while the application is processing the streamed data.<br/>
        /// Propagated errors might be encountered from the following functions:
        /// <list type="bullet">
        ///   <item><see cref="CryptCreateHash"/></item>
        ///   <item><see cref="CryptDecrypt"/></item>
        ///   <item><see cref="CryptGetHashParam"/></item>
        ///   <item><see cref="CryptGetUserKey"/></item>
        ///   <item><see cref="CryptHashData"/></item>
        ///   <item><see cref="CryptImportKey"/></item>
        ///   <item><see cref="CryptSignHash"/></item>
        ///   <item><see cref="CryptVerifySignature"/></item>
        /// </list>
        /// The following error codes are most commonly returned.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_E_ALREADY_DECRYPTED
        ///     </td>
        ///     <td>
        ///       The message content has already been decrypted. This error can be returned if the <paramref name="CtrlType"/> parameter is set to <b>CMSG_CTRL_DECRYPT</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_CONTROL_TYPE
        ///     </td>
        ///     <td>
        ///       The control type is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_HASH_VALUE
        ///     </td>
        ///     <td>
        ///       The hash value is incorrect.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_INVALID_INDEX
        ///     </td>
        ///     <td>
        ///       The index value is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_INVALID_MSG_TYPE
        ///     </td>
        ///     <td>
        ///       The message type is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_RECIPIENT_NOT_FOUND
        ///     </td>
        ///     <td>
        ///       The enveloped data message does not contain the specified recipient. This error can be returned if the <paramref name="CtrlType"/> parameter is set to <b>CMSG_CTRL_DECRYPT</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_UNKNOWN_ALGO
        ///     </td>
        ///     <td>
        ///       The cryptographic algorithm is unknown.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       One or more arguments are not valid. This error can be returned if the <paramref name="CtrlType"/> parameter is set to <b>CMSG_CTRL_DECRYPT</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_OUTOFMEMORY
        ///     </td>
        ///     <td>
        ///       Not enough memory was available to complete the operation.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptMsgControl(IntPtr Message,CRYPT_MESSAGE_FLAGS Flags,CMSG_CTRL CtrlType,ref CMSG_CTRL_DECRYPT_PARA CtrlPara);
        #endregion
        #region M:CryptMsgGetParam(IntPtr,CMSG_PARAM,Int32,Byte[],{ref}Int32):Boolean
        /// <summary>
        /// The <b>CryptMsgGetParam</b> function acquires a message parameter after a cryptographic message has been encoded or decoded. This function is called after the final <see cref="CryptMsgUpdate"/> call.
        /// </summary>
        /// <param name="Message">Handle of a cryptographic message.</param>
        /// <param name="ParamType">
        /// Indicates the parameter types of data to be retrieved. The type of data to be retrieved determines the type of structure to use for <paramref name="Data"/>.<br/>
        /// For an encoded message, only the CMSG_BARE_CONTENT, CMSG_ENCODE_SIGNER, CMSG_CONTENT_PARAM and CMSG_COMPUTED_HASH_PARAM <paramref name="ParamType"/> are valid.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td style="width:20%">
        ///       CMSG_ATTR_CERT_COUNT_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <see cref="Int32"/>.<br/>
        ///       Returns the count of the attribute certificates in a SIGNED or ENVELOPED message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_ATTR_CERT_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array.<br/>
        ///       Retrieves an attribute certificate. To get all the attribute certificates, call <b>CryptMsgGetParam</b> varying dwIndex set to 0 the number of attributes minus one.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_BARE_CONTENT_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array.<br/>
        ///       Retrieves the encoded content of an encoded cryptographic message, without the outer layer of the CONTENT_INFO structure. That is, only the encoding of the PKCS #7 defined ContentInfo.content field is returned.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CERT_COUNT_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to <see cref="Int32"/>.<br/>
        ///       Returns the number of certificates in a received SIGNED or ENVELOPED message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CERT_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array.<br/>
        ///       Returns a signer's certificate. To get all of the signer's certificates, call <b>CryptMsgGetParam</b>, varying <paramref name="SignerIndex"/> from 0 to the number of available certificates minus one.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_COMPUTED_HASH_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array.<br/>
        ///       Returns the hash calculated of the data in the message. This type is applicable to both encode and decode.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CONTENT_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array.<br/>
        ///       Returns the whole PKCS #7 message from a message opened to encode.
        ///       Retrieves the inner content of a message opened to decode.
        ///       If the message is enveloped, the inner type is data, and <b>CryptMsgControl</b> has been called to decrypt the message, the decrypted content is returned.
        ///       If the inner type is not data, the encoded BLOB that requires further decoding is returned.
        ///       If the message is not enveloped and the inner content is DATA, the returned data is the octets of the inner content.
        ///       This type is applicable to both encode and decode.<br/>
        ///       For decoding, if the type is CMSG_DATA, the content's octets are returned; else, the encoded inner content is returned.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CRL_COUNT_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to <see cref="Int32"/>.<br/>
        ///       Returns the count of CRLs in a received, SIGNED or ENVELOPED message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CRL_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array.<br/>
        ///       Returns a CRL.
        ///       To get all the CRLs, call <b>CryptMsgGetParam</b>, varying <paramref name="SignerIndex"/> from 0 to the number of available CRLs minus one.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CRL_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array.<br/>
        ///       Changes the contents of an already encoded message.
        ///       The message must first be decoded with a call to <b>CryptMsgOpenToDecode</b>.
        ///       Then the change to the message is made through a call to <see cref="CryptMsgControl"/>, <see cref="CryptMsgCountersign"/>, or <see cref="CryptMsgCountersignEncoded"/>.
        ///       The message is then encoded again with a call to <b>CryptMsgGetParam</b>, specifying CMSG_ENCODED_MESSAGE to get a new encoding that reflects the changes made.
        ///       This can be used, for instance, to add a time-stamp attribute to a message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_ENCODED_SIGNER
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array.<br/>
        ///       Returns the encoded CMSG_SIGNER_INFO signer information for a message signer.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_ENCRYPTED_DIGEST
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array.<br/>
        ///       Returns the encrypted hash of a signature. Typically used for performing time-stamping.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_ENCRYPT_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array for a <see cref="CRYPT_ALGORITHM_IDENTIFIER"/> structure.<br/>
        ///       Returns the encryption algorithm used to encrypted the message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_ENVELOPE_ALGORITHM_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array for a <see cref="CRYPT_ALGORITHM_IDENTIFIER"/> structure.<br/>
        ///       Returns the encryption algorithm used to encrypt an ENVELOPED message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_HASH_ALGORITHM_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array for a <see cref="CRYPT_ALGORITHM_IDENTIFIER"/> structure.<br/>
        ///       Returns the hash algorithm used to hash the message when it was created.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_HASH_DATA_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array.<br/>
        ///       Returns the hash value stored in the message when it was created.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_INNER_CONTENT_TYPE_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array to receive a <b>null</b>-terminated object identifier (OID) string.<br/>
        ///       Returns the inner content type of a received message.
        ///       This type is not applicable to messages of type DATA.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_RECIPIENT_COUNT_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <see cref="Int32"/>.<br/>
        ///       Returns the number of key transport recipients of an ENVELOPED received message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CMS_RECIPIENT_COUNT_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <see cref="Int32"/>.<br/>
        ///       Returns the total count of all message recipients including key agreement and mail list recipients.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_RECIPIENT_INDEX_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <see cref="Int32"/>.<br/>
        ///       Returns the index of the key transport recipient used to decrypt an ENVELOPED message.
        ///       This value is available only after a message has been decrypted.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CMS_RECIPIENT_INDEX_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <see cref="Int32"/>.<br/>
        ///       Returns the index of the key transport, key agreement, or mail list recipient used to decrypt an ENVELOPED message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CMS_RECIPIENT_ENCRYPTED_KEY_INDEX_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <see cref="Int32"/>.<br/>
        ///       Returns the index of the encrypted key of a key agreement recipient used to decrypt an ENVELOPED message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_RECIPIENT_INFO_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array to receive a <see cref="CERT_INFO"/> structure.<br/>
        ///       Returns certificate information about a key transport message's recipient.
        ///       To get certificate information on all key transport message's recipients, repetitively call <b>CryptMsgGetParam</b>, varying <paramref name="SignerIndex"/> from 0 to the number of recipients minus one.
        ///       Only the <see cref="CERT_INFO.Issuer"/>, <see cref="CERT_INFO.SerialNumber"/>, and <see cref="CERT_INFO.PublicKeyAlgorithm"/> members of the <see cref="CERT_INFO"/> structure returned are available and valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CMS_RECIPIENT_INFO_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array to receive a <see cref="CMSG_CMS_RECIPIENT_INFO"/> structure.<br/>
        ///       Returns information about a key transport, key agreement, or mail list recipient.
        ///       It is not limited to key transport message recipients.
        ///       To get information on all of a message's recipients, repetitively call <b>CryptMsgGetParam</b>, varying <paramref name="SignerIndex"/> from 0 to the number of recipients minus one.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNER_AUTH_ATTR_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array to receive a <see cref="CRYPT_ATTRIBUTES"/> structure.<br/>
        ///       Returns the authenticated attributes of a message signer.
        ///       To retrieve the authenticated attributes for a specified signer, call <b>CryptMsgGetParam</b> with dwIndex equal to that signer's index.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNER_CERT_INFO_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array to receive the <see cref="CERT_INFO"/> structure.<br/>
        ///       Returns information on a message signer needed to identify the signer's certificate.
        ///       A certificate's Issuer and SerialNumber can be used to uniquely identify a certificate for retrieval.
        ///       To retrieve information for all the signers, repetitively call <b>CryptMsgGetParam</b> varying <paramref name="SignerIndex"/> from 0 to the number of signers minus one.
        ///       Only the <see cref="CERT_INFO.Issuer"/> and <see cref="CERT_INFO.SerialNumber"/> fields in the <see cref="CERT_INFO"/> structure returned contain available, valid data.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNER_CERT_ID_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array to receive a <see cref="CERT_ID"/> structure.<br/>
        ///       Returns information on a message signer needed to identify the signer's public key.
        ///       This could be a certificate's <see cref="CERT_ISSUER_SERIAL_NUMBER.Issuer"/> and <see cref="CERT_ISSUER_SERIAL_NUMBER.SerialNumber"/>, a <see cref="CERT_ID.KeyID"/>, or a <see cref="CERT_ID.HashId"/>.
        ///       To retrieve information for all the signers, call <b>CryptMsgGetParam</b> varying dwIndex from 0 to the number of signers minus one.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNER_COUNT_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <see cref="Int32"/>.<br/>
        ///       Returns the number of signers of a received SIGNED message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNER_HASH_ALGORITHM_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array to receive the <see cref="CRYPT_ALGORITHM_IDENTIFIER"/> structure.<br/>
        ///       Returns the hash algorithm used by a signer of the message.
        ///       To get the hash algorithm for a specified signer, call <b>CryptMsgGetParam</b> with <paramref name="SignerIndex"/> equal to that signer's index.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNER_INFO_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array to receive a <see cref="CMSG_SIGNER_INFO"/> structure.<br/>
        ///       Returns information on a message signer. This includes the issuer and serial number of the signer's
        ///       certificate and authenticated and unauthenticated attributes of the signer's certificate. To retrieve
        ///       signer information on all of the signers of a message, call <b>CryptMsgGetParam</b> varying
        ///       <paramref name="SignerIndex"/> from 0 to the number of signers minus one.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CMS_SIGNER_INFO_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array to receive a <see cref="CMSG_CMS_SIGNER_INFO"/> structure.<br/>
        ///       Returns information on a message signer. This includes a signerId and authenticated and unauthenticated
        ///       attributes. To retrieve signer information on all of the signers of a message, call <b>CryptMsgGetParam</b>
        ///       varying <paramref name="SignerIndex"/> from 0 to the number of signers minus one.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNER_UNAUTH_ATTR_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array to receive a <see cref="CRYPT_ATTRIBUTES"/> structure.<br/>
        ///       Returns a message signer's unauthenticated attributes. To retrieve the unauthenticated attributes for
        ///       a specified signer, call <b>CryptMsgGetParam</b> with <paramref name="SignerIndex"/> equal to that
        ///       signer's index.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_TYPE_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <see cref="Int32"/>.<br/>
        ///       Returns the message type of a decoded message of unknown type. The retrieved message type can be
        ///       compared to supported types to determine whether processing can continued. For supported message types,
        ///       see the <b>MessageType</b> parameter of <see cref="CryptMsgOpenToDecode"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_UNPROTECTED_ATTR_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <b>BYTE</b> array to receive a <see cref="CMSG_ATTR"/> structure.<br/>
        ///       Returns the unprotected attributes in an enveloped message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_VERSION_PARAM
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> data type: pointer to a <see cref="Int32"/>.<br/>
        ///       Returns the version of the decoded message. For more information, see the table in the Remarks section.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="SignerIndex">Index for the parameter being retrieved, where applicable. When a parameter is not being retrieved, this parameter is ignored and is set to zero.</param>
        /// <param name="Data">
        /// A pointer to a buffer that receives the data retrieved. The form of this data will vary depending on the value of the <paramref name="ParamType"/> parameter.<br/>
        /// This parameter can be <b>NULL</b> to set the size of this information for memory allocation purposes.<br/>
        /// When processing the data returned in this buffer, applications need to use the actual size of the data returned. The actual size can be slightly smaller than the size of the buffer specified on input. (On input, buffer sizes are usually specified large enough to ensure that the largest possible output data will fit in the buffer.) On output, the variable pointed to by this parameter is updated to reflect the actual size of the data copied to the buffer.
        /// </param>
        /// <param name="Size">
        /// A pointer to a variable that specifies the size, in bytes, of the buffer pointed to by the <paramref name="Data"/> parameter. When the function returns, the variable pointed to by the <paramref name="Size"/> parameter contains the number of bytes stored in the buffer.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// <table class="primary_errors_table">
        ///   <tr>
        ///     <td style="width:20%">
        ///       CRYPT_E_ATTRIBUTES_MISSING
        ///     </td>
        ///     <td>
        ///       The message does not contain the requested attributes.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_INVALID_INDEX
        ///     </td>
        ///     <td>
        ///       The index value is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_INVALID_MSG_TYPE
        ///     </td>
        ///     <td>
        ///       The message type is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_NOT_DECRYPTED
        ///     </td>
        ///     <td>
        ///       The message content has not been decrypted yet.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_OID_FORMAT
        ///     </td>
        ///     <td>
        ///       The object identifier is badly formatted.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_UNEXPECTED_ENCODING
        ///     </td>
        ///     <td>
        ///       The message is not encoded as expected.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       One or more arguments are not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_MORE_DATA
        ///     </td>
        ///     <td>
        ///       The specified buffer is not large enough to hold the returned data.
        ///     </td>
        ///   </tr>
        /// </table>
        /// For <paramref name="ParamType"/> CMSG_COMPUTED_HASH_PARAM, an error can be propagated from <see cref="CryptGetHashParam"/>.<br/>
        /// If the function fails, <see cref="LastErrorService.GetLastError"/> may return an Abstract Syntax Notation One (ASN.1) encoding/decoding error.
        /// </returns>
        Boolean CryptMsgGetParam(IntPtr Message,CMSG_PARAM ParamType,Int32 SignerIndex,Byte[] Data,ref Int32 Size);
        #endregion
        #region M:CryptMsgUpdate(IntPtr,Byte[],Int32,Boolean):Boolean
        /// <summary>
        /// The function adds contents to a cryptographic message. The use of this function allows messages to be
        /// constructed piece by piece through repetitive calls of <b>CryptMsgUpdate</b>. The added message content is
        /// either encoded or decoded depending on whether the message was opened with <see cref="CryptMsgOpenToEncode"/> or <see cref="CryptMsgOpenToDecode"/>.
        /// </summary>
        /// <param name="Message">Cryptographic message handle of the message to be updated.</param>
        /// <param name="Data">A pointer to the buffer holding the data to be encoded or decoded.</param>
        /// <param name="Size">Number of bytes of data in the <paramref name="Data"/> buffer.</param>
        /// <param name="Final">
        /// Indicates that the last block of data for encoding or decoding is being processed. Correct usage of this flag
        /// is dependent upon whether the message being processed has detached data. The inclusion of detached data in
        /// a message is indicated by setting <b>Flags</b> to <b>CMSG_DETACHED_FLAG</b> in the call to the function that opened the message.<br/>
        /// If <b>CMSG_DETACHED_FLAG</b> was not set and the message was opened using either <see cref="CryptMsgOpenToDecode"/> or
        /// <see cref="CryptMsgOpenToEncode"/>, <paramref name="Final"/> is set to <b>TRUE</b>, and <b>CryptMsgUpdate</b> is only called once.<br/>
        /// If the <b>CMSG_DETACHED_FLAG</b> flag was set and a message is opened using <see cref="CryptMsgOpenToEncode"/>,
        /// <paramref name="Final"/> is set to <b>TRUE</b> only on the last call to <b>CryptMsgUpdate</b>.<br/>
        /// If the <b>CMSG_DETACHED_FLAG</b> flag was set and a message is opened using <see cref="CryptMsgOpenToDecode"/>,
        /// <paramref name="Final"/> is set to <b>TRUE</b> when the header is processed by a single call to <b>CryptMsgUpdate</b>.
        /// It is set to <b>FALSE</b> while processing the detached data in subsequent calls to <b>CryptMsgUpdate</b> until
        /// the last detached data block is to be processed. On the last call to <b>CryptMsgUpdate</b>, it is set to <b>TRUE</b>.<br/>
        /// When detached data is decoded, the header and the content of a message are contained in different BLOBs. Each BLOB requires
        /// that <paramref name="Final"/> be set to <b>TRUE</b> when the last call to the function is made for that BLOB.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// Errors encountered in the application defined callback function specified by pStreamInfo in <see cref="CryptMsgOpenToDecode"/> and
        /// <see cref="CryptMsgOpenToEncode"/> might be propagated to <b>CryptMsgUpdate</b> if streaming is used.
        /// If this happens, <b>SetLastError</b> is not called by <b>CryptMsgUpdate</b> after the callback function returns, which preserves any
        /// errors encountered under the control of the application. It is the responsibility of the callback
        /// function (or one of the APIs that it calls) to call <b>SetLastError</b> if an error occurs while the application is processing the streamed data.
        /// The following table lists the error codes most commonly returned by the <see cref="LastErrorService.GetLastError"/> function.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_E_INVALID_MSG_TYPE
        ///     </td>
        ///     <td>
        ///       The message type is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_MSG_ERROR
        ///     </td>
        ///     <td>
        ///       An error was encountered doing a cryptographic operation.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_OID_FORMAT
        ///     </td>
        ///     <td>
        ///       The object identifier is badly formatted.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_UNEXPECTED_ENCODING
        ///     </td>
        ///     <td>
        ///       The message is not encoded as expected.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_UNKNOWN_ALGO
        ///     </td>
        ///     <td>
        ///       The cryptographic algorithm is unknown.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       One or more arguments are not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_OUTOFMEMORY
        ///     </td>
        ///     <td>
        ///       Ran out of memory.
        ///     </td>
        ///   </tr>
        /// </table>
        /// Propagated errors might be encountered from any of the following functions:
        /// <list type="bullet">
        ///   <item><see cref="CryptHashData"/></item>
        ///   <item><see cref="CryptGetHashParam"/></item>
        ///   <item><see cref="CryptSignHash"/></item>
        ///   <item><see cref="CryptGetKeyParam"/></item>
        ///   <item><see cref="CryptEncrypt"/></item>
        ///   <item><see cref="CryptCreateHash"/></item>
        /// </list>
        /// If the function fails, <see cref="LastErrorService.GetLastError"/> may return an Abstract Syntax Notation One (ASN.1) encoding/decoding error.
        /// </returns>
        Boolean CryptMsgUpdate(IntPtr Message,Byte[] Data,Int32 Size,Boolean Final);
        #endregion
        #region M:CryptMsgUpdate(IntPtr,IntPtr,Int32,Boolean):Boolean
        /// <summary>
        /// The function adds contents to a cryptographic message. The use of this function allows messages to be
        /// constructed piece by piece through repetitive calls of <b>CryptMsgUpdate</b>. The added message content is
        /// either encoded or decoded depending on whether the message was opened with <see cref="CryptMsgOpenToEncode"/> or <see cref="CryptMsgOpenToDecode"/>.
        /// </summary>
        /// <param name="Message">Cryptographic message handle of the message to be updated.</param>
        /// <param name="Data">A pointer to the buffer holding the data to be encoded or decoded.</param>
        /// <param name="Size">Number of bytes of data in the <paramref name="Data"/> buffer.</param>
        /// <param name="Final">
        /// Indicates that the last block of data for encoding or decoding is being processed. Correct usage of this flag
        /// is dependent upon whether the message being processed has detached data. The inclusion of detached data in
        /// a message is indicated by setting <b>Flags</b> to <b>CMSG_DETACHED_FLAG</b> in the call to the function that opened the message.<br/>
        /// If <b>CMSG_DETACHED_FLAG</b> was not set and the message was opened using either <see cref="CryptMsgOpenToDecode"/> or
        /// <see cref="CryptMsgOpenToEncode"/>, <paramref name="Final"/> is set to <b>TRUE</b>, and <b>CryptMsgUpdate</b> is only called once.<br/>
        /// If the <b>CMSG_DETACHED_FLAG</b> flag was set and a message is opened using <see cref="CryptMsgOpenToEncode"/>,
        /// <paramref name="Final"/> is set to <b>TRUE</b> only on the last call to <b>CryptMsgUpdate</b>.<br/>
        /// If the <b>CMSG_DETACHED_FLAG</b> flag was set and a message is opened using <see cref="CryptMsgOpenToDecode"/>,
        /// <paramref name="Final"/> is set to <b>TRUE</b> when the header is processed by a single call to <b>CryptMsgUpdate</b>.
        /// It is set to <b>FALSE</b> while processing the detached data in subsequent calls to <b>CryptMsgUpdate</b> until
        /// the last detached data block is to be processed. On the last call to <b>CryptMsgUpdate</b>, it is set to <b>TRUE</b>.<br/>
        /// When detached data is decoded, the header and the content of a message are contained in different BLOBs. Each BLOB requires
        /// that <paramref name="Final"/> be set to <b>TRUE</b> when the last call to the function is made for that BLOB.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// Errors encountered in the application defined callback function specified by pStreamInfo in <see cref="CryptMsgOpenToDecode"/> and
        /// <see cref="CryptMsgOpenToEncode"/> might be propagated to <b>CryptMsgUpdate</b> if streaming is used.
        /// If this happens, <b>SetLastError</b> is not called by <b>CryptMsgUpdate</b> after the callback function returns, which preserves any
        /// errors encountered under the control of the application. It is the responsibility of the callback
        /// function (or one of the APIs that it calls) to call <b>SetLastError</b> if an error occurs while the application is processing the streamed data.
        /// The following table lists the error codes most commonly returned by the <see cref="LastErrorService.GetLastError"/> function.
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///       CRYPT_E_INVALID_MSG_TYPE
        ///     </td>
        ///     <td>
        ///       The message type is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_MSG_ERROR
        ///     </td>
        ///     <td>
        ///       An error was encountered doing a cryptographic operation.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_OID_FORMAT
        ///     </td>
        ///     <td>
        ///       The object identifier is badly formatted.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_UNEXPECTED_ENCODING
        ///     </td>
        ///     <td>
        ///       The message is not encoded as expected.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_UNKNOWN_ALGO
        ///     </td>
        ///     <td>
        ///       The cryptographic algorithm is unknown.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       One or more arguments are not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_OUTOFMEMORY
        ///     </td>
        ///     <td>
        ///       Ran out of memory.
        ///     </td>
        ///   </tr>
        /// </table>
        /// Propagated errors might be encountered from any of the following functions:
        /// <list type="bullet">
        ///   <item><see cref="CryptHashData"/></item>
        ///   <item><see cref="CryptGetHashParam"/></item>
        ///   <item><see cref="CryptSignHash"/></item>
        ///   <item><see cref="CryptGetKeyParam"/></item>
        ///   <item><see cref="CryptEncrypt"/></item>
        ///   <item><see cref="CryptCreateHash"/></item>
        /// </list>
        /// If the function fails, <see cref="LastErrorService.GetLastError"/> may return an Abstract Syntax Notation One (ASN.1) encoding/decoding error.
        /// </returns>
        Boolean CryptMsgUpdate(IntPtr Message,IntPtr Data,Int32 Size,Boolean Final);
        #endregion
        #region M:CryptSetHashParam(IntPtr,Int32,Byte[],Int32):Boolean
        /// <summary>
        /// The <b>CryptSetHashParam</b> function customizes the operations of a
        /// <a href="https://learn.microsoft.com/en-us/windows/desktop/SecGloss/h-gly">hash object</a>, including
        /// setting up initial hash contents and selecting a specific hashing algorithm.
        /// </summary>
        /// <param name="Hash">A handle to the hash object on which to set parameters (HCRYPTHASH).</param>
        /// <param name="Param">This parameter can be one of the following values.
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///        HP_HMAC_INFO
        ///      </td>
        ///      <td>
        ///        A pointer to an <b>HMAC_INFO</b> structure that specifies the cryptographic hash algorithm and the inner and outer strings to be used.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_HASHVAL
        ///      </td>
        ///      <td>
        ///        A byte array that contains a hash value to place directly into the hash object. Before setting this value, the size of the hash value must be determined by using the <see cref="CryptGetHashParam"/> function to read the HP_HASHSIZE value.<br/>
        ///        Some <a href="https://learn.microsoft.com/en-us/windows/desktop/SecGloss/c-gly">cryptographic service providers</a> (CSPs) do not support this capability.
        ///      </td>
        ///    </tr>
        /// </table>
        /// <b>CryptoPro CSP</b>:<br/>
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///   <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///        HP_HASHVAL
        ///      </td>
        ///      <td>
        ///        Устанавливается для объекта функции хэширования типа CALG_GR3411, CALG_GR3411_2012_256 и CALG_GR3411_2012_512. Величина, 32 байта для объектов типа CALG_GR3411 и CALG_GR3411_2012_256, 64 байта для объектов типа CALG_GR3411_2012_512, в little-endian порядке байт в соответствии с типом GostR3411-94-Digest CPCMS [RFC 4490], должна быть установлена в буфер <paramref name="Data"/>.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_HASHVAL_BLOB
        ///      </td>
        ///      <td>
        ///        Устанавливается для объекта функции хэширования типа CALG_GR3411, CALG_GR3411_2012_256 и CALG_GR3411_2012_512. В буфер <paramref name="Data"/> передаётся указатель на структуру <see cref="CRYPT_DATA_BLOB"/>, содержащую хэш-значение.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_HASHSIZE
        ///      </td>
        ///      <td>
        ///        Устанавливается для объекта функции хэширования типа CALG_G28147_IMIT, CALG_GR3413_2015_M_IMIT, CALG_GR3413_2015_K_IMIT. В буфер <paramref name="Data"/> записывается величина <see cref="Int32"/>, определяющая число байтов имитовставки в диапазоне от 1 до 4, от 1 до 8, от 1 до 16 соответственно. По умолчанию длина имитовставки для объекта хэша CALG_G28147_IMIT равна 4 байтам, для CALG_GR3413_2015_M_IMIT и CALG_GR3413_2015_K_IMIT составляет 4 и 8 байт соответственно.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        KP_HASHOID
        ///      </td>
        ///      <td>
        ///        Идентификатор функции хэширования. Строка, заканчивающаяся нулем. Не допускается задавать для объектов типа CALG_GR3411_HMAC / CALG_GR3411_HMAC34, CALG_GR3411_2012_256_HMAC / CALG_GR3411_2012_512_HMAC и CALG_GR3411_2012_256 / CALG_GR3411_2012_512.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_TLS1PRF_SEED
        ///      </td>
        ///      <td>
        ///        Составляющая аргумента функции GOSTR3411_PRF. Принимает различные значения в сообщениях клиента и сервера при реализации TLS Handshake Protocol. Задается в виде структуры <see cref="CRYPT_DATA_BLOB"/>.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_TLS1PRF_LABEL
        ///      </td>
        ///      <td>
        ///        Составляющая аргумента функции GOSTR3411_PRF. Принимает различные значения в сообщениях клиента и сервера при реализации TLS Handshake Protocol. Задается в виде структуры <see cref="CRYPT_DATA_BLOB"/>.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_PBKDF2_SALT
        ///      </td>
        ///      <td>
        ///        Задает синхропосылку размерности от 8 до 32 байт для объекта хэша типа CALG_PBKDF2_94_256, CALG_PBKDF2_2012_256 и CALG_PBKDF2_2012_512. Задается в виде структуры <see cref="CRYPT_DATA_BLOB"/>.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_PBKDF2_PASSWORD
        ///      </td>
        ///      <td>
        ///        Задает пароль для объекта хэша типа CALG_PBKDF2_94_256, CALG_PBKDF2_2012_256 и CALG_PBKDF2_2012_512. Задается в виде структуры <see cref="CRYPT_DATA_BLOB"/>.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_PBKDF2_COUNT
        ///      </td>
        ///      <td>
        ///        Устанавливается для объекта функции хэширования типа CALG_PBKDF2_94_256, CALG_PBKDF2_2012_256 и CALG_PBKDF2_2012_512. В буфер <paramref name="Data"/> записывается величина <see cref="Int32"/>, определяющая число итераций алгоритма. По умолчанию количество итераций равно 2000, минимальное допустимое значение параметра 1000.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_PADDING
        ///      </td>
        ///      <td>
        ///        Устанавливается для объектов функции хэширования типа CALG_ANSI_X9_19_MAC и CALG_MAC. В буфер <paramref name="Data"/> записывается величина <see cref="Int32"/>, определяющая тип выравнивания данных. По умолчанию тип паддинга — ZERO_PADDING, возможен также ISO_IEC_7816_4_PADDING.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_OPEN
        ///      </td>
        ///      <td>
        ///        Если через параметр <paramref name="Data"/> передаётся величина FALSE типа <see cref="Int32"/>, производит повторную инициализацию объекта хэша типа CALG_GR3411, CALG_GR3411_HMAC, CALG_GR3411_HMAC34, CALG_G28147_IMIT, CALG_GR3413_2015_M_IMIT, CALG_GR3413_2015_K_IMIT, CALG_GR3411_2012_256, CALG_GR3411_2012_512, CALG_GR3411_2012_256_HMAC, CALG_GR3411_2012_512_HMAC в случае, если он закрыт. Если в <paramref name="Data"/> передаётся величина TRUE типа <see cref="Int32"/>, закрытый объект функции хэширования типа CALG_GR3411, CALG_GR3411_2012_256, CALG_GR3411_2012_512 или CALG_G28147_IMIT открывается для дальнейшего хэширования данных. Передача в <paramref name="Data"/> величины TRUE для объектов хэша типа CALG_GR3413_2015_M_IMIT, CALG_GR3413_2015_K_IMIT приводит к завершению функции с ошибкой NTE_BAD_TYPE.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_KEYMIXSTART
        ///      </td>
        ///      <td>
        ///        Осуществляет диверсификацию ключа, ассоциированного с объектом хэширования, по алгоритму CALG_PRO_DIVERS. Через параметр <paramref name="Data"/> передаётся блоб диверсификации ключа в форме <see cref="CRYPT_DATA_BLOB"/>, см. <see cref="CryptImportKey"/>. Допускается многократный вызов с различными параметрами диверсификации.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_HASHSTARTVECT
        ///      </td>
        ///      <td>
        ///        Задает стартовый вектор для алгоритмов имитовставки по ГОСТ 28147-89 (8 байт) и хэширования по ГОСТ Р 34.11-94 (32 байта).
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_OPAQUEBLOB
        ///      </td>
        ///      <td>
        ///        Устанавливает блоб, содержащий внутреннее состояние объекта имитовставки ГОСТ 28147-89.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_HASHSTATEBLOB
        ///      </td>
        ///      <td>
        ///        Устанавливает блоб, содержащий внутреннее состояние объектов хэширования CALG_GR3411, CALG_GR3411_2012_256, CALG_GR3411_2012_512.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_SHAREDKEYMODE
        ///      </td>
        ///      <td>
        ///        Устанавливает значение числа от 2 до 5 компонент, на которые раскладывается ключ объектом CALG_SHAREDKEY_HASH, либо из которых собирается ключ объектом CALG_FITTINGKEY_HASH.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_HMAC_FIXEDKEY
        ///      </td>
        ///      <td>
        ///        Устанавливает значение ключа для объекта хэша типа CALG_GR3411_HMAC_FIXEDKEY, CALG_GR3411_2012_256_HMAC_FIXEDKEY и CALG_GR3411_2012_512_HMAC_FIXEDKEY. Через параметр <paramref name="Data"/> передаётся ключ в форме <see cref="CRYPT_DATA_BLOB"/>. Если размер переданных в блобе данных меньше размера соответствующего ключа, то ключ дополняется нулями, иначе - в качестве ключа устанавливается результат хэширования переданных данных.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_PRFKEYMAT_SEED
        ///      </td>
        ///      <td>
        ///        Устанавливает значение seed для объекта хэша типа CALG_GR3411_PRFKEYMAT, CALG_GR3411_2012_256_PRFKEYMAT и CALG_GR3411_2012_512_PRFKEYMAT. Через параметр <paramref name="Data"/> передаётся seed в форме <see cref="CRYPT_DATA_BLOB"/>. При этом предыдущее значение seed будет удалено.
        ///      </td>
        ///    </tr>
        /// </table>
        /// </param>
        /// <param name="Data">A value data buffer. Place the value data in this buffer before calling <b>CryptSetHashParam</b>. The form of this data varies, depending on the value number.</param>
        /// <param name="Flags">
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///        CRYPT_PASS_THROUGHT_DATA_BLOB
        ///      </td>
        ///      <td>
        ///        Decorate input <paramref name="Data"/> parameter with <see cref="CRYPT_DATA_BLOB"/> structure.
        ///      </td>
        ///    </tr>
        ///  </table>
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns <see langword="true"/>.<br/>
        /// If the function fails, it returns <see langword="false"/>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// The error codes prefaced by "NTE" are generated by the particular CSP you are using. Some possible error codes follow.
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///        ERROR_INVALID_HANDLE
        ///      </td>
        ///      <td>
        ///        One of the parameters specifies a handle that is not valid.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        ERROR_BUSY
        ///      </td>
        ///      <td>
        ///        The CSP context is currently being used by another process.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        ERROR_INVALID_PARAMETER
        ///      </td>
        ///      <td>
        ///        One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        NTE_BAD_FLAGS
        ///      </td>
        ///      <td>
        ///        The <paramref name="Flags"/> parameter is nonzero or the <paramref name="Data"/> buffer contains a value that is not valid.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        NTE_BAD_HASH
        ///      </td>
        ///      <td>
        ///        The hash object specified by the <paramref name="Hash"/> parameter is not valid.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        NTE_BAD_TYPE
        ///      </td>
        ///      <td>
        ///        The <paramref name="Param"/> parameter specifies an unknown value.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        NTE_BAD_UID
        ///      </td>
        ///      <td>
        ///        The CSP context that was specified when the key was created cannot be found.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        NTE_FAIL
        ///      </td>
        ///      <td>
        ///        The function failed in some unexpected way.
        ///      </td>
        ///    </tr>
        /// </table>
        /// <b>CryptoPro CSP</b>:<br/>
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///        NTE_BAD_HASH_STATE
        ///      </td>
        ///      <td>
        ///        An attempt was made to get the value of the hash function for "non-closed" hash object.
        ///      </td>
        ///    </tr>
        /// </table>
        /// </returns>
        Boolean CryptSetHashParam(IntPtr Hash,Int32 Param,Byte[] Data,Int32 Flags);
        #endregion
        #region M:CryptSetHashParam(IntPtr,Int32,{ref}CRYPT_DATA_BLOB,Int32):Boolean
        /// <summary>
        /// The <b>CryptSetHashParam</b> function customizes the operations of a
        /// <a href="https://learn.microsoft.com/en-us/windows/desktop/SecGloss/h-gly">hash object</a>, including
        /// setting up initial hash contents and selecting a specific hashing algorithm throught <see cref="CRYPT_DATA_BLOB"/>.
        /// </summary>
        /// <param name="Hash">A handle to the hash object on which to set parameters (HCRYPTHASH).</param>
        /// <param name="Param">This parameter can be one of the following values.<br/>
        /// <b>CryptoPro CSP</b>:<br/>
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///        HP_HASHVAL_BLOB
        ///      </td>
        ///      <td>
        ///        Устанавливается для объекта функции хэширования типа CALG_GR3411, CALG_GR3411_2012_256 и CALG_GR3411_2012_512. В буфер <paramref name="Data"/> передаётся указатель на структуру <see cref="CRYPT_DATA_BLOB"/>, содержащую хэш-значение.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_TLS1PRF_SEED
        ///      </td>
        ///      <td>
        ///        Составляющая аргумента функции GOSTR3411_PRF. Принимает различные значения в сообщениях клиента и сервера при реализации TLS Handshake Protocol. Задается в виде структуры <see cref="CRYPT_DATA_BLOB"/>.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_TLS1PRF_LABEL
        ///      </td>
        ///      <td>
        ///        Составляющая аргумента функции GOSTR3411_PRF. Принимает различные значения в сообщениях клиента и сервера при реализации TLS Handshake Protocol. Задается в виде структуры <see cref="CRYPT_DATA_BLOB"/>.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_PBKDF2_SALT
        ///      </td>
        ///      <td>
        ///        Задает синхропосылку размерности от 8 до 32 байт для объекта хэша типа CALG_PBKDF2_94_256, CALG_PBKDF2_2012_256 и CALG_PBKDF2_2012_512. Задается в виде структуры <see cref="CRYPT_DATA_BLOB"/>.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_PBKDF2_PASSWORD
        ///      </td>
        ///      <td>
        ///        Задает пароль для объекта хэша типа CALG_PBKDF2_94_256, CALG_PBKDF2_2012_256 и CALG_PBKDF2_2012_512. Задается в виде структуры <see cref="CRYPT_DATA_BLOB"/>.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_KEYMIXSTART
        ///      </td>
        ///      <td>
        ///        Осуществляет диверсификацию ключа, ассоциированного с объектом хэширования, по алгоритму CALG_PRO_DIVERS. Через параметр <paramref name="Data"/> передаётся блоб диверсификации ключа в форме <see cref="CRYPT_DATA_BLOB"/>, см. <see cref="CryptImportKey"/>. Допускается многократный вызов с различными параметрами диверсификации.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_OPAQUEBLOB
        ///      </td>
        ///      <td>
        ///        Устанавливает блоб, содержащий внутреннее состояние объекта имитовставки ГОСТ 28147-89.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_HASHSTATEBLOB
        ///      </td>
        ///      <td>
        ///        Устанавливает блоб, содержащий внутреннее состояние объектов хэширования CALG_GR3411, CALG_GR3411_2012_256, CALG_GR3411_2012_512.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_HMAC_FIXEDKEY
        ///      </td>
        ///      <td>
        ///        Устанавливает значение ключа для объекта хэша типа CALG_GR3411_HMAC_FIXEDKEY, CALG_GR3411_2012_256_HMAC_FIXEDKEY и CALG_GR3411_2012_512_HMAC_FIXEDKEY. Через параметр <paramref name="Data"/> передаётся ключ в форме <see cref="CRYPT_DATA_BLOB"/>. Если размер переданных в блобе данных меньше размера соответствующего ключа, то ключ дополняется нулями, иначе - в качестве ключа устанавливается результат хэширования переданных данных.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        HP_PRFKEYMAT_SEED
        ///      </td>
        ///      <td>
        ///        Устанавливает значение seed для объекта хэша типа CALG_GR3411_PRFKEYMAT, CALG_GR3411_2012_256_PRFKEYMAT и CALG_GR3411_2012_512_PRFKEYMAT. Через параметр <paramref name="Data"/> передаётся seed в форме <see cref="CRYPT_DATA_BLOB"/>. При этом предыдущее значение seed будет удалено.
        ///      </td>
        ///    </tr>
        /// </table>
        /// </param>
        /// <param name="Data">A value data structure.</param>
        /// <param name="Flags">This parameter is reserved for future use and must be set to zero.</param>
        /// <returns>
        /// If the function succeeds, the function returns <see langword="true"/>.<br/>
        /// If the function fails, it returns <see langword="false"/>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// The error codes prefaced by "NTE" are generated by the particular CSP you are using. Some possible error codes follow.
        ///  <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///        ERROR_INVALID_HANDLE
        ///      </td>
        ///      <td>
        ///        One of the parameters specifies a handle that is not valid.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        ERROR_BUSY
        ///      </td>
        ///      <td>
        ///        The CSP context is currently being used by another process.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        ERROR_INVALID_PARAMETER
        ///      </td>
        ///      <td>
        ///        One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        NTE_BAD_FLAGS
        ///      </td>
        ///      <td>
        ///        The <paramref name="Flags"/> parameter is nonzero or the <paramref name="Data"/> buffer contains a value that is not valid.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        NTE_BAD_HASH
        ///      </td>
        ///      <td>
        ///        The hash object specified by the <paramref name="Hash"/> parameter is not valid.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        NTE_BAD_TYPE
        ///      </td>
        ///      <td>
        ///        The <paramref name="Param"/> parameter specifies an unknown value.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        NTE_BAD_UID
        ///      </td>
        ///      <td>
        ///        The CSP context that was specified when the <paramref name="Key"/> key was created cannot be found.
        ///      </td>
        ///    </tr>
        ///    <tr>
        ///      <td>
        ///        NTE_FAIL
        ///      </td>
        ///      <td>
        ///        The function failed in some unexpected way.
        ///      </td>
        ///    </tr>
        /// </table>
        /// <b>CryptoPro CSP</b>:<br/>
        /// <table style="font-family: Arial;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
        ///    <tr>
        ///      <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;width:20%">
        ///        NTE_BAD_HASH_STATE
        ///      </td>
        ///      <td>
        ///        An attempt was made to get the value of the hash function for "non-closed" hash object.
        ///      </td>
        ///    </tr>
        /// </table>
        /// </returns>
        Boolean CryptSetHashParam(IntPtr Hash,Int32 Param,ref CRYPT_DATA_BLOB Data,Int32 Flags);
        #endregion
        #region M:CryptSetKeyParam(IntPtr,KEY_PARAM,Byte[],Int32):Boolean
        /// <summary>
        /// The function customizes various aspects of a session key's operations. The values set by this function are not persisted to memory and can only be used with in a single session.<br/>
        /// The <b>Microsoft Base Cryptographic Provider</b> does not permit setting values for key exchange or signature keys; however, custom providers can define values that can be set for its keys.
        /// </summary>
        /// <param name="Key">A handle to the key for which values are to be set.</param>
        /// <param name="Param">
        /// The following tables contain predefined values that can be used.<br/>
        /// For all key types, this parameter can contain one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       KP_ALGID
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to an appropriate <see cref="ALG_ID"/>.
        ///       This is used when exchanging session keys with the Microsoft Base Digital Signature Standard (DSS), Diffie-Hellman Cryptographic Provider, or compatible CSPs.
        ///       After a key is agreed upon with the <see cref="CryptImportKey"/> function, the session key is enabled for use by setting its algorithm type.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_CERTIFICATE
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> is the address of a buffer that contains the X.509 certificate that has been encoded by using Distinguished Encoding Rules (DER).
        ///       The public key in the certificate must match the corresponding signature or exchange key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_PERMISSIONS
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a DWORD value that specifies zero or more permission flags.
        ///       For a description of these flags, see <see cref="CryptGetKeyParam"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_SALT
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a BYTE array that specifies a new salt value to be made part of the session key.
        ///       The size of the salt value varies depending on the CSP being used.
        ///       Before setting this value, determine the size of the salt value by calling the <see cref="CryptGetKeyParam"/> function.
        ///       Salt values are used to make the session keys more unique, which makes dictionary attacks more difficult.
        ///       The salt value is zero by default for Microsoft Base Cryptographic Provider.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_SALT_EX
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a <see cref="CRYPT_INTEGER_BLOB"/> structure that contains the salt.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a Digital Signature Standard (DSS) key is specified by the <paramref name="Key"/> parameter, the <paramref name="Param"/> value can also be set to one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       KP_G
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to the generator G from the DSS key BLOB.
        ///       The data is in the form of a <see cref="CRYPT_INTEGER_BLOB"/> structure, where the <see cref="CRYPT_INTEGER_BLOB.Data"/> member is the value, and the <see cref="CRYPT_INTEGER_BLOB.Size"/> member is the length of the value.
        ///       The value is expected with no header information and in little-endian form.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_P
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to the prime modulus P of a DSS key BLOB. The data is in the form of a <see cref="CRYPT_INTEGER_BLOB"/> structure.
        ///       The <see cref="CRYPT_INTEGER_BLOB.Data"/> member is the value, and the <see cref="CRYPT_INTEGER_BLOB.Size"/> member is the length of the value.
        ///       The value is expected with no header information and in little-endian form.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_Q
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to the prime Q of a DSS key BLOB.
        ///       The data is in the form of a <see cref="CRYPT_INTEGER_BLOB"/> structure where the <see cref="CRYPT_INTEGER_BLOB.Data"/> member is the value, and the <see cref="CRYPT_INTEGER_BLOB.Size"/> member is the length of the value.
        ///       The value is expected with no header information and in little-endian form.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_X
        ///     </td>
        ///     <td>
        ///       After the P, Q, and G values have been set, a call that specifies the KP_X value for dwParam and NULL for the <paramref name="Data"/> parameter can be made to the <b>CryptSetKeyParam</b> function.
        ///       This causes the X and Y values to be generated.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a Diffie-Hellman algorithm or Digital Signature Algorithm (DSA) key is specified by <paramref name="Key"/>, the <paramref name="Param"/> value can also be set to one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       KP_CMS_DH_KEY_INFO
        ///     </td>
        ///     <td>
        ///       Sets the information for an imported Diffie-Hellman key.
        ///       The <paramref name="Data"/> parameter is the address of a <see cref="CMS_DH_KEY_INFO"/> structure that contains the key information to be set.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_PUB_PARAMS
        ///     </td>
        ///     <td>
        ///       Sets the public parameters (P, Q, G, and so on) of a DSS or Diffie-Hellman key.
        ///       The key handle for this key must be in the PREGEN state, generated with the CRYPT_PREGEN flag.
        ///       The <paramref name="Data"/> parameter must be a pointer to a <see cref="DATA_BLOB"/> structure where the data in this structure is a DHPUBKEY_VER3 or DSSPUBKEY_VER3 BLOB.
        ///       The function copies the public parameters from this <see cref="CRYPT_INTEGER_BLOB"/> structure to the key handle.
        ///       After this call is made, the KP_X parameter value should be used with <b>CryptSetKeyParam</b> to create the actual private key.
        ///       The KP_PUB_PARAMS parameter is used as one call rather than multiple calls with the parameter values KP_P, KP_Q, and KP_G.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a block cipher session key is specified by the <paramref name="Key"/> parameter, the <paramref name="Param"/> value can also be set to one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       KP_EFFECTIVE_KEYLEN
        ///     </td>
        ///     <td>
        ///       This value type can only be used with RC2 keys and has been added because of the implementation of the <b>CryptSetKeyParam</b> function in the Microsoft Enhanced Cryptographic Provider prior to Windows 2000.
        ///       In the previous implementation, the RC2 keys in the Enhanced Provider were 128 bits in strength, but the effective key length used to expand keys into the key table was only 40 bits.
        ///       This reduced the strength of the algorithm to 40 bits.
        ///       To maintain backward compatibility, the previous implementation will remain as is.
        ///       However, the effective key length can be set to be greater than 40 bits by using KP_EFFECTIVE_KEYLEN in the <b>CryptSetKeyParam</b> call.
        ///       The effective key length is passed in the <paramref name="Data"/> parameter as a pointer to a <b>DWORD</b> value with the effective key length value.
        ///       The minimum effective key length on the Microsoft Base Cryptographic Provider is one, and the maximum is 40.
        ///       In the Microsoft Enhanced Cryptographic Provider, the minimum is one and the maximum is 1,024.
        ///       The key length must be set prior to encrypting or decrypting with the key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_HIGHEST_VERSION
        ///     </td>
        ///     <td>
        ///       Sets the highest Transport Layer Security (TLS) version allowed. This property only applies to SSL and TLS keys.
        ///       The <paramref name="Data"/> parameter is the address of a <b>DWORD</b> variable that contains the highest TLS version number supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_IV
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a BYTE array that specifies the initialization vector.
        ///       This array must contain BlockLength/8 elements. For example, if the block length is 64 bits, the initialization vector consists of 8 bytes.<br/>
        ///       The initialization vector is set to zero by default for the Microsoft Base Cryptographic Provider.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_KEYVAL
        ///     </td>
        ///     <td>
        ///       Set the key value for a Data Encryption Standard (DES) key.
        ///       The <paramref name="Data"/> parameter is the address of a buffer that contains the key.
        ///       This buffer must be the same length as the key.
        ///       This property only applies to DES keys.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_PADDING
        ///     </td>
        ///     <td>
        ///       Set the padding mode. The <paramref name="Data"/> parameter is a pointer to a DWORD value that receives a numeric identifier that identifies the padding method used by the cipher.
        ///       This can be one of the following values.
        ///       <table class="table_value_meaning">
        ///         <tr>
        ///           <td>
        ///             PKCS5_PADDING
        ///           </td>
        ///           <td>
        ///             Specifies the PKCS 5 (sec 6.2) padding method.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             RANDOM_PADDING
        ///           </td>
        ///           <td>
        ///             The padding uses a random number. This padding method is not supported by the Microsoft supplied CSPs.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             ZERO_PADDING
        ///           </td>
        ///           <td>
        ///             The padding uses zeros. This padding method is not supported by the Microsoft supplied CSPs.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_MODE
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a <b>DWORD</b> value that specifies the cipher mode to be used.
        ///       For a list of the defined cipher modes, see <see cref="CryptGetKeyParam"/>.
        ///       The cipher mode is set to <b>CRYPT_MODE_CBC</b> by default for the Microsoft Base Cryptographic Provider.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_MODE_BITS
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a <b>DWORD</b> value that indicates the number of bits processed per cycle when the Output Feedback (OFB) or Cipher Feedback (CFB) cipher mode is used.
        ///       The number of bits processed per cycle is set to 8 by default for the Microsoft Base Cryptographic Provider.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If an RSA key is specified in the <paramref name="Key"/> parameter, the <paramref name="Param"/> parameter value can be the following value.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       KP_OAEP_PARAMS
        ///     </td>
        ///     <td>
        ///       Set the Optimal Asymmetric Encryption Padding (OAEP) (PKCS #1 version 2) parameters for the key.
        ///       The <paramref name="Data"/> parameter is the address of a CRYPT_DATA_BLOB structure that contains the OAEP label.
        ///       This property only applies to RSA keys.
        ///     </td>
        ///   </tr>
        /// </table>
        /// Note that the following values are not used:
        /// <list type="bullet">
        ///   <item>KP_ADMIN_PIN</item>
        ///   <item>KP_CMS_KEY_INFO</item>
        ///   <item>KP_INFO</item>
        ///   <item>KP_KEYEXCHANGE_PIN</item>
        ///   <item>KP_PRECOMP_MD5</item>
        ///   <item>KP_PRECOMP_SHA</item>
        ///   <item>KP_PREHASH</item>
        ///   <item>KP_PUB_EX_LEN</item>
        ///   <item>KP_PUB_EX_VAL</item>
        ///   <item>KP_RA</item>
        ///   <item>KP_RB</item>
        ///   <item>KP_ROUNDS</item>
        ///   <item>KP_RP</item>
        ///   <item>KP_SIGNATURE_PIN</item>
        ///   <item>KP_Y</item>
        /// </list>
        /// </param>
        /// <param name="Data">
        /// A pointer to a buffer initialized with the value to be set before calling <b>CryptSetKeyParam</b>. The form of this data varies depending on the value of <paramref name="Param"/>.
        /// </param>
        /// <param name="Flags">
        /// Used only when dwParam is <b>KP_ALGID</b>.
        /// The <paramref name="Flags"/> parameter is used to pass in flag values for the enabled key.
        /// The <paramref name="Flags"/> parameter can hold values such as the key size and the other flag values allowed when generating the same type of key with <see cref="CryptGenKey"/>.
        /// For information about allowable flag values, see <see cref="CryptGenKey"/>.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns <see langword="true"/>.<br/>
        /// If the function fails, it returns <see langword="false"/>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// The error codes prefaced by "NTE" are generated by the particular CSP you are using. Some possible error codes follow.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       ERROR_BUSY
        ///     </td>
        ///     <td>
        ///       The CSP context is currently being used by another process.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter is nonzero, or the <paramref name="Data"/> buffer contains a value that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Param"/> parameter specifies an unknown parameter.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the <paramref name="Key"/> key was created cannot be found.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_FAIL
        ///     </td>
        ///     <td>
        ///       The function failed in some unexpected way.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_FIXEDPARAMETER
        ///     </td>
        ///     <td>
        ///       Some CSPs have hard-coded P, Q, and G values. If this is the case, then using KP_P, KP_Q, and KP_G for the value of dwParam causes this error.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptSetKeyParam(IntPtr Key,KEY_PARAM Param,Byte[] Data,Int32 Flags);
        #endregion
        #region M:CryptSetKeyParam(IntPtr,KEY_PARAM,IntPtr,Int32):Boolean
        /// <summary>
        /// The function customizes various aspects of a session key's operations. The values set by this function are not persisted to memory and can only be used with in a single session.<br/>
        /// The <b>Microsoft Base Cryptographic Provider</b> does not permit setting values for key exchange or signature keys; however, custom providers can define values that can be set for its keys.
        /// </summary>
        /// <param name="Key">A handle to the key for which values are to be set.</param>
        /// <param name="Param">
        /// The following tables contain predefined values that can be used.<br/>
        /// For all key types, this parameter can contain one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       KP_ALGID
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to an appropriate <see cref="ALG_ID"/>.
        ///       This is used when exchanging session keys with the Microsoft Base Digital Signature Standard (DSS), Diffie-Hellman Cryptographic Provider, or compatible CSPs.
        ///       After a key is agreed upon with the <see cref="CryptImportKey"/> function, the session key is enabled for use by setting its algorithm type.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_CERTIFICATE
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> is the address of a buffer that contains the X.509 certificate that has been encoded by using Distinguished Encoding Rules (DER).
        ///       The public key in the certificate must match the corresponding signature or exchange key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_PERMISSIONS
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a DWORD value that specifies zero or more permission flags.
        ///       For a description of these flags, see <see cref="CryptGetKeyParam"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_SALT
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a BYTE array that specifies a new salt value to be made part of the session key.
        ///       The size of the salt value varies depending on the CSP being used.
        ///       Before setting this value, determine the size of the salt value by calling the <see cref="CryptGetKeyParam"/> function.
        ///       Salt values are used to make the session keys more unique, which makes dictionary attacks more difficult.
        ///       The salt value is zero by default for Microsoft Base Cryptographic Provider.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_SALT_EX
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a <see cref="CRYPT_INTEGER_BLOB"/> structure that contains the salt.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a Digital Signature Standard (DSS) key is specified by the <paramref name="Key"/> parameter, the <paramref name="Param"/> value can also be set to one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       KP_G
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to the generator G from the DSS key BLOB.
        ///       The data is in the form of a <see cref="CRYPT_INTEGER_BLOB"/> structure, where the <see cref="CRYPT_INTEGER_BLOB.Data"/> member is the value, and the <see cref="CRYPT_INTEGER_BLOB.Size"/> member is the length of the value.
        ///       The value is expected with no header information and in little-endian form.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_P
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to the prime modulus P of a DSS key BLOB. The data is in the form of a <see cref="CRYPT_INTEGER_BLOB"/> structure.
        ///       The <see cref="CRYPT_INTEGER_BLOB.Data"/> member is the value, and the <see cref="CRYPT_INTEGER_BLOB.Size"/> member is the length of the value.
        ///       The value is expected with no header information and in little-endian form.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_Q
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to the prime Q of a DSS key BLOB.
        ///       The data is in the form of a <see cref="CRYPT_INTEGER_BLOB"/> structure where the <see cref="CRYPT_INTEGER_BLOB.Data"/> member is the value, and the <see cref="CRYPT_INTEGER_BLOB.Size"/> member is the length of the value.
        ///       The value is expected with no header information and in little-endian form.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_X
        ///     </td>
        ///     <td>
        ///       After the P, Q, and G values have been set, a call that specifies the KP_X value for dwParam and NULL for the <paramref name="Data"/> parameter can be made to the <b>CryptSetKeyParam</b> function.
        ///       This causes the X and Y values to be generated.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a Diffie-Hellman algorithm or Digital Signature Algorithm (DSA) key is specified by <paramref name="Key"/>, the <paramref name="Param"/> value can also be set to one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       KP_CMS_DH_KEY_INFO
        ///     </td>
        ///     <td>
        ///       Sets the information for an imported Diffie-Hellman key.
        ///       The <paramref name="Data"/> parameter is the address of a <see cref="CMS_DH_KEY_INFO"/> structure that contains the key information to be set.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_PUB_PARAMS
        ///     </td>
        ///     <td>
        ///       Sets the public parameters (P, Q, G, and so on) of a DSS or Diffie-Hellman key.
        ///       The key handle for this key must be in the PREGEN state, generated with the CRYPT_PREGEN flag.
        ///       The <paramref name="Data"/> parameter must be a pointer to a <see cref="DATA_BLOB"/> structure where the data in this structure is a DHPUBKEY_VER3 or DSSPUBKEY_VER3 BLOB.
        ///       The function copies the public parameters from this <see cref="CRYPT_INTEGER_BLOB"/> structure to the key handle.
        ///       After this call is made, the KP_X parameter value should be used with <b>CryptSetKeyParam</b> to create the actual private key.
        ///       The KP_PUB_PARAMS parameter is used as one call rather than multiple calls with the parameter values KP_P, KP_Q, and KP_G.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a block cipher session key is specified by the <paramref name="Key"/> parameter, the <paramref name="Param"/> value can also be set to one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       KP_EFFECTIVE_KEYLEN
        ///     </td>
        ///     <td>
        ///       This value type can only be used with RC2 keys and has been added because of the implementation of the <b>CryptSetKeyParam</b> function in the Microsoft Enhanced Cryptographic Provider prior to Windows 2000.
        ///       In the previous implementation, the RC2 keys in the Enhanced Provider were 128 bits in strength, but the effective key length used to expand keys into the key table was only 40 bits.
        ///       This reduced the strength of the algorithm to 40 bits.
        ///       To maintain backward compatibility, the previous implementation will remain as is.
        ///       However, the effective key length can be set to be greater than 40 bits by using KP_EFFECTIVE_KEYLEN in the <b>CryptSetKeyParam</b> call.
        ///       The effective key length is passed in the <paramref name="Data"/> parameter as a pointer to a <b>DWORD</b> value with the effective key length value.
        ///       The minimum effective key length on the Microsoft Base Cryptographic Provider is one, and the maximum is 40.
        ///       In the Microsoft Enhanced Cryptographic Provider, the minimum is one and the maximum is 1,024.
        ///       The key length must be set prior to encrypting or decrypting with the key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_HIGHEST_VERSION
        ///     </td>
        ///     <td>
        ///       Sets the highest Transport Layer Security (TLS) version allowed. This property only applies to SSL and TLS keys.
        ///       The <paramref name="Data"/> parameter is the address of a <b>DWORD</b> variable that contains the highest TLS version number supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_IV
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a BYTE array that specifies the initialization vector.
        ///       This array must contain BlockLength/8 elements. For example, if the block length is 64 bits, the initialization vector consists of 8 bytes.<br/>
        ///       The initialization vector is set to zero by default for the Microsoft Base Cryptographic Provider.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_KEYVAL
        ///     </td>
        ///     <td>
        ///       Set the key value for a Data Encryption Standard (DES) key.
        ///       The <paramref name="Data"/> parameter is the address of a buffer that contains the key.
        ///       This buffer must be the same length as the key.
        ///       This property only applies to DES keys.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_PADDING
        ///     </td>
        ///     <td>
        ///       Set the padding mode. The <paramref name="Data"/> parameter is a pointer to a DWORD value that receives a numeric identifier that identifies the padding method used by the cipher.
        ///       This can be one of the following values.
        ///       <table class="table_value_meaning">
        ///         <tr>
        ///           <td>
        ///             PKCS5_PADDING
        ///           </td>
        ///           <td>
        ///             Specifies the PKCS 5 (sec 6.2) padding method.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             RANDOM_PADDING
        ///           </td>
        ///           <td>
        ///             The padding uses a random number. This padding method is not supported by the Microsoft supplied CSPs.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             ZERO_PADDING
        ///           </td>
        ///           <td>
        ///             The padding uses zeros. This padding method is not supported by the Microsoft supplied CSPs.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_MODE
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a <b>DWORD</b> value that specifies the cipher mode to be used.
        ///       For a list of the defined cipher modes, see <see cref="CryptGetKeyParam"/>.
        ///       The cipher mode is set to <b>CRYPT_MODE_CBC</b> by default for the Microsoft Base Cryptographic Provider.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       KP_MODE_BITS
        ///     </td>
        ///     <td>
        ///       <paramref name="Data"/> points to a <b>DWORD</b> value that indicates the number of bits processed per cycle when the Output Feedback (OFB) or Cipher Feedback (CFB) cipher mode is used.
        ///       The number of bits processed per cycle is set to 8 by default for the Microsoft Base Cryptographic Provider.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If an RSA key is specified in the <paramref name="Key"/> parameter, the <paramref name="Param"/> parameter value can be the following value.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       KP_OAEP_PARAMS
        ///     </td>
        ///     <td>
        ///       Set the Optimal Asymmetric Encryption Padding (OAEP) (PKCS #1 version 2) parameters for the key.
        ///       The <paramref name="Data"/> parameter is the address of a CRYPT_DATA_BLOB structure that contains the OAEP label.
        ///       This property only applies to RSA keys.
        ///     </td>
        ///   </tr>
        /// </table>
        /// Note that the following values are not used:
        /// <list type="bullet">
        ///   <item>KP_ADMIN_PIN</item>
        ///   <item>KP_CMS_KEY_INFO</item>
        ///   <item>KP_INFO</item>
        ///   <item>KP_KEYEXCHANGE_PIN</item>
        ///   <item>KP_PRECOMP_MD5</item>
        ///   <item>KP_PRECOMP_SHA</item>
        ///   <item>KP_PREHASH</item>
        ///   <item>KP_PUB_EX_LEN</item>
        ///   <item>KP_PUB_EX_VAL</item>
        ///   <item>KP_RA</item>
        ///   <item>KP_RB</item>
        ///   <item>KP_ROUNDS</item>
        ///   <item>KP_RP</item>
        ///   <item>KP_SIGNATURE_PIN</item>
        ///   <item>KP_Y</item>
        /// </list>
        /// </param>
        /// <param name="Data">
        /// A pointer to a buffer initialized with the value to be set before calling <b>CryptSetKeyParam</b>. The form of this data varies depending on the value of <paramref name="Param"/>.
        /// </param>
        /// <param name="Flags">
        /// Used only when dwParam is <b>KP_ALGID</b>.
        /// The <paramref name="Flags"/> parameter is used to pass in flag values for the enabled key.
        /// The <paramref name="Flags"/> parameter can hold values such as the key size and the other flag values allowed when generating the same type of key with <see cref="CryptGenKey"/>.
        /// For information about allowable flag values, see <see cref="CryptGenKey"/>.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns <see langword="true"/>.<br/>
        /// If the function fails, it returns <see langword="false"/>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// The error codes prefaced by "NTE" are generated by the particular CSP you are using. Some possible error codes follow.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       ERROR_BUSY
        ///     </td>
        ///     <td>
        ///       The CSP context is currently being used by another process.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter is nonzero, or the <paramref name="Data"/> buffer contains a value that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Param"/> parameter specifies an unknown parameter.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the <paramref name="Key"/> key was created cannot be found.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_FAIL
        ///     </td>
        ///     <td>
        ///       The function failed in some unexpected way.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_FIXEDPARAMETER
        ///     </td>
        ///     <td>
        ///       Some CSPs have hard-coded P, Q, and G values. If this is the case, then using KP_P, KP_Q, and KP_G for the value of dwParam causes this error.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptSetKeyParam(IntPtr Key,KEY_PARAM Param,IntPtr Data,Int32 Flags);
        #endregion
        #region M:CryptSetProvParam(IntPtr,CRYPT_PARAM,Byte[],Int32):Boolean
        /// <summary>
        /// This function customizes the operations of a cryptographic service provider (CSP).
        /// This function is commonly used to set a security descriptor on the key container associated with a CSP to control access to the private keys in that key container.
        /// </summary>
        /// <param name="Context">
        /// The handle of a CSP for which to set values.
        /// This handle must have already been created by using the <see cref="CryptAcquireContext"/> function.
        /// </param>
        /// <param name="Param">
        /// Specifies the parameter to set. This can be one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       PP_CLIENT_HWND
        ///     </td>
        ///     <td>
        ///       Set the window handle that the provider uses as the parent of any dialog boxes it creates.
        ///       <paramref name="Data"/> contains a pointer to an <b>HWND</b> that contains the parent window handle.<br/>
        ///       This parameter must be set before calling <see cref="CryptAcquireContext"/> because many CSPs will display a user interface when <see cref="CryptAcquireContext"/> is called.
        ///       You can pass <b>NULL</b> for the <paramref name="Context"/> parameter to set this window handle for all cryptographic contexts subsequently acquired within this process.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_DELETEKEY
        ///     </td>
        ///     <td>
        ///       Delete the ephemeral key associated with a hash, encryption, or verification context.
        ///       This will free memory and clear registry settings associated with the key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYEXCHANGE_ALG
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYEXCHANGE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies that the key exchange PIN is contained in <paramref name="Data"/>.
        ///       The PIN is represented as a <b>null</b>-terminated ASCII string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYEXCHANGE_KEYSIZE
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYSET_SEC_DESCR
        ///     </td>
        ///     <td>
        ///       Sets the security descriptor on the key storage container.
        ///       The <paramref name="Data"/> parameter is the address of a <see cref="SECURITY_DESCRIPTOR"/> structure that contains the new security descriptor for the key storage container.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_PIN_PROMPT_STRING
        ///     </td>
        ///     <td>
        ///       Sets an alternate prompt string to display to the user when the user's PIN is requested.
        ///       The <paramref name="Data"/> parameter is a pointer to a <b>null</b>-terminated Unicode string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ROOT_CERTSTORE
        ///     </td>
        ///     <td>
        ///       Sets the root certificate store for the smart card.
        ///       The provider will copy the root certificates from this store onto the smart card.<br/>
        ///       The <paramref name="Data"/> parameter is an <b>HCERTSTORE</b> variable that contains the handle of the new certificate store.
        ///       The provider will copy the certificates from the store during this call, so it is safe to close this store after this function is called.<br/>
        ///       <b>Windows XP and Windows Server 2003</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SIGNATURE_ALG
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SIGNATURE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies the signature PIN.
        ///       The <paramref name="Data"/> parameter is a <b>null</b>-terminated ASCII string that represents the PIN.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SIGNATURE_KEYSIZE
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_UI_PROMPT
        ///     </td>
        ///     <td>
        ///       For a smart card provider, sets the search string that is displayed to the user as a prompt to insert the smart card.
        ///       This string is passed as the <b>lpstrSearchDesc</b> member of the <a href="https://learn.microsoft.com/en-us/windows/desktop/api/winscard/ns-winscard-opencardname_exa">OPENCARDNAME_EX</a> structure that is passed to the <a href="https://learn.microsoft.com/en-us/windows/desktop/api/winscard/nf-winscard-scarduidlgselectcarda">SCardUIDlgSelectCard</a> function.
        ///       This string is used for the lifetime of the calling process.<br/>
        ///       The <paramref name="Data"/> parameter is a pointer to a null-terminated Unicode string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_USE_HARDWARE_RNG
        ///     </td>
        ///     <td>
        ///       Specifies that the CSP must exclusively use the hardware random number generator (RNG).
        ///       When <b>PP_USE_HARDWARE_RNG</b> is set, random values are taken exclusively from the hardware RNG and no other sources are used.
        ///       If a hardware RNG is supported by the CSP and it can be exclusively used, the function succeeds and returns <b>TRUE</b>; otherwise, the function fails and returns <b>FALSE</b>.
        ///       The <paramref name="Data"/> parameter must be <b>NULL</b> and <paramref name="Flags"/> must be zero when using this value.<br/>
        ///       None of the Microsoft CSPs currently support using a hardware RNG.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_USER_CERTSTORE
        ///     </td>
        ///     <td>
        ///       Specifies the user certificate store for the smart card.
        ///       This certificate store contains all of the user certificates that are stored on the smart card.
        ///       The certificates in this store are encoded by using PKCS_7_ASN_ENCODING or X509_ASN_ENCODING encoding and should contain the <b>CERT_KEY_PROV_INFO_PROP_ID</b> property.<br/>
        ///       The <paramref name="Data"/> parameter is an <b>HCERTSTORE</b> variable that receives the handle of an in-memory certificate store.
        ///       When this handle is no longer needed, the caller must close it by using the <see cref="CertCloseStore"/> function.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SECURE_KEYEXCHANGE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies that an encrypted key exchange PIN is contained in <paramref name="Data"/>.
        ///       The <paramref name="Data"/> parameter contains a <see cref="DATA_BLOB"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SECURE_SIGNATURE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies that an encrypted signature PIN is contained in <paramref name="Data"/>.
        ///       The <paramref name="Data"/> parameter contains a <see cref="DATA_BLOB"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SMARTCARD_READER
        ///     </td>
        ///     <td>
        ///       Specifies the name of the smart card reader.
        ///       The <paramref name="Data"/> parameter is the address of an ANSI character array that contains a <b>null</b>-terminated ANSI string that contains the name of the smart card reader.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SMARTCARD_GUID
        ///     </td>
        ///     <td>
        ///       Specifies the identifier of the smart card.
        ///       The <paramref name="Data"/> parameter is the address of a <b>GUID</b> structure that contains the identifier of the smart card.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
		/// </table>
        /// </param>
        /// <param name="Data">
        /// A pointer to a data buffer that contains the value to be set as a provider parameter.
        /// The form of this data varies depending on the <paramref name="Param"/> value.
        /// If <paramref name="Param"/> contains <b>PP_USE_HARDWARE_RNG</b>, this parameter must be <b>NULL</b>.
        /// </param>
        /// <param name="Flags">
        /// If <paramref name="Param"/> contains PP_KEYSET_SEC_DESCR, <paramref name="Flags"/> contains the <b>SECURITY_INFORMATION</b> applicable bit flags, as defined in the Platform SDK.
        /// Key-container security is handled by using <a href="https://learn.microsoft.com/en-us/windows/desktop/api/winbase/nf-winbase-setfilesecuritya">SetFileSecurity</a> and <a href="https://learn.microsoft.com/en-us/windows/desktop/api/winbase/nf-winbase-getfilesecuritya">GetFileSecurity</a>.<br/>
        /// These bit flags can be combined by using a bitwise-OR operation.
        /// For more information, see <see cref="CryptGetProvParam"/>.<br/>
        /// If <paramref name="Param"/> is <b>PP_USE_HARDWARE_RNG</b> or <b>PP_DELETEKEY</b>, <paramref name="Flags"/> must be set to zero.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns <see langword="true"/>.<br/>
        /// If the function fails, it returns <see langword="false"/>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// The error codes prefaced by "NTE" are generated by the particular CSP you are using. Some possible error codes follow.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       ERROR_BUSY
        ///     </td>
        ///     <td>
        ///       The CSP context is currently being used by another process.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid.
        ///       This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter is nonzero or the <paramref name="Data"/> buffer contains a value that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Param"/> parameter specifies an unknown parameter.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the hKey key was created cannot be found.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_FAIL
        ///     </td>
        ///     <td>
        ///       The function failed in some unexpected way.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Param,Byte[] Data,Int32 Flags);
        #endregion
        #region M:CryptSetProvParam(IntPtr,CRYPT_PARAM,IntPtr,Int32):Boolean
        /// <summary>
        /// This function customizes the operations of a cryptographic service provider (CSP).
        /// This function is commonly used to set a security descriptor on the key container associated with a CSP to control access to the private keys in that key container.
        /// </summary>
        /// <param name="Context">
        /// The handle of a CSP for which to set values.
        /// This handle must have already been created by using the <see cref="CryptAcquireContext"/> function.
        /// </param>
        /// <param name="Param">
        /// Specifies the parameter to set. This can be one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       PP_CLIENT_HWND
        ///     </td>
        ///     <td>
        ///       Set the window handle that the provider uses as the parent of any dialog boxes it creates.
        ///       <paramref name="Data"/> contains a pointer to an <b>HWND</b> that contains the parent window handle.<br/>
        ///       This parameter must be set before calling <see cref="CryptAcquireContext"/> because many CSPs will display a user interface when <see cref="CryptAcquireContext"/> is called.
        ///       You can pass <b>NULL</b> for the <paramref name="Context"/> parameter to set this window handle for all cryptographic contexts subsequently acquired within this process.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_DELETEKEY
        ///     </td>
        ///     <td>
        ///       Delete the ephemeral key associated with a hash, encryption, or verification context.
        ///       This will free memory and clear registry settings associated with the key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYEXCHANGE_ALG
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYEXCHANGE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies that the key exchange PIN is contained in <paramref name="Data"/>.
        ///       The PIN is represented as a <b>null</b>-terminated ASCII string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYEXCHANGE_KEYSIZE
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_KEYSET_SEC_DESCR
        ///     </td>
        ///     <td>
        ///       Sets the security descriptor on the key storage container.
        ///       The <paramref name="Data"/> parameter is the address of a <see cref="SECURITY_DESCRIPTOR"/> structure that contains the new security descriptor for the key storage container.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_PIN_PROMPT_STRING
        ///     </td>
        ///     <td>
        ///       Sets an alternate prompt string to display to the user when the user's PIN is requested.
        ///       The <paramref name="Data"/> parameter is a pointer to a <b>null</b>-terminated Unicode string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_ROOT_CERTSTORE
        ///     </td>
        ///     <td>
        ///       Sets the root certificate store for the smart card.
        ///       The provider will copy the root certificates from this store onto the smart card.<br/>
        ///       The <paramref name="Data"/> parameter is an <b>HCERTSTORE</b> variable that contains the handle of the new certificate store.
        ///       The provider will copy the certificates from the store during this call, so it is safe to close this store after this function is called.<br/>
        ///       <b>Windows XP and Windows Server 2003</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SIGNATURE_ALG
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SIGNATURE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies the signature PIN.
        ///       The <paramref name="Data"/> parameter is a <b>null</b>-terminated ASCII string that represents the PIN.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SIGNATURE_KEYSIZE
        ///     </td>
        ///     <td>
        ///       This constant is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_UI_PROMPT
        ///     </td>
        ///     <td>
        ///       For a smart card provider, sets the search string that is displayed to the user as a prompt to insert the smart card.
        ///       This string is passed as the <b>lpstrSearchDesc</b> member of the <a href="https://learn.microsoft.com/en-us/windows/desktop/api/winscard/ns-winscard-opencardname_exa">OPENCARDNAME_EX</a> structure that is passed to the <a href="https://learn.microsoft.com/en-us/windows/desktop/api/winscard/nf-winscard-scarduidlgselectcarda">SCardUIDlgSelectCard</a> function.
        ///       This string is used for the lifetime of the calling process.<br/>
        ///       The <paramref name="Data"/> parameter is a pointer to a null-terminated Unicode string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_USE_HARDWARE_RNG
        ///     </td>
        ///     <td>
        ///       Specifies that the CSP must exclusively use the hardware random number generator (RNG).
        ///       When <b>PP_USE_HARDWARE_RNG</b> is set, random values are taken exclusively from the hardware RNG and no other sources are used.
        ///       If a hardware RNG is supported by the CSP and it can be exclusively used, the function succeeds and returns <b>TRUE</b>; otherwise, the function fails and returns <b>FALSE</b>.
        ///       The <paramref name="Data"/> parameter must be <b>NULL</b> and <paramref name="Flags"/> must be zero when using this value.<br/>
        ///       None of the Microsoft CSPs currently support using a hardware RNG.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_USER_CERTSTORE
        ///     </td>
        ///     <td>
        ///       Specifies the user certificate store for the smart card.
        ///       This certificate store contains all of the user certificates that are stored on the smart card.
        ///       The certificates in this store are encoded by using PKCS_7_ASN_ENCODING or X509_ASN_ENCODING encoding and should contain the <b>CERT_KEY_PROV_INFO_PROP_ID</b> property.<br/>
        ///       The <paramref name="Data"/> parameter is an <b>HCERTSTORE</b> variable that receives the handle of an in-memory certificate store.
        ///       When this handle is no longer needed, the caller must close it by using the <see cref="CertCloseStore"/> function.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SECURE_KEYEXCHANGE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies that an encrypted key exchange PIN is contained in <paramref name="Data"/>.
        ///       The <paramref name="Data"/> parameter contains a <see cref="DATA_BLOB"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SECURE_SIGNATURE_PIN
        ///     </td>
        ///     <td>
        ///       Specifies that an encrypted signature PIN is contained in <paramref name="Data"/>.
        ///       The <paramref name="Data"/> parameter contains a <see cref="DATA_BLOB"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SMARTCARD_READER
        ///     </td>
        ///     <td>
        ///       Specifies the name of the smart card reader.
        ///       The <paramref name="Data"/> parameter is the address of an ANSI character array that contains a <b>null</b>-terminated ANSI string that contains the name of the smart card reader.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PP_SMARTCARD_GUID
        ///     </td>
        ///     <td>
        ///       Specifies the identifier of the smart card.
        ///       The <paramref name="Data"/> parameter is the address of a <b>GUID</b> structure that contains the identifier of the smart card.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This parameter is not supported.
        ///     </td>
        ///   </tr>
		/// </table>
        /// </param>
        /// <param name="Data">
        /// A pointer to a data buffer that contains the value to be set as a provider parameter.
        /// The form of this data varies depending on the <paramref name="Param"/> value.
        /// If <paramref name="Param"/> contains <b>PP_USE_HARDWARE_RNG</b>, this parameter must be <b>NULL</b>.
        /// </param>
        /// <param name="Flags">
        /// If <paramref name="Param"/> contains PP_KEYSET_SEC_DESCR, <paramref name="Flags"/> contains the <b>SECURITY_INFORMATION</b> applicable bit flags, as defined in the Platform SDK.
        /// Key-container security is handled by using <a href="https://learn.microsoft.com/en-us/windows/desktop/api/winbase/nf-winbase-setfilesecuritya">SetFileSecurity</a> and <a href="https://learn.microsoft.com/en-us/windows/desktop/api/winbase/nf-winbase-getfilesecuritya">GetFileSecurity</a>.<br/>
        /// These bit flags can be combined by using a bitwise-OR operation.
        /// For more information, see <see cref="CryptGetProvParam"/>.<br/>
        /// If <paramref name="Param"/> is <b>PP_USE_HARDWARE_RNG</b> or <b>PP_DELETEKEY</b>, <paramref name="Flags"/> must be set to zero.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns <see langword="true"/>.<br/>
        /// If the function fails, it returns <see langword="false"/>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// The error codes prefaced by "NTE" are generated by the particular CSP you are using. Some possible error codes follow.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       ERROR_BUSY
        ///     </td>
        ///     <td>
        ///       The CSP context is currently being used by another process.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid.
        ///       This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_FLAGS
        ///     </td>
        ///     <td>
        ///       The <paramref name="Flags"/> parameter is nonzero or the <paramref name="Data"/> buffer contains a value that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_TYPE
        ///     </td>
        ///     <td>
        ///       The <paramref name="Param"/> parameter specifies an unknown parameter.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the hKey key was created cannot be found.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_FAIL
        ///     </td>
        ///     <td>
        ///       The function failed in some unexpected way.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptSetProvParam(IntPtr Context,CRYPT_PARAM Param,IntPtr Data,Int32 Flags);
        #endregion
        #region M:CryptSignHash(IntPtr,KEY_SPEC_TYPE,Byte[],{ref}Int32):Boolean
        /// <summary>
        /// The CryptSignHash function signs hash.
        /// </summary>
        /// <param name="Hash">Handle of the hash object to be signed (HCRYPTHASH).</param>
        /// <param name="KeySpec">
        /// Identifies the private key to use from the provider's container. It can be AT_KEYEXCHANGE or AT_SIGNATURE.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       AT_KEYEXCHANGE
        ///     </td>
        ///     <td>
        ///       Key exchange.
        ///     </td>
        ///    </tr>
        ///   <tr>
        ///     <td>
        ///       AT_SIGNATURE
        ///     </td>
        ///     <td>
        ///       Digital signature.
        ///     </td>
        ///    </tr>
        /// </table>
        /// </param>
        /// <param name="Signature">
        /// A pointer to a buffer receiving the signature data.<br/>
        /// This parameter can be <see langword="null"/> to set the buffer size for memory allocation purposes.
        /// </param>
        /// <param name="Length">A reference to a <see cref="Int32"/> value that specifies the size, in bytes, of the <paramref name="Signature"/> buffer. When the function returns, the <see cref="Int32"/> value contains the number of bytes stored in the buffer.</param>
        /// <returns>
        /// If the function succeeds, the function returns <see langword="true"/>.<br/>
        /// If the function fails, it returns <see langword="false"/>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// The error codes prefaced by "NTE" are generated by the particular CSP you are using. Some possible error codes follow.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_MORE_DATA
        ///     </td>
        ///     <td>
        ///       The buffer specified by the pbSignature parameter is not large enough to hold the returned data. The required buffer size, in bytes, is in the <paramref name="Length"/> value.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_ALGID
        ///     </td>
        ///     <td>
        ///       The <paramref name="Hash"/> handle specifies an algorithm that this CSP does not support, or the <paramref name="KeySpec"/> parameter has an incorrect value.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_HASH
        ///     </td>
        ///     <td>
        ///       The hash object specified by the <paramref name="Hash"/> parameter is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The CSP context that was specified when the hash object was created cannot be found.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_NO_KEY
        ///     </td>
        ///     <td>
        ///       The private key specified by <paramref name="KeySpec"/> does not exist.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_NO_MEMORY
        ///     </td>
        ///     <td>
        ///       The CSP ran out of memory during the operation.
        ///     </td>
        ///   </tr>
        /// </table>
        /// <b>CryptoPro CSP</b>:<br/>
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       NTE_FAIL
        ///     </td>
        ///     <td>
        ///       Нарушение целостности ключей в ОЗУ.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_SILENT_CONTEXT
        ///     </td>
        ///     <td>
        ///       Операция не может быть выполнена без пользовательского интерфейса.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SCARD_W_CANCELLED_BY_USER
        ///     </td>
        ///     <td>
        ///       Пользователь прервал операцию нажатием клавиши Cancel.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SCARD_W_WRONG_CHV
        ///     </td>
        ///     <td>
        ///       Пользователь ввёл неправильный пароль или пароль, установленный функцией <see cref="CryptSetProvParam"/>, неправильный.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SCARD_E_INVALID_CHV
        ///     </td>
        ///     <td>
        ///       Пользователь ввёл пароль с нарушением формата или пароль, установленный функцией <see cref="CryptSetProvParam"/>, имеет неправильный формат. Например, пароль имеет недопустимую длину или содержит недопустимые символы.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SCARD_W_CHV_BLOCKED
        ///     </td>
        ///     <td>
        ///       Ввод Pin-кода был заблокирован смарт-картой, т.к. исчерпалось количество попыток, разрешенное картой для ввода.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SCARD_W_REMOVED_CARD
        ///     </td>
        ///     <td>
        ///       Носитель контейнера был удален из считывателя.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SCARD_E_NO_KEY_CONTAINER
        ///     </td>
        ///     <td>
        ///       Контекст открыт с флагом CRYPT_DEFAULT_CONTAINER_OPTIONAL, и не связан ни с одним контейнером, поэтому выполнение данной операции недоступно.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_BAD_FORMAT
        ///     </td>
        ///     <td>
        ///       Неподходящий формат подписываемого документа.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_MESSAGE_EXCEEDS_MAX_SIZE
        ///     </td>
        ///     <td>
        ///       Попытка подписи слишком большого документа.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptSignHash(IntPtr Hash, KEY_SPEC_TYPE KeySpec, Byte[] Signature, ref Int32 Length);
        #endregion
        #region M:CryptVerifyCertificateSignature(IntPtr,SubjectType,IntPtr,Int32,IntPtr,Int32):Boolean
        /// <summary>
        /// This function verifies the signature of a subject certificate, certificate revocation list, certificate request, or keygen request by using the issuer's public key.
        /// The function does not require access to a private key.
        /// </summary>
        /// <param name="Context">
        /// This parameter is not used and should be set to <b>NULL</b>.<br/>
        /// <b>Windows Server 2003 and Windows XP</b>: A handle to the cryptographic service provider used to verify the signature.
        /// This parameter's data type is <b>HCRYPTPROV</b>.<br/>
        /// <b>NULL</b> is passed unless there is a strong reason for passing in a specific cryptographic provider.
        /// Passing in <b>NULL</b> causes the default RSA or DSS provider to be acquired.
        /// </param>
        /// <param name="SubjectType">
        /// The subject type. This parameter can be one of the following subject types.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       CRYPT_VERIFY_CERT_SIGN_SUBJECT_BLOB
        ///     </td>
        ///     <td>
        ///       <paramref name="Subject"/> is a pointer to a <see cref="CRYPT_DATA_BLOB"/> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_VERIFY_CERT_SIGN_SUBJECT_CERT
        ///     </td>
        ///     <td>
        ///       <paramref name="Subject"/> is a pointer to a <see cref="CERT_CONTEXT"/> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_VERIFY_CERT_SIGN_SUBJECT_CRL
        ///     </td>
        ///     <td>
        ///       <paramref name="Subject"/> is a pointer to a <see cref="CRL_CONTEXT"/> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_VERIFY_CERT_SIGN_SUBJECT_OCSP_BASIC_SIGNED_RESPONSE
        ///     </td>
        ///     <td>
        ///       <paramref name="Subject"/> is a pointer to an <a href="https://learn.microsoft.com/en-us/windows/desktop/api/wincrypt/ns-wincrypt-ocsp_basic_signed_response_info">OCSP_BASIC_SIGNED_RESPONSE_INFO</a> structure.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This subject type is not supported.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Subject">A pointer to a structure of the type indicated by <paramref name="SubjectType"/> that contains the signature to be verified.</param>
        /// <param name="IssuerType">
        /// The issuer type. This parameter can be one of the following issuer types.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       CRYPT_VERIFY_CERT_SIGN_ISSUER_PUBKEY
        ///     </td>
        ///     <td>
        ///       <paramref name="Issuer"/> is a pointer to a <see cref="CERT_PUBLIC_KEY_INFO"/> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_VERIFY_CERT_SIGN_ISSUER_CERT
        ///     </td>
        ///     <td>
        ///       <paramref name="Issuer"/> is a pointer to a <see cref="CERT_CONTEXT"/> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_VERIFY_CERT_SIGN_ISSUER_CHAIN
        ///     </td>
        ///     <td>
        ///       <paramref name="Issuer"/> is a pointer to a <see cref="CERT_CHAIN_CONTEXT"/> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_VERIFY_CERT_SIGN_ISSUER_NULL
        ///     </td>
        ///     <td>
        ///       <paramref name="Issuer"/> must be <b>NULL</b>.
        ///     </td>
        ///   </tr>
        /// </table>
        /// <div class="note">
        ///   <b>Note</b>:
        ///   If <paramref name="IssuerType"/> is <b>CRYPT_VERIFY_CERT_SIGN_ISSUER_NULL</b> and the signature algorithm is a hashing algorithm, the signature is expected to contain only unencrypted hash octets.
        ///   Only <b>CRYPT_VERIFY_CERT_SIGN_ISSUER_NULL</b> can be specified in this nonencrypted signature case.
        ///   If any other <paramref name="IssuerType"/> is specified, verification fails and <see cref="LastErrorService.GetLastError"/> returns <b>E_INVALIDARG</b>.
        /// </div>
        /// </param>
        /// <param name="Issuer">
        /// A pointer to a structure of the type indicated by the value of <paramref name="IssuerType"/>.
        /// The structure contains access to the public key needed to verify the signature.</param>
        /// <param name="Flags">
        /// Flags that modify the function behavior. This can be zero or a bitwise <b>OR</b> of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       CRYPT_VERIFY_CERT_SIGN_DISABLE_MD2_MD4_FLAG
        ///     </td>
        ///     <td>
        ///       If you set this flag and <b>CryptVerifyCertificateSignature</b> detects an MD2 or MD4 algorithm, the function returns <b>FALSE</b> and sets <see cref="LastErrorService.GetLastError"/> to <b>NTE_BAD_ALGID</b>.
        ///       The signature is still verified, but this combination of errors enables the caller, now knowing that an MD2 or MD4 algorithm was used, to decide whether to trust or reject the signature.<br/>
        ///       <b>Windows 8 and Windows Server 2012</b>: Support for this flag begins.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_VERIFY_CERT_SIGN_SET_STRONG_PROPERTIES_FLAG
        ///     </td>
        ///     <td>
        ///       Sets strong signature properties, after successful verification, on the subject pointed to by the <paramref name="Subject"/> parameter.<br/>
        ///       The following property is set on the certificate context:
        ///       <list type="bullet">
        ///         <item><b>CERT_SIGN_HASH_CNG_ALG_PROP_ID</b></item>
        ///       </list>
        ///       The following properties are set on the CRL context:
        ///       <list type="bullet">
        ///         <item><b>CERT_SIGN_HASH_CNG_ALG_PROP_ID</b></item>
        ///         <item><b>CERT_ISSUER_PUB_KEY_BIT_LENGTH_PROP_ID</b></item>
        ///       </list>
        ///       <div class="note">
        ///         <b>Note</b>:
        ///         This flag is only applicable if <b>CRYPT_VERIFY_CERT_SIGN_SUBJECT_CRL</b> is specified in the <paramref name="SubjectType"/> parameter.
        ///       </div>
        ///       <b>Windows 8 and Windows Server 2012</b>: Support for this flag begins.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_VERIFY_CERT_SIGN_RETURN_STRONG_PROPERTIES_FLAG
        ///     </td>
        ///     <td>
        ///       Returns a pointer to a <b>CRYPT_VERIFY_CERT_SIGN_STRONG_PROPERTIES_INFO</b> structure in the pvExtra parameter.
        ///       The structure contains the length, in bits, of the public key and the names of the signing and hashing algorithms used.<br/>
        ///       You must call <b>CryptMemFree</b> to free the structure.
        ///       If memory cannot be allocated for the <b>CRYPT_VERIFY_CERT_SIGN_STRONG_PROPERTIES_INFO</b> structure, this function returns successfully but sets the pvExtra parameter to <b>NULL</b>.
        ///       <div class="note">
        ///         <b>Note</b>:
        ///         This flag is only applicable if <b>CRYPT_VERIFY_CERT_SIGN_SUBJECT_OCSP_BASIC_SIGNED_RESPONSE</b> is specified in the <paramref name="SubjectType"/> parameter.
        ///       </div>
        ///       <b>Windows 8 and Windows Server 2012</b>: Support for this flag begins.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <returns>
        /// Returns nonzero if successful or zero otherwise.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// <div class="note">
        ///   <b>Note</b>:
        ///   Errors from the called functions <see cref="CryptCreateHash"/>, <see cref="CryptImportKey"/>, <see cref="CryptVerifySignature"/>, and <see cref="CryptHashData"/> may be propagated to this function.
        /// </div>
        /// On failure, this function will cause the following error codes to be returned from <see cref="LastErrorService.GetLastError"/>.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       ERROR_FILE_NOT_FOUND
        ///     </td>
        ///     <td>
        ///       Invalid certificate encoding type. Currently only <b>X509_ASN_ENCODING</b> is supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_ALGID
        ///     </td>
        ///     <td>
        ///       The signature algorithm's object identifier (OID) does not map to a known or supported hash algorithm.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_SIGNATURE
        ///     </td>
        ///     <td>
        ///       The signature was not valid.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If the function fails, <see cref="LastErrorService.GetLastError"/> may return an Abstract Syntax Notation One (ASN.1) encoding/decoding error.
        /// </returns>
        Boolean CryptVerifyCertificateSignature(IntPtr Context,Int32 SubjectType,IntPtr Subject,Int32 IssuerType,IntPtr Issuer,Int32 Flags);
        #endregion
        #region M:CryptVerifySignature(IntPtr,Byte[],Int32,IntPtr):Boolean
        /// <summary>
        /// This function verifies the signature of a hash object.<br/>
        /// Before calling this function, <see cref="CryptCreateHash"/> must be called to create the handle of a hash object.
        /// <see cref="CryptHashData"/> or <see cref="CryptHashSessionKey"/> is then used to add data or session keys to the hash object.<br/>
        /// After <b>CryptVerifySignature</b> completes, only <see cref="CryptDestroyHash"/> can be called by using the <paramref name="Hash"/> handle.
        /// </summary>
        /// <param name="Hash">A handle to the hash object to verify.</param>
        /// <param name="Signature">The address of the signature data to be verified.</param>
        /// <param name="SignatureSize">The number of bytes in the pbSignature signature data.</param>
        /// <param name="Key">
        /// A handle to the public key to use to authenticate the signature.
        /// This public key must belong to the key pair that was originally used to create the digital signature.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns <see langword="true"/>.<br/>
        /// If the function fails, it returns <see langword="false"/>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/>
        /// The error codes prefaced by "NTE" are generated by the particular CSP you are using. Some possible error codes follow.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_HANDLE
        ///     </td>
        ///     <td>
        ///       One of the parameters specifies a handle that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_INVALID_PARAMETER
        ///     </td>
        ///     <td>
        ///       One of the parameters contains a value that is not valid. This is most often a pointer that is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_HASH
        ///     </td>
        ///     <td>
        ///       The hash object specified by the hHash parameter is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_KEY
        ///     </td>
        ///     <td>
        ///       The <paramref name="Key"/> parameter does not contain a handle to a valid public key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_SIGNATURE
        ///     </td>
        ///     <td>
        ///       The signature was not valid. This might be because the data itself has changed, the description string did not match, or the wrong public key was specified by <paramref name="Key"/>.<br/>
        ///       This error can also be returned if the hashing or signature algorithms do not match the ones used to create the signature.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_BAD_UID
        ///     </td>
        ///     <td>
        ///       The cryptographic service provider (CSP) context that was specified when the hash object was created cannot be found.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       NTE_NO_MEMORY
        ///     </td>
        ///     <td>
        ///       The CSP ran out of memory during the operation.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        Boolean CryptVerifySignature(IntPtr Hash,Byte[] Signature,Int32 SignatureSize,IntPtr Key);
        #endregion
        #region M:CertNameToStrA({ref}CRYPT_BLOB,Int32,IntPtr,Int32):Int32
        /// <summary>
        /// This function converts an encoded name in a <see cref="CERT_NAME_BLOB"/> structure to a null-terminated character string.<br/>
        /// The string representation follows the distinguished name specifications in <a href="https://www.ietf.org/rfc/rfc1779.txt">RFC 1779</a>.
        /// </summary>
        /// <param name="Name">A pointer to the <see cref="CERT_NAME_BLOB"/> structure to be converted.</param>
        /// <param name="StrType">
        /// This parameter specifies the format of the output string.
        /// This parameter also specifies other options for the contents of the string.<br/>
        /// This parameter can be one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       CERT_SIMPLE_NAME_STR
        ///     </td>
        ///     <td>
        ///       All object identifiers (OIDs) are discarded.
        ///       <b>CERT_RDN</b> entries are separated by a comma followed by a space (, ).
        ///       Multiple attributes in a <b>CERT_RDN</b> are separated by a plus sign enclosed within spaces ( + ), for example, Microsoft, Kim Abercrombie + Programmer.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_OID_NAME_STR
        ///     </td>
        ///     <td>
        ///       OIDs are included with an equal sign (=) separator from their attribute value.
        ///       <b>CERT_RDN</b> entries are separated by a comma followed by a space (, ).
        ///       Multiple attributes in a <b>CERT_RDN</b> are separated by a plus sign followed by a space (+ ).
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_X500_NAME_STR
        ///     </td>
        ///     <td>
        ///       OIDs are converted to their X.500 key names; otherwise, they are the same as <b>CERT_OID_NAME_STR</b>.
        ///       If an OID does not have a corresponding X.500 name, the OID is used with a prefix of OID.<br/>
        ///       The RDN value is quoted if it contains leading or trailing white space or one of the following characters:
        ///       <list type="bullet">
        ///         <item>Comma (,)</item>
        ///         <item>Plus sign (+)</item>
        ///         <item>Equal sign (=)</item>
        ///         <item>Inch mark (")</item>
        ///         <item>Backslash followed by the letter n (\n)</item>
        ///         <item>Less than sign (&lt;)</item>
        ///         <item>Greater than sign (>)</item>
        ///         <item>Number sign (#)</item>
        ///         <item>Semicolon (;)</item>
        ///       </list>
        ///       The quotation character is an inch mark (").
        ///       If the RDN value contains an inch mark, it is enclosed within quotation marks ("").
        ///     </td>
        ///   </tr>
        /// </table>
        /// The following options can also be combined with the value above to specify additional options for the string.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_SEMICOLON_FLAG
        ///     </td>
        ///     <td>
        ///       Replace the comma followed by a space (, ) separator with a semicolon followed by a space (; ) separator.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_CRLF_FLAG
        ///     </td>
        ///     <td>
        ///       Replace the comma followed by a space (, ) separator with a backslash followed by the letter r followed by a backslash followed by the letter n (\r\n) separator.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_NO_PLUS_FLAG
        ///     </td>
        ///     <td>
        ///       Replace the plus sign enclosed within spaces ( + ) separator with a single space separator.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_NO_QUOTING_FLAG
        ///     </td>
        ///     <td>
        ///       Disable quoting.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_REVERSE_FLAG
        ///     </td>
        ///     <td>
        ///       The order of the RDNs in the distinguished name string is reversed after decoding.
        ///       This flag is not set by default.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_DISABLE_IE4_UTF8_FLAG
        ///     </td>
        ///     <td>
        ///       By default, a CERT_RDN_T61_STRING X.500 key string is decoded as UTF8.
        ///       If UTF8 decoding fails, the X.500 key is decoded as an 8 bit character.
        ///       Use CERT_NAME_STR_DISABLE_IE4_UTF8_FLAG to skip the initial attempt to decode as UTF8.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_ENABLE_PUNYCODE_FLAG
        ///     </td>
        ///     <td>
        ///       If the name pointed to by the pName parameter contains an email RDN, and the host name portion of the email address contains a Punycode encoded <b>IA5String</b>, the name is converted to the Unicode equivalent.<br/>
        ///       <b>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This value is not supported.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Buffer">
        /// A pointer to a character buffer that receives the returned string.
        /// The size of this buffer is specified in the <paramref name="Size"/> parameter.
        /// </param>
        /// <param name="Size">
        /// The size, in characters, of the <paramref name="Buffer"/> buffer.
        /// The size must include the terminating null character.</param>
        /// <returns>
        /// Returns the number of characters converted, including the terminating null character.<br/>
        /// If <paramref name="Buffer"/> is <see cref="IntPtr.Zero"/> or <paramref name="Size"/> is zero, returns the required size of the destination string.
        /// </returns>
        /// <remarks>
        /// If <paramref name="Buffer"/> is not <b>NULL</b> and <paramref name="Size"/> is not zero, the returned <paramref name="Buffer"/> is always a null-terminated string.<br/>
        /// We recommend against using multicomponent RDNs (e.g., CN=James+O=Microsoft) to avoid possible ordering problems when decoding occurs. Instead, consider using single valued RDNs (e.g., CN=James, O=Microsoft).<br/>
        /// The string representation follows the distinguished name specifications in <a href="https://www.ietf.org/rfc/rfc1779.txt">RFC 1779</a> except for the deviations described in the following list.
        /// <list type="bullet">
        ///   <item>Names that contain quotes are enclosed within double quotation marks.</item>
        ///   <item>Empty strings are enclosed within double quotation marks.</item>
        ///   <item>Strings that contain consecutive spaces are not enclosed within quotation marks.</item>
        ///   <item>Relative Distinguished Name (RDN) values of type <b>CERT_RDN_ENCODED_BLOB</b> or <b>CERT_RDN_OCTET_STRING</b> are formatted in hexadecimal.</item>
        ///   <item>If an OID does not have a corresponding X.500 name, the “OID” prefix is used before OID.</item>
        ///   <item>RDN values are enclosed with double quotation marks (instead of "\") if they contain leading white space, trailing white space, or one of the following characters:
        ///    <list type="bullet">
        ///      <item>Comma (,)</item>
        ///      <item>Plus sign (+)</item>
        ///      <item>Equal sign (=)</item>
        ///      <item>Inch mark (")</item>
        ///      <item>Backslash (/)</item>
        ///      <item>Less than sign (&lt;)</item>
        ///      <item>Greater than sign (>)</item>
        ///      <item>Number sign (#)</item>
        ///      <item>Semicolon (;)</item>
        ///    </list>
        ///   </item>
        ///   <item>The X.500 key name for stateOrProvinceName (2.5.4.8) OID is "S". This value is different from the RFC 1779 X.500 key name ("ST").</item>
        /// </list>
        /// In addition, the following X.500 key names are not mentioned in RFC 1779, but may be returned by this API:
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Key
        ///     </th>
        ///     <th>
        ///       Object identifier string
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E
        ///     </td>
        ///     <td>
        ///       1.2.840.113549.1.9.1
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       T
        ///     </td>
        ///     <td>
        ///       2.5.4.12
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       G
        ///     </td>
        ///     <td>
        ///       2.5.4.42
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       I
        ///     </td>
        ///     <td>
        ///       2.5.4.43
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SN
        ///     </td>
        ///     <td>
        ///       2.5.4.4
        ///     </td>
        ///   </tr>
        /// </table>
        /// </remarks>
        Int32 CertNameToStrA(ref CRYPT_BLOB Name,Int32 StrType,IntPtr Buffer,Int32 Size);
        #endregion
        #region M:CertNameToStrW({ref}CRYPT_BLOB,Int32,IntPtr,Int32):Int32
        /// <summary>
        /// This function converts an encoded name in a <see cref="CERT_NAME_BLOB"/> structure to a null-terminated character string.<br/>
        /// The string representation follows the distinguished name specifications in <a href="https://www.ietf.org/rfc/rfc1779.txt">RFC 1779</a>.
        /// </summary>
        /// <param name="Name">A pointer to the <see cref="CERT_NAME_BLOB"/> structure to be converted.</param>
        /// <param name="StrType">
        /// This parameter specifies the format of the output string.
        /// This parameter also specifies other options for the contents of the string.<br/>
        /// This parameter can be one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       CERT_SIMPLE_NAME_STR
        ///     </td>
        ///     <td>
        ///       All object identifiers (OIDs) are discarded.
        ///       <b>CERT_RDN</b> entries are separated by a comma followed by a space (, ).
        ///       Multiple attributes in a <b>CERT_RDN</b> are separated by a plus sign enclosed within spaces ( + ), for example, Microsoft, Kim Abercrombie + Programmer.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_OID_NAME_STR
        ///     </td>
        ///     <td>
        ///       OIDs are included with an equal sign (=) separator from their attribute value.
        ///       <b>CERT_RDN</b> entries are separated by a comma followed by a space (, ).
        ///       Multiple attributes in a <b>CERT_RDN</b> are separated by a plus sign followed by a space (+ ).
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_X500_NAME_STR
        ///     </td>
        ///     <td>
        ///       OIDs are converted to their X.500 key names; otherwise, they are the same as <b>CERT_OID_NAME_STR</b>.
        ///       If an OID does not have a corresponding X.500 name, the OID is used with a prefix of OID.<br/>
        ///       The RDN value is quoted if it contains leading or trailing white space or one of the following characters:
        ///       <list type="bullet">
        ///         <item>Comma (,)</item>
        ///         <item>Plus sign (+)</item>
        ///         <item>Equal sign (=)</item>
        ///         <item>Inch mark (")</item>
        ///         <item>Backslash followed by the letter n (\n)</item>
        ///         <item>Less than sign (&lt;)</item>
        ///         <item>Greater than sign (>)</item>
        ///         <item>Number sign (#)</item>
        ///         <item>Semicolon (;)</item>
        ///       </list>
        ///       The quotation character is an inch mark (").
        ///       If the RDN value contains an inch mark, it is enclosed within quotation marks ("").
        ///     </td>
        ///   </tr>
        /// </table>
        /// The following options can also be combined with the value above to specify additional options for the string.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_SEMICOLON_FLAG
        ///     </td>
        ///     <td>
        ///       Replace the comma followed by a space (, ) separator with a semicolon followed by a space (; ) separator.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_CRLF_FLAG
        ///     </td>
        ///     <td>
        ///       Replace the comma followed by a space (, ) separator with a backslash followed by the letter r followed by a backslash followed by the letter n (\r\n) separator.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_NO_PLUS_FLAG
        ///     </td>
        ///     <td>
        ///       Replace the plus sign enclosed within spaces ( + ) separator with a single space separator.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_NO_QUOTING_FLAG
        ///     </td>
        ///     <td>
        ///       Disable quoting.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_REVERSE_FLAG
        ///     </td>
        ///     <td>
        ///       The order of the RDNs in the distinguished name string is reversed after decoding.
        ///       This flag is not set by default.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_DISABLE_IE4_UTF8_FLAG
        ///     </td>
        ///     <td>
        ///       By default, a CERT_RDN_T61_STRING X.500 key string is decoded as UTF8.
        ///       If UTF8 decoding fails, the X.500 key is decoded as an 8 bit character.
        ///       Use CERT_NAME_STR_DISABLE_IE4_UTF8_FLAG to skip the initial attempt to decode as UTF8.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_NAME_STR_ENABLE_PUNYCODE_FLAG
        ///     </td>
        ///     <td>
        ///       If the name pointed to by the pName parameter contains an email RDN, and the host name portion of the email address contains a Punycode encoded <b>IA5String</b>, the name is converted to the Unicode equivalent.<br/>
        ///       <b>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP</b>: This value is not supported.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Buffer">
        /// A pointer to a character buffer that receives the returned string.
        /// The size of this buffer is specified in the <paramref name="Size"/> parameter.
        /// </param>
        /// <param name="Size">
        /// The size, in characters, of the <paramref name="Buffer"/> buffer.
        /// The size must include the terminating null character.</param>
        /// <returns>
        /// Returns the number of characters converted, including the terminating null character.<br/>
        /// If <paramref name="Buffer"/> is <see cref="IntPtr.Zero"/> or <paramref name="Size"/> is zero, returns the required size of the destination string.
        /// </returns>
        /// <remarks>
        /// If <paramref name="Buffer"/> is not <b>NULL</b> and <paramref name="Size"/> is not zero, the returned <paramref name="Buffer"/> is always a null-terminated string.<br/>
        /// We recommend against using multicomponent RDNs (e.g., CN=James+O=Microsoft) to avoid possible ordering problems when decoding occurs. Instead, consider using single valued RDNs (e.g., CN=James, O=Microsoft).<br/>
        /// The string representation follows the distinguished name specifications in <a href="https://www.ietf.org/rfc/rfc1779.txt">RFC 1779</a> except for the deviations described in the following list.
        /// <list type="bullet">
        ///   <item>Names that contain quotes are enclosed within double quotation marks.</item>
        ///   <item>Empty strings are enclosed within double quotation marks.</item>
        ///   <item>Strings that contain consecutive spaces are not enclosed within quotation marks.</item>
        ///   <item>Relative Distinguished Name (RDN) values of type <b>CERT_RDN_ENCODED_BLOB</b> or <b>CERT_RDN_OCTET_STRING</b> are formatted in hexadecimal.</item>
        ///   <item>If an OID does not have a corresponding X.500 name, the “OID” prefix is used before OID.</item>
        ///   <item>RDN values are enclosed with double quotation marks (instead of "\") if they contain leading white space, trailing white space, or one of the following characters:
        ///    <list type="bullet">
        ///      <item>Comma (,)</item>
        ///      <item>Plus sign (+)</item>
        ///      <item>Equal sign (=)</item>
        ///      <item>Inch mark (")</item>
        ///      <item>Backslash (/)</item>
        ///      <item>Less than sign (&lt;)</item>
        ///      <item>Greater than sign (>)</item>
        ///      <item>Number sign (#)</item>
        ///      <item>Semicolon (;)</item>
        ///    </list>
        ///   </item>
        ///   <item>The X.500 key name for stateOrProvinceName (2.5.4.8) OID is "S". This value is different from the RFC 1779 X.500 key name ("ST").</item>
        /// </list>
        /// In addition, the following X.500 key names are not mentioned in RFC 1779, but may be returned by this API:
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Key
        ///     </th>
        ///     <th>
        ///       Object identifier string
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E
        ///     </td>
        ///     <td>
        ///       1.2.840.113549.1.9.1
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       T
        ///     </td>
        ///     <td>
        ///       2.5.4.12
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       G
        ///     </td>
        ///     <td>
        ///       2.5.4.42
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       I
        ///     </td>
        ///     <td>
        ///       2.5.4.43
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       SN
        ///     </td>
        ///     <td>
        ///       2.5.4.4
        ///     </td>
        ///   </tr>
        /// </table>
        /// </remarks>
        Int32 CertNameToStrW(ref CRYPT_BLOB Name,Int32 StrType,IntPtr Buffer,Int32 Size);
        #endregion
        #region M:CertAlgIdToOID(ALG_ID):IntPtr
        /// <summary>
        /// This function converts a CryptoAPI algorithm identifier (ALG_ID) to an Abstract Syntax Notation One (ASN.1) object identifier (OID) string.
        /// </summary>
        /// <param name="Id">Value to be converted to an OID.</param>
        /// <returns>
        /// If the function succeeds, the function returns the null-terminated OID string.<br/>
        /// If no OID string corresponds to the algorithm identifier, the function returns <see cref="IntPtr.Zero"/>.
        /// </returns>
        IntPtr CertAlgIdToOID(ALG_ID Id);
        #endregion
        #region M:CertCreateCertificateContext(Byte[]):IntPtr
        /// <summary>
        /// This function creates a certificate context from an encoded certificate.
        /// The created context is not persisted to a certificate store.
        /// The function makes a copy of the encoded certificate within the created context.
        /// </summary>
        /// <param name="Source">A pointer to a buffer that contains the encoded certificate from which the context is to be created.</param>
        /// <returns>
        /// If the function succeeds, the function returns a pointer to a read-only <see cref="CERT_CONTEXT"/>.
        /// When you have finished using the certificate context, free it by calling the <see cref="CertFreeCertificateContext"/> function.<br/>
        /// If the function is unable to decode and create the certificate context, it returns <see cref="IntPtr.Zero"/>.
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. Some possible error codes follow.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Return code
        ///     </th>
        ///     <th>
        ///       Description
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       A certificate encoding is not valid.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If the function fails, <see cref="LastErrorService.GetLastError"/> may return an Abstract Syntax Notation One (ASN.1) encoding/decoding error.
        /// </returns>
        IntPtr CertCreateCertificateContext(Byte[] Source);
        #endregion
        #region M:CertCreateCRLContext(Byte[]):IntPtr
        /// <summary>
        /// This function creates a certificate revocation list (CRL) context from an encoded CRL.
        /// The created context is not persisted to a certificate store.
        /// It makes a copy of the encoded CRL within the created context.
        /// </summary>
        /// <param name="Source">A pointer to a buffer containing the encoded CRL from which the context is to be created.</param>
        /// <returns>
        /// If the function succeeds, the return value is a pointer to a read-only <see cref="CRL_CONTEXT"/>.<br/>
        /// If the function fails and is unable to decode and create the <see cref="CRL_CONTEXT"/>, the return value is <see cref="IntPtr.Zero"/>.
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. The following table shows a possible error code.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Return code
        ///     </th>
        ///     <th>
        ///       Description
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       A CRL encoding is not valid.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If the function fails, <see cref="LastErrorService.GetLastError"/> may return an Abstract Syntax Notation One (ASN.1) encoding/decoding error.
        /// </returns>
        IntPtr CertCreateCRLContext(Byte[] Source);
        #endregion
        #region M:CertDuplicateCertificateContext(IntPtr):IntPtr
        /// <summary>
        /// This function duplicates a certificate context by incrementing its reference count.
        /// </summary>
        /// <param name="Context">A pointer to the <see cref="CERT_CONTEXT"/> structure for which the reference count is incremented.</param>
        /// <returns>
        /// Currently, a copy is not made of the context, and the returned pointer to a context has the same value as the pointer to a context that was input.
        /// If the pointer passed into this function is <see cref="IntPtr.Zero"/>, <see cref="IntPtr.Zero"/> is returned.
        /// When you have finished using the duplicate context, decrease its reference count by calling the <see cref="CertFreeCertificateContext"/> function.
        /// </returns>
        IntPtr CertDuplicateCertificateContext(IntPtr Context);
        #endregion
        #region M:CertDuplicateCRLContext(IntPtr):IntPtr
        /// <summary>
        /// This function duplicates a certificate revocation list (CRL) context by incrementing its reference count.
        /// </summary>
        /// <param name="Context">A pointer to the <see cref="CRL_CONTEXT"/> structure for which the reference count is being incremented.</param>
        /// <returns>
        /// Currently, a copy is not made of the context, and the returned context is the same as the context that was input.
        /// If the pointer passed into this function is <see cref="IntPtr.Zero"/>, <see cref="IntPtr.Zero"/> is returned.
        /// </returns>
        IntPtr CertDuplicateCRLContext(IntPtr Context);
        #endregion
        #region M:CertEnumCertificatesInStore(IntPtr,IntPtr):IntPtr
        /// <summary>
        /// This function retrieves the first or next certificate in a certificate store.
        /// Used in a loop, this function can retrieve in sequence all certificates in a certificate store.
        /// </summary>
        /// <param name="CertStore">A handle of a certificate store.</param>
        /// <param name="PrevCertContext">
        /// A pointer to the <see cref="CERT_CONTEXT"/> of the previous certificate context found.<br/>
        /// This parameter must be <see cref="IntPtr.Zero"/> to begin the enumeration and get the first certificate in the store.
        /// Successive certificates are enumerated by setting <paramref name="PrevCertContext"/> to the pointer returned by a previous call to the function.
        /// This function frees the <see cref="CERT_CONTEXT"/> referenced by non-<see cref="IntPtr.Zero"/> values of this parameter.<br/>
        /// For logical stores, including collection stores, a duplicate of the certificate context returned by this function cannot be used to begin a new subsequence of enumerations because the duplicated certificate loses the initial enumeration state.
        /// The enumeration skips any certificate previously deleted by <see cref="CertDeleteCertificateFromStore"/>.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns a pointer to the next <see cref="CERT_CONTEXT"/> in the store.
        /// If no more certificates exist in the store, the function returns <see cref="IntPtr.Zero"/>.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. Some possible error codes follow.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Return code
        ///     </th>
        ///     <th>
        ///       Description
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       The handle in the <paramref name="CertStore"/> parameter is not the same as that in the certificate context pointed to by <paramref name="PrevCertContext"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_NOT_FOUND
        ///     </td>
        ///     <td>
        ///       No certificates were found.
        ///       This happens if the store is empty or if the function reached the end of the store's list.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       ERROR_NO_MORE_FILES
        ///     </td>
        ///     <td>
        ///       Applies to external stores.
        ///       No certificates were found.
        ///       This happens if the store is empty or if the function reached the end of the store's list.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        IntPtr CertEnumCertificatesInStore(IntPtr CertStore,IntPtr PrevCertContext);
        #endregion
        #region M:CertEnumCRLsInStore(IntPtr,IntPtr):IntPtr
        /// <summary>
        /// This function retrieves the first or next certificate revocation list (CRL) context in a certificate store.
        /// Used in a loop, this function can retrieve in sequence all CRL contexts in a certificate store.
        /// </summary>
        /// <param name="CertStore">Handle of a certificate store.</param>
        /// <param name="PrevCrlContext">
        /// A pointer to the previous CRL_CONTEXT structure found.
        /// The <paramref name="PrevCrlContext"/> parameter must be <see cref="IntPtr.Zero"/> to get the first CRL in the store.
        /// Successive CRLs are enumerated by setting <paramref name="PrevCrlContext"/> to the pointer returned by a previous call to the function.
        /// This function frees the <see cref="CRL_CONTEXT"/> referenced by non-<see cref="IntPtr.Zero"/> values of this parameter.
        /// The enumeration skips any CRLs previously deleted by <see cref="CertDeleteCRLFromStore"/>.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a pointer to the next <see cref="CRL_CONTEXT"/> in the store.<br/>
        /// <see cref="IntPtr.Zero"/> is returned if the function fails. For extended error information, call <see cref="LastErrorService.GetLastError"/>. Some possible error codes follow.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Return code
        ///     </th>
        ///     <th>
        ///       Description
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       The handle in the <paramref name="CertStore"/> parameter is not the same as that in the certificate context pointed to by <paramref name="PrevCrlContext"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_NOT_FOUND
        ///     </td>
        ///     <td>
        ///       No CRL was found.
        ///       This happens if the store is empty or the end of the store's list is reached.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        IntPtr CertEnumCRLsInStore(IntPtr CertStore,IntPtr PrevCrlContext);
        #endregion
        #region M:CertFindCertificateInStore(IntPtr,Int32,Int32,IntPtr,IntPtr):IntPtr
        /// <summary>
        /// This function finds the first or next certificate context in a certificate store that matches a search criteria established by the <paramref name="FindType"/> and its associated <paramref name="FindPara"/>.
        /// This function can be used in a loop to find all of the certificates in a certificate store that match the specified find criteria.
        /// </summary>
        /// <param name="CertStore">A handle of the certificate store to be searched.</param>
        /// <param name="FindFlags">
        /// Used with some <paramref name="FindType"/> values to modify the search criteria.
        /// For most <paramref name="FindType"/> values, <paramref name="FindFlags"/> is not used and should be set to zero.</param>
        /// <param name="FindType">
        /// Specifies the type of search being made.
        /// The search type determines the data type, contents, and the use of <paramref name="FindPara"/>.
        /// This parameter can be one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_ANY
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <b>NULL</b>, not used.<br/>
        ///       No search criteria used. Returns the next certificate in the store.
        ///       <div class="note">
        ///         <b>Note</b>:
        ///         The order of the certificate context may not be preserved within the store.
        ///         To access a specific certificate you must iterate across the certificates in the store.
        ///       </div>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_CERT_ID
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CERT_ENHKEY_USAGE"/> structure.<br/>
        ///       Searches for a certificate in the store that has either an enhanced key usage extension or an enhanced key usage property and a usage identifier that matches the <see cref="CERT_ENHKEY_USAGE.UsageIdentifierCount"/> member in the <see cref="CERT_ENHKEY_USAGE"/> structure.<br/>
        ///       A certificate has an enhanced key usage extension if it has a <see cref="CERT_EXTENSION"/> structure with the <see cref="CERT_EXTENSION.pszObjId"/> member set to <b>szOID_ENHANCED_KEY_USAGE</b>.<br/>
        ///       A certificate has an enhanced key usage property if its CERT_ENHKEY_USAGE_PROP_ID identifier is set.<br/>
        ///       If CERT_FIND_OPTIONAL_ENHKEY_USAGE_FLAG is set in <paramref name="FindFlags"/>, certificates without the key usage extension or property are also matches.
        ///       Setting this flag takes precedence over passing NULL in <paramref name="FindPara"/>.<br/>
        ///       If CERT_FIND_EXT_ONLY_ENHKEY_USAGE_FLAG is set, a match is done only on the key usage extension.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_EXISTING
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CERT_CONTEXT"/> structure.<br/>
        ///       Searches for a certificate that is an exact match of the specified certificate context.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_HASH
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CRYPT_HASH_BLOB"/> structure.<br/>
        ///       Searches for a certificate with a SHA1 hash that matches the hash in the <b>CRYPT_HASH_BLOB</b> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_HAS_PRIVATE_KEY
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: NULL, not used.<br/>
        ///       Searches for a certificate that has a private key. The key can be ephemeral or saved on disk. The key can be a legacy Cryptography API (CAPI) key or a CNG key.
        ///       <div class="note">
        ///         <b>Note</b>:
        ///         The order of the certificate context may not be preserved within the store. Therefore, to access a specific certificate, you must iterate across all certificates.
        ///       </div>
        ///       <b>Windows 8 and Windows Server 2012</b>: Support for this flag begins.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_ISSUER_ATTR
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <b>CERT_RDN</b> structure.<br/>
        ///       Searches for a certificate with specified issuer attributes that match attributes in the <b>CERT_RDN</b> structure.
        ///       If these values are set, the function compares attributes of the issuer in a certificate with elements of the <b>CERT_RDN_ATTR</b> array in this <b>CERT_RDN</b> structure.
        ///       Comparisons iterate through the <b>CERT_RDN_ATTR</b> attributes looking for a match with the certificate's issuer attributes.<br/>
        ///       If the <b>pszObjId</b> member of <b>CERT_RDN_ATTR</b> is <b>NULL</b>, the attribute object identifier is ignored.<br/>
        ///       If the <b>dwValueType</b> member of <b>CERT_RDN_ATTR</b> is <b>CERT_RDN_ANY_TYPE</b>, the value type is ignored.<br/>
        ///       If the <b>pbData</b> member of <b>CERT_RDN_VALUE_BLOB</b> is <b>NULL</b>, any value is a match.<br/>
        ///       Currently only an exact, case-sensitive match is supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_ISSUER_NAME
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CERT_NAME_BLOB"/> structure.<br/>
        ///       Search for a certificate with an exact match of the entire issuer name with the name in <see cref="CERT_NAME_BLOB"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_ISSUER_OF
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CERT_CONTEXT"/> structure.<br/>
        ///       Searches for a certificate with a subject that matches the issuer in <see cref="CERT_CONTEXT"/>.<br/>
        ///       Instead of using <b>CertFindCertificateInStore</b> with this value, use the <see cref="CertGetCertificateChain"/> function.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_ISSUER_STR
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: Null-terminated Unicode string.<br/>
        ///       Searches for a certificate that contains the specified issuer name string.
        ///       The certificate's issuer member is converted to a name string of the appropriate type using the appropriate form of <b>CertNameToStr</b> formatted as CERT_SIMPLE_NAME_STR.
        ///       Then a case-insensitive substring-within-a-string match is performed.<br/>
        ///       If the substring match fails and the subject contains an email <b>RDN</b> with Punycode encoded string, <b>CERT_NAME_STR_ENABLE_PUNYCODE_FLAG</b> is used to convert the subject to a Unicode string and the substring match is performed again.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_KEY_IDENTIFIER
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CRYPT_HASH_BLOB"/> structure.<br/>
        ///       Searches for a certificate with a <b>CERT_KEY_IDENTIFIER_PROP_ID</b> property that matches the key identifier in <see cref="CRYPT_HASH_BLOB"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_KEY_SPEC
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="Int32"/> variable that contains a key specification.<br/>
        ///       Searches for a certificate that has a <b>CERT_KEY_SPEC_PROP_ID</b> property that matches the key specification in <paramref name="FindPara"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_MD5_HASH
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CRYPT_HASH_BLOB"/> structure.<br/>
        ///       Searches for a certificate with an MD5 hash that matches the hash in <see cref="CRYPT_HASH_BLOB"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_PROPERTY
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="Int32"/> variable that contains a property identifier.<br/>
        ///       Searches for a certificate with a property that matches the property identifier specified by the <see cref="Int32"/> value in <paramref name="FindPara"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_PUBLIC_KEY
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CERT_PUBLIC_KEY_INFO"/> structure.<br/>
        ///       Searches for a certificate with a public key that matches the public key in the <see cref="CERT_PUBLIC_KEY_INFO"/> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_SHA1_HASH
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CRYPT_HASH_BLOB"/> structure.<br/>
        ///       Searches for a certificate with a SHA1 hash that matches the hash in the <see cref="CRYPT_HASH_BLOB"/> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_SIGNATURE_HASH
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CRYPT_HASH_BLOB"/> structure.<br/>
        ///       Searches for a certificate with a signature hash that matches the signature hash in the <see cref="CRYPT_HASH_BLOB"/> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_SUBJECT_ATTR
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <b>CERT_RDN</b> structure.<br/>
        ///       Searches for a certificate with specified subject attributes that match attributes in the <b>CERT_RDN</b> structure.
        ///       If RDN values are set, the function compares attributes of the subject in a certificate with elements of the <b>CERT_RDN_ATTR</b> array in this <b>CERT_RDN</b> structure.
        ///       Comparisons iterate through the <b>CERT_RDN_ATTR</b> attributes looking for a match with the certificate's subject's attributes.<br/>
        ///       If the <b>pszObjId</b> member of <b>CERT_RDN_ATTR</b> is NULL, the attribute object identifier is ignored.<br/>
        ///       If the <b>dwValueType</b> member of <b>CERT_RDN_ATTR</b> is CERT_RDN_ANY_TYPE, the value type is ignored.<br/>
        ///       If the <b>pbData</b> member of <b>CERT_RDN_VALUE_BLOB</b> is NULL, any value is a match.<br/>
        ///       Currently only an exact, case-sensitive match is supported.<br/>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_SUBJECT_CERT
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CERT_INFO"/> structure.<br/>
        ///       Searches for a certificate with both an issuer and a serial number that match the issuer and serial number in the <see cref="CERT_INFO"/> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_SUBJECT_NAME
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CERT_NAME_BLOB"/> structure.<br/>
        ///       Searches for a certificate with an exact match of the entire subject name with the name in the <see cref="CERT_NAME_BLOB"/> structure.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_SUBJECT_STR
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: Null-terminated Unicode string.<br/>
        ///       Searches for a certificate that contains the specified subject name string.
        ///       The certificate's subject member is converted to a name string of the appropriate type using the appropriate form of <b>CertNameToStr</b> formatted as <b>CERT_SIMPLE_NAME_STR</b>.
        ///       Then a case-insensitive substring-within-a-string match is performed. 
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_CROSS_CERT_DIST_POINTS
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: Not used.<br/>
        ///       Find a certificate that has either a cross certificate distribution point extension or property.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FIND_PUBKEY_MD5_HASH
        ///     </td>
        ///     <td>
        ///       Data type of <paramref name="FindPara"/>: <see cref="CRYPT_HASH_BLOB"/> structure.<br/>
        ///       Find a certificate whose MD5-hashed public key matches the specified hash.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="FindPara">Points to a data item or structure used with <paramref name="FindType"/>.</param>
        /// <param name="PrevCertContext">
        /// A pointer to the last <see cref="CERT_CONTEXT"/> structure returned by this function.
        /// This parameter must be <see cref="IntPtr.Zero"/> on the first call of the function.
        /// To find successive certificates meeting the search criteria, set <paramref name="PrevCertContext"/> to the pointer returned by the previous call to the function.
        /// This function frees the <see cref="CERT_CONTEXT"/> referenced by non-<see cref="IntPtr.Zero"/> values of this parameter.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns a pointer to a read-only <see cref="CERT_CONTEXT"/> structure.<br/>
        /// If the function fails and a certificate that matches the search criteria is not found, the return value is NULL.<br/>
        /// A non-<see cref="IntPtr.Zero"/>&#32;<see cref="CERT_CONTEXT"/> that CertFindCertificateInStore returns must be freed by <see cref="CertFreeCertificateContext"/> or by being passed as the <paramref name="PrevCertContext"/> parameter on a subsequent call to <b>CertFindCertificateInStore</b>.<br/>
        /// For extended error information, call GetLastError. Some possible error codes follow.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Return code
        ///     </th>
        ///     <th>
        ///       Description
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_NOT_FOUND
        ///     </td>
        ///     <td>
        ///       No certificate was found matching the search criteria.
        ///       This can happen if the store is empty or the end of the store's list is reached.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       The handle in the <paramref name="CertStore"/> parameter is not the same as that in the certificate context pointed to by the <paramref name="PrevCertContext"/> parameter, or a value that is not valid was specified in the <paramref name="FindType"/> parameter.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        IntPtr CertFindCertificateInStore(IntPtr CertStore,Int32 FindFlags,Int32 FindType,IntPtr FindPara,IntPtr PrevCertContext);
        #endregion
        #region M:CertGetIssuerCertificateFromStore(IntPtr,IntPtr,IntPtr,{ref}Int32):IntPtr
        /// <summary>
        /// This function retrieves the certificate context from the certificate store for the first or next issuer of the specified subject certificate.
        /// </summary>
        /// <param name="CertStore">Handle of a certificate store.</param>
        /// <param name="SubjectContext">
        /// A pointer to a <see cref="CERT_CONTEXT"/> structure that contains the subject information.
        /// This parameter can be obtained from any certificate store or can be created by the calling application using the <see cref="CertCreateCertificateContext"/> function.
        /// </param>
        /// <param name="PrevIssuerContext">
        /// A pointer to a <see cref="CERT_CONTEXT"/> structure that contains the issuer information.
        /// An issuer can have multiple certificates, especially when a validity period is about to change.
        /// This parameter must be <see cref="IntPtr.Zero"/> on the call to get the first issuer certificate.
        /// To get the next certificate for the issuer, set <paramref name="PrevIssuerContext"/> to the <see cref="CERT_CONTEXT"/> structure returned by the previous call.<br/>
        /// This function frees the <see cref="CERT_CONTEXT"/> referenced by non-<see cref="IntPtr.Zero"/> values of this parameter.
        /// </param>
        /// <param name="Flags">
        /// The following flags enable verification checks on the returned certificate.
        /// They can be combined using a bitwise-<b>OR</b> operation to enable multiple verifications.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_NO_CRL_FLAG
        ///     </td>
        ///     <td>
        ///       Indicates no matching CRL was found.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_NO_ISSUER_FLAG
        ///     </td>
        ///     <td>
        ///       Indicates no issuer certificate was found.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_REVOCATION_FLAG
        ///     </td>
        ///     <td>
        ///       Checks whether the subject certificate is on the issuer's revocation list.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_SIGNATURE_FLAG
        ///     </td>
        ///     <td>
        ///       Uses the public key in the issuer's certificate to verify the signature on the subject certificate.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_TIME_VALIDITY_FLAG
        ///     </td>
        ///     <td>
        ///       Gets the current time and verifies that it is within the subject certificate's validity period.
        ///     </td>
        ///   </tr>
        /// </table>
        /// If a verification check of an enabled type succeeds, its flag is set to zero.
        /// If it fails, its flag remains set upon return.
        /// For <b>CERT_STORE_REVOCATION_FLAG</b>, the verification succeeds if the function does not find a CRL related to the subject certificate.<br/>
        /// If <b>CERT_STORE_REVOCATION_FLAG</b> is set and the issuer does not have a CRL in the store, <b>CERT_STORE_NO_CRL_FLAG</b> is set and <b>CERT_STORE_REVOCATION_FLAG</b> remains set.<br/>
        /// If <b>CERT_STORE_SIGNATURE_FLAG</b> or <b>CERT_STORE_REVOCATION_FLAG</b> is set, <b>CERT_STORE_NO_ISSUER_FLAG</b> is set if the function does not find an issuer certificate in the store.<br/>
        /// In the case of a verification check failure, a pointer to the issuer's <see cref="CERT_CONTEXT"/> is still returned and <see cref="LastErrorService.GetLastError"/> is not updated.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a pointer to a read-only issuer <see cref="CERT_CONTEXT"/>.<br/>
        /// If the function fails and the first or next issuer certificate is not found, the return value is <see cref="IntPtr.Zero"/>.<br/>
        /// Only the last returned <see cref="CERT_CONTEXT"/> structure must be freed by calling <see cref="CertFreeCertificateContext"/>.
        /// When the returned <see cref="CERT_CONTEXT"/> from one call to the function is supplied as the <paramref name="PrevIssuerContext"/> parameter on a subsequent call, the context is freed as part of the action of the function.<br/>
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>. Some possible error codes follow.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Return code
        ///     </th>
        ///     <th>
        ///       Description
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_NOT_FOUND
        ///     </td>
        ///     <td>
        ///       No issuer was found for the subject certificate.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_SELF_SIGNED
        ///     </td>
        ///     <td>
        ///       The issuer certificate is the same as the subject certificate. It is a self-signed root certificate.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       The handle in the <paramref name="CertStore"/> parameter is not the same as that of the certificate context pointed to by the <paramref name="PrevIssuerContext"/> parameter, or an unsupported flag was set in <paramref name="Flags"/>.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        IntPtr CertGetIssuerCertificateFromStore(IntPtr CertStore,IntPtr SubjectContext,IntPtr PrevIssuerContext,ref Int32 Flags);
        #endregion
        #region M:CertOpenStore(IntPtr,Int32,IntPtr,Int32,IntPtr):IntPtr
        /// <summary>
        /// This function opens a certificate store by using a specified store provider type. While this function can open a certificate store for most purposes, <see cref="CertOpenSystemStore"/> is recommended to open the most common certificate stores.
        /// <b>CertOpenStore</b> is required for more complex options and special cases.
        /// </summary>
        /// <param name="StoreProvider">
        /// A pointer to a null-terminated ANSI string that contains the store provider type.<br/>
        /// The following values represent the predefined store types.
        /// The store provider type determines the contents of the <paramref name="Para"/> parameter and the use and meaning of the high word of the <paramref name="Flags"/> parameter.
        /// Additional store providers can be installed or registered by using the <see cref="CryptInstallOIDFunctionAddress"/> or <see cref="CryptRegisterOIDFunction"/> function.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_COLLECTION<br/>
        ///       sz_CERT_STORE_PROV_COLLECTION
        ///     </td>
        ///     <td>
        ///       Opens a store that will be a collection of other stores.
        ///       Stores are added to or removed from the collection by using <see cref="CertAddStoreToCollection"/> and <see cref="CertRemoveStoreFromCollection"/>.
        ///       When a store is added to a collection, all certificates, CRLs, and CTLs in that store become available to searches or enumerations of the collection store.<br/>
        ///       The high word of <paramref name="Flags"/> is set to zero.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter must be <b>NULL</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_FILE
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs read from a specified open file.
        ///       This provider expects the file to contain only a serialized store and not either PKCS #7 signed messages or a single encoded certificate.<br/>
        ///       The file pointer must be positioned at the beginning of the serialized store information.
        ///       After the data in the serialized store has been loaded into the certificate store, the file pointer is positioned at the beginning of any data that can follow the serialized store data in the file.
        ///       If <b>CERT_FILE_STORE_COMMIT_ENABLE</b> is set in <paramref name="Flags"/>, the file handle is duplicated and the store is always committed as a serialized store.
        ///       The file is not closed when the store is closed.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter must contain a pointer to the handle of a file opened by using <a href="https://learn.microsoft.com/en-us/windows/desktop/api/fileapi/nf-fileapi-createfilea">CreateFile</a>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_FILENAME_A
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from a file.
        ///       The provider opens the file and first attempts to read the file as a serialized store, then as a PKCS #7 signed message, and finally as a single encoded certificate.<br/>
        ///       The <paramref name="EncodingType"/> parameter must contain the encoding types to be used with both messages and certificates.
        ///       If the file contains an X.509 encoded certificate, the open operation fails and a call to the <see cref="LastErrorService.GetLastError"/> function will return <b>ERROR_ACCESS_DENIED</b>.
        ///       If the <b>CERT_FILE_STORE_COMMIT_ENABLE</b> flag is set in <paramref name="Flags"/>, the <b>dwCreationDisposition</b> value passed to <b>CreateFile</b> is as follows:
        ///       <list type="bullet">
        ///         <item>If the <b>CERT_STORE_CREATE_NEW_FLAG</b> flag is set, <b>CreateFile</b> uses <b>CREATE_NEW</b>.</item>
        ///         <item>If the <b>CERT_STORE_OPEN_EXISTING_FLAG</b> flag is set, <b>CreateFile</b> uses <b>OPEN_EXISTING</b>.</item>
        ///         <item>For all other settings of <paramref name="Flags"/>, <b>CreateFile</b> uses <b>OPEN_ALWAYS</b>.</item>
        ///       </list>
        ///       If <paramref name="Flags"/> includes <b>CERT_FILE_STORE_COMMIT_ENABLE</b>, the file is committed as either a PKCS #7 or a serialized store depending on the file type opened.
        ///       If the file was empty or if the file name has either a .p7c or .spc extension, the file is committed as a PKCS #7.
        ///       Otherwise, the file is committed as a serialized store.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter must contain a pointer to null-terminated ANSI string that contains the name of an existing, unopened file.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_FILENAME(_W)<br/>
        ///       sz_CERT_STORE_PROV_FILENAME(_W)
        ///     </td>
        ///     <td>
        ///       Same as <b>CERT_STORE_PROV_FILENAME_A</b>.
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter must contain a pointer to null-terminated Unicode string that contains the name of an existing, unopened file.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_LDAP(_W)<br/>
        ///       sz_CERT_STORE_PROV_LDAP(_W)
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from the results of an LDAP query.<br/>
        ///       To perform write operations on the store, the query string must specify a BASE query with no filter and a single attribute.<br/>
        ///       <b><paramref name="Para"/> value</b>: If the <paramref name="Flags"/> parameter contains <b>CERT_LDAP_STORE_OPENED_FLAG</b>, set <paramref name="Para"/> to the address of a <b>CERT_LDAP_STORE_OPENED_PARA</b> structure that specifies the established LDAP session to use.<br/>
        ///       Otherwise, set <paramref name="Para"/> to point to a null-terminated Unicode string that contains the LDAP query string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_MEMORY<br/>
        ///       sz_CERT_STORE_PROV_MEMORY
        ///     </td>
        ///     <td>
        ///       Creates a certificate store in cached memory.
        ///       No certificates, certificate revocation lists (CRLs), or certificate trust lists (CTLs) are initially loaded into the store.
        ///       Typically used to create a temporary store.<br/>
        ///       Any addition of certificates, CRLs, or CTLs or changes in properties of certificates, CRLs, or CTLs in a memory store are not automatically saved.
        ///       They can be saved to a file or to a memory BLOB by using <see cref="CertSaveStore"/>.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_MSG
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from the specified cryptographic message.
        ///       The <paramref name="EncodingType"/> parameter must contain the encoding types used with both messages and certificates.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter contains an <b>HCRYPTMSG</b> handle of the encoded message, returned by a call to <see cref="CryptMsgOpenToDecode"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_PHYSICAL(_W)<br/>
        ///       sz_CERT_STORE_PROV_PHYSICAL(_W)
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from a specified physical store that is a member of a logical system store.<br/>
        ///       Two names are separated with an intervening backslash (\), for example "Root.Default".
        ///       Here, "Root" is the name of the system store and ".Default" is the name of the physical store.
        ///       The system and physical store names cannot contain any backslashes.
        ///       The high word of dwFlags indicates the system store location, usually <b>CERT_SYSTEM_STORE_CURRENT_USER</b>.
        ///       Some physical store locations can be opened remotely.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a null-terminated Unicode string that contains both the system store name and physical names.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_PKCS7<br/>
        ///       sz_CERT_STORE_PROV_PKCS7
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from an encoded PKCS #7 signed message.
        ///       The <paramref name="EncodingType"/> parameter must specify the encoding types to be used with both messages and certificates.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a <see cref="CRYPT_DATA_BLOB"/> structure that represents the encoded message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_PKCS12<br/>
        ///       sz_CERT_STORE_PROV_PKCS12
        ///     </td>
        ///     <td>
        ///       Initializes the store with the contents of a PKCS #12 packet.<br/>
        ///       If the PKCS #12 packet is protected with a NULL or empty password, this function will succeed in opening the store.<br/>
        ///       Beginning with Windows 8 and Windows Server 2012, if the password embedded in the PFX packet was protected to an Active Directory (AD) principal and the current user, as a member of that principal, has permission to decrypt the password, this function will succeed in opening the store.
        ///       For more information, see the <paramref name="Para"/> parameter and the <b>PKCS12_PROTECT_TO_DOMAIN_SIDS</b> flag of the <see cref="PFXExportCertStoreEx"/> function.<br/>
        ///       You can protect PFX passwords to an AD principal beginning in Windows 8 and Windows Server 2012.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a <see cref="CRYPT_DATA_BLOB"/> structure that represents the PKCS #12 packet.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_REG
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from a registry subkey.<br/>
        ///       This provider opens or creates the registry subkeys Certificates, CRLs, and CTLs under the key passed in <paramref name="Para"/>.
        ///       The input key is not closed by the provider.
        ///       Before returning, the provider opens its own copy of the key passed in <paramref name="Para"/>.
        ///       If <b>CERT_STORE_READONLY_FLAG</b> is set in the low word of dwFlags, registry subkeys are opened by using the <b>RegOpenKey</b> with <b>KEY_READ_ACCESS</b>.
        ///       Otherwise, registry subkeys are created by using <b>RegCreateKey</b> with <b>KEY_ALL_ACCESS</b>.
        ///       Any changes to the contents of the opened store are immediately persisted to the registry.
        ///       However, if <b>CERT_STORE_READONLY_FLAG</b> is set in the low word of dwFlags, any attempt to add to the contents of the store or to change a context's property results in an error with <see cref="LastErrorService.GetLastError"/> returning the <b>E_ACCESSDENIED</b> code.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter contains the handle of an open registry key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SERIALIZED<br/>
        ///       sz_CERT_STORE_PROV_SERIALIZED
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from a memory location that contains a serialized store.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a <see cref="CRYPT_DATA_BLOB"/> structure that contains the serialized memory BLOB.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SMART_CARD(_W)<br/>
        ///       sz_CERT_STORE_PROV_SMART_CARD(_W)
        ///     </td>
        ///     <td>
        ///       Not currently used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SYSTEM_A
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from the specified system store.<br/>
        ///       The system store is a logical, collection store that consists of one or more physical stores.
        ///       A physical store associated with a system store is registered with the <b>CertRegisterPhysicalStore</b> function.
        ///       After the system store is opened, all of the physical stores that are associated with it are also opened by calls to <b>CertOpenStore</b> and are added to the system store collection by using the <see cref="CertAddStoreToCollection"/> function.
        ///       The high word of <paramref name="Flags"/> indicates the system store location, usually set to <b>CERT_SYSTEM_STORE_CURRENT_USER</b>.
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a null-terminated ANSI string that contains a system store name, such as "My" or "Root".
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SYSTEM(_W)<br/>
        ///       sz_CERT_STORE_PROV_SYSTEM(_W)
        ///     </td>
        ///     <td>
        ///       Same as <b>CERT_STORE_PROV_SYSTEM_A</b>.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a null-terminated Unicode string that contains a system store name, such as "My" or "Root".
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SYSTEM_REGISTRY_A
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from a physical registry store.
        ///       The physical store is not opened as a collection store.
        ///       Enumerations and searches go through only the certificates, CRLs, and CTLs in that one physical store.<br/>
        ///       The high word of <paramref name="Flags"/> indicates the system store location, usually set to <b>CERT_SYSTEM_STORE_CURRENT_USER</b>.
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a null-terminated ANSI string that contains a system store name, such as "My" or "Root".
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SYSTEM_REGISTRY(_W)<br/>
        ///       sz_CERT_STORE_PROV_SYSTEM_REGISTRY(_W)
        ///     </td>
        ///     <td>
        ///       Same as <b>CERT_STORE_PROV_SYSTEM_REGISTRY_A</b>.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a null-terminated Unicode string that contains a system store name, such as "My" or "Root".
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="EncodingType">
        /// Specifies the certificate encoding type and message encoding type.
        /// Encoding is used only when the <b>dwSaveAs</b> parameter of the <see cref="CertSaveStore"/> function contains <b>CERT_STORE_SAVE_AS_PKCS7</b>.
        /// Otherwise, the <paramref name="EncodingType"/> parameter is not used.<br/>
        /// This parameter is only applicable when the <b>CERT_STORE_PROV_MSG</b>, <b>CERT_STORE_PROV_PKCS7</b>, or <b>CERT_STORE_PROV_FILENAME</b> provider type is specified in the <paramref name="StoreProvider"/> parameter.
        /// For all other provider types, this parameter is unused and should be set to zero.<br/>
        /// This parameter can be a combination of one or more of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PKCS_7_ASN_ENCODING
        ///     </td>
        ///     <td>
        ///       Specifies PKCS #7 message encoding.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       X509_ASN_ENCODING
        ///     </td>
        ///     <td>
        ///       Specifies X.509 certificate encoding.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Context">
        /// This parameter is not used and should be set to NULL.<br/>
        /// <b>Windows Server 2003 and Windows XP</b>: A handle to a cryptographic provider.
        /// Passing <b>NULL</b> for this parameter causes an appropriate, default provider to be used.
        /// Using the default provider is recommended.
        /// The default or specified cryptographic provider is used for all store functions that verify the signature of a subject certificate or CRL.
        /// This parameter's data type is <b>HCRYPTPROV</b>.
        /// </param>
        /// <param name="Flags">
        /// These values consist of high-word and low-word values combined by using a bitwise-<b>OR</b> operation.<br/>
        /// The low-word portion of dwFlags controls a variety of general characteristics of the certificate store opened.
        /// This portion can be used with all store provider types.
        /// The low-word portion of <paramref name="Flags"/> can be one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_BACKUP_RESTORE_FLAG
        ///     </td>
        ///     <td>
        ///       Use the thread's SE_BACKUP_NAME and SE_RESTORE_NAME privileges to open registry or file-based system stores.
        ///       If the thread does not have these privileges, this function must fail with an access denied error.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_CREATE_NEW_FLAG
        ///     </td>
        ///     <td>
        ///       A new store is created if one did not exist. The function fails if the store already exists.<br/>
        ///       If neither <b>CERT_STORE_OPEN_EXISTING_FLAG</b> nor <b>CERT_STORE_CREATE_NEW_FLAG</b> is set, a store is opened if it exists or is created and opened if it did not already exist.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG
        ///     </td>
        ///     <td>
        ///       Defer closing of the store's provider until all certificates, CRLs, or CTLs obtained from the store are no longer in use.
        ///       The store is actually closed when the last certificate, CRL, or CTL obtained from the store is freed.
        ///       Any changes made to properties of these certificates, CRLs, and CTLs, even after the call to <see cref="CertCloseStore"/>, are persisted.<br/>
        ///       If this flag is not set and certificates, CRLs, or CTLs obtained from the store are still in use, any changes to the properties of those certificates, CRLs, and CTLs will not be persisted.<br/>
        ///       If this function is called with <b>CERT_CLOSE_STORE_FORCE_FLAG</b>, <b>CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG</b> is ignored.<br/>
        ///       When this flag is set and a non-<b>NULL</b> <paramref name="Context"/> parameter value is passed, that provider will continue to be used even after the call to this function.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_DELETE_FLAG
        ///     </td>
        ///     <td>
        ///       The store is deleted instead of being opened.
        ///       This function returns <b>NULL</b> for both success and failure of the deletion.
        ///       To determine the success of the deletion, call <see cref="LastErrorService.GetLastError"/>, which returns zero if the store was deleted and a nonzero value if it was not deleted.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_ENUM_ARCHIVED_FLAG
        ///     </td>
        ///     <td>
        ///       Normally, an enumeration of all certificates in the store will ignore any certificate with the <b>CERT_ARCHIVED_PROP_ID</b> property set.
        ///       If this flag is set, an enumeration of the certificates in the store will contain all of the certificates in the store, including those that have the <b>CERT_ARCHIVED_PROP_ID</b> property.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_MAXIMUM_ALLOWED_FLAG
        ///     </td>
        ///     <td>
        ///       Open the store with the maximum set of allowed permissions.
        ///       If this flag is specified, registry stores are first opened with write access and if that fails, they are reopened with read-only access.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_NO_CRYPT_RELEASE_FLAG
        ///     </td>
        ///     <td>
        ///       This flag is not used when the <paramref name="Context"/> parameter is <b>NULL</b>.
        ///       This flag is only valid when a non-<b>NULL</b> CSP handle is passed as the <paramref name="Context"/> parameter.
        ///       Setting this flag prevents the automatic release of a nondefault CSP when the certificate store is closed.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_OPEN_EXISTING_FLAG
        ///     </td>
        ///     <td>
        ///       Only open an existing store. If the store does not exist, the function fails.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_READONLY_FLAG
        ///     </td>
        ///     <td>
        ///       Open the store in read-only mode.
        ///       Any attempt to change the contents of the store will result in an error.
        ///       When this flag is set and a registry based store provider is being used, the registry subkeys are opened by using <b>RegOpenKey</b> with <b>KEY_READ_ACCESS</b>.
        ///       Otherwise, the registry subkeys are created by using <b>RegCreateKey</b> with <b>KEY_ALL_ACCESS</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_SET_LOCALIZED_NAME_FLAG
        ///     </td>
        ///     <td>
        ///       If this flag is supported, the provider sets the store's <b>CERT_STORE_LOCALIZED_NAME_PROP_ID</b> property.
        ///       The localized name can be retrieved by calling the <see cref="CertGetStoreProperty"/> function with <b>dwPropID</b> set to <b>CERT_STORE_LOCALIZED_NAME_PROP_ID</b>.
        ///       This flag is supported for providers of types <b>CERT_STORE_PROV_FILENAME</b>, <b>CERT_STORE_PROV_SYSTEM</b>, <b>CERT_STORE_PROV_SYSTEM_REGISTRY</b>, and <b>CERT_STORE_PROV_PHYSICAL_W</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_SHARE_CONTEXT_FLAG
        ///     </td>
        ///     <td>
        ///       When opening a store multiple times, you can set this flag to ensure efficient memory usage by reusing the memory for the encoded parts of a certificate, CRL, or CTL context across the opened instances of the stores.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_UPDATE_KEYID_FLAG
        ///     </td>
        ///     <td>
        ///       Lists of key identifiers exist within <b>CurrentUser</b> and <b>LocalMachine</b>.
        ///       These key identifiers have properties much like the properties of certificates.
        ///       If the <b>CERT_STORE_UPDATE_KEYID_FLAG</b> is set, then for every key identifier in the store's location that has a <b>CERT_KEY_PROV_INFO_PROP_ID</b> property, that property is automatically updated from the key identifier property <b>CERT_KEY_PROV_INFO_PROP_ID</b> or the <b>CERT_KEY_IDENTIFIER_PROP_ID</b> of the certificate related to that key identifier.
        ///     </td>
        ///   </tr>
        /// </table>
        /// The <b>CERT_STORE_PROV_SYSTEM</b>, <b>CERT_STORE_PROV_SYSTEM_REGISTRY</b>, and <b>CERT_STORE_PROV_PHYSICAL</b> provider types use the following high words of <paramref name="Flags"/> to specify system store registry locations:
        /// <list type="bullet">
        ///   <item>CERT_SYSTEM_STORE_CURRENT_SERVICE</item>
        ///   <item>CERT_SYSTEM_STORE_CURRENT_USER</item>
        ///   <item>CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY</item>
        ///   <item>CERT_SYSTEM_STORE_LOCAL_MACHINE</item>
        ///   <item>CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE</item>
        ///   <item>CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY</item>
        ///   <item>CERT_SYSTEM_STORE_SERVICES</item>
        ///   <item>CERT_SYSTEM_STORE_USERS</item>
        /// </list>
        /// By default, a system store location is opened relative to the <b>HKEY_CURRENT_USER</b>, <b>HKEY_LOCAL_MACHINE</b>, or <b>HKEY_USERS</b> predefined registry key.<br/>
        /// The following high-word flags override this default behavior.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_SYSTEM_STORE_RELOCATE_FLAG
        ///     </td>
        ///     <td>
        ///       When set, <paramref name="Para"/> must contain a pointer to a <see cref="CERT_SYSTEM_STORE_RELOCATE_PARA"/> structure rather than a string.
        ///       The structure indicates both the name of the store and its location in the registry.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_SYSTEM_STORE_UNPROTECTED_FLAG
        ///     </td>
        ///     <td>
        ///       By default, when the CurrentUser "Root" store is opened, any SystemRegistry roots not on the protected root list are deleted from the cache before this function returns.
        ///       When this flag is set, this default is overridden and all of the roots in the SystemRegistry are returned and no check of the protected root list is made.
        ///     </td>
        ///   </tr>
        /// </table>
        /// The <b>CERT_STORE_PROV_REGISTRY</b> provider uses the following high-word flags.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_REGISTRY_STORE_REMOTE_FLAG
        ///     </td>
        ///     <td>
        ///       <paramref name="Para"/> contains a handle to a registry key on a remote computer.
        ///       To access a registry key on a remote computer, security permissions on the remote computer must be set to allow access.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_REGISTRY_STORE_SERIALIZED_FLAG
        ///     </td>
        ///     <td>
        ///       The <b>CERT_STORE_PROV_REG</b> provider saves certificates, CRLs, and CTLs in a single serialized store subkey instead of performing the default save operation.
        ///       The default is that each certificate, CRL, or CTL is saved as a separate registry subkey under the appropriate subkey.<br/>
        ///       This flag is mainly used for stores downloaded from the group policy template (GPT), such as the CurrentUserGroupPolicy and LocalMachineGroupPolicy stores.<br/>
        ///       When <b>CERT_REGISTRY_STORE_SERIALIZED_FLAG</b> is set, store additions, deletions, or property changes are not persisted until there is a call to either <see cref="CertCloseStore"/> or <see cref="CertControlStore"/> using <b>CERT_STORE_CTRL_COMMIT</b>.
        ///     </td>
        ///   </tr>
        /// </table>
        /// The <b>CERT_STORE_PROV_FILE</b> and <b>CERT_STORE_PROV_FILENAME</b> provider types use the following high-word flags.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FILE_STORE_COMMIT_ENABLE
        ///     </td>
        ///     <td>
        ///       Setting this flag commits any additions to the store or any changes made to properties of contexts in the store to the file store either when <see cref="CertCloseStore"/> is called or when <see cref="CertControlStore"/> is called with <b>CERT_STORE_CONTROL_COMMIT</b>.<br/>
        ///       <b>CertOpenStore</b> fails with <b>E_INVALIDARG</b> if both <b>CERT_FILE_STORE_COMMIT_ENABLE</b> and <b>CERT_STORE_READONLY_FLAG</b> are set in <paramref name="Flags"/>.
        ///     </td>
        ///   </tr>
        /// </table>
        /// The <b>CERT_STORE_PROV_LDAP</b> provider type uses the following high-word flags.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_LDAP_STORE_AREC_EXCLUSIVE_FLAG
        ///     </td>
        ///     <td>
        ///       Performs an A-Record-only DNS lookup on the URL named in the <paramref name="Para"/> parameter.
        ///       This prevents false DNS queries from being generated when resolving URL host names.
        ///       Use this flag when passing a host name as opposed to a domain name for the <paramref name="Para"/> parameter.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_LDAP_STORE_OPENED_FLAG
        ///     </td>
        ///     <td>
        ///       Use this flag to use an existing LDAP session.
        ///       When this flag is specified, the <paramref name="Para"/> parameter is the address of a <b>CERT_LDAP_STORE_OPENED_PARA</b> structure that contains information about the LDAP session to use.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_LDAP_STORE_SIGN_FLAG
        ///     </td>
        ///     <td>
        ///       To provide integrity required by some applications, digitally sign all LDAP traffic to and from an LDAP server by using the Kerberos authentication protocol.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_LDAP_STORE_UNBIND_FLAG
        ///     </td>
        ///     <td>
        ///       Use this flag with the <b>CERT_LDAP_STORE_OPENED_FLAG</b> flag to cause the LDAP session to be unbound when the store is closed.
        ///       The system will unbind the LDAP session by using the <a href="https://learn.microsoft.com/en-us/previous-versions/windows/desktop/api/winldap/nf-winldap-ldap_unbind">ldap_unbind</a> function when the store is closed.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Para">
        /// A 32-bit value that can contain additional information for this function.
        /// The contents of this parameter depends on the value of the <paramref name="StoreProvider"/> and other parameters.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns a handle to the certificate store.
        /// When you have finished using the store, release the handle by calling the <see cref="CertCloseStore"/> function.<br/>
        /// If the function fails, it returns <b>NULL</b>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.
        /// <div class="note">
        ///   <b>Note</b>:
        ///   <b>CreateFile</b>, <b>ReadFile</b>, or registry errors might be propagated and their error codes returned.
        ///   <b>CertOpenStore</b> has a single error code of its own, the <b>ERROR_FILE_NOT_FOUND</b> code, which indicates that the function was unable to find the provider specified by the <paramref name="StoreProvider"/> parameter.
        /// </div>
        /// </returns>
        IntPtr CertOpenStore(IntPtr StoreProvider,Int32 EncodingType,IntPtr Context,Int32 Flags,IntPtr Para);
        #endregion
        #region M:CertOpenStore(IntPtr,Int32,IntPtr,Int32,String):IntPtr
        /// <summary>
        /// This function opens a certificate store by using a specified store provider type. While this function can open a certificate store for most purposes, <see cref="CertOpenSystemStore"/> is recommended to open the most common certificate stores.
        /// <b>CertOpenStore</b> is required for more complex options and special cases.
        /// </summary>
        /// <param name="StoreProvider">
        /// A pointer to a null-terminated ANSI string that contains the store provider type.<br/>
        /// The following values represent the predefined store types.
        /// The store provider type determines the contents of the <paramref name="Para"/> parameter and the use and meaning of the high word of the <paramref name="Flags"/> parameter.
        /// Additional store providers can be installed or registered by using the <see cref="CryptInstallOIDFunctionAddress"/> or <see cref="CryptRegisterOIDFunction"/> function.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_COLLECTION<br/>
        ///       sz_CERT_STORE_PROV_COLLECTION
        ///     </td>
        ///     <td>
        ///       Opens a store that will be a collection of other stores.
        ///       Stores are added to or removed from the collection by using <see cref="CertAddStoreToCollection"/> and <see cref="CertRemoveStoreFromCollection"/>.
        ///       When a store is added to a collection, all certificates, CRLs, and CTLs in that store become available to searches or enumerations of the collection store.<br/>
        ///       The high word of <paramref name="Flags"/> is set to zero.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter must be <b>NULL</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_FILE
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs read from a specified open file.
        ///       This provider expects the file to contain only a serialized store and not either PKCS #7 signed messages or a single encoded certificate.<br/>
        ///       The file pointer must be positioned at the beginning of the serialized store information.
        ///       After the data in the serialized store has been loaded into the certificate store, the file pointer is positioned at the beginning of any data that can follow the serialized store data in the file.
        ///       If <b>CERT_FILE_STORE_COMMIT_ENABLE</b> is set in <paramref name="Flags"/>, the file handle is duplicated and the store is always committed as a serialized store.
        ///       The file is not closed when the store is closed.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter must contain a pointer to the handle of a file opened by using <a href="https://learn.microsoft.com/en-us/windows/desktop/api/fileapi/nf-fileapi-createfilea">CreateFile</a>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_FILENAME_A
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from a file.
        ///       The provider opens the file and first attempts to read the file as a serialized store, then as a PKCS #7 signed message, and finally as a single encoded certificate.<br/>
        ///       The <paramref name="EncodingType"/> parameter must contain the encoding types to be used with both messages and certificates.
        ///       If the file contains an X.509 encoded certificate, the open operation fails and a call to the <see cref="LastErrorService.GetLastError"/> function will return <b>ERROR_ACCESS_DENIED</b>.
        ///       If the <b>CERT_FILE_STORE_COMMIT_ENABLE</b> flag is set in <paramref name="Flags"/>, the <b>dwCreationDisposition</b> value passed to <b>CreateFile</b> is as follows:
        ///       <list type="bullet">
        ///         <item>If the <b>CERT_STORE_CREATE_NEW_FLAG</b> flag is set, <b>CreateFile</b> uses <b>CREATE_NEW</b>.</item>
        ///         <item>If the <b>CERT_STORE_OPEN_EXISTING_FLAG</b> flag is set, <b>CreateFile</b> uses <b>OPEN_EXISTING</b>.</item>
        ///         <item>For all other settings of <paramref name="Flags"/>, <b>CreateFile</b> uses <b>OPEN_ALWAYS</b>.</item>
        ///       </list>
        ///       If <paramref name="Flags"/> includes <b>CERT_FILE_STORE_COMMIT_ENABLE</b>, the file is committed as either a PKCS #7 or a serialized store depending on the file type opened.
        ///       If the file was empty or if the file name has either a .p7c or .spc extension, the file is committed as a PKCS #7.
        ///       Otherwise, the file is committed as a serialized store.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter must contain a pointer to null-terminated ANSI string that contains the name of an existing, unopened file.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_FILENAME(_W)<br/>
        ///       sz_CERT_STORE_PROV_FILENAME(_W)
        ///     </td>
        ///     <td>
        ///       Same as <b>CERT_STORE_PROV_FILENAME_A</b>.
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter must contain a pointer to null-terminated Unicode string that contains the name of an existing, unopened file.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_LDAP(_W)<br/>
        ///       sz_CERT_STORE_PROV_LDAP(_W)
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from the results of an LDAP query.<br/>
        ///       To perform write operations on the store, the query string must specify a BASE query with no filter and a single attribute.<br/>
        ///       <b><paramref name="Para"/> value</b>: If the <paramref name="Flags"/> parameter contains <b>CERT_LDAP_STORE_OPENED_FLAG</b>, set <paramref name="Para"/> to the address of a <b>CERT_LDAP_STORE_OPENED_PARA</b> structure that specifies the established LDAP session to use.<br/>
        ///       Otherwise, set <paramref name="Para"/> to point to a null-terminated Unicode string that contains the LDAP query string.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_MEMORY<br/>
        ///       sz_CERT_STORE_PROV_MEMORY
        ///     </td>
        ///     <td>
        ///       Creates a certificate store in cached memory.
        ///       No certificates, certificate revocation lists (CRLs), or certificate trust lists (CTLs) are initially loaded into the store.
        ///       Typically used to create a temporary store.<br/>
        ///       Any addition of certificates, CRLs, or CTLs or changes in properties of certificates, CRLs, or CTLs in a memory store are not automatically saved.
        ///       They can be saved to a file or to a memory BLOB by using <see cref="CertSaveStore"/>.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_MSG
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from the specified cryptographic message.
        ///       The <paramref name="EncodingType"/> parameter must contain the encoding types used with both messages and certificates.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter contains an <b>HCRYPTMSG</b> handle of the encoded message, returned by a call to <see cref="CryptMsgOpenToDecode"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_PHYSICAL(_W)<br/>
        ///       sz_CERT_STORE_PROV_PHYSICAL(_W)
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from a specified physical store that is a member of a logical system store.<br/>
        ///       Two names are separated with an intervening backslash (\), for example "Root.Default".
        ///       Here, "Root" is the name of the system store and ".Default" is the name of the physical store.
        ///       The system and physical store names cannot contain any backslashes.
        ///       The high word of dwFlags indicates the system store location, usually <b>CERT_SYSTEM_STORE_CURRENT_USER</b>.
        ///       Some physical store locations can be opened remotely.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a null-terminated Unicode string that contains both the system store name and physical names.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_PKCS7<br/>
        ///       sz_CERT_STORE_PROV_PKCS7
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from an encoded PKCS #7 signed message.
        ///       The <paramref name="EncodingType"/> parameter must specify the encoding types to be used with both messages and certificates.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a <see cref="CRYPT_DATA_BLOB"/> structure that represents the encoded message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_PKCS12<br/>
        ///       sz_CERT_STORE_PROV_PKCS12
        ///     </td>
        ///     <td>
        ///       Initializes the store with the contents of a PKCS #12 packet.<br/>
        ///       If the PKCS #12 packet is protected with a NULL or empty password, this function will succeed in opening the store.<br/>
        ///       Beginning with Windows 8 and Windows Server 2012, if the password embedded in the PFX packet was protected to an Active Directory (AD) principal and the current user, as a member of that principal, has permission to decrypt the password, this function will succeed in opening the store.
        ///       For more information, see the <paramref name="Para"/> parameter and the <b>PKCS12_PROTECT_TO_DOMAIN_SIDS</b> flag of the <see cref="PFXExportCertStoreEx"/> function.<br/>
        ///       You can protect PFX passwords to an AD principal beginning in Windows 8 and Windows Server 2012.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a <see cref="CRYPT_DATA_BLOB"/> structure that represents the PKCS #12 packet.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_REG
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from a registry subkey.<br/>
        ///       This provider opens or creates the registry subkeys Certificates, CRLs, and CTLs under the key passed in <paramref name="Para"/>.
        ///       The input key is not closed by the provider.
        ///       Before returning, the provider opens its own copy of the key passed in <paramref name="Para"/>.
        ///       If <b>CERT_STORE_READONLY_FLAG</b> is set in the low word of dwFlags, registry subkeys are opened by using the <b>RegOpenKey</b> with <b>KEY_READ_ACCESS</b>.
        ///       Otherwise, registry subkeys are created by using <b>RegCreateKey</b> with <b>KEY_ALL_ACCESS</b>.
        ///       Any changes to the contents of the opened store are immediately persisted to the registry.
        ///       However, if <b>CERT_STORE_READONLY_FLAG</b> is set in the low word of dwFlags, any attempt to add to the contents of the store or to change a context's property results in an error with <see cref="LastErrorService.GetLastError"/> returning the <b>E_ACCESSDENIED</b> code.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter contains the handle of an open registry key.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SERIALIZED<br/>
        ///       sz_CERT_STORE_PROV_SERIALIZED
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from a memory location that contains a serialized store.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a <see cref="CRYPT_DATA_BLOB"/> structure that contains the serialized memory BLOB.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SMART_CARD(_W)<br/>
        ///       sz_CERT_STORE_PROV_SMART_CARD(_W)
        ///     </td>
        ///     <td>
        ///       Not currently used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SYSTEM_A
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from the specified system store.<br/>
        ///       The system store is a logical, collection store that consists of one or more physical stores.
        ///       A physical store associated with a system store is registered with the <b>CertRegisterPhysicalStore</b> function.
        ///       After the system store is opened, all of the physical stores that are associated with it are also opened by calls to <b>CertOpenStore</b> and are added to the system store collection by using the <see cref="CertAddStoreToCollection"/> function.
        ///       The high word of <paramref name="Flags"/> indicates the system store location, usually set to <b>CERT_SYSTEM_STORE_CURRENT_USER</b>.
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a null-terminated ANSI string that contains a system store name, such as "My" or "Root".
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SYSTEM(_W)<br/>
        ///       sz_CERT_STORE_PROV_SYSTEM(_W)
        ///     </td>
        ///     <td>
        ///       Same as <b>CERT_STORE_PROV_SYSTEM_A</b>.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a null-terminated Unicode string that contains a system store name, such as "My" or "Root".
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SYSTEM_REGISTRY_A
        ///     </td>
        ///     <td>
        ///       Initializes the store with certificates, CRLs, and CTLs from a physical registry store.
        ///       The physical store is not opened as a collection store.
        ///       Enumerations and searches go through only the certificates, CRLs, and CTLs in that one physical store.<br/>
        ///       The high word of <paramref name="Flags"/> indicates the system store location, usually set to <b>CERT_SYSTEM_STORE_CURRENT_USER</b>.
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a null-terminated ANSI string that contains a system store name, such as "My" or "Root".
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_PROV_SYSTEM_REGISTRY(_W)<br/>
        ///       sz_CERT_STORE_PROV_SYSTEM_REGISTRY(_W)
        ///     </td>
        ///     <td>
        ///       Same as <b>CERT_STORE_PROV_SYSTEM_REGISTRY_A</b>.<br/>
        ///       <b><paramref name="Para"/> value</b>: The <paramref name="Para"/> parameter points to a null-terminated Unicode string that contains a system store name, such as "My" or "Root".
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="EncodingType">
        /// Specifies the certificate encoding type and message encoding type.
        /// Encoding is used only when the <b>dwSaveAs</b> parameter of the <see cref="CertSaveStore"/> function contains <b>CERT_STORE_SAVE_AS_PKCS7</b>.
        /// Otherwise, the <paramref name="EncodingType"/> parameter is not used.<br/>
        /// This parameter is only applicable when the <b>CERT_STORE_PROV_MSG</b>, <b>CERT_STORE_PROV_PKCS7</b>, or <b>CERT_STORE_PROV_FILENAME</b> provider type is specified in the <paramref name="StoreProvider"/> parameter.
        /// For all other provider types, this parameter is unused and should be set to zero.<br/>
        /// This parameter can be a combination of one or more of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       PKCS_7_ASN_ENCODING
        ///     </td>
        ///     <td>
        ///       Specifies PKCS #7 message encoding.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       X509_ASN_ENCODING
        ///     </td>
        ///     <td>
        ///       Specifies X.509 certificate encoding.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Context">
        /// This parameter is not used and should be set to NULL.<br/>
        /// <b>Windows Server 2003 and Windows XP</b>: A handle to a cryptographic provider.
        /// Passing <b>NULL</b> for this parameter causes an appropriate, default provider to be used.
        /// Using the default provider is recommended.
        /// The default or specified cryptographic provider is used for all store functions that verify the signature of a subject certificate or CRL.
        /// This parameter's data type is <b>HCRYPTPROV</b>.
        /// </param>
        /// <param name="Flags">
        /// These values consist of high-word and low-word values combined by using a bitwise-<b>OR</b> operation.<br/>
        /// The low-word portion of dwFlags controls a variety of general characteristics of the certificate store opened.
        /// This portion can be used with all store provider types.
        /// The low-word portion of <paramref name="Flags"/> can be one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_BACKUP_RESTORE_FLAG
        ///     </td>
        ///     <td>
        ///       Use the thread's SE_BACKUP_NAME and SE_RESTORE_NAME privileges to open registry or file-based system stores.
        ///       If the thread does not have these privileges, this function must fail with an access denied error.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_CREATE_NEW_FLAG
        ///     </td>
        ///     <td>
        ///       A new store is created if one did not exist. The function fails if the store already exists.<br/>
        ///       If neither <b>CERT_STORE_OPEN_EXISTING_FLAG</b> nor <b>CERT_STORE_CREATE_NEW_FLAG</b> is set, a store is opened if it exists or is created and opened if it did not already exist.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG
        ///     </td>
        ///     <td>
        ///       Defer closing of the store's provider until all certificates, CRLs, or CTLs obtained from the store are no longer in use.
        ///       The store is actually closed when the last certificate, CRL, or CTL obtained from the store is freed.
        ///       Any changes made to properties of these certificates, CRLs, and CTLs, even after the call to <see cref="CertCloseStore"/>, are persisted.<br/>
        ///       If this flag is not set and certificates, CRLs, or CTLs obtained from the store are still in use, any changes to the properties of those certificates, CRLs, and CTLs will not be persisted.<br/>
        ///       If this function is called with <b>CERT_CLOSE_STORE_FORCE_FLAG</b>, <b>CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG</b> is ignored.<br/>
        ///       When this flag is set and a non-<b>NULL</b> <paramref name="Context"/> parameter value is passed, that provider will continue to be used even after the call to this function.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_DELETE_FLAG
        ///     </td>
        ///     <td>
        ///       The store is deleted instead of being opened.
        ///       This function returns <b>NULL</b> for both success and failure of the deletion.
        ///       To determine the success of the deletion, call <see cref="LastErrorService.GetLastError"/>, which returns zero if the store was deleted and a nonzero value if it was not deleted.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_ENUM_ARCHIVED_FLAG
        ///     </td>
        ///     <td>
        ///       Normally, an enumeration of all certificates in the store will ignore any certificate with the <b>CERT_ARCHIVED_PROP_ID</b> property set.
        ///       If this flag is set, an enumeration of the certificates in the store will contain all of the certificates in the store, including those that have the <b>CERT_ARCHIVED_PROP_ID</b> property.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_MAXIMUM_ALLOWED_FLAG
        ///     </td>
        ///     <td>
        ///       Open the store with the maximum set of allowed permissions.
        ///       If this flag is specified, registry stores are first opened with write access and if that fails, they are reopened with read-only access.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_NO_CRYPT_RELEASE_FLAG
        ///     </td>
        ///     <td>
        ///       This flag is not used when the <paramref name="Context"/> parameter is <b>NULL</b>.
        ///       This flag is only valid when a non-<b>NULL</b> CSP handle is passed as the <paramref name="Context"/> parameter.
        ///       Setting this flag prevents the automatic release of a nondefault CSP when the certificate store is closed.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_OPEN_EXISTING_FLAG
        ///     </td>
        ///     <td>
        ///       Only open an existing store. If the store does not exist, the function fails.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_READONLY_FLAG
        ///     </td>
        ///     <td>
        ///       Open the store in read-only mode.
        ///       Any attempt to change the contents of the store will result in an error.
        ///       When this flag is set and a registry based store provider is being used, the registry subkeys are opened by using <b>RegOpenKey</b> with <b>KEY_READ_ACCESS</b>.
        ///       Otherwise, the registry subkeys are created by using <b>RegCreateKey</b> with <b>KEY_ALL_ACCESS</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_SET_LOCALIZED_NAME_FLAG
        ///     </td>
        ///     <td>
        ///       If this flag is supported, the provider sets the store's <b>CERT_STORE_LOCALIZED_NAME_PROP_ID</b> property.
        ///       The localized name can be retrieved by calling the <see cref="CertGetStoreProperty"/> function with <b>dwPropID</b> set to <b>CERT_STORE_LOCALIZED_NAME_PROP_ID</b>.
        ///       This flag is supported for providers of types <b>CERT_STORE_PROV_FILENAME</b>, <b>CERT_STORE_PROV_SYSTEM</b>, <b>CERT_STORE_PROV_SYSTEM_REGISTRY</b>, and <b>CERT_STORE_PROV_PHYSICAL_W</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_SHARE_CONTEXT_FLAG
        ///     </td>
        ///     <td>
        ///       When opening a store multiple times, you can set this flag to ensure efficient memory usage by reusing the memory for the encoded parts of a certificate, CRL, or CTL context across the opened instances of the stores.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_STORE_UPDATE_KEYID_FLAG
        ///     </td>
        ///     <td>
        ///       Lists of key identifiers exist within <b>CurrentUser</b> and <b>LocalMachine</b>.
        ///       These key identifiers have properties much like the properties of certificates.
        ///       If the <b>CERT_STORE_UPDATE_KEYID_FLAG</b> is set, then for every key identifier in the store's location that has a <b>CERT_KEY_PROV_INFO_PROP_ID</b> property, that property is automatically updated from the key identifier property <b>CERT_KEY_PROV_INFO_PROP_ID</b> or the <b>CERT_KEY_IDENTIFIER_PROP_ID</b> of the certificate related to that key identifier.
        ///     </td>
        ///   </tr>
        /// </table>
        /// The <b>CERT_STORE_PROV_SYSTEM</b>, <b>CERT_STORE_PROV_SYSTEM_REGISTRY</b>, and <b>CERT_STORE_PROV_PHYSICAL</b> provider types use the following high words of <paramref name="Flags"/> to specify system store registry locations:
        /// <list type="bullet">
        ///   <item>CERT_SYSTEM_STORE_CURRENT_SERVICE</item>
        ///   <item>CERT_SYSTEM_STORE_CURRENT_USER</item>
        ///   <item>CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY</item>
        ///   <item>CERT_SYSTEM_STORE_LOCAL_MACHINE</item>
        ///   <item>CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE</item>
        ///   <item>CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY</item>
        ///   <item>CERT_SYSTEM_STORE_SERVICES</item>
        ///   <item>CERT_SYSTEM_STORE_USERS</item>
        /// </list>
        /// By default, a system store location is opened relative to the <b>HKEY_CURRENT_USER</b>, <b>HKEY_LOCAL_MACHINE</b>, or <b>HKEY_USERS</b> predefined registry key.<br/>
        /// The following high-word flags override this default behavior.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_SYSTEM_STORE_RELOCATE_FLAG
        ///     </td>
        ///     <td>
        ///       When set, <paramref name="Para"/> must contain a pointer to a <see cref="CERT_SYSTEM_STORE_RELOCATE_PARA"/> structure rather than a string.
        ///       The structure indicates both the name of the store and its location in the registry.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_SYSTEM_STORE_UNPROTECTED_FLAG
        ///     </td>
        ///     <td>
        ///       By default, when the CurrentUser "Root" store is opened, any SystemRegistry roots not on the protected root list are deleted from the cache before this function returns.
        ///       When this flag is set, this default is overridden and all of the roots in the SystemRegistry are returned and no check of the protected root list is made.
        ///     </td>
        ///   </tr>
        /// </table>
        /// The <b>CERT_STORE_PROV_REGISTRY</b> provider uses the following high-word flags.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_REGISTRY_STORE_REMOTE_FLAG
        ///     </td>
        ///     <td>
        ///       <paramref name="Para"/> contains a handle to a registry key on a remote computer.
        ///       To access a registry key on a remote computer, security permissions on the remote computer must be set to allow access.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_REGISTRY_STORE_SERIALIZED_FLAG
        ///     </td>
        ///     <td>
        ///       The <b>CERT_STORE_PROV_REG</b> provider saves certificates, CRLs, and CTLs in a single serialized store subkey instead of performing the default save operation.
        ///       The default is that each certificate, CRL, or CTL is saved as a separate registry subkey under the appropriate subkey.<br/>
        ///       This flag is mainly used for stores downloaded from the group policy template (GPT), such as the CurrentUserGroupPolicy and LocalMachineGroupPolicy stores.<br/>
        ///       When <b>CERT_REGISTRY_STORE_SERIALIZED_FLAG</b> is set, store additions, deletions, or property changes are not persisted until there is a call to either <see cref="CertCloseStore"/> or <see cref="CertControlStore"/> using <b>CERT_STORE_CTRL_COMMIT</b>.
        ///     </td>
        ///   </tr>
        /// </table>
        /// The <b>CERT_STORE_PROV_FILE</b> and <b>CERT_STORE_PROV_FILENAME</b> provider types use the following high-word flags.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_FILE_STORE_COMMIT_ENABLE
        ///     </td>
        ///     <td>
        ///       Setting this flag commits any additions to the store or any changes made to properties of contexts in the store to the file store either when <see cref="CertCloseStore"/> is called or when <see cref="CertControlStore"/> is called with <b>CERT_STORE_CONTROL_COMMIT</b>.<br/>
        ///       <b>CertOpenStore</b> fails with <b>E_INVALIDARG</b> if both <b>CERT_FILE_STORE_COMMIT_ENABLE</b> and <b>CERT_STORE_READONLY_FLAG</b> are set in <paramref name="Flags"/>.
        ///     </td>
        ///   </tr>
        /// </table>
        /// The <b>CERT_STORE_PROV_LDAP</b> provider type uses the following high-word flags.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_LDAP_STORE_AREC_EXCLUSIVE_FLAG
        ///     </td>
        ///     <td>
        ///       Performs an A-Record-only DNS lookup on the URL named in the <paramref name="Para"/> parameter.
        ///       This prevents false DNS queries from being generated when resolving URL host names.
        ///       Use this flag when passing a host name as opposed to a domain name for the <paramref name="Para"/> parameter.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_LDAP_STORE_OPENED_FLAG
        ///     </td>
        ///     <td>
        ///       Use this flag to use an existing LDAP session.
        ///       When this flag is specified, the <paramref name="Para"/> parameter is the address of a <b>CERT_LDAP_STORE_OPENED_PARA</b> structure that contains information about the LDAP session to use.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_LDAP_STORE_SIGN_FLAG
        ///     </td>
        ///     <td>
        ///       To provide integrity required by some applications, digitally sign all LDAP traffic to and from an LDAP server by using the Kerberos authentication protocol.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_LDAP_STORE_UNBIND_FLAG
        ///     </td>
        ///     <td>
        ///       Use this flag with the <b>CERT_LDAP_STORE_OPENED_FLAG</b> flag to cause the LDAP session to be unbound when the store is closed.
        ///       The system will unbind the LDAP session by using the <a href="https://learn.microsoft.com/en-us/previous-versions/windows/desktop/api/winldap/nf-winldap-ldap_unbind">ldap_unbind</a> function when the store is closed.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Para">
        /// A 32-bit value that can contain additional information for this function.
        /// The contents of this parameter depends on the value of the <paramref name="StoreProvider"/> and other parameters.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns a handle to the certificate store.
        /// When you have finished using the store, release the handle by calling the <see cref="CertCloseStore"/> function.<br/>
        /// If the function fails, it returns <b>NULL</b>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.
        /// <div class="note">
        ///   <b>Note</b>:
        ///   <b>CreateFile</b>, <b>ReadFile</b>, or registry errors might be propagated and their error codes returned.
        ///   <b>CertOpenStore</b> has a single error code of its own, the <b>ERROR_FILE_NOT_FOUND</b> code, which indicates that the function was unable to find the provider specified by the <paramref name="StoreProvider"/> parameter.
        /// </div>
        /// </returns>
        IntPtr CertOpenStore(IntPtr StoreProvider,Int32 EncodingType,IntPtr Context,Int32 Flags,String Para);
        #endregion
        #region M:CryptMsgOpenToDecode(CRYPT_OPEN_MESSAGE_FLAGS,CMSG_TYPE,IntPtr,IntPtr,IntPtr):IntPtr
        /// <summary>
        /// This function opens a cryptographic message for decoding and returns a handle of the opened message.
        /// The message remains open until the <see cref="CryptMsgClose"/> function is called.
        /// </summary>
        /// <param name="Flags">
        /// This parameter can be one of the following flags.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_DETACHED_FLAG
        ///     </td>
        ///     <td>
        ///       Indicates that the message to be decoded is detached.
        ///       If this flag is not set, the message is not detached.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CRYPT_RELEASE_CONTEXT_FLAG
        ///     </td>
        ///     <td>
        ///       If set, the <paramref name="Context"/> passed to this function is released on the final <see cref="CryptMsgUpdate"/>.
        ///       The handle is not released if the function fails.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Type">
        /// Specifies the type of message to decode.
        /// In most cases, the message type is determined from the message header and zero is passed for this parameter.
        /// In some cases, notably with Internet Explorer 3.0, messages do not have headers and the type of message to be decoded must be supplied in this function call.
        /// If the header is missing and zero is passed for this parameter, the function fails.<br/><br/>
        /// This parameter can be one of the following predefined message types.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_DATA
        ///     </td>
        ///     <td>
        ///       The message is encoded data.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_ENVELOPED
        ///     </td>
        ///     <td>
        ///       The message is an enveloped message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_HASHED
        ///     </td>
        ///     <td>
        ///       The message is a hashed message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNED
        ///     </td>
        ///     <td>
        ///       The message is a signed message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNED_AND_ENVELOPED
        ///     </td>
        ///     <td>
        ///       The message is a signed and enveloped message.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Context">
        /// This parameter is not used and should be set to <b>NULL</b>.<br/><br/>
        /// <b>Windows Server 2003 and Windows XP</b>: Specifies a handle for the cryptographic provider to use for hashing the message.
        /// For signed messages, <paramref name="Context"/> is used for signature verification.
        /// This parameter's data type is <b>HCRYPTPROV</b>.<br/><br/>
        /// Unless there is a strong reason for passing in a specific cryptographic provider in <paramref name="Context"/>, set this parameter to <b>NULL</b>.
        /// Passing in <b>NULL</b> causes the default RSA or DSS provider to be acquired before performing hash, signature verification, or recipient encryption operations.
        /// </param>
        /// <param name="StreamInfo">
        /// When streaming is not being used, this parameter must be set to <b>NULL</b>.<br/><br/>
        /// <div class="note">
        ///   <b>Note</b>:
        ///   Streaming is not used with <b>CMSG_HASHED</b> messages.
        ///   When dealing with hashed data, this parameter must be set to <b>NULL</b>.
        /// </div>
        /// When streaming is being used, the <paramref name="StreamInfo"/> parameter is a pointer to a <b>CMSG_STREAM_INFO</b> structure that contains a pointer to a callback to be called when <see cref="CryptMsgUpdate"/> is executed or when <see cref="CryptMsgControl"/> is executed when decoding a streamed enveloped message.<br/><br/>
        /// For a signed message, the callback is passed a block of the decoded bytes from the inner content of the message.<br/><br/>
        /// For an enveloped message, after each call to <see cref="CryptMsgUpdate"/>, you must check to determine whether the <b>CMSG_ENVELOPE_ALGORITHM_PARAM</b> property is available by calling the <see cref="CryptMsgGetParam"/> function.
        /// <see cref="CryptMsgGetParam"/> will fail and <see cref="LastErrorService.GetLastError"/> will return <b>CRYPT_E_STREAM_MSG_NOT_READY</b> until <see cref="CryptMsgUpdate"/> has processed enough of the message to make the <b>CMSG_ENVELOPE_ALGORITHM_PARAM</b> property available.
        /// When the <b>CMSG_ENVELOPE_ALGORITHM_PARAM</b> property is available, you can iterate through the recipients, retrieving a <see cref="CERT_INFO"/> structure for each recipient by using the <see cref="CryptMsgGetParam"/> function to retrieve the <b>CMSG_RECIPIENT_INFO_PARAM</b> property.
        /// To prevent a denial of service attack from an enveloped message that has an artificially large header block, you must keep track of the amount of memory that has been passed to the <see cref="CryptMsgUpdate"/> function during this process.
        /// If the amount of data exceeds an application defined limit before the <b>CMSG_ENVELOPE_ALGORITHM_PARAM</b> property is available, you must stop processing the message and call the <see cref="CryptMsgClose"/> function to cause the operating system to release any memory that has been allocated for the message.
        /// A suggested limit is the maximum allowable size of a message.
        /// For example, if the maximum message size is 10 MB, the limit for this test should be 10 MB.<br/><br/>
        /// The <see cref="CERT_INFO"/> structure is used to find a matching certificate in a previously opened certificate store by using the <see cref="CertGetSubjectCertificateFromStore"/> function.
        /// When the correct certificate is found, the <see cref="CertGetCertificateContextProperty"/> function with a <b>CERT_KEY_PROV_INFO_PROP_ID</b> parameter is called to retrieve a <see cref="CRYPT_KEY_PROV_INFO"/> structure.
        /// The structure contains the information necessary to acquire the recipient's private key by calling <see cref="CryptAcquireContext"/>, using the <b>pwszContainerName</b>, <b>pwszProvName</b>, <b>dwProvType</b>, and dwFlags members of the <see cref="CRYPT_KEY_PROV_INFO"/> structure. The <paramref name="Context"/> acquired and the <b>dwKeySpec</b> member of the <see cref="CRYPT_KEY_PROV_INFO"/> structure are passed to the <see cref="CryptMsgControl"/> structure as a member of the <see cref="CMSG_CTRL_DECRYPT_PARA"/> structure to permit the start of the decryption of the inner content.
        /// The streaming code will then perform the decryption as the data is input.
        /// The resulting blocks of plaintext are passed to the callback function specified by the <b>pfnStreamOutput</b> member of the <see cref="CMSG_STREAM_INFO"/> structure to handle the output of the decrypted message.
        /// <div class="note">
        ///   <b>Note</b>:
        ///   Streamed decoding of an enveloped message queues the <b>ciphertext</b> in memory until <see cref="CryptMsgControl"/> is called to start the decryption.
        ///   The application must initiate decrypting in a timely manner so that the data can be saved to disk or routed elsewhere before the accumulated <i>ciphertext</i> becomes too large and the system runs out of memory.
        /// </div>
        /// In the case of a signed message enclosed in an enveloped message, the <a href="https://learn.microsoft.com/en-us/windows/desktop/SecGloss/p-gly">plaintext</a> output from the streaming decode of the enveloped message can be fed into another streaming decode to process the signed message.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns the handle of the opened message.<br/><br/>
        /// If the function fails, it returns <b>NULL</b>.
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/><br/>
        /// The following table lists the error codes most commonly returned by the <see cref="LastErrorService.GetLastError"/> function.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Return code
        ///     </th>
        ///     <th>
        ///       Description
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       One or more arguments are not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_OUTOFMEMORY
        ///     </td>
        ///     <td>
        ///       A memory allocation failure occurred.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        IntPtr CryptMsgOpenToDecode(CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,IntPtr Context,IntPtr StreamInfo);
        #endregion
        #region M:CryptMsgOpenToDecode(CRYPT_OPEN_MESSAGE_FLAGS,CMSG_TYPE,IntPtr,{ref}CMSG_STREAM_INFO):IntPtr
        /// <summary>
        /// This function opens a cryptographic message for decoding and returns a handle of the opened message.
        /// The message remains open until the <see cref="CryptMsgClose"/> function is called.
        /// </summary>
        /// <param name="Flags">
        /// This parameter can be one of the following flags.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_DETACHED_FLAG
        ///     </td>
        ///     <td>
        ///       Indicates that the message to be decoded is detached.
        ///       If this flag is not set, the message is not detached.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CRYPT_RELEASE_CONTEXT_FLAG
        ///     </td>
        ///     <td>
        ///       If set, the <paramref name="Context"/> passed to this function is released on the final <see cref="CryptMsgUpdate"/>.
        ///       The handle is not released if the function fails.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Type">
        /// Specifies the type of message to decode.
        /// In most cases, the message type is determined from the message header and zero is passed for this parameter.
        /// In some cases, notably with Internet Explorer 3.0, messages do not have headers and the type of message to be decoded must be supplied in this function call.
        /// If the header is missing and zero is passed for this parameter, the function fails.<br/><br/>
        /// This parameter can be one of the following predefined message types.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_DATA
        ///     </td>
        ///     <td>
        ///       The message is encoded data.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_ENVELOPED
        ///     </td>
        ///     <td>
        ///       The message is an enveloped message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_HASHED
        ///     </td>
        ///     <td>
        ///       The message is a hashed message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNED
        ///     </td>
        ///     <td>
        ///       The message is a signed message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNED_AND_ENVELOPED
        ///     </td>
        ///     <td>
        ///       The message is a signed and enveloped message.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Context">
        /// This parameter is not used and should be set to <b>NULL</b>.<br/><br/>
        /// <b>Windows Server 2003 and Windows XP</b>: Specifies a handle for the cryptographic provider to use for hashing the message.
        /// For signed messages, <paramref name="Context"/> is used for signature verification.
        /// This parameter's data type is <b>HCRYPTPROV</b>.<br/><br/>
        /// Unless there is a strong reason for passing in a specific cryptographic provider in <paramref name="Context"/>, set this parameter to <b>NULL</b>.
        /// Passing in <b>NULL</b> causes the default RSA or DSS provider to be acquired before performing hash, signature verification, or recipient encryption operations.
        /// </param>
        /// <param name="StreamInfo">
        /// When streaming is not being used, this parameter must be set to <b>NULL</b>.<br/><br/>
        /// <div class="note">
        ///   <b>Note</b>:
        ///   Streaming is not used with <b>CMSG_HASHED</b> messages.
        ///   When dealing with hashed data, this parameter must be set to <b>NULL</b>.
        /// </div>
        /// When streaming is being used, the <paramref name="StreamInfo"/> parameter is a pointer to a <b>CMSG_STREAM_INFO</b> structure that contains a pointer to a callback to be called when <see cref="CryptMsgUpdate"/> is executed or when <see cref="CryptMsgControl"/> is executed when decoding a streamed enveloped message.<br/><br/>
        /// For a signed message, the callback is passed a block of the decoded bytes from the inner content of the message.<br/><br/>
        /// For an enveloped message, after each call to <see cref="CryptMsgUpdate"/>, you must check to determine whether the <b>CMSG_ENVELOPE_ALGORITHM_PARAM</b> property is available by calling the <see cref="CryptMsgGetParam"/> function.
        /// <see cref="CryptMsgGetParam"/> will fail and <see cref="LastErrorService.GetLastError"/> will return <b>CRYPT_E_STREAM_MSG_NOT_READY</b> until <see cref="CryptMsgUpdate"/> has processed enough of the message to make the <b>CMSG_ENVELOPE_ALGORITHM_PARAM</b> property available.
        /// When the <b>CMSG_ENVELOPE_ALGORITHM_PARAM</b> property is available, you can iterate through the recipients, retrieving a <see cref="CERT_INFO"/> structure for each recipient by using the <see cref="CryptMsgGetParam"/> function to retrieve the <b>CMSG_RECIPIENT_INFO_PARAM</b> property.
        /// To prevent a denial of service attack from an enveloped message that has an artificially large header block, you must keep track of the amount of memory that has been passed to the <see cref="CryptMsgUpdate"/> function during this process.
        /// If the amount of data exceeds an application defined limit before the <b>CMSG_ENVELOPE_ALGORITHM_PARAM</b> property is available, you must stop processing the message and call the <see cref="CryptMsgClose"/> function to cause the operating system to release any memory that has been allocated for the message.
        /// A suggested limit is the maximum allowable size of a message.
        /// For example, if the maximum message size is 10 MB, the limit for this test should be 10 MB.<br/><br/>
        /// The <see cref="CERT_INFO"/> structure is used to find a matching certificate in a previously opened certificate store by using the <see cref="CertGetSubjectCertificateFromStore"/> function.
        /// When the correct certificate is found, the <see cref="CertGetCertificateContextProperty"/> function with a <b>CERT_KEY_PROV_INFO_PROP_ID</b> parameter is called to retrieve a <see cref="CRYPT_KEY_PROV_INFO"/> structure.
        /// The structure contains the information necessary to acquire the recipient's private key by calling <see cref="CryptAcquireContext"/>, using the <b>pwszContainerName</b>, <b>pwszProvName</b>, <b>dwProvType</b>, and dwFlags members of the <see cref="CRYPT_KEY_PROV_INFO"/> structure. The <paramref name="Context"/> acquired and the <b>dwKeySpec</b> member of the <see cref="CRYPT_KEY_PROV_INFO"/> structure are passed to the <see cref="CryptMsgControl"/> structure as a member of the <see cref="CMSG_CTRL_DECRYPT_PARA"/> structure to permit the start of the decryption of the inner content.
        /// The streaming code will then perform the decryption as the data is input.
        /// The resulting blocks of plaintext are passed to the callback function specified by the <b>pfnStreamOutput</b> member of the <see cref="CMSG_STREAM_INFO"/> structure to handle the output of the decrypted message.
        /// <div class="note">
        ///   <b>Note</b>:
        ///   Streamed decoding of an enveloped message queues the <b>ciphertext</b> in memory until <see cref="CryptMsgControl"/> is called to start the decryption.
        ///   The application must initiate decrypting in a timely manner so that the data can be saved to disk or routed elsewhere before the accumulated <i>ciphertext</i> becomes too large and the system runs out of memory.
        /// </div>
        /// In the case of a signed message enclosed in an enveloped message, the <a href="https://learn.microsoft.com/en-us/windows/desktop/SecGloss/p-gly">plaintext</a> output from the streaming decode of the enveloped message can be fed into another streaming decode to process the signed message.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns the handle of the opened message.<br/><br/>
        /// If the function fails, it returns <b>NULL</b>.
        /// For extended error information, call <see cref="LastErrorService.GetLastError"/>.<br/><br/>
        /// The following table lists the error codes most commonly returned by the <see cref="LastErrorService.GetLastError"/> function.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Return code
        ///     </th>
        ///     <th>
        ///       Description
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       One or more arguments are not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_OUTOFMEMORY
        ///     </td>
        ///     <td>
        ///       A memory allocation failure occurred.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </returns>
        IntPtr CryptMsgOpenToDecode(CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,IntPtr Context,ref CMSG_STREAM_INFO StreamInfo);
        #endregion
        #region M:CryptMsgOpenToEncode(CRYPT_OPEN_MESSAGE_FLAGS,CMSG_TYPE,CMSG_SIGNED_ENCODE_INFO,{ref}CMSG_STREAM_INFO):IntPtr
        /// <summary>
        /// This function opens a cryptographic message for encoding and returns a handle of the opened message.
        /// The message remains open until <see cref="CryptMsgClose"/> is called.
        /// </summary>
        /// <param name="Flags">
        /// Currently defined <paramref name="Flags"/> are shown in the following table.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_BARE_CONTENT_FLAG
        ///     </td>
        ///     <td>
        ///       The streamed output will not have an outer ContentInfo wrapper (as defined by PKCS #7).
        ///       This makes it suitable to be streamed into an enclosing message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_DETACHED_FLAG
        ///     </td>
        ///     <td>
        ///       There is detached data being supplied for the subsequent calls to <see cref="CryptMsgUpdate"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_AUTHENTICATED_ATTRIBUTES_FLAG
        ///     </td>
        ///     <td>
        ///       Authenticated attributes are forced to be included in the SignerInfo (as defined by PKCS #7) in cases where they would not otherwise be required.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CONTENTS_OCTETS_FLAG
        ///     </td>
        ///     <td>
        ///       Used when calculating the size of a message that has been encoded by using Distinguished Encoding Rules (DER) and that is nested inside an enveloped message.
        ///       This is particularly useful when performing streaming.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CMS_ENCAPSULATED_CONTENT_FLAG
        ///     </td>
        ///     <td>
        ///       When set, non-data type-inner content is encapsulated within an <b>OCTET STRING</b>.
        ///       Applicable to both signed and enveloped messages.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CRYPT_RELEASE_CONTEXT_FLAG
        ///     </td>
        ///     <td>
        ///       If set, the <b>hCryptProv</b> that is passed to this function is released on the final <see cref="CryptMsgUpdate"/>.
        ///       The handle is not released if the function fails.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Type">
        /// Indicates the message type. This must be one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_DATA
        ///     </td>
        ///     <td>
        ///       This value is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNED
        ///     </td>
        ///     <td>
        ///       The <paramref name="EncodeInfo"/> parameter is the address of a <see cref="CMSG_SIGNED_ENCODE_INFO"/> structure that contains the encoding information.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_ENVELOPED
        ///     </td>
        ///     <td>
        ///       The <paramref name="EncodeInfo"/> parameter is the address of a <see cref="CMSG_ENVELOPED_ENCODE_INFO"/> structure that contains the encoding information.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNED_AND_ENVELOPED
        ///     </td>
        ///     <td>
        ///       This value is not currently implemented.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_HASHED
        ///     </td>
        ///     <td>
        ///       The <paramref name="EncodeInfo"/> parameter is the address of a <see cref="CMSG_HASHED_ENCODE_INFO"/> structure that contains the encoding information.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="EncodeInfo">
        /// The address of a structure that contains the encoding information.
        /// The type of data depends on the value of the <paramref name="Type"/> parameter.
        /// For details, see <paramref name="Type"/>.
        /// </param>
        /// <param name="StreamInfo">
        /// When streaming is being used, this parameter is the address of a <see cref="CMSG_STREAM_INFO"/> structure.
        /// The callback function specified by the <b>pfnStreamOutput</b> member of the <see cref="CMSG_STREAM_INFO"/> structure is called when <see cref="CryptMsgUpdate"/> is executed.
        /// The callback is passed the encoded bytes that result from the encoding.
        /// For more information about how to use the callback, see <see cref="CMSG_STREAM_INFO"/>.
        /// <div class="note">
        ///   <b>Note</b>:
        ///   When streaming is being used, the application must not release any data handles that are passed in the <paramref name="EncodeInfo"/> parameter, such as the provider handle in the <b>hCryptProv</b> member of the <see cref="CMSG_SIGNER_ENCODE_INFO"/> structure, until after the message handle returned by this function is closed by using the <see cref="CryptMsgClose"/> function.
        /// </div>
        /// When streaming is not being used, this parameter is set to <b>NULL</b>.<br/><br/>
        /// Streaming is not used with the <b>CMSG_HASHED</b> message type.
        /// When dealing with hashed data, this parameter must be set to <b>NULL</b>.<br/><br/>
        /// Consider the case of a signed message being enclosed in an enveloped message.
        /// The encoded output from the streamed encoding of the signed message feeds into another streaming encoding of the enveloped message.
        /// The callback for the streaming encoding calls <see cref="CryptMsgUpdate"/> to encode the enveloped message.
        /// The callback for the enveloped message receives the encoded bytes of the nested signed message.
        /// </param>
        /// <returns>
        /// If the function succeeds, it returns a handle to the opened message.
        /// This handle must be closed when it is no longer needed by passing it to the <see cref="CryptMsgClose"/> function.<br/><br/>
        /// If this function fails, <b>NULL</b> is returned.<br/><br/>
        /// To retrieve extended error information, use the <see cref="LastErrorService.GetLastError"/> function.<br/><br/>
        /// The following table lists the error codes most commonly returned by the <see cref="LastErrorService.GetLastError"/> function.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Return code
        ///     </th>
        ///     <th>
        ///       Description
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_INVALID_MSG_TYPE
        ///     </td>
        ///     <td>
        ///       The message type is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_OID_FORMAT
        ///     </td>
        ///     <td>
        ///       The OID is badly formatted.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_UNKNOWN_ALGO
        ///     </td>
        ///     <td>
        ///       The cryptographic algorithm is unknown.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       One or more arguments are not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_OUTOFMEMORY
        ///     </td>
        ///     <td>
        ///       There is not enough memory.
        ///     </td>
        ///   </tr>
        /// </table>
        /// In addition, if <paramref name="Type"/> is <b>CMSG_SIGNED</b>, errors can be propagated from <see cref="CryptCreateHash"/>.<br/><br/>
        /// If <paramref name="Type"/> is <b>CMSG_ENVELOPED</b>, errors can be propagated from <see cref="CryptGenKey"/>, <see cref="CryptImportKey"/>, and <see cref="CryptExportKey"/>.<br/><br/>
        /// If <paramref name="Type"/> is <b>CMSG_HASHED</b>, errors can be propagated from <see cref="CryptCreateHash"/>.
        /// </returns>
        IntPtr CryptMsgOpenToEncode(CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,CMSG_SIGNED_ENCODE_INFO EncodeInfo,ref CMSG_STREAM_INFO StreamInfo);
        #endregion
        #region M:CryptMsgOpenToEncode(CRYPT_OPEN_MESSAGE_FLAGS,CMSG_TYPE,{ref}CMSG_ENVELOPED_ENCODE_INFO,{ref}CMSG_STREAM_INFO):IntPtr
        /// <summary>
        /// This function opens a cryptographic message for encoding and returns a handle of the opened message.
        /// The message remains open until <see cref="CryptMsgClose"/> is called.
        /// </summary>
        /// <param name="Flags">
        /// Currently defined <paramref name="Flags"/> are shown in the following table.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_BARE_CONTENT_FLAG
        ///     </td>
        ///     <td>
        ///       The streamed output will not have an outer ContentInfo wrapper (as defined by PKCS #7).
        ///       This makes it suitable to be streamed into an enclosing message.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_DETACHED_FLAG
        ///     </td>
        ///     <td>
        ///       There is detached data being supplied for the subsequent calls to <see cref="CryptMsgUpdate"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_AUTHENTICATED_ATTRIBUTES_FLAG
        ///     </td>
        ///     <td>
        ///       Authenticated attributes are forced to be included in the SignerInfo (as defined by PKCS #7) in cases where they would not otherwise be required.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CONTENTS_OCTETS_FLAG
        ///     </td>
        ///     <td>
        ///       Used when calculating the size of a message that has been encoded by using Distinguished Encoding Rules (DER) and that is nested inside an enveloped message.
        ///       This is particularly useful when performing streaming.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CMS_ENCAPSULATED_CONTENT_FLAG
        ///     </td>
        ///     <td>
        ///       When set, non-data type-inner content is encapsulated within an <b>OCTET STRING</b>.
        ///       Applicable to both signed and enveloped messages.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_CRYPT_RELEASE_CONTEXT_FLAG
        ///     </td>
        ///     <td>
        ///       If set, the <b>hCryptProv</b> that is passed to this function is released on the final <see cref="CryptMsgUpdate"/>.
        ///       The handle is not released if the function fails.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Type">
        /// Indicates the message type. This must be one of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_DATA
        ///     </td>
        ///     <td>
        ///       This value is not used.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNED
        ///     </td>
        ///     <td>
        ///       The <paramref name="EncodeInfo"/> parameter is the address of a <see cref="CMSG_SIGNED_ENCODE_INFO"/> structure that contains the encoding information.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_ENVELOPED
        ///     </td>
        ///     <td>
        ///       The <paramref name="EncodeInfo"/> parameter is the address of a <see cref="CMSG_ENVELOPED_ENCODE_INFO"/> structure that contains the encoding information.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_SIGNED_AND_ENVELOPED
        ///     </td>
        ///     <td>
        ///       This value is not currently implemented.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CMSG_HASHED
        ///     </td>
        ///     <td>
        ///       The <paramref name="EncodeInfo"/> parameter is the address of a <see cref="CMSG_HASHED_ENCODE_INFO"/> structure that contains the encoding information.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="EncodeInfo">
        /// The address of a structure that contains the encoding information.
        /// The type of data depends on the value of the <paramref name="Type"/> parameter.
        /// For details, see <paramref name="Type"/>.
        /// </param>
        /// <param name="StreamInfo">
        /// When streaming is being used, this parameter is the address of a <see cref="CMSG_STREAM_INFO"/> structure.
        /// The callback function specified by the <b>pfnStreamOutput</b> member of the <see cref="CMSG_STREAM_INFO"/> structure is called when <see cref="CryptMsgUpdate"/> is executed.
        /// The callback is passed the encoded bytes that result from the encoding.
        /// For more information about how to use the callback, see <see cref="CMSG_STREAM_INFO"/>.
        /// <div class="note">
        ///   <b>Note</b>:
        ///   When streaming is being used, the application must not release any data handles that are passed in the <paramref name="EncodeInfo"/> parameter, such as the provider handle in the <b>hCryptProv</b> member of the <see cref="CMSG_SIGNER_ENCODE_INFO"/> structure, until after the message handle returned by this function is closed by using the <see cref="CryptMsgClose"/> function.
        /// </div>
        /// When streaming is not being used, this parameter is set to <b>NULL</b>.<br/><br/>
        /// Streaming is not used with the <b>CMSG_HASHED</b> message type.
        /// When dealing with hashed data, this parameter must be set to <b>NULL</b>.<br/><br/>
        /// Consider the case of a signed message being enclosed in an enveloped message.
        /// The encoded output from the streamed encoding of the signed message feeds into another streaming encoding of the enveloped message.
        /// The callback for the streaming encoding calls <see cref="CryptMsgUpdate"/> to encode the enveloped message.
        /// The callback for the enveloped message receives the encoded bytes of the nested signed message.
        /// </param>
        /// <returns>
        /// If the function succeeds, it returns a handle to the opened message.
        /// This handle must be closed when it is no longer needed by passing it to the <see cref="CryptMsgClose"/> function.<br/><br/>
        /// If this function fails, <b>NULL</b> is returned.<br/><br/>
        /// To retrieve extended error information, use the <see cref="LastErrorService.GetLastError"/> function.<br/><br/>
        /// The following table lists the error codes most commonly returned by the <see cref="LastErrorService.GetLastError"/> function.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Return code
        ///     </th>
        ///     <th>
        ///       Description
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_INVALID_MSG_TYPE
        ///     </td>
        ///     <td>
        ///       The message type is not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_OID_FORMAT
        ///     </td>
        ///     <td>
        ///       The OID is badly formatted.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_E_UNKNOWN_ALGO
        ///     </td>
        ///     <td>
        ///       The cryptographic algorithm is unknown.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_INVALIDARG
        ///     </td>
        ///     <td>
        ///       One or more arguments are not valid.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       E_OUTOFMEMORY
        ///     </td>
        ///     <td>
        ///       There is not enough memory.
        ///     </td>
        ///   </tr>
        /// </table>
        /// In addition, if <paramref name="Type"/> is <b>CMSG_SIGNED</b>, errors can be propagated from <see cref="CryptCreateHash"/>.<br/><br/>
        /// If <paramref name="Type"/> is <b>CMSG_ENVELOPED</b>, errors can be propagated from <see cref="CryptGenKey"/>, <see cref="CryptImportKey"/>, and <see cref="CryptExportKey"/>.<br/><br/>
        /// If <paramref name="Type"/> is <b>CMSG_HASHED</b>, errors can be propagated from <see cref="CryptCreateHash"/>.
        /// </returns>
        IntPtr CryptMsgOpenToEncode(CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,ref CMSG_ENVELOPED_ENCODE_INFO EncodeInfo,ref CMSG_STREAM_INFO StreamInfo);
        #endregion
        #region M:CertGetCertificateChain(IntPtr,IntPtr,{ref}FILETIME,IntPtr,IntPtr,{ref}CERT_CHAIN_PARA,CERT_CHAIN_FLAGS,CERT_CHAIN_CONTEXT**):Boolean
        /// <summary>
        /// This function builds a certificate chain context starting from an end certificate and going back, if possible, to a trusted root certificate.
        /// </summary>
        /// <param name="ChainEngine">
        /// A handle of the chain engine (namespace and cache) to be used.
        /// If <paramref name="ChainEngine"/> is <b>NULL</b>, the default chain engine, HCCE_CURRENT_USER, is used.
        /// This parameter can be set to HCCE_LOCAL_MACHINE.
        /// </param>
        /// <param name="Context">
        /// A pointer to the <see cref="CERT_CONTEXT"/> of the end certificate, the certificate for which a chain is being built.
        /// This certificate context will be the zero-index element in the first simple chain.
        /// </param>
        /// <param name="Time">
        /// A reference to a <see cref="FILETIME"/> variable that indicates the time for which the chain is to be validated.
        /// Note that the time does not affect trust list, revocation, or root store checking.
        /// Trust in a particular certificate being a trusted root is based on the current state of the root store and not the state of the root store at a time passed in by this parameter.
        /// For revocation, a certificate revocation list (CRL), itself, must be valid at the current time.
        /// The value of this parameter is used to determine whether a certificate listed in a CRL has been revoked.
        /// </param>
        /// <param name="AdditionalStore">
        /// A handle to any additional store to search for supporting certificates and certificate trust lists (CTLs).
        /// This parameter can be <b>NULL</b> if no additional store is to be searched.
        /// </param>
        /// <param name="ChainPara">A reference to a <see cref="CERT_CHAIN_PARA"/> structure that includes chain-building parameters.</param>
        /// <param name="Flags">
        /// Flag values that indicate special processing. This parameter can be a combination of one or more of the following flags.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_CACHE_END_CERT
        ///     </td>
        ///     <td>
        ///       When this flag is set, the end certificate is cached, which might speed up the chain-building process.
        ///       By default, the end certificate is not cached, and it would need to be verified each time a chain is built for it.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_REVOCATION_CHECK_CACHE_ONLY
        ///     </td>
        ///     <td>
        ///       Revocation checking only accesses cached URLs.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_REVOCATION_CHECK_OCSP_CERT
        ///     </td>
        ///     <td>
        ///       This flag is used internally during chain building for an online certificate status protocol (OCSP) signer certificate to prevent cyclic revocation checks.
        ///       During chain building, if the OCSP response is signed by an independent OCSP signer, then, in addition to the original chain build, there is a second chain built for the OCSP signer certificate itself.
        ///       This flag is used during this second chain build to inhibit a recursive independent OCSP signer certificate.
        ///       If the signer certificate contains the <b>szOID_PKIX_OCSP_NOCHECK</b> extension, revocation checking is skipped for the leaf signer certificate.
        ///       Both OCSP and CRL checking are allowed.<br/>
        ///       <b>Windows Server 2003 and Windows XP</b>: This value is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_CACHE_ONLY_URL_RETRIEVAL
        ///     </td>
        ///     <td>
        ///       Uses only cached URLs in building a certificate chain.
        ///       The Internet and intranet are not searched for URL-based objects.
        ///       <div class="note">
        ///         <b>Note</b>:
        ///         This flag is not applicable to revocation checking.
        ///         Set <b>CERT_CHAIN_REVOCATION_CHECK_CACHE_ONLY</b> to use only cached URLs for revocation checking.
        ///       </div>
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_DISABLE_PASS1_QUALITY_FILTERING
        ///     </td>
        ///     <td>
        ///       For performance reasons, the second pass of chain building only considers potential chain paths that have quality greater than or equal to the highest quality determined during the first pass.
        ///       The first pass only considers valid signature, complete chain, and trusted roots to calculate chain quality.
        ///       This flag can be set to disable this optimization and consider all potential chain paths during the second pass.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_DISABLE_MY_PEER_TRUST
        ///     </td>
        ///     <td>
        ///       This flag is not supported.
        ///       Certificates in the "My" store are never considered for peer trust.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_ENABLE_PEER_TRUST
        ///     </td>
        ///     <td>
        ///       End entity certificates in the "TrustedPeople" store are trusted without performing any chain building.
        ///       This function does not set the <b>CERT_TRUST_IS_PARTIAL_CHAIN</b> or <b>CERT_TRUST_IS_UNTRUSTED_ROOT</b> <b>dwErrorStatus</b> member bits of the <paramref name="ChainContext"/> parameter.<br/>
        ///       <b>Windows Server 2003 Windows XP</b>: This flag is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_OPT_IN_WEAK_SIGNATURE
        ///     </td>
        ///     <td>
        ///       Setting this flag indicates the caller wishes to opt into weak signature checks.<br/>
        ///       This flag is available in the rollup update for each OS starting with Windows 7 and Windows Server 2008 R2.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_RETURN_LOWER_QUALITY_CONTEXTS
        ///     </td>
        ///     <td>
        ///       The default is to return only the highest quality chain path.
        ///       Setting this flag will return the lower quality chains.
        ///       These are returned in the <b>cLowerQualityChainContext</b> and <b>rgpLowerQualityChainContext</b> fields of the chain context.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_DISABLE_AUTH_ROOT_AUTO_UPDATE
        ///     </td>
        ///     <td>
        ///       Setting this flag inhibits the auto update of third-party roots from the Windows Update Web Server.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_REVOCATION_ACCUMULATIVE_TIMEOUT
        ///     </td>
        ///     <td>
        ///       When you set <b>CERT_CHAIN_REVOCATION_ACCUMULATIVE_TIMEOUT</b> and you also specify a value for the <b>dwUrlRetrievalTimeout</b> member of the <see cref="CERT_CHAIN_PARA"/> structure, the value you specify in <b>dwUrlRetrievalTimeout</b> represents the cumulative timeout across all revocation URL retrievals.<br/>
        ///       If you set <b>CERT_CHAIN_REVOCATION_ACCUMULATIVE_TIMEOUT</b> but do not specify a <b>dwUrlRetrievalTimeout</b> value, the maximum cumulative timeout is set, by default, to 20 seconds.
        ///       Each URL tested will timeout after half of the remaining cumulative balance has passed.
        ///       That is, the first URL times out after 10 seconds, the second after 5 seconds, the third after 2.5 seconds and so on until a URL succeeds, 20 seconds has passed, or there are no more URLs to test.<br/>
        ///       If you do not set <b>CERT_CHAIN_REVOCATION_ACCUMULATIVE_TIMEOUT</b>, each revocation URL in the chain is assigned a maximum timeout equal to the value specified in <b>dwUrlRetrievalTimeout</b>.
        ///       If you do not specify a value for the <b>dwUrlRetrievalTimeout</b> member, each revocation URL is assigned a maximum default timeout of 15 seconds.
        ///       If no URL succeeds, the maximum cumulative timeout value is 15 seconds multiplied by the number of URLs in the chain.<br/>
        ///       You can set the default values by using Group Policy.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_TIMESTAMP_TIME
        ///     </td>
        ///     <td>
        ///       When this flag is set, <paramref name="Time"/> is used as the time stamp time to determine whether the end certificate was time valid.
        ///       Current time can also be used to determine whether the end certificate remains time valid.
        ///       All other certification authority (CA) and root certificates in the chain are checked by using current time and not <paramref name="Time"/>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_DISABLE_AIA
        ///     </td>
        ///     <td>
        ///       Setting this flag explicitly turns off Authority Information Access (AIA) retrievals.
        ///     </td>
        ///   </tr>
        /// </table>
        /// You can also set the following revocation flags, but only one flag from this group may be set at a time.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_REVOCATION_CHECK_END_CERT
        ///     </td>
        ///     <td>
        ///       Revocation checking is done on the end certificate and only the end certificate.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_REVOCATION_CHECK_CHAIN
        ///     </td>
        ///     <td>
        ///       Revocation checking is done on all of the certificates in every chain.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CHAIN_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT
        ///     </td>
        ///     <td>
        ///       Revocation checking is done on all certificates in all of the chains except the root certificate.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="ChainContext">
        /// The address of a pointer to the chain context created.
        /// When you have finished using the chain context, release the chain by calling the <see cref="CertFreeCertificateChain"/> function.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// </returns>
        unsafe Boolean CertGetCertificateChain(IntPtr ChainEngine,IntPtr Context,ref FILETIME Time,IntPtr AdditionalStore,ref CERT_CHAIN_PARA ChainPara,CERT_CHAIN_FLAGS Flags,CERT_CHAIN_CONTEXT** ChainContext);
        #endregion
        #region M:CertGetValidUsages(Int32,CERT_CONTEXT*,Int32*,IntPtr,Int32*):Boolean
        /// <summary>
        /// This function returns an array of usages that consist of the intersection of the valid usages for all certificates in an array of certificates.
        /// </summary>
        /// <param name="cCerts">The number of certificates in the array to be checked.</param>
        /// <param name="rghCerts">An array of certificates to be checked for valid usage.</param>
        /// <param name="cNumOIDs">
        /// The number of valid usages found as the intersection of the valid usages of all certificates in the array.
        /// If all of the certificates are valid for all usages, <paramref name="cNumOIDs"/> is set to negative one (–1).
        /// </param>
        /// <param name="rghOIDs">
        /// An array of the object identifiers (OIDs) of the valid usages that are shared by all of the certificates in the <paramref name="rghCerts"/> array.
        /// This parameter can be <b>NULL</b> to set the size of this structure for memory allocation purposes.
        /// </param>
        /// <param name="pcbOIDs">
        /// A pointer to a <see cref="Int32"/> value that specifies the size, in bytes, of the <paramref name="rghOIDs"/> array and the strings pointed to.
        /// When the function returns, the <see cref="Int32"/> value contains the number of bytes needed for the array.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero (<b>TRUE</b>).<br/>
        /// If the function fails, the return value is zero (<b>FALSE</b>). For extended error information, call <see cref="LastErrorService.GetLastError"/>. One possible error code is the following.
        /// </returns>
        unsafe Boolean CertGetValidUsages(Int32 cCerts,CERT_CONTEXT* rghCerts,Int32* cNumOIDs,IntPtr rghOIDs,Int32* pcbOIDs);
        #endregion
        #region M:CertCreateSelfSignCertificate(IntPtr,{ref}CERT_NAME_BLOB,Int32,CRYPT_KEY_PROV_INFO*,CRYPT_ALGORITHM_IDENTIFIER*,SYSTEMTIME*,SYSTEMTIME*,CERT_EXTENSIONS*):IntPtr
        /// <summary>
        /// This function builds a self-signed certificate and returns a pointer to a <see cref="CERT_CONTEXT"/> structure that represents the certificate.
        /// </summary>
        /// <param name="CryptProvOrNCryptKey">
        /// A handle of a cryptographic provider used to sign the certificate created.
        /// If <b>NULL</b>, information from the <paramref name="KeyProvInfo"/> parameter is used to acquire the needed handle.
        /// If <paramref name="KeyProvInfo"/> is also <b>NULL</b>, the default provider type, <b>PROV_RSA_FULL</b> provider type, the default key specification, <b>AT_SIGNATURE</b>, and a newly created key container with a unique container name are used.<br/>
        /// This handle must be an <b>HCRYPTPROV</b> handle that has been created by using the <see cref="CryptAcquireContext"/> function or an <b>NCRYPT_KEY_HANDLE</b> handle that has been created by using the <b>NCryptOpenKey</b> function.
        /// New applications should always pass in the <b>NCRYPT_KEY_HANDLE</b> handle of a CNG cryptographic service provider (CSP).
        /// </param>
        /// <param name="SubjectIssuer">
        /// A pointer to a BLOB that contains the distinguished name (DN) for the certificate subject.
        /// Minimally, a reference to an empty DN must be provided.
        /// This BLOB is normally created by using the <see cref="CertStrToName"/> function.
        /// It can also be created by using the <b>CryptEncodeObject</b> function and specifying either the <b>X509_NAME</b> or <b>X509_UNICODE_NAME</b> StructType.
        /// </param>
        /// <param name="Flags">
        /// A set of flags that override the default behavior of this function.
        /// This can be zero or a combination of one or more of the following values.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CREATE_SELFSIGN_NO_KEY_INFO
        ///     </td>
        ///     <td>
        ///       By default, the returned <see cref="CERT_CONTEXT"/> references the private keys by setting the <b>CERT_KEY_PROV_INFO_PROP_ID</b>.
        ///       If you do not want the returned <see cref="CERT_CONTEXT"/> to reference private keys by setting the <b>CERT_KEY_PROV_INFO_PROP_ID</b>, specify <b>CERT_CREATE_SELFSIGN_NO_KEY_INFO</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CERT_CREATE_SELFSIGN_NO_SIGN
        ///     </td>
        ///     <td>
        ///       By default, the certificate being created is signed.
        ///       If the certificate being created is only a dummy placeholder, the certificate might not need to be signed.
        ///       Signing of the certificate is skipped if <b>CERT_CREATE_SELFSIGN_NO_SIGN</b> is specified.
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="KeyProvInfo">
        /// A pointer to a <see cref="CRYPT_KEY_PROV_INFO"/> structure.
        /// Before a certificate is created, the CSP is queried for the key provider, key provider type, and the key container name.
        /// If the CSP queried does not support these queries, the function fails.
        /// If the default provider does not support these queries, a <paramref name="KeyProvInfo"/> value must be specified.
        /// The RSA BASE does support these queries.<br/>
        /// If the <paramref name="KeyProvInfo"/> parameter is not <b>NULL</b>, the corresponding values are set in the <b>CERT_KEY_PROV_INFO_PROP_ID</b> value of the generated certificate.
        /// You must ensure that all parameters of the supplied structure are correctly specified.
        /// </param>
        /// <param name="SignatureAlgorithm">
        /// A pointer to a <see cref="CRYPT_ALGORITHM_IDENTIFIER"/> structure.
        /// If <b>NULL</b>, the default algorithm, SHA1RSA, is used.
        /// </param>
        /// <param name="StartTime">
        /// A pointer to a <see cref="SYSTEMTIME"/> structure.
        /// If <b>NULL</b>, the system current time is used by default.
        /// </param>
        /// <param name="EndTime">
        /// A pointer to a <see cref="SYSTEMTIME"/> structure.
        /// If <b>NULL</b>, the <paramref name="StartTime"/> value plus one year will be used by default.
        /// </param>
        /// <param name="Extensions">
        /// A pointer to a <see cref="CERT_EXTENSIONS"/> array of <see cref="CERT_EXTENSION"/> structures.
        /// By default, the array is empty.
        /// An alternate subject name, if desired, can be specified as one of these extensions.
        /// </param>
        /// <returns>
        /// If the function succeeds, a <see cref="CERT_CONTEXT"/> variable that points to the created certificate is returned.
        /// If the function fails, it returns <b>NULL</b>. For extended error information, call <see cref="LastErrorService.GetLastError"/>.
        /// </returns>
        unsafe IntPtr CertCreateSelfSignCertificate(IntPtr CryptProvOrNCryptKey,ref CERT_NAME_BLOB SubjectIssuer,Int32 Flags,CRYPT_KEY_PROV_INFO* KeyProvInfo,CRYPT_ALGORITHM_IDENTIFIER* SignatureAlgorithm,SYSTEMTIME* StartTime,SYSTEMTIME* EndTime,CERT_EXTENSIONS* Extensions);
        #endregion
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
         *   <table class="table_value_meaning">
         *     <tr>
         *       <td>
         *         <see cref="HRESULT.CRYPT_E_NOT_FOUND"/>
         *       </td>
         *       <td>
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
        #region M:CryptFindOIDInfo(CRYPT_OID_INFO_KEY_TYPE,void*,Int32):IntPtr
        /// <summary>
        /// This function retrieves the first predefined or registered <see cref="CRYPT_OID_INFO"/> structure that matches a specified key type and key.
        /// The search can be limited to object identifiers (OIDs) within a specified OID group.<br/>
        /// Use it to list all or selected subsets of <see cref="CRYPT_OID_INFO"/> structures.
        /// New <see cref="CRYPT_OID_INFO"/> structures can be registered by using <see cref="CryptRegisterOIDInfo"/>.
        /// User-registered OIDs can be removed from the list of registered OIDs by using <see cref="CryptUnregisterOIDInfo"/>.<br/>
        /// New OIDs can be placed in the list of registered OIDs either before or after the predefined entries.
        /// Because <b>CryptFindOIDInfo</b> returns the first key on the list that matches the search criteria, a newly registered OID placed before a predefined OID entry with the same key overrides a predefined entry.
        /// </summary>
        /// <param name="KeyType">
        /// Specifies the key type to use when finding OID information.<br/>
        /// This parameter can be one of the following key types.
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_OID_INFO_OID_KEY
        ///     </td>
        ///     <td>
        ///       <paramref name="Key"/> is the address of a null-terminated ANSI string that contains the OID string to find.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_OID_INFO_NAME_KEY
        ///     </td>
        ///     <td>
        ///       <paramref name="Key"/> is the address of a null-terminated Unicode string that contains the name to find.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_OID_INFO_ALGID_KEY
        ///     </td>
        ///     <td>
        ///       <paramref name="Key"/> is the address of an <see cref="ALG_ID"/> variable. The following ALG_IDs are supported:
        ///       <list type="bullet">
        ///         <item>Hash Algorithms</item>
        ///         <item>Symmetric Encryption Algorithms</item>
        ///         <item>Public Key Algorithms</item>
        ///       </list>
        ///       Algorithms that are not listed are supported by using Cryptography API: Next Generation (CNG) only; instead, use <b>CRYPT_OID_INFO_CNG_ALGID_KEY</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_OID_INFO_SIGN_KEY
        ///     </td>
        ///     <td>
        ///       <paramref name="Key"/> is the address of an array of two ALG_IDs where the first element contains the hash algorithm identifier and the second element contains the public key algorithm identifier.<br/>
        ///       The following ALG_ID combinations are supported.
        ///       <table class="table_value_meaning" style="width:50%">
        ///         <tr>
        ///           <th>
        ///             Signature algorithm identifier
        ///           </th>
        ///           <th>
        ///             Hash algorithm identifier
        ///           </th>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CALG_RSA_SIGN
        ///           </td>
        ///           <td>
        ///             CALG_SHA1<br/>
        ///             CALG_MD5<br/>
        ///             CALG_MD4<br/>
        ///             CALG_MD2
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CALG_DSS_SIGN
        ///           </td>
        ///           <td>
        ///             CALG_SHA1
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CALG_NO_SIGN
        ///           </td>
        ///           <td>
        ///             CALG_SHA1<br/>
        ///             CALG_NO_SIGN
        ///           </td>
        ///         </tr>
        ///       </table>
        ///       Algorithms that are not listed are supported through CNG only; instead, use <b>CRYPT_OID_INFO_CNG_SIGN_KEY</b>.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_OID_INFO_CNG_ALGID_KEY
        ///     </td>
        ///     <td>
        ///       <paramref name="Key"/> is the address of a null-terminated Unicode string that contains the CNG algorithm identifier to find.
        ///       This can be one of the predefined CNG Algorithm Identifiers or another registered algorithm identifier.<br/>
        ///       <b>Windows Server 2003 R2 Windows Server 2003</b>: This key type is not supported.
        ///     </td>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_OID_INFO_CNG_SIGN_KEY
        ///     </td>
        ///     <td>
        ///       <paramref name="Key"/> is the address of an array of two null-terminated Unicode string pointers where the first string contains the hash CNG algorithm identifier and the second string contains the public key CNG algorithm identifier.
        ///       These can be from the predefined CNG Algorithm Identifiers or another registered algorithm identifier.<br/>
        ///       <b>Windows Server 2003 R2 Windows Server 2003</b>: This key type is not supported.<br/>
        ///       Optionally, the following key types can be specified in the dwKeyType parameter by using the logical OR operator (|).
        ///       <table class="table_value_meaning" style="width:50%">
        ///         <tr>
        ///           <th>
        ///             Value
        ///           </th>
        ///           <th>
        ///             Meaning
        ///           </th>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_OID_INFO_PUBKEY_SIGN_KEY_FLAG
        ///           </td>
        ///           <td>
        ///             Skips public keys in the <b>CRYPT_PUBKEY_ALG_OID_GROUP_ID</b> group that are explicitly flagged with the <b>CRYPT_OID_PUBKEY_ENCRYPT_ONLY_FLAG</b> flag.
        ///           </td>
        ///         </tr>
        ///         <tr>
        ///           <td>
        ///             CRYPT_OID_INFO_PUBKEY_ENCRYPT_KEY_FLAG
        ///           </td>
        ///           <td>
        ///             Skips public keys in the <b>CRYPT_PUBKEY_ALG_OID_GROUP_ID</b> group that are explicitly flagged with the <b>CRYPT_OID_PUBKEY_SIGN_ONLY_FLAG</b> flag.
        ///           </td>
        ///         </tr>
        ///       </table>
        ///     </td>
        ///   </tr>
        /// </table>
        /// </param>
        /// <param name="Key">
        /// The address of a buffer that contains additional search information.
        /// This parameter depends on the value of the <paramref name="KeyType"/> parameter.
        /// For more information, see the table under <paramref name="KeyType"/>.
        /// </param>
        /// <param name="GroupId">
        /// The group identifier to use when finding OID information.
        /// Setting this parameter to zero searches all groups according to the dwKeyType parameter.
        /// Otherwise, only the indicated <paramref name="GroupId"/> is searched.<br/>
        /// For information about code that lists the OID information by group identifier, see <see cref="CryptEnumOIDInfo"/>.<br/>
        /// Optionally, the following flag can be specified in the <paramref name="GroupId"/> parameter by using the logical OR operator (|).
        /// <table class="table_value_meaning">
        ///   <tr>
        ///     <th>
        ///       Value
        ///     </th>
        ///     <th>
        ///       Meaning
        ///     </th>
        ///   </tr>
        ///   <tr>
        ///     <td>
        ///       CRYPT_OID_DISABLE_SEARCH_DS_FLAG
        ///     </td>
        ///     <td>
        ///       Disables searching the directory server.
        ///     </td>
        ///   </tr>
        /// </table>
        /// The bit length shifted left 16 bits can be specified in the <paramref name="GroupId"/> parameter by using the logical OR operator (|).
        /// </param>
        /// <returns>
        /// Returns a pointer to a constant structure of type <see cref="CRYPT_OID_INFO"/>. The returned pointer must not be freed. When the specified key and group is not found, <b>NULL</b> is returned.
        /// </returns>
        unsafe IntPtr CryptFindOIDInfo(CRYPT_OID_INFO_KEY_TYPE KeyType,void* Key,Int32 GroupId);
        #endregion
        }
    }
