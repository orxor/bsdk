using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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
            EnsureEntries();
            (new X509CertificateChainPolicy(policy,Entries)).Validate(GetCertificateChain(certificate,null),0);
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
            EnsureEntries();
            var ft = default(FILETIME);
            *(Int64*)(&ft) = time.ToFileTime();
            return Entries.CertGetCertificateChain(chainEngine,
                context, ref ft, additionalStore, ref chainPara, flags,
                IntPtr.Zero, chainContext);
            }
        #endregion

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
        private void EnsureEntries() {
            if (Entries == null) {
                Entries = (ICryptoAPI)GetService(typeof(ICryptoAPI));
                }
            }
        #endregion

        private const Int32 SIGNATURE_BUFFER_SIZE = 64*1024;
        }
    }
