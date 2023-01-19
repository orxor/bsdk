using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_IS_OFFLINE_REVOCATION)]
    public class CertificateRevocationOfflineException : CertificateRevocationException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateRevocationOfflineException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateRevocationOfflineException()
            :base(HRESULT.CRYPT_E_REVOCATION_OFFLINE,Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_IS_OFFLINE_REVOCATION)))
            {
            }
        }
    }