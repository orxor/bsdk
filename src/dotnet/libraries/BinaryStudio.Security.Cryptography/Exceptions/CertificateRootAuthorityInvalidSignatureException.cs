using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    public class CertificateRootAuthorityInvalidSignatureException : CertificateInvalidSignatureException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateInvalidSignatureException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateRootAuthorityInvalidSignatureException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_IS_NOT_SIGNATURE_VALID)))
            {
            }
        }
    }