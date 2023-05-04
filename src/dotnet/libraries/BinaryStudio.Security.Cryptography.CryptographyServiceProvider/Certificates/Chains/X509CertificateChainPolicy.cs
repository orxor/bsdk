using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Services;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public partial class X509CertificateChainPolicy
        {
        internal CryptographicFunctions Entries;
        private X509CertificateChainPolicy Source;
        public CertificateChainPolicy Policy { get; }
        internal X509CertificateChainPolicy(CertificateChainPolicy policy, CryptographicFunctions entries)
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
                e = HResultException.GetExceptionForHR((HRESULT)Marshal.GetLastWin32Error());
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
                throw HResultException.GetExceptionForHR(hr);
                }
            }
        #endregion
        #region M:Validate(Boolean)
        protected virtual void Validate(Boolean status) {
            if (!status) {
                var e = HResultException.GetExceptionForHR((HRESULT)Marshal.GetLastWin32Error());
                #if DEBUG
                Debug.Print($"Validate:{e.Message}");
                #endif
                throw e;
                }
            }
        #endregion

        #region M:ExceptionForStatus(CertificateChainErrorStatus):Exception
        private static Exception ExceptionForStatus(CertificateChainErrorStatus status) {
            var e = (CryptographicException)Activator.CreateInstance(types[status],
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.NonPublic,
                null,
                null,
                null);
            e.Data["Status"] = status;
            return e.SetStackTrace(new StackTrace(true));
            }
        #endregion
        #region M:ExceptionForStatus(HRESULT,CertificateChainErrorStatus,UInt32):Exception
        protected static Exception ExceptionForStatus(HRESULT scode, CertificateChainErrorStatus status, UInt32 mask) {
            var o = ((Int32)status) & mask;
            var r = new List<Exception>();
            if ((o & (Int32)CertificateChainErrorStatus.TrustIsNotTimeValid)                          != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.TrustIsNotTimeValid));                          o &= ~(Int32)CertificateChainErrorStatus.TrustIsNotTimeValid;                          }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_IS_NOT_TIME_NESTED)                != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_IS_NOT_TIME_NESTED));                o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_IS_NOT_TIME_NESTED;                }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_IS_REVOKED)                        != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_IS_REVOKED));                        o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_IS_REVOKED;                        }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_IS_NOT_SIGNATURE_VALID)            != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_IS_NOT_SIGNATURE_VALID));            o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_IS_NOT_SIGNATURE_VALID;            }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_IS_NOT_VALID_FOR_USAGE)            != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_IS_NOT_VALID_FOR_USAGE));            o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_IS_NOT_VALID_FOR_USAGE;            }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_IS_UNTRUSTED_ROOT)                 != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_IS_UNTRUSTED_ROOT));                 o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_IS_UNTRUSTED_ROOT;                 }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_REVOCATION_STATUS_UNKNOWN)         != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_REVOCATION_STATUS_UNKNOWN));         o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_REVOCATION_STATUS_UNKNOWN;         }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_IS_CYCLIC)                         != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_IS_CYCLIC));                         o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_IS_CYCLIC;                         }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_INVALID_EXTENSION)                 != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_INVALID_EXTENSION));                 o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_INVALID_EXTENSION;                 }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_INVALID_POLICY_CONSTRAINTS)        != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_INVALID_POLICY_CONSTRAINTS));        o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_INVALID_POLICY_CONSTRAINTS;        }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_INVALID_BASIC_CONSTRAINTS)         != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_INVALID_BASIC_CONSTRAINTS));         o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_INVALID_BASIC_CONSTRAINTS;         }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_INVALID_NAME_CONSTRAINTS)          != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_INVALID_NAME_CONSTRAINTS));          o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_INVALID_NAME_CONSTRAINTS;          }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_SUPPORTED_NAME_CONSTRAINT) != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_SUPPORTED_NAME_CONSTRAINT)); o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_SUPPORTED_NAME_CONSTRAINT; }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_DEFINED_NAME_CONSTRAINT)   != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_DEFINED_NAME_CONSTRAINT));   o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_DEFINED_NAME_CONSTRAINT;   }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_PERMITTED_NAME_CONSTRAINT) != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_PERMITTED_NAME_CONSTRAINT)); o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_PERMITTED_NAME_CONSTRAINT; }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_EXCLUDED_NAME_CONSTRAINT)      != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_EXCLUDED_NAME_CONSTRAINT));      o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_EXCLUDED_NAME_CONSTRAINT;      }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_IS_OFFLINE_REVOCATION)             != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_IS_OFFLINE_REVOCATION));             o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_IS_OFFLINE_REVOCATION;             }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_NO_ISSUANCE_CHAIN_POLICY)          != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_NO_ISSUANCE_CHAIN_POLICY));          o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_NO_ISSUANCE_CHAIN_POLICY;          }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_IS_EXPLICIT_DISTRUST)              != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_IS_EXPLICIT_DISTRUST));              o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_IS_EXPLICIT_DISTRUST;              }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_SUPPORTED_CRITICAL_EXT)    != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_SUPPORTED_CRITICAL_EXT));    o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_SUPPORTED_CRITICAL_EXT;    }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_IS_PARTIAL_CHAIN)                  != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_IS_PARTIAL_CHAIN));                  o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_IS_PARTIAL_CHAIN;                  }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_TIME_VALID)             != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_TIME_VALID));             o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_TIME_VALID;             }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_SIGNATURE_VALID)        != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_SIGNATURE_VALID));        o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_SIGNATURE_VALID;        }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_VALID_FOR_USAGE)        != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_VALID_FOR_USAGE));        o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_CTL_IS_NOT_VALID_FOR_USAGE;        }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_WEAK_SIGNATURE)                != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_WEAK_SIGNATURE));                o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_WEAK_SIGNATURE;                }
            if ((o & (Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_WEAK_HYGIENE)                  != 0) { r.Add(ExceptionForStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_WEAK_HYGIENE));                  o &= ~(Int32)CertificateChainErrorStatus.CERT_TRUST_HAS_WEAK_HYGIENE;                  }
            if (o != 0) { throw new NotSupportedException(); }
            return ((r.Count == 1)
                ? r[0]
                : (new CertificateException(scode,r)).SetStackTrace(new StackTrace(true)));
            }
        #endregion
        #region M:ExceptionForStatus(HRESULT,X509CertificateChainContext):Exception
        protected static Exception ExceptionForStatus(HRESULT scode,X509CertificateChainContext context) {
            var r = new List<Exception>();
            foreach (var chainS in context) {
                var o = new List<Exception>();
                foreach (var chainE in chainS)
                    {
                    if (chainE.ErrorStatus == 0) { continue; }
                    o.Add(ExceptionForStatus(scode,chainE.ErrorStatus,0xffffffff)
                        .Add("ChainElement",chainE));
                    }
                if (o.Count > 0) {
                    r.Add(((o.Count == 1)
                        ? o[0]
                        : (new CertificateException(scode,o)).SetStackTrace(new StackTrace(true)))
                        .Add("ChainIndex",chainS.ChainIndex));
                    }
                }
            return ((r.Count == 1)
                ? r[0]
                : new CertificateException(scode,r));
            }
        #endregion

        private static readonly IDictionary<CertificateChainErrorStatus,Type> types = new Dictionary<CertificateChainErrorStatus,Type>();
        static X509CertificateChainPolicy() {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(Exception)))) {
                var status = (CertificateChainErrorStatusAttribute)type.GetCustomAttributes(typeof(CertificateChainErrorStatusAttribute),false).FirstOrDefault();
                if (status != null) {
                    try
                        {
                        types.Add(status.Status,type);
                        }
                    catch(ArgumentException e)
                        {
                        e.Data["Key"] = status.Status;
                        e.Data["Type"] = type.FullName;
                        if (types.TryGetValue(status.Status, out var r)) {
                            e.Data["ExistingType"] = r.FullName;
                            }
                        throw;
                        }
                    }
                }
            }
        }
    }
