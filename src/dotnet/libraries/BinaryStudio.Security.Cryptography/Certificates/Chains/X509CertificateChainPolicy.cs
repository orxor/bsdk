using BinaryStudio.PlatformComponents.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public partial class X509CertificateChainPolicy
        {
        private ICryptoAPI Entries;
        private X509CertificateChainPolicy Source;
        public CertificateChainPolicy Policy { get; }
        internal X509CertificateChainPolicy(CertificateChainPolicy policy, ICryptoAPI entries)
            {
            if (entries == null) { throw new ArgumentNullException(nameof(entries)); }
            Entries = entries;
            Policy = policy;
            EnsureSource();
            }

        #region M:ValidatePolicy(CertificateChainPolicy)
        protected virtual void ValidatePolicy(CertificateChainPolicy policy) {
            if ((policy >= CertificateChainPolicy.CERT_CHAIN_POLICY_BASE) && (policy <= CertificateChainPolicy.CERT_CHAIN_POLICY_SSL_KEY_PIN)) { return; }
            if ((policy == CertificateChainPolicy.CERT_CHAIN_POLICY_ICAO)) { return; }
            throw new ArgumentOutOfRangeException(nameof(policy));
            }
        #endregion
        #region M:Validate(X509Certificate)
        public virtual void Validate(X509CertificateChainContext context) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            }
        #endregion
        #region M:EnsureSource
        protected virtual void EnsureSource() {
            ValidatePolicy(Policy);
            if (Source == null) {
                switch (Policy) {
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_BASE:
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_AUTHENTICODE:
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_AUTHENTICODE_TS:
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_SSL:
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_BASIC_CONSTRAINTS:
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_NT_AUTH:
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_MICROSOFT_ROOT:
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_EV:
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_SSL_F12:
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_SSL_HPKP_HEADER:
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_THIRD_PARTY_ROOT:
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_SSL_KEY_PIN:
                        {
                        Source = new EX509CertificateChainPolicy(Policy,Entries);
                        }
                        break;
                    case CertificateChainPolicy.CERT_CHAIN_POLICY_ICAO:
                        {
                        Source = new IX509CertificateChainPolicy(Policy,Entries);
                        }
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(Policy));
                    }
                }
            }
        #endregion
        }
    }
