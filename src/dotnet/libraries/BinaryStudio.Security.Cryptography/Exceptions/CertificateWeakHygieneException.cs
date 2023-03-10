using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_WEAK_HYGIENE)]
    public class CertificateWeakHygieneException : CertificateException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateWeakHygieneException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateWeakHygieneException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_HAS_WEAK_HYGIENE)))
            {
            }
        }
    }