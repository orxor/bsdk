﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Internal;
using BinaryStudio.Services;

namespace BinaryStudio.Security.Cryptography
    {
    public abstract partial class CryptographicContext
        {
        #region M:AcquireContext(Oid,CryptographicContextFlags):CryptographicContext
        public static CryptographicContext AcquireContext(Oid algid, CryptographicContextFlags flags) {
            if (algid == null) { throw new ArgumentNullException(nameof(algid)); }
            EnsureAlgIdCache();
            if (PAlgId.TryGetValue(algid.Value,out var ptype))   { return new CryptographicContextI(ptype,flags); }
            if (!SAlgId.TryGetValue(algid.Value,out var nalgid)) { nalgid = OidToAlgId(algid); }
            var entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
            foreach (var type in RegisteredProviders) {
                if (entries.CryptAcquireContext(out var r, null, type.ProviderName, (Int32)type.ProviderType, (Int32)flags)) {
                    var context = new CryptographicContextI(r);
                    foreach (var alg in context.SupportedAlgorithms) {
                        if (alg.Key == nalgid) {
                            SAlgId[algid.Value] = nalgid;
                            PAlgId[algid.Value] = type.ProviderType;
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
            var entries = (CryptographicFunctions)CryptographicContext.DefaultContext.GetService(typeof(CryptographicFunctions));
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
        #region M:AcquireContext(X509Certificate,CRYPT_ACQUIRE_FLAGS):CryptographicContext
        public static CryptographicContext AcquireContext(X509Certificate certificate,CRYPT_ACQUIRE_FLAGS flags) {
            return new CryptographicContextI(certificate,flags);
            }
        #endregion

        #region M:DeleteContainer(CRYPT_PROVIDER_TYPE,String)
        public static void DeleteContainer(CRYPT_PROVIDER_TYPE providertype, String container) {
            var entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
            Validate(entries.CryptAcquireContext(out var r, container, null,
                (Int32)providertype,(Int32)CryptographicContextFlags.CRYPT_DELETEKEYSET));
            }
        #endregion

        private static readonly IDictionary<String,CRYPT_PROVIDER_TYPE> PAlgId = new ConcurrentDictionary<String, CRYPT_PROVIDER_TYPE>();
        private static IDictionary<String,ALG_ID> SAlgId;
        private static readonly Object SAlgIdO = new Object();
        private static void EnsureAlgIdCache() {
            lock (SAlgIdO) {
                if (SAlgId == null) {
                    SAlgId = new Dictionary<String,ALG_ID>();
                    var entries = (CryptographicFunctions)CryptographicContext.DefaultContext.GetService(typeof(CryptographicFunctions));
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
    }
