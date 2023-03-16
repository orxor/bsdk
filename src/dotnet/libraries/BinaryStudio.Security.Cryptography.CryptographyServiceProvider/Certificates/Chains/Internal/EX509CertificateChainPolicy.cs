using System;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public partial class X509CertificateChainPolicy
        {
        private class EX509CertificateChainPolicy : X509CertificateChainPolicy
            {
            public EX509CertificateChainPolicy(CertificateChainPolicy policy, CryptographicFunctions entries)
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
            #region M:Validate(X509Certificate,CERT_CHAIN_POLICY_FLAGS)
            public override unsafe void Validate(X509CertificateChainContext context,CERT_CHAIN_POLICY_FLAGS flags) {
                if (context == null) { throw new ArgumentNullException(nameof(context)); }
                var policyPara = new CERT_CHAIN_POLICY_PARA {
                    Size = sizeof(CERT_CHAIN_POLICY_PARA),
                    Flags = flags,
                    ExtraPolicyPara = IntPtr.Zero
                    };
                var policyStatus = new CERT_CHAIN_POLICY_STATUS {
                    Size = sizeof(CERT_CHAIN_POLICY_STATUS),
                    ChainIndex = 0,
                    ElementIndex = 0,
                    Error = 0,
                    ExtraPolicyStatus = IntPtr.Zero
                    };
                Validate(Entries.CertVerifyCertificateChainPolicy(
                    new IntPtr((Int32)Policy),
                    (IntPtr)context.ChainContext,
                    ref policyPara,
                    ref policyStatus));
                if (policyStatus.Error != 0) {
                    throw ExceptionForStatus(policyStatus.Error,context)
                        .Add("ChainContext", context)
                        .Add("Policy", Policy);
                    }
                }
            #endregion
            }
        }
    }
