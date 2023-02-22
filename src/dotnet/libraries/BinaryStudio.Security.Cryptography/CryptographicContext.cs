using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
#if LINUX
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.DiagnosticServices.Logging;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
#endif
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents;
using BinaryStudio.Security.Cryptography.CryptographyServiceProvider;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Internal;
using BinaryStudio.Serialization;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    #if LINUX
    using Process=System.Diagnostics.Process;
    #endif
    public abstract class CryptographicContext : CryptographicObject
        {
        public static CryptographicContext DefaultContext { get; }
        public static IEnumerable<RegisteredProviderInfo> RegisteredProviders { get { return DefaultContext.GetRegisteredProviders(); }}
        public static IDictionary<CRYPT_PROVIDER_TYPE,String> AvailableTypes { get { return DefaultContext.GetAvailableTypes(); }}
        public virtual IDictionary<ALG_ID,String> SupportedAlgorithms { get { return new ReadOnlyDictionary<ALG_ID,String>(new Dictionary<ALG_ID,String>()); }}
        public virtual String ProviderName { get { return "Auto"; }}
        public virtual String Container { get; }
        public virtual KEY_SPEC_TYPE KeySpec { get; }
        public virtual CRYPT_PROVIDER_TYPE ProviderType { get { return CRYPT_PROVIDER_TYPE.AUTO; }}
        public virtual Boolean IsMachineKeySet { get; }
        public virtual CryptographicContextFlags ProviderFlags { get; }

        #region P:Version:Version
        public virtual Version Version { get {
            var r = GetParameter<UInt32>(CRYPT_PARAM.PP_VERSION, 0, null);
            return new Version((Int32)(r & 0xFF00) >> 8, (Int32)(r & 0xFF));
            }}
        #endregion
        #region P:Keys:IEnumerable<CryptKey>
        public virtual IEnumerable<CryptKey> Keys { get {
            using (var contextA = new CryptographicContextI(this,
                CryptographicContextFlags.CRYPT_SILENT|CryptographicContextFlags.CRYPT_VERIFYCONTEXT| (IsMachineKeySet
                    ? CryptographicContextFlags.CRYPT_MACHINE_KEYSET
                    : CryptographicContextFlags.CRYPT_NONE))) {
                var c = contextA.GetParameter<String>(CRYPT_PARAM.PP_ENUMCONTAINERS, CRYPT_FIRST, Encoding.ASCII);
                while (c != null) {
                    using (var contextB = new CryptographicContextI(this,c,
                        CryptographicContextFlags.CRYPT_SILENT| (IsMachineKeySet
                            ? CryptographicContextFlags.CRYPT_MACHINE_KEYSET
                            : CryptographicContextFlags.CRYPT_NONE))) {
                        yield return contextB.GetUserKey(
                            KEY_SPEC_TYPE.AT_KEYEXCHANGE|KEY_SPEC_TYPE.AT_SIGNATURE,
                            contextB.FullQualifiedContainerName);
                        }
                    c = contextA.GetParameter<String>(CRYPT_PARAM.PP_ENUMCONTAINERS, CRYPT_NEXT, Encoding.ASCII);
                    }
                }
            }}
        #endregion
        #region P:FullQualifiedContainerName:String
        public String FullQualifiedContainerName { get {
            var r = GetParameter<String>(CRYPT_PARAM.PP_FQCN, 0, Encoding.ASCII);
            return (r != null)
                    ? r
                    : null;
            }}
        #endregion
        #region P:UniqueContainer:String
        public String UniqueContainer { get {
            var r = GetParameter<String>(CRYPT_PARAM.PP_UNIQUE_CONTAINER, 0, Encoding.ASCII);
            return (r != null)
                    ? r
                    : null;
            }}
        #endregion
        #region P:SecureCode:SecureString
        public SecureString SecureCode {
            set
                {
                throw new NotImplementedException();
                }
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
                if (!entries.CryptEnumProviders(i, IntPtr.Zero, 0, out var type, builder, ref sz)) {
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
                if (!entries.CryptEnumProviderTypes(i, IntPtr.Zero, 0, out var type, builder, ref sz)) {
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
        #region M:GetCertificateChain(X509Certificate,X509CertificateStorage,OidCollection,OidCollection,TimeSpan,DateTime,CERT_CHAIN_FLAGS,IntPtr)
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
        private unsafe X509CertificateChainContext GetCertificateChain(X509Certificate certificate, X509CertificateStorage store,
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
        #region M:GetCertificateChain(X509Certificate,X509CertificateStorage,CERT_CHAIN_FLAGS,DateTime):X509CertificateChainContext
        public X509CertificateChainContext GetCertificateChain(X509Certificate certificate, X509CertificateStorage store,
            CERT_CHAIN_FLAGS flags,DateTime datetime)
            {
            return GetCertificateChain(certificate,store,null,null,TimeSpan.Zero,datetime,flags,IntPtr.Zero);
            }
        #endregion
        #region M:GetCertificateChain(X509Certificate,X509CertificateStorage,CERT_CHAIN_FLAGS):X509CertificateChainContext
        public X509CertificateChainContext GetCertificateChain(X509Certificate certificate, X509CertificateStorage store,CERT_CHAIN_FLAGS flags)
            {
            return GetCertificateChain(certificate,store,flags,DateTime.Now);
            }
        #endregion
        #region M:GetCertificateChain(X509Certificate,X509CertificateStorage):X509CertificateChainContext
        public X509CertificateChainContext GetCertificateChain(X509Certificate certificate, X509CertificateStorage store)
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
        #region M:VerifyAttachedMessageSignature(Stream)
        public virtual void VerifyAttachedMessageSignature(Stream InputStream) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            VerifyAttachedMessageSignature(InputStream,null,out var signers);
            }
        #endregion
        #region M:VerifyAttachedMessageSignature(Stream,Stream,{out}IList<X509Certificate>)
        public virtual void VerifyAttachedMessageSignature(Stream InputStream,Stream OutputStream,out IList<X509Certificate> Signers)
            {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            Signers = EmptyList<X509Certificate>.Value;
            using (var message = CryptographicMessage.OpenToDecode((Bytes,Final)=> {
                OutputStream?.Write(Bytes,0,Bytes.Length);
                }))
                {
                var Block = new Byte[SIGNATURE_BUFFER_SIZE];
                for (;;) {
                    Yield();
                    var sz = InputStream.Read(Block, 0, Block.Length);
                    if (sz == 0) { break; }
                    message.Update(Block, sz, false);
                    }
                message.Update(EmptyArray<Byte>.Value, 0, true);
                using (var store = new MessageCertificateStorage(message.Handle)) {
                    for (var signerindex = 0;; signerindex++) {
                        var r = message.GetParameter(CMSG_PARAM.CMSG_SIGNER_CERT_INFO_PARAM, signerindex, out var hr);
                        if (r.Length != 0) {
                            unsafe {
                                fixed (Byte* blob = r) {
                                    var digest    = message.GetParameter(CMSG_PARAM.CMSG_COMPUTED_HASH_PARAM, signerindex);
                                    var encdigest = message.GetParameter(CMSG_PARAM.CMSG_ENCRYPTED_DIGEST,    signerindex);
                                    #if DEBUG
                                    #if NET35
                                    Debug.Print("SIGNER_{0}:CMSG_COMPUTED_HASH_PARAM:{1}", signerindex, String.Join(String.Empty, digest.Select(i => i.ToString("X2")).ToArray()));
                                    Debug.Print("SIGNER_{0}:CMSG_ENCRYPTED_DIGEST:[{2}]{1}", signerindex, String.Join(String.Empty, encdigest.Select(i => i.ToString("X2")).ToArray()), encdigest.Length);
                                    #else
                                    Debug.Print("SIGNER_{0}:CMSG_COMPUTED_HASH_PARAM:{1}", signerindex, String.Join(String.Empty, digest.ToString("X")));
                                    Debug.Print("SIGNER_{0}:CMSG_ENCRYPTED_DIGEST:[{2}]{1}", signerindex, String.Join(String.Empty, encdigest.ToString("X")), encdigest.Length);
                                    #endif
                                    #endif
                                    var certinfo = (CERT_INFO*)blob;
                                    var certificate = store.Find(certinfo);
                                    if (certificate == null) { throw new Exception(); }
                                    if (certificate != null) {
                                        if (!message.Control(
                                            CRYPT_MESSAGE_FLAGS.CRYPT_MESSAGE_NONE,
                                            CMSG_CTRL.CMSG_CTRL_VERIFY_SIGNATURE,
                                            (IntPtr)((CERT_CONTEXT*)certificate.Handle)->CertInfo))
                                            {
                                            throw HResultException.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
                                            }
                                        }
                                    }
                                }
                            }
                        else
                            {
                            if (hr == HRESULT.CRYPT_E_INVALID_INDEX) { break; }
                            throw HResultException.GetExceptionForHR((Int32)hr);
                            }
                        }
                    }
                }
            }
        #endregion

        #region M:Dispose(Boolean)
        /// <summary>
        /// Releases the unmanaged resources used by the instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
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
                IntPtr.Zero, chainContext);
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
        public ALG_ID FindAlgId(Oid AlgId, Int32 KeyType)
            {
            return (ALG_ID)(-1);
            }

        #if LINUX
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
        #endif

        #region M:GetParameter(CRYPT_PARAM,Int32):void*
        internal Byte[] GetParameter(CRYPT_PARAM key, Int32 flags) {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
            }
        #endregion
        #region M:CryptGetKeyParam(IntPtr,KEY_PARAM,Byte[],{ref}Int32,Int32):Boolean
        internal Boolean CryptGetKeyParam(IntPtr Key, KEY_PARAM Param, Byte[] Data, ref Int32 DataSize, Int32 Flags) {
            EnsureEntries(out var entries);
            return entries.CryptGetKeyParam(Key,Param,Data,ref DataSize, Flags);
            }
        #endregion

        static CryptographicContext() {
            #if LINUX
            LDConfigEnsure();
            #region {ViPNet CSP}
            if (File.Exists(ITCSLibrary)) {
                if (!libraries.Any(i => String.Equals(i.Value,ITCSLibrary,StringComparison.OrdinalIgnoreCase))) {
                    var AssemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var LDLibraryPath = new List<String>((Environment.GetEnvironmentVariable("LD_LIBRARY_PATH")??String.Empty).Split(':',StringSplitOptions.RemoveEmptyEntries));
                    if (!LDLibraryPath.Contains(ITCSLibraryPath)) {
                        throw new InvalidProgramException($"ViPNet CSP installed at '{ITCSLibraryPath}' but does not configured for run-time bindings. Use {{ldconfig}} to configure dynamic linker run-time bindings or set {{LD_LIBRARY_PATH}} environment variable to specify library path explicitly.");
                        }
                    }
                DefaultContext= new PCryptographicContext();
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
                        DefaultContext= new CCryptographicContext();
                        }
                    }
                }
            #endregion
            #else
            DefaultContext= new SCryptographicContext();
            #endif
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
        private ICryptoAPI Entries;
        internal virtual void EnsureEntries(out ICryptoAPI entries) {
            if (Entries == null) {
                Entries = (ICryptoAPI)GetService(typeof(ICryptoAPI));
                }
            entries = Entries;
            }
        #endregion

        private const Int32 SIGNATURE_BUFFER_SIZE = 64*1024;
        private const Int32 CRYPT_FIRST = 1;
        private const Int32 CRYPT_NEXT  = 2;
        }
    }
