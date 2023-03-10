using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_IS_NOT_VALID_FOR_USAGE)]
    public class CertificateInvalidUsageException : CertificateException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateInvalidUsageException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateInvalidUsageException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_IS_NOT_VALID_FOR_USAGE)))
            {
            }
        }
    }