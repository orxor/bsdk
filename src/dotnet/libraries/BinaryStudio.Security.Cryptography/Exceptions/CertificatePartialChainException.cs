using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_IS_PARTIAL_CHAIN)]
    public class CertificatePartialChainException : CertificateException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificatePartialChainException"/> class with a system-supplied message that describes the error.</summary>
        public CertificatePartialChainException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_IS_PARTIAL_CHAIN)))
            {
            }
        }
    }