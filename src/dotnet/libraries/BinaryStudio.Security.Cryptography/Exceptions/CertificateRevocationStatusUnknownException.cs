using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_REVOCATION_STATUS_UNKNOWN)]
    public class CertificateRevocationStatusUnknownException : CertificateRevocationException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateRevocationStatusUnknownException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateRevocationStatusUnknownException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_REVOCATION_STATUS_UNKNOWN)))
            {
            }
        }
    }