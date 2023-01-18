using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public partial class X509CertificateChainPolicy
        {
        private class EX509CertificateChainPolicy : X509CertificateChainPolicy
            {
            public EX509CertificateChainPolicy(CertificateChainPolicy policy, ICryptoAPI entries)
                : base(policy,entries)
                {
                }

            #region M:EnsureSource
            protected override void EnsureSource() {
                ValidatePolicy(Policy);
                }
            #endregion
            #region M:ValidatePolicy(CertificateChainPolicy)
            protected override void ValidatePolicy(CertificateChainPolicy policy) {
                if (policy < CertificateChainPolicy.CERT_CHAIN_POLICY_BASE || policy > CertificateChainPolicy.CERT_CHAIN_POLICY_SSL_KEY_PIN) {
                    throw new ArgumentOutOfRangeException(nameof(policy));
                    }
                }
            #endregion
            }
        }
    }
