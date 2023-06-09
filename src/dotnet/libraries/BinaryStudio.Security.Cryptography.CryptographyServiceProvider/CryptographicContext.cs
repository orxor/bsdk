using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
#if !NET35 && !NET40
using System.Reflection;
#endif
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
#if FEATURE_SECURE_STRING_PASSWORD
using System.Security;
#endif
using BinaryStudio.DiagnosticServices;
#if !NET35 && !NET40
using BinaryStudio.DiagnosticServices.Logging;
#endif
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.CryptographyServiceProvider;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Internal;
using BinaryStudio.Services;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;
using Process=System.Diagnostics.Process;

namespace BinaryStudio.Security.Cryptography
    {
    using CERT_NAME_BLOB=CRYPT_BLOB;
    public abstract partial class CryptographicContext : CryptographicObject
        {
        public const String URI_GOST_CIPHER	                 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gost28147";
        public const String	URI_GOST_DIGEST	                 = "http://www.w3.org/2001/04/xmldsig-more#gostr3411";
        public const String	URI_GOST_HMAC_GOSTR3411	         = "http://www.w3.org/2001/04/xmldsig-more#hmac-gostr3411";
        public const String	URI_GOST_SIGN                    = "http://www.w3.org/2001/04/xmldsig-more#gostr34102001-gostr3411";
        public const String	URI_GOST_TRANSPORT               = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:transport-gost2001";
        public const String	URI_GOST_TRANSPORT_GOST_2012_256 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:transport-gost2012-256";
        public const String	URI_GOST_TRANSPORT_GOST_2012_512 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:transport-gost2012-512";
        public const String	URN_GOST_DIGEST                  = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr3411";
        public const String	URN_GOST_DIGEST_2012_256         = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256";
        public const String	URN_GOST_DIGEST_2012_512         = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-512";
        public const String	URN_GOST_HMAC_GOSTR3411          = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:hmac-gostr3411";
        public const String	URN_GOST_SIGN                    = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102001-gostr3411";
        public const String	URN_GOST_SIGN_2012_256           = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256";
        public const String	URN_GOST_SIGN_2012_512           = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-512";

        public static CryptographicContext DefaultContext { get { return (CryptographicContext)GlobalServices.GetService(typeof(CryptographicContext)); }}
        public static IEnumerable<RegisteredProviderInfo> RegisteredProviders { get { return DefaultContext.GetRegisteredProviders(); }}
        public static IDictionary<CRYPT_PROVIDER_TYPE,String> AvailableTypes { get { return DefaultContext.GetAvailableTypes(); }}
        public virtual IDictionary<ALG_ID,String> SupportedAlgorithms { get { return new ReadOnlyDictionary<ALG_ID,String>(new Dictionary<ALG_ID,String>()); }}
        public virtual String ProviderName { get { return "Auto"; }}
        public virtual String Container { get; }
        public virtual KEY_SPEC_TYPE KeySpec { get;internal set; }
        public virtual CRYPT_PROVIDER_TYPE ProviderType { get { return CRYPT_PROVIDER_TYPE.AUTO; }}
        public virtual Boolean IsMachineKeySet { get; }
        public virtual CryptographicContextFlags ProviderFlags { get; }
        public RequestSecureString RequestSecureString { get;set; }

        public virtual String SignatureOID { get { return GetParameter<String>(CRYPT_PARAM.PP_CP_SIGNATUREOID, 0, Encoding.ASCII); }}
        public virtual String CipherOID { get { return GetParameter<String>(CRYPT_PARAM.PP_CP_CIPHEROID, 0, Encoding.ASCII); }}
        public virtual String DiffieHellmanOID { get { return GetParameter<String>(CRYPT_PARAM.PP_CP_DHOID, 0, Encoding.ASCII); }}
        public virtual String HashOID { get { return GetParameter<String>(CRYPT_PARAM.PP_CP_HASHOID, 0, Encoding.ASCII); }}
        public virtual ALG_ID SignatureAlgId { get { return (ALG_ID)GetParameter<Int32>(CRYPT_PARAM.PP_SIGNATURE_ALG, 0, Encoding.ASCII); }}
        public virtual ALG_ID KeyExchangeAlgId { get { return (ALG_ID)GetParameter<Int32>(CRYPT_PARAM.PP_KEYEXCHANGE_ALG, 0, Encoding.ASCII); }}

        #region P:Keys:IEnumerable<CryptKey>
        public virtual IEnumerable<CryptKey> Keys { get {
            using (var contextA = new CryptographicContextI(this,
                CryptographicContextFlags.CRYPT_SILENT|CryptographicContextFlags.CRYPT_VERIFYCONTEXT| (IsMachineKeySet
                    ? CryptographicContextFlags.CRYPT_MACHINE_KEYSET
                    : CryptographicContextFlags.CRYPT_NONE))) {
                var c = contextA.GetParameter<String>(CRYPT_PARAM.PP_ENUMCONTAINERS, CRYPT_FIRST, Encoding.ASCII);
                while (c != null) {
                    Debug.WriteLine($"Container:{{{c}}}");
                    var contextB = new CryptographicContextI(this,c,
                        CryptographicContextFlags.CRYPT_SILENT| (IsMachineKeySet
                            ? CryptographicContextFlags.CRYPT_MACHINE_KEYSET
                            : CryptographicContextFlags.CRYPT_NONE));
                    yield return contextB.GetUserKey(
                        KEY_SPEC_TYPE.AT_KEYEXCHANGE|KEY_SPEC_TYPE.AT_SIGNATURE,
                        contextB.FullQualifiedContainerName ?? contextB.Container);
                    c = contextA.GetParameter<String>(CRYPT_PARAM.PP_ENUMCONTAINERS, CRYPT_NEXT, Encoding.ASCII);
                    }
                }
            }}
        #endregion
        #region P:FullQualifiedContainerName:String
        public String FullQualifiedContainerName { get {
            var r = GetParameter<String>(CRYPT_PARAM.PP_CP_FQCN, 0, Encoding.ASCII);
            return (r != null)
                    ? r
                    : UniqueContainer;
            }}
        #endregion
        #region P:UniqueContainer:String
        public String UniqueContainer { get {
            var r = GetParameter<String>(CRYPT_PARAM.PP_UNIQUE_CONTAINER, 0, Encoding.ASCII);
            return (r != null)
                    ? r
                    : Container;
            }}
        #endregion
        #if FEATURE_SECURE_STRING_PASSWORD
        #region P:SecureCode:SecureString
        public SecureString SecureCode {
            set
                {
                if (value != null) {
                    var i = Marshal.SecureStringToGlobalAllocAnsi(value);
                    try
                        {
                        for (;;) {
                            try
                                {
                                SetParameter(CRYPT_PARAM.PP_KEYEXCHANGE_PIN, i, 0);
                                return;
                                }
                            catch (ResourceIsBusyException)
                                {
                                Thread.Sleep(5000);
                                }
                            }
                        }
                    finally
                        {
                        Marshal.ZeroFreeGlobalAllocAnsi(i);
                        }
                    }
                else
                    {
                    SetParameter(CRYPT_PARAM.PP_KEYEXCHANGE_PIN, IntPtr.Zero, 0);
                    SetParameter(CRYPT_PARAM.PP_SIGNATURE_PIN, IntPtr.Zero, 0);
                    }
                }
            }
        #endregion
        #else
        #region P:SecureCode:String
        public String SecureCode {
            set
                {
                if (value != null) {
                    var i = Marshal.StringToHGlobalAnsi(value);
                    try
                        {
                        for (;;) {
                            try
                                {
                                SetParameter(CRYPT_PARAM.PP_KEYEXCHANGE_PIN, i, 0);
                                return;
                                }
                            catch (ResourceIsBusyException)
                                {
                                Thread.Sleep(5000);
                                }
                            }
                        }
                    finally
                        {
                        Marshal.FreeHGlobal(i);
                        }
                    }
                else
                    {
                    SetParameter(CRYPT_PARAM.PP_KEYEXCHANGE_PIN, IntPtr.Zero, 0);
                    SetParameter(CRYPT_PARAM.PP_SIGNATURE_PIN, IntPtr.Zero, 0);
                    }
                }
            }
        #endregion
        #endif
        #region P:Version:Version
        public virtual Version Version { get {
            var r = GetParameter<UInt32>(CRYPT_PARAM.PP_VERSION, 0, null);
            return new Version((Int32)(r & 0xFF00) >> 8, (Int32)(r & 0xFF));
            }}
        #endregion
        #region P:UseHardwareRNG:Boolean
        public virtual Boolean UseHardwareRNG {
            get { return GetParameter<Int32>(CRYPT_PARAM.PP_USE_HARDWARE_RNG, 0, null) != 0; }
            set { SetParameter(CRYPT_PARAM.PP_USE_HARDWARE_RNG,value ? 1 : 0,0); }
            }
        #endregion

        #region M:GetRegisteredProviders:IEnumerable<RegisteredProviderInfo>
        private IEnumerable<RegisteredProviderInfo> GetRegisteredProviders() {
            EnsureEntries(out var entries);
            var i = 0;
            var r = new Dictionary<String,CRYPT_PROVIDER_TYPE>();
            var builder = new StringBuilder(512);
            for (;;) {
                var sz = builder.Capacity;
                if (!entries.CryptEnumProviders(i, out var type, builder, ref sz)) {
                    var e = (Win32ErrorCode)GetLastWin32Error();
                    if (e == Win32ErrorCode.ERROR_MORE_DATA) {
                        builder.Capacity = sz + 1;
                        continue;
                        }
                    break;
                    }
                r.Add(builder.ToString(), (CRYPT_PROVIDER_TYPE)type);
                i++;
                }
            foreach (var o in r)
                {
                yield return new RegisteredProviderInfo(o.Value, o.Key);
                }
            }
        #endregion
        #region P:GetAvailableTypes:IDictionary<CRYPT_PROVIDER_TYPE,String>
        private IDictionary<CRYPT_PROVIDER_TYPE,String> GetAvailableTypes() {
            EnsureEntries(out var entries);
            var r = new Dictionary<CRYPT_PROVIDER_TYPE, String>();
            var i = 0;
            var builder = new StringBuilder(512);
            for (;;) {
                var sz = builder.Capacity;
                if (!entries.CryptEnumProviderTypes(i, out var type, builder, ref sz)) {
                    var e = (Win32ErrorCode)GetLastWin32Error();
                    if (e == Win32ErrorCode.ERROR_MORE_DATA) {
                        builder.Capacity = sz + 1;
                        continue;
                        }
                    break;
                    }
                r.Add((CRYPT_PROVIDER_TYPE)type, builder.ToString());
                i++;
                }
            return new ReadOnlyDictionary<CRYPT_PROVIDER_TYPE,String>(r);
            }
        #endregion
        #region M:GetCertificateChain(X509Certificate,IX509CertificateStorage,OidCollection,OidCollection,TimeSpan,DateTime,CERT_CHAIN_FLAGS,IntPtr)
        /// <summary>Builds a certificate chain context starting from an end certificate and going back, if possible, to a trusted root certificate.</summary>
        /// <param name="certificate">The end certificate, the certificate for which a chain is being built. This certificate context will be the zero-index element in the first simple chain.</param>
        /// <param name="store">The additional store to search for supporting certificates and certificate trust lists (CTLs). This parameter can be null if no additional store is to be searched.</param>
        /// <param name="applicationPolicy">Application policy.</param>
        /// <param name="issuancePolicy">Issuance policy.</param>
        /// <param name="timeout">Optional time, before revocation checking times out. This member is optional.</param>
        /// <param name="datetime">Indicates the time for which the chain is to be validated.</param>
        /// <param name="flags">Flag values that indicate special processing.</param>
        /// <param name="chainEngine">A handle of the chain engine.</param>
        /// <returns>Returns chain context created.</returns>
        private unsafe X509CertificateChainContext GetCertificateChain(X509Certificate certificate, IX509CertificateStorage store,
            OidCollection applicationPolicy, OidCollection issuancePolicy, TimeSpan timeout, DateTime datetime,
            CERT_CHAIN_FLAGS flags, IntPtr chainEngine)
            {
            var chainPara = new CERT_CHAIN_PARA {
                Size = sizeof(CERT_CHAIN_PARA)
                };

            CERT_CHAIN_CONTEXT* chainContext = null;
            var applicationPolicyHandle = LocalMemoryHandle.InvalidHandle;
            var certificatePolicyHandle = LocalMemoryHandle.InvalidHandle;
            try
                {
                if (!IsNullOrEmpty(applicationPolicy)) {
                    chainPara.RequestedUsage.Type = USAGE_MATCH_TYPE.USAGE_MATCH_TYPE_AND;
                    chainPara.RequestedUsage.Usage.UsageIdentifierCount = applicationPolicy.Count;
                    chainPara.RequestedUsage.Usage.UsageIdentifierArray = applicationPolicyHandle = CopyToMemory(applicationPolicy);
                    }
                #if CERT_CHAIN_PARA_HAS_EXTRA_FIELDS
                if (!IsNullOrEmpty(issuancePolicy)) {
                    chainPara.RequestedIssuancePolicy.Type = USAGE_MATCH_TYPE.USAGE_MATCH_TYPE_AND;
                    chainPara.RequestedIssuancePolicy.Usage.UsageIdentifierCount = issuancePolicy.Count;
                    chainPara.RequestedIssuancePolicy.Usage.UsageIdentifierArray = certificatePolicyHandle = CopyToMemory(issuancePolicy);
                    }
                #endif
                #if CERT_CHAIN_PARA_HAS_EXTRA_FIELDS
                chainPara.UrlRetrievalTimeout = (Int32)Math.Floor(timeout.TotalMilliseconds);
                #endif
                Validate(GetCertificateChain(chainEngine, certificate.Handle, datetime, (store != null) ? store.Handle : IntPtr.Zero, ref chainPara, flags, &chainContext));
                return new X509CertificateChainContext(chainContext);
                }
            finally
                {
                certificatePolicyHandle.Dispose();
                applicationPolicyHandle.Dispose();
                }
            }
        #endregion
        #region M:GetCertificateChain(X509Certificate,IX509CertificateStorage,CERT_CHAIN_FLAGS,DateTime):X509CertificateChainContext
        public X509CertificateChainContext GetCertificateChain(X509Certificate certificate, IX509CertificateStorage store,
            CERT_CHAIN_FLAGS flags,DateTime datetime)
            {
            return GetCertificateChain(certificate,store,null,null,TimeSpan.Zero,datetime,flags,IntPtr.Zero);
            }
        #endregion
        #region M:GetCertificateChain(X509Certificate,IX509CertificateStorage,CERT_CHAIN_FLAGS):X509CertificateChainContext
        public X509CertificateChainContext GetCertificateChain(X509Certificate certificate, IX509CertificateStorage store,CERT_CHAIN_FLAGS flags)
            {
            return GetCertificateChain(certificate,store,flags,DateTime.Now);
            }
        #endregion
        #region M:GetCertificateChain(X509Certificate,IX509CertificateStorage):X509CertificateChainContext
        public X509CertificateChainContext GetCertificateChain(X509Certificate certificate, IX509CertificateStorage store)
            {
            return GetCertificateChain(certificate,store,CERT_CHAIN_FLAGS.CERT_CHAIN_REVOCATION_CHECK_CHAIN);
            }
        #endregion
        #region M:VerifyObject(X509Certificate,CertificateChainPolicy)
        public virtual void VerifyObject(X509Certificate certificate,CertificateChainPolicy policy) {
            if (certificate == null) { throw new ArgumentNullException(nameof(certificate)); }
            if (policy == 0) { throw new ArgumentOutOfRangeException(nameof(policy)); }
            EnsureEntries(out var entries);
            (new X509CertificateChainPolicy(policy,entries)).Validate(GetCertificateChain(certificate,null),0);
            }
        #endregion

        #region M:Dispose(Boolean)
        /** <inheritdoc/> */
        protected override void Dispose(Boolean disposing) {
            if (disposing) {
                }
            }
        #endregion

        #region M:GetCertificateChain(IntPtr,IntPtr,DateTime,IntPtr,{ref}CERT_CHAIN_PARA,CERT_CHAIN_FLAGS,CERT_CHAIN_CONTEXT**):Boolean
        private unsafe Boolean GetCertificateChain(IntPtr chainEngine,
            IntPtr context, DateTime time, IntPtr additionalStore,
            ref CERT_CHAIN_PARA chainPara, CERT_CHAIN_FLAGS flags,
            CERT_CHAIN_CONTEXT** chainContext)
            {
            EnsureEntries(out var entries);
            var ft = default(FILETIME);
            *(Int64*)(&ft) = time.ToFileTime();
            return entries.CertGetCertificateChain(chainEngine,
                context, ref ft, additionalStore, ref chainPara, flags,
                chainContext);
            }
        #endregion
        #region M:CertOIDToAlgId(Oid):ALG_ID
        private ALG_ID CertOIDToAlgId(Oid value) {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            switch (value.Value) {
                case ObjectIdentifiers.szOID_NIST_sha256: { return ALG_ID.CALG_SHA_256; }
                case ObjectIdentifiers.szOID_NIST_sha384: { return ALG_ID.CALG_SHA_384; }
                case ObjectIdentifiers.szOID_NIST_sha512: { return ALG_ID.CALG_SHA_512; }
                case ObjectIdentifiers.szOID_CP_GOST_R3411_12_256: { return ALG_ID.CALG_GR3411_2012_256; }
                case ObjectIdentifiers.szOID_CP_GOST_R3411_12_512: { return ALG_ID.CALG_GR3411_2012_512; }
                }
            EnsureEntries(out var entries);
            return entries.CertOIDToAlgId(value.Value);
            }
        #endregion
        #region M:OidToAlgId(Oid):ALG_ID
        public static ALG_ID OidToAlgId(Oid value) {
            return DefaultContext.CertOIDToAlgId(value);
            }
        #endregion

        public void SignMessage(X509Certificate Certificate, Stream InputStream, out Byte[] Digest,out Byte[] Signature, RequestSecureString RequestSecureString) {
            if (Certificate == null) { throw new ArgumentNullException(nameof(Certificate)); }
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            EnsureEntries(out var entries);
            Digest = null;
            Signature = null;
            VerifyObject(Certificate,CertificateChainPolicy.CERT_CHAIN_POLICY_BASE);
            using (var context = RequestSigningSecureString(Certificate,RequestSecureString)) {
                using (var engine = new CryptHashAlgorithm(context, GetAlgId(Certificate.HashAlgorithm))) {
                    Digest = engine.Compute(InputStream);
                    var SignatureLength = 0;
                    Validate(entries.CryptSignHash(engine.Handle,Certificate.KeySpec, null, ref SignatureLength));
                    Validate(entries.CryptSignHash(engine.Handle,Certificate.KeySpec, Signature = new Byte[SignatureLength], ref SignatureLength));
                    }
                }
            }

        internal CryptographicContext RequestSigningSecureString(X509Certificate certificate,RequestSecureString RequestSecureString) {
            var flags = CRYPT_ACQUIRE_FLAGS.CRYPT_ACQUIRE_CACHE_FLAG;
            if (certificate.GetProperty(CERT_PROP_ID.CERT_KEY_PROV_INFO_PROP_ID, out var Info) == HRESULT.S_OK) {
                flags |= CRYPT_ACQUIRE_FLAGS.CRYPT_ACQUIRE_USE_PROV_INFO_FLAG;
                }
            if (ProviderFlags.HasFlag(CryptographicContextFlags.CRYPT_SILENT)) { flags |= CRYPT_ACQUIRE_FLAGS.CRYPT_ACQUIRE_SILENT_FLAG; }
            CryptographicContext r;
            try
                {
                r = AcquireContext(this,certificate,flags);
                }
            catch (CertificatePrivateKeyMissingException)
                {
                if (String.IsNullOrEmpty(certificate.Container)) { throw; }
                r = AcquireContext(ProviderType,certificate.Container,ProviderFlags & ~CryptographicContextFlags.CRYPT_VERIFYCONTEXT);
                r.KeySpec = certificate.KeySpec;
                }
            if (RequestSecureString != null) {
                var e = new RequestSecureStringEventArgs{
                    Info = certificate.Subject,
                    Container = r.FullQualifiedContainerName
                    };
                lock(StoredSecureStrings) {
                    if (StoredSecureStrings.TryGetValue(r.FullQualifiedContainerName,out var StoredSecureString)) {
                        r.SecureCode = StoredSecureString;
                        return r;
                        }
                    }
                var scode = RequestSecureString.GetSecureString(r, e);
                switch (scode) {
                    case HRESULT.PLA_S_PROPERTY_IGNORED: break;
                    case HRESULT.S_OK:
                        {
                        if (e.Canceled) { throw HResultException.GetExceptionForHR(HRESULT.COR_E_OPERATIONCANCELED); }
                        lock(StoredSecureStrings) {
                            StoredSecureStrings[r.FullQualifiedContainerName] = e.SecureString;
                            }
                        r.SecureCode = e.SecureString;
                        }
                        break;
                    default: throw HResultException.GetExceptionForHR(scode);
                    }
                }
            return r;
            }


        public ALG_ID FindAlgId(Oid AlgId, Int32 KeyType)
            {
            return (ALG_ID)(-1);
            }

        #region M:GetAlgId(String):ALG_ID
        internal static ALG_ID GetAlgId(String algid) {
            if (algid == null) { throw new ArgumentNullException(nameof(algid)); }
            if (String.IsNullOrEmpty(algid)) { throw new ArgumentOutOfRangeException(nameof(algid)); }
            ALG_ID r = 0;
            switch (algid) {
                case "Gost_34_11_2012_256_HashAlgorithm":
                case "Gost2012-256":
                case "ГОСТ Р 34.11-2012-256":
                case URN_GOST_DIGEST_2012_256:
                case ObjectIdentifiers.szOID_CP_GOST_R3411_12_256:
                    r = ALG_ID.CALG_GR3411_2012_256;
                    break;
                case "Gost_34_11_2012_512_HashAlgorithm":
                case "Gost2012-512":
                case "ГОСТ Р 34.11-2012-512":
                case URN_GOST_DIGEST_2012_512:
                case ObjectIdentifiers.szOID_CP_GOST_R3411_12_512:
                    r = ALG_ID.CALG_GR3411_2012_512;
                    break;
                case "Gost1994HashAlgorithm":
                case "Gost1994":
                case "ГОСТ Р 34.11-94":
                case URN_GOST_DIGEST:
                case ObjectIdentifiers.szOID_CP_GOST_R3411:
                    r = ALG_ID.CALG_GR3411;
                    break;
                default: throw new NotSupportedException("Неизвестный алгоритм");
                }
            return r;
            }
        #endregion
        #region M:GetAlgId(Oid):ALG_ID
        public static ALG_ID GetAlgId(Oid algid) {
            if (algid == null) { throw new ArgumentNullException(nameof(algid)); }
            return GetAlgId(algid.Value);
            }
        #endregion

        private class LDConfigItem
            {
            public String FileName { get; }
            public String FullPath { get; }
            public LDConfigItem(String source) {
                var regex = new Regex(@"(.+)\s+[(](.+)[)]\s+=>\s+(.+)");
                var match = regex.Match(source);
                if (match.Success) {
                    FileName = match.Groups[1].Value;
                    FullPath = match.Groups[3].Value;
                    Platform = match.Groups[2].Value.Split(new Char[]{','}, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToArray();
                    }
                }

            public Boolean IsValid(String platform) {
                if (Platform == null) { return false; }
                if (String.IsNullOrEmpty(platform)) { return true; }
                return Platform.Contains(platform);
                }

            public String[] Platform { get; }

            /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</summary>
            /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
            public override String ToString()
                {
                return $"{FileName}:[{String.Join(";", Platform)}]:{FullPath}";
                }
            }
        private static Dictionary<String, String> libraries;
        private static void LDConfigEnsure() {
            if (libraries != null) { return; }
            var r = new List<LDConfigItem>();
            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "/sbin/ldconfig",
                    Arguments = "-p",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                    }
                };
            process.Start();
            for (;;) {
                var line = process.StandardOutput.ReadLine();
                if (line == null) { break; }
                var i = new LDConfigItem(line.Trim());
                if (i.IsValid(String.Empty)) {
                    r.Add(i);
                    }
                }
            process.WaitForExit();
            libraries = new Dictionary<String, String>();
            foreach (var i in r) {
                if (i.IsValid("x86-64")) {
                    libraries[i.FileName] = i.FullPath;
                    }
                }
            }

        private const String ITCSLibraryPath = "/opt/itcs/lib";
        private const String ITCSLibrary = "/opt/itcs/lib/libcrypt32.so";
        [DllImport("c", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl, EntryPoint ="symlink")] private static extern Int32 SimLink(String source, String target);

        private static void Validate(Int32 r) {
            if (r == -1) {
                var e = HResultException.GetExceptionForHR((PosixError)Marshal.GetLastWin32Error());
                Console.WriteLine(Exceptions.ToString(e));
                }
            }

        #region M:GetParameter(CRYPT_PARAM,Int32):void*
        internal Byte[] GetParameter(CRYPT_PARAM key, Int32 flags) {
            EnsureEntries(out var entries);
            for (var i = 0x200;;) {
                var r = new Byte[i];
                if (entries.CryptGetProvParam(Handle, key, r, ref i, flags)) { return r; }
                var e = (Win32ErrorCode)GetLastWin32Error();
                if (e == Win32ErrorCode.ERROR_MORE_DATA)
                    {
                    continue;
                    }
                break;
                }
            return null;
            }
        #endregion
        #region M:GetParameter<T>(CRYPT_PARAM,Int32,Encoding):T
        internal unsafe T GetParameter<T>(CRYPT_PARAM key, Int32 flags, Encoding encoding) {
            var r = GetParameter(key, flags);
            if (r == null) { return default; }
            if (typeof(T) == typeof(String)) { return (T)(Object)encoding.GetString(r, 0, r.Length).TrimEnd('\0'); }
            fixed (Byte* i = r)
                {
                if (typeof(T) == typeof(Int32))  { return (T)(Object)(*(Int32*)i);  }
                if (typeof(T) == typeof(UInt32)) { return (T)(Object)(*(UInt32*)i); }
                if (typeof(T) == typeof(IntPtr)) { return (T)(Object)(*(IntPtr*)i); }
                }
            return default(T);
            }
        #endregion
        #region M:GetUserKey(KEY_SPEC_TYPE,String):CryptKey
        private CryptKey GetUserKey(KEY_SPEC_TYPE keyspec, String container)
            {
            EnsureEntries(out var entries);
            try
                {
                IntPtr r;
                     if (keyspec == KEY_SPEC_TYPE.AT_KEYEXCHANGE)  { Validate(entries.CryptGetUserKey(Handle, keyspec, out r)); }
                else if (keyspec == KEY_SPEC_TYPE.AT_SIGNATURE)    { Validate(entries.CryptGetUserKey(Handle, keyspec, out r)); }
                else
                     {
                     if (!entries.CryptGetUserKey(Handle, KEY_SPEC_TYPE.AT_KEYEXCHANGE, out r)) {
                        keyspec = KEY_SPEC_TYPE.AT_SIGNATURE;
                        Validate(entries.CryptGetUserKey(Handle,keyspec, out r));
                        }
                     else
                        {
                        keyspec = KEY_SPEC_TYPE.AT_KEYEXCHANGE;
                        }
                     }
                return new CryptKey(this,r,keyspec,container);
                }
            catch (Exception e)
                {
                e.Data.Add("KeySpec", keyspec);
                e.Data.Add("Container", container);
                throw;
                }
            }
        #endregion
        #region M:CryptGetKeyParam(IntPtr,KEY_PARAM,Byte[],{ref}Int32,Int32):Boolean
        internal Boolean CryptGetKeyParam(IntPtr Key, KEY_PARAM Param, Byte[] Data, ref Int32 DataSize, Int32 Flags) {
            EnsureEntries(out var entries);
            return entries.CryptGetKeyParam(Key,Param,Data,ref DataSize, Flags);
            }
        #endregion

        protected unsafe void SetParameter(CRYPT_PARAM parameter, Int32 value, Int32 flags) {
            EnsureEntries(out var entries);
            var r = new Byte[sizeof(Int32)];
            fixed (Byte* o = r) {
                *(Int32*)o = value;
                Validate(entries.CryptSetProvParam(Handle,parameter,r,flags));
                }
            }

        protected void SetParameter(CRYPT_PARAM parameter, IntPtr value, Int32 flags) {
            EnsureEntries(out var entries);
            try
                {
                Validate(entries.CryptSetProvParam(Handle,parameter,value,flags));
                }
            catch (Exception e)
                {
                e.Add("ParameterIndex",parameter);
                throw;
                }
            }

        internal Byte[] CertStrToName(String name) {
            EnsureEntries(out var entries);
            var size = 0;
            Byte[] r;
            Validate(entries.CertStrToName(X509_ASN_ENCODING,name,CERT_X500_NAME_STR,IntPtr.Zero, null,ref size,IntPtr.Zero));
            Validate(entries.CertStrToName(X509_ASN_ENCODING,name,CERT_X500_NAME_STR,IntPtr.Zero, r = new Byte[size],ref size,IntPtr.Zero));
            return r;
            }
        #region M:GetService(Type):Object
        /** <inheritdoc/> */
        public override Object GetService(Type service) {
            if (service == typeof(CryptographicFunctions)) { return DefaultContext.GetService(service); }
            return base.GetService(service);
            }
        #endregion
        #if FEATURE_SECURE_STRING_PASSWORD
        #region M:GetSecureString(String):SecureString
        public unsafe static SecureString GetSecureString(String value) {
            fixed (Char* c = value) {
                return new SecureString(c, value.Length);
                }
            }
        #endregion
        #else
        #region M:GetSecureString(String):String
        public static String GetSecureString(String value) {
            return value;
            }
        #endregion
        #endif
        #region M:Write(Stream,UInt32)
        private static unsafe void Write(Stream stream, UInt32 r) {
            var buffer = new Byte[sizeof(UInt32)];
            fixed (Byte* i = buffer) {
                *(UInt32*)i = r;
                }
            stream.Write(buffer,0,buffer.Length);
            }
        #endregion
        #region M:Write(Stream,Int32)
        private static unsafe void Write(Stream stream, Int32 r) {
            var buffer = new Byte[sizeof(Int32)];
            fixed (Byte* i = buffer) {
                *(Int32*)i = r;
                }
            stream.Write(buffer,0,buffer.Length);
            }
        #endregion
        #region M:Write(Stream,Byte[])
        private static unsafe void Write(Stream stream, Byte[] r) {
            stream.Write(r,0,r.Length);
            }
        #endregion
        #region M:ReadUInt32(Stream,{out}UInt32):HRESULT
        private static unsafe HRESULT ReadUInt32(Stream stream, out UInt32 r) {
            r = 0;
            var buffer = new Byte[sizeof(UInt32)];
            if (stream.Read(buffer,0, sizeof(UInt32)) == sizeof(UInt32)) {
                fixed (Byte* i = buffer) {
                    r = *(UInt32*)i;
                    return HRESULT.S_OK;
                    }
                }
            return HRESULT.CRYPT_E_STREAM_INSUFFICIENT_DATA;
            }
        #endregion
        #region M:ReadInt32(Stream,{out}Int32):HRESULT
        private static unsafe HRESULT ReadInt32(Stream stream, out Int32 r)
            {
            r = 0;
            var buffer = new Byte[sizeof(Int32)];
            if (stream.Read(buffer,0, sizeof(Int32)) == sizeof(Int32)) {
                fixed (Byte* i = buffer) {
                    r = *(Int32*)i;
                    return HRESULT.S_OK;
                    }
                }
            return HRESULT.CRYPT_E_STREAM_INSUFFICIENT_DATA;
            }
        #endregion
        #region M:ReadByte(Stream,{out}Byte):HRESULT
        private static HRESULT ReadByte(Stream stream, out Byte r)
            {
            r = 0;
            var buffer = new Byte[sizeof(Byte)];
            if (stream.Read(buffer,0, sizeof(Byte)) == sizeof(Byte)) {
                r = buffer[0];
                return HRESULT.S_OK;
                }
            return HRESULT.CRYPT_E_STREAM_INSUFFICIENT_DATA;
            }
        #endregion
        #region M:ReadPascalString(Stream,{out}String):HRESULT
        private HRESULT ReadPascalString(Stream stream, out String r) {
            return ReadPascalString(stream, Encoding.UTF8, out r);
            }
        #endregion
        #region M:ReadPascalString(Stream,Encoding,{out}String):HRESULT
        private HRESULT ReadPascalString(Stream stream, Encoding encoding, out String r) {
            r = null;
            Validate(ReadByte(stream, out var length));
            var buffer = new Byte[length];
            if (stream.Read(buffer, 0, length) == length) {
                r = encoding.GetString(buffer);
                return HRESULT.S_OK;
                }
            return HRESULT.CRYPT_E_STREAM_INSUFFICIENT_DATA;
            }
        #endregion
        #region M:ReadBlock(Stream,{out}Byte[]):HRESULT
        private HRESULT ReadBlock(Stream stream,out Byte[] r) {
            Validate(ReadInt32(stream, out var size));
            r = new Byte[size];
            if (stream.Read(r, 0, size) == size) { return HRESULT.S_OK; }
            r = null;
            return HRESULT.CRYPT_E_STREAM_INSUFFICIENT_DATA;
            }
        #endregion
        #region M:ReadBlock(Stream,Int32,{out}Byte[]):HRESULT
        private static HRESULT ReadBlock(Stream stream,Int32 size, out Byte[] r) {
            r = new Byte[size];
            if (stream.Read(r, 0, size) == size) { return HRESULT.S_OK; }
            r = null;
            return HRESULT.CRYPT_E_STREAM_INSUFFICIENT_DATA;
            }
        #endregion

        #if FEATURE_SECURE_STRING_PASSWORD
        private static readonly IDictionary<String,SecureString> StoredSecureStrings = new Dictionary<String,SecureString>();
        #else
        private static readonly IDictionary<String,String> StoredSecureStrings = new Dictionary<String,String>();
        #endif

        static CryptographicContext() {
            EncryptTaskCount = Is64BitProcess
                ? 32
                : 8;
            GlobalServices.ServiceResolve += ServiceResolve;
            }

        private static void ServiceResolve(Object sender, ServiceResolveEventArgs e) {
            if (e.RequestingService is Type type) {
                if ((type == typeof(CryptographicFunctions)) || (type == typeof(CryptographicContext))) {
                    #if !NET35 && !NET40
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                        LDConfigEnsure();
                        #region {ViPNet CSP}
                        if (File.Exists(ITCSLibrary)) {
                            if (!libraries.Any(i => String.Equals(i.Value,ITCSLibrary,StringComparison.OrdinalIgnoreCase))) {
                                var AssemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                                var LDLibraryPath = new List<String>((Environment.GetEnvironmentVariable("LD_LIBRARY_PATH")??String.Empty).Split(new []{':' },StringSplitOptions.RemoveEmptyEntries));
                                if (!LDLibraryPath.Contains(ITCSLibraryPath)) {
                                    throw new InvalidProgramException($"ViPNet CSP installed at '{ITCSLibraryPath}' but does not configured for run-time bindings. Use {{ldconfig}} to configure dynamic linker run-time bindings or set {{LD_LIBRARY_PATH}} environment variable to specify library path explicitly.");
                                    }
                                }
                            e.Service = new CryptographicContextP();
                            return;
                            }
                        #endregion
                        #region {Crypto PRO CSP}
                        if (File.Exists("/etc/opt/cprocsp/config64.ini")) {
                            var cnfig = File.ReadAllText("/etc/opt/cprocsp/config64.ini");
                            var regex = new Regex(@"[""]libcapi20[.]so[""]\p{Zs}*[=]\p{Zs}*[""](.+libcapi20[.]so)[""]\n");
                            var match = regex.Match(cnfig);
                            if (match.Success) {
                                var capiso = match.Groups[1].Value;
                                if (File.Exists(capiso)) {
                                    PlatformContext.Logger.Log(LogLevel.Information, $"library:{capiso}");
                                    e.Service = new CryptographicContextC();
                                    return;
                                    }
                                }
                            }
                        #endregion
                        return;
                        }
                    #endif
                    e.Service = new CryptographicContextS();
                    }
                }
            }

        #region M:CopyToMemory(OidCollection):LocalMemoryHandle
        private static unsafe LocalMemoryHandle CopyToMemory(OidCollection values) {
            var items = values.OfType<Oid>().Select(i => i.Value).ToArray();
            var n = items.Length*IntPtr.Size;
            var offset = n;
            foreach (var i in items) {
                n += i.Length + 1;
                }
            var r = LocalMemoryHandle.Alloc(n);
            var p = (IntPtr)r;
            var c = p;
            for (var i = 0; i < items.Length; i++) {
                *(IntPtr*)(void*)c = p + offset;
                for (var j = 0; j < items[i].Length; j++) {
                    *(Byte*)(p + offset) = (Byte)items[i][j];
                    ++offset;
                    }
                *(Byte*)(p + offset) = 0;
                ++offset;
                c += IntPtr.Size;
                }
            return r;
            }
        #endregion
        #region M:EnsureEntries
        private CryptographicFunctions Entries;
        internal virtual void EnsureEntries(out CryptographicFunctions entries) {
            if (Entries == null) {
                Entries = (CryptographicFunctions)GetService(typeof(CryptographicFunctions));
                }
            entries = Entries;
            }
        #endregion

        private const UInt32 CMSG_INDEFINITE_LENGTH = 0xFFFFFFFF;
        private const Int32 SIGNATURE_BUFFER_SIZE = 64*1024;
        private const Int32 CRYPT_FIRST = 1;
        private const Int32 CRYPT_NEXT  = 2;
        }
    }
