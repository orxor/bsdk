using System;
using System.Collections.Generic;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public interface IX509CertificateStorage : IDisposable
        {
        IntPtr Handle { get; }
        X509StoreLocation Location { get; }
        IEnumerable<X509Certificate> Certificates { get; }
        IEnumerable<X509CertificateRevocationList> CertificateRevocationLists { get; }
        /**
         * <summary>
         * Finds a subject certificate context uniquely identified by its issuer and serial number in certificate store.
         * </summary>
         * <param name="Info">A pointer to a <see cref="CERT_INFO"/> structure. Only the <see cref="CERT_INFO.Issuer"/> and <see cref="CERT_INFO.SerialNumber"/> members are used.</param>
         * <returns>The certificate if succeeds, otherwise <see langword="null"/>.</returns>
         * <seealso cref="CryptographicFunctions.CertGetSubjectCertificateFromStore"/>
         */
        unsafe X509Certificate Find(CERT_INFO* Info);

        /// <summary>
        /// Adds a certificate to an X.509 certificate store.
        /// </summary>
        /// <param name="o">The certificate to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="o"/> is null.</exception>
        /// <seealso cref="M:BinaryStudio.Security.Cryptography.CryptographicFunctions.CertAddCertificateContextToStore(System.IntPtr,System.IntPtr,BinaryStudio.PlatformComponents.Win32.CERT_STORE_ADD)"/>
        /// <seealso cref="M:BinaryStudio.Security.Cryptography.CryptographicFunctions.CertAddCertificateContextToStore(System.IntPtr,System.IntPtr,BinaryStudio.PlatformComponents.Win32.CERT_STORE_ADD,System.IntPtr@)"/>
        void Add(X509Certificate o);
        void Add(X509CertificateRevocationList o);
        void Remove(X509Certificate o);
        void Remove(X509CertificateRevocationList o);
        }
    }