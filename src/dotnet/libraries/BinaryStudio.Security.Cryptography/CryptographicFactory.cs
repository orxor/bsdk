using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Internal;

namespace BinaryStudio.Security.Cryptography
    {
    public abstract partial class CryptographicContext
        {
        #region M:AcquireContext(Oid,CryptographicContextFlags):CryptographicContext
        public static CryptographicContext AcquireContext(Oid algid, CryptographicContextFlags flags) {
            if (algid == null) { throw new ArgumentNullException(nameof(algid)); }
            EnsureAlgIdCache();
            if (!SAlgId.TryGetValue(algid.Value,out var nalgid)) { nalgid = CryptographicContext.OidToAlgId(algid); }
            var entries = (ICryptoAPI)CryptographicContext.DefaultContext.GetService(typeof(ICryptoAPI));
            foreach (var type in CryptographicContext.RegisteredProviders) {
                if (entries.CryptAcquireContext(out var r, null, type.ProviderName, (Int32)type.ProviderType, (Int32)flags)) {
                    var context = new CryptographicContextI(r);
                    foreach (var alg in context.SupportedAlgorithms) {
                        if (alg.Key == nalgid) {
                            return context;
                            }
                        }
                    }
                }
            return null;
            }
        #endregion
        #region M:AcquireContext(String,String,CRYPT_PROVIDER_TYPE,CryptographicContextFlags):CryptographicContext
        public static CryptographicContext AcquireContext(String container, String provider, CRYPT_PROVIDER_TYPE providertype, CryptographicContextFlags flags) {
            var entries = (ICryptoAPI)CryptographicContext.DefaultContext.GetService(typeof(ICryptoAPI));
            Validate(entries.CryptAcquireContext(out var r,container,provider,(Int32)providertype,(Int32)flags));
            return new CryptographicContextI(r);
            }
        #endregion
        #region M:AcquireContext(CRYPT_PROVIDER_TYPE,CryptographicContextFlags):CryptographicContext
        public static CryptographicContext AcquireContext(CRYPT_PROVIDER_TYPE providertype, CryptographicContextFlags flags) {
            return new CryptographicContextI(providertype,flags);
            }
        #endregion
        #region M:AcquireContext(CRYPT_PROVIDER_TYPE,String,CryptographicContextFlags):CryptographicContext
        public static CryptographicContext AcquireContext(CRYPT_PROVIDER_TYPE providertype, String container, CryptographicContextFlags flags) {
            return new CryptographicContextI(container,null,providertype,flags);
            }
        #endregion
        #region M:AcquireContext(CryptographicContext,X509Certificate,CRYPT_ACQUIRE_FLAGS):CryptographicContext
        public static CryptographicContext AcquireContext(CryptographicContext context, X509Certificate certificate,CRYPT_ACQUIRE_FLAGS flags) {
            return new CryptographicContextI(context,certificate,flags);
            }
        #endregion

        private static IDictionary<String,ALG_ID> SAlgId;
        private static void EnsureAlgIdCache() {
            if (SAlgId == null) {
                SAlgId = new Dictionary<String,ALG_ID>();
                var entries = (ICryptoAPI)CryptographicContext.DefaultContext.GetService(typeof(ICryptoAPI));
                entries.CryptEnumOIDInfo(CRYPT_ALG_OID_GROUP_ID.CRYPT_SIGN_ALG_OID_GROUP_ID, IntPtr.Zero, delegate(IntPtr info,IntPtr arg) {
                    unsafe
                        {
                        var i = (CRYPT_OID_INFO*)info;
                        if (i->OID != IntPtr.Zero) {
                            var o = Marshal.PtrToStringAnsi(i->OID);
                            SAlgId[o] = *(ALG_ID*)i->ExtraInfo.Data;
                            }
                        }
                    return true;
                    });
                SAlgId[ObjectIdentifiers.szOID_ECDSA_SHA1]   = ALG_ID.CALG_ECDSA;
                SAlgId[ObjectIdentifiers.szOID_ECDSA_SHA256] = ALG_ID.CALG_ECDSA;
                SAlgId[ObjectIdentifiers.szOID_ECDSA_SHA384] = ALG_ID.CALG_ECDSA;
                SAlgId[ObjectIdentifiers.szOID_ECDSA_SHA512] = ALG_ID.CALG_ECDSA;
                SAlgId[ObjectIdentifiers.szOID_ECDSA_SHA224] = ALG_ID.CALG_ECDSA;
                }
            }
        }
    }
