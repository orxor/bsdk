using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_INVALID_NAME_CONSTRAINTS)]
    public class CertificateInvalidNameConstraintException : CertificateConstraintException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateInvalidNameConstraintException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateInvalidNameConstraintException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_INVALID_NAME_CONSTRAINTS)))
            {
            }
        }
    }