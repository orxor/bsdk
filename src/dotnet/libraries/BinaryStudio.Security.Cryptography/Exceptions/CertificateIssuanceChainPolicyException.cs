using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_NO_ISSUANCE_CHAIN_POLICY)]
    public class CertificateIssuanceChainPolicyException : CertificateException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateIssuanceChainPolicyException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateIssuanceChainPolicyException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_NO_ISSUANCE_CHAIN_POLICY)))
            {
            }
        }
    }