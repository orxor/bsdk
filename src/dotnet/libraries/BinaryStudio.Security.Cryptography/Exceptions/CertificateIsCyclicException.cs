using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_IS_CYCLIC)]
    public class CertificateIsCyclicException : CertificateException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateIsCyclicException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateIsCyclicException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_IS_CYCLIC)))
            {
            }
        }
    }