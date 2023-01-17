using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public class X509CertificateChainElement: IX509CertificateChainStatus
        {
        public X509Certificate Certificate { get; }
        public X509CertificateRevocationList CertificateRevocationList { get; }
        public Int32 ElementIndex { get; }
        public CertificateChainErrorStatus ErrorStatus { get; }
        public CertificateChainInfoStatus InfoStatus { get; }

        /// <summary>Initializes a new instance of the <see cref="X509CertificateChainElement"/> class from specified source.</summary>
        /// <param name="source">Source of chain element.</param>
        /// <param name="index">Element index.</param>
        internal unsafe X509CertificateChainElement(CERT_CHAIN_ELEMENT* source, Int32 index) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            ErrorStatus = source->TrustStatus.ErrorStatus;
            InfoStatus  = source->TrustStatus.InfoStatus;
            ElementIndex = index;
            if (source->CertContext != null) {
                if (source->CertContext->CertEncodedSize > 0) {
                    Certificate = new X509Certificate((IntPtr)source->CertContext);
                    }
                }
            if (source->RevocationInfo != null) {
                if ((source->RevocationInfo->CrlInfo != null) &&
                    (source->RevocationInfo->CrlInfo->BaseCrlContext != null) &&
                    (source->RevocationInfo->CrlInfo->BaseCrlContext->CrlEncodedSize > 0)) {
                    CertificateRevocationList = new X509CertificateRevocationList((IntPtr)source->RevocationInfo->CrlInfo->BaseCrlContext);
                    }
                }
            }
        }
    }
