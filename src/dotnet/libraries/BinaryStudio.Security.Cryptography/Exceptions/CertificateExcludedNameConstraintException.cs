using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    [UsedImplicitly]
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_EXCLUDED_NAME_CONSTRAINT)]
    public class CertificateExcludedNameConstraintException : CertificateConstraintException
        {
        public CertificateExcludedNameConstraintException()
            : base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_HAS_EXCLUDED_NAME_CONSTRAINT)))
            {
            }
        }
    }