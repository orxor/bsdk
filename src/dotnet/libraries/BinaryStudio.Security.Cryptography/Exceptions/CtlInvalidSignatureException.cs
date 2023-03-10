using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_SIGNATURE_VALID)]
    public class CtlInvalidSignatureException : CtlException
        {
        /// <summary>Initializes a new instance of the <see cref="CtlInvalidSignatureException"/> class with a system-supplied message that describes the error.</summary>
        public CtlInvalidSignatureException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_SIGNATURE_VALID)))
            {
            }
        }
    }