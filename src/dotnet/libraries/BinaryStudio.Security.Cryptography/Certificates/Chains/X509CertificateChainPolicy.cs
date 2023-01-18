using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    using HRESULT=HResult;
    public partial class X509CertificateChainPolicy
        {
        internal ICryptoAPI Entries;
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
        #region M:Validate(X509Certificate,CERT_CHAIN_POLICY_FLAGS)
        public virtual void Validate(X509CertificateChainContext context,CERT_CHAIN_POLICY_FLAGS flags) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            Source.Validate(context,flags);
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
        #region M:Validate([Out]Exception,Boolean):Boolean
        protected virtual Boolean Validate(out Exception e, Boolean status) {
            e = null;
            if (!status) {
                e = HResultException.GetExceptionForHR(Marshal.GetLastWin32Error());
                #if DEBUG
                Debug.Print($"Validate:{e.Message}");
                #endif
                return false;
                }
            return true;
            }
        #endregion
        #region M:Validate(HRESULT)
        protected virtual void Validate(HRESULT hr) {
            if (hr != HRESULT.S_OK) {
                throw HResultException.GetExceptionForHR((Int32)hr);
                }
            }
        #endregion
        #region M:Validate(Boolean)
        protected virtual void Validate(Boolean status) {
            if (!status) {
                var e = HResultException.GetExceptionForHR(Marshal.GetLastWin32Error());
                #if DEBUG
                Debug.Print($"Validate:{e.Message}");
                #endif
                throw e;
                }
            }
        #endregion
        }
    }
