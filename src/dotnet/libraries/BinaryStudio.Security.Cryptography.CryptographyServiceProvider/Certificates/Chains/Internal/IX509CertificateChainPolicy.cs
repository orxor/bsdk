using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public partial class X509CertificateChainPolicy
        {
        private class IX509CertificateChainPolicy : X509CertificateChainPolicy
            {
            public IX509CertificateChainPolicy(CertificateChainPolicy policy, ICryptoAPI entries)
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
                if (policy != CertificateChainPolicy.CERT_CHAIN_POLICY_ICAO) {
                    throw new ArgumentOutOfRangeException(nameof(policy));
                    }
                }
            #endregion
            }
        }
    }
