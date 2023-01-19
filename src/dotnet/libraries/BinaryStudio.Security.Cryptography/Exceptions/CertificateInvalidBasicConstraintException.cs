using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_INVALID_BASIC_CONSTRAINTS)]
    public class CertificateInvalidBasicConstraintException : CertificateConstraintException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateInvalidBasicConstraintException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateInvalidBasicConstraintException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_INVALID_BASIC_CONSTRAINTS)))
            {
            }
        }
    }