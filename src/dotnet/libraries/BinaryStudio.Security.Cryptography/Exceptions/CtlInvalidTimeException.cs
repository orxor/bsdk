using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_TIME_VALID)]
    public class CtlInvalidTimeException : CtlException
        {
        /// <summary>Initializes a new instance of the <see cref="CtlInvalidTimeException"/> class with a system-supplied message that describes the error.</summary>
        public CtlInvalidTimeException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_TIME_VALID)))
            {
            }
        }
    }