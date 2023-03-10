using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_WEAK_SIGNATURE)]
    public class CertificateWeakSignatureException : CertificateSignatureException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateWeakSignatureException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateWeakSignatureException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_HAS_WEAK_SIGNATURE)))
            {
            }
        }
    }