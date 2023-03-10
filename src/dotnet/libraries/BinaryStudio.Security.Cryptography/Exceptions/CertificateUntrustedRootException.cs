using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_IS_UNTRUSTED_ROOT)]
    public class CertificateUntrustedRootException : CertificateException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateUntrustedRootException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateUntrustedRootException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_IS_UNTRUSTED_ROOT)))
            {
            }
        }
    }