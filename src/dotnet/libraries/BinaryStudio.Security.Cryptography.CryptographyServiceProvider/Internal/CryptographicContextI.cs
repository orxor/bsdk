using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Services;

namespace BinaryStudio.Security.Cryptography.Internal
    {
    internal class CryptographicContextI : CryptographicContext
        {
        public override IntPtr Handle { get; }
        public override Boolean IsMachineKeySet { get; }
        public override String ProviderName { get { return providername; }}
        public override String Container { get; }
        public override CRYPT_PROVIDER_TYPE ProviderType { get; }
        public override CryptographicContextFlags ProviderFlags { get; }

        public override unsafe IDictionary<ALG_ID,String> SupportedAlgorithms { get {
            EnsureEntries(out var entries);
            var r = new Dictionary<ALG_ID, String>();
            var sz = 1024;
            var buffer = new LocalMemory(sz);
            var cflags = CRYPT_FIRST;
            while (entries.CryptGetProvParam(Handle,CRYPT_PARAM.PP_ENUMALGS, buffer, ref sz, cflags)) {
                var alg = (PROV_ENUMALGS*)buffer;
                r.Add(alg->AlgId, ToString(&(alg->Name), alg->NameLength, Encoding.ASCII));
                cflags = CRYPT_NEXT;
                }
            return r;
            }}

        #region ctor{IntPtr}
        public CryptographicContextI(IntPtr source) {
            if (source == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(source)); }
            Handle = source;
            }
        #endregion
        #region ctor{String,String,CRYPT_PROVIDER_TYPE,CryptographicContextFlags}
        public CryptographicContextI(String container, String provider, CRYPT_PROVIDER_TYPE providertype, CryptographicContextFlags flags) {
            var entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
            ProviderFlags = flags;
            IsMachineKeySet = flags.HasFlag(CryptographicContextFlags.CRYPT_MACHINE_KEYSET);
            try
                {
                Validate(entries,entries.CryptAcquireContext(out var r,container,provider,(Int32)providertype,(Int32)flags));
                Container = container;
                ProviderType = providertype;
                providername = provider;
                Handle = r;
                KeySpec = (KEY_SPEC_TYPE)GetParameter<Int32>(CRYPT_PARAM.PP_KEYSPEC,0,null);
                if (String.IsNullOrEmpty(providername)) { providername = GetParameter<String>(CRYPT_PARAM.PP_NAME, 0, Encoding.ASCII); }
                }
            catch (Exception e)
                {
                e.Add("ProviderType",providertype);
                throw;
                }
            }
        #endregion
        #region ctor{CryptographicContext,X509Certificate,CRYPT_ACQUIRE_FLAGS}
        public CryptographicContextI(CryptographicContext context, X509Certificate certificate,CRYPT_ACQUIRE_FLAGS flags) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            if (certificate == null) { throw new ArgumentNullException(nameof(certificate)); }
            var entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
            ProviderFlags = context.ProviderFlags;
            IsMachineKeySet = certificate.IsMachineKeySet;
            try
                {
                Validate(entries.CryptAcquireCertificatePrivateKey(certificate.Handle,flags,IntPtr.Zero,out var r, out var keyspec, out var freeprov));
                Handle = r;
                KeySpec = keyspec;
                }
            catch(Exception e)
                {
                e.Add("ContextFlags",flags);
                e.Add("ProviderFlags",ProviderFlags);
                throw;
                }
            ProviderType = (CRYPT_PROVIDER_TYPE)GetParameter<Int32>(CRYPT_PARAM.PP_PROVTYPE,0,null);
            providername = GetParameter<String>(CRYPT_PARAM.PP_NAME, 0, Encoding.ASCII);
            }
        #endregion
        #region ctor{X509Certificate,CRYPT_ACQUIRE_FLAGS}
        public CryptographicContextI(X509Certificate certificate,CRYPT_ACQUIRE_FLAGS flags) {
            if (certificate == null) { throw new ArgumentNullException(nameof(certificate)); }
            var entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
            IsMachineKeySet = certificate.IsMachineKeySet;
            try
                {
                Validate(entries.CryptAcquireCertificatePrivateKey(certificate.Handle,flags,IntPtr.Zero,out var r, out var keyspec, out var freeprov));
                Handle = r;
                KeySpec = keyspec;
                }
            catch(Exception e)
                {
                e.Add("ContextFlags",flags);
                throw;
                }
            ProviderType = (CRYPT_PROVIDER_TYPE)GetParameter<Int32>(CRYPT_PARAM.PP_PROVTYPE,0,null);
            providername = GetParameter<String>(CRYPT_PARAM.PP_NAME, 0, Encoding.ASCII);
            }
        #endregion
        #region ctor{CryptographicContext,CryptographicContextFlags}
        public CryptographicContextI(CryptographicContext context, CryptographicContextFlags flags) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            var entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
            Container = context.Container;
            ProviderType = context.ProviderType;
            providername = context.ProviderName;
            ProviderFlags = flags;
            IsMachineKeySet = flags.HasFlag(CryptographicContextFlags.CRYPT_MACHINE_KEYSET);
            Validate(entries.CryptAcquireContext(out var r,Container,ProviderName,(Int32)ProviderType,(Int32)flags));
            Handle = r;
            ProviderType = (CRYPT_PROVIDER_TYPE)GetParameter<Int32>(CRYPT_PARAM.PP_PROVTYPE,0,null);
            Task.Factory.StartNew(() => { providername = GetParameter<String>(CRYPT_PARAM.PP_NAME, 0, Encoding.ASCII); });
            KeySpec = context.KeySpec;
            }
        #endregion
        #region ctor{CryptographicContext,String,CryptographicContextFlags}
        public CryptographicContextI(CryptographicContext context, String container,CryptographicContextFlags flags) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            var entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
            Container = container;
            ProviderType = context.ProviderType;
            providername = context.ProviderName;
            ProviderFlags = flags;
            IsMachineKeySet = flags.HasFlag(CryptographicContextFlags.CRYPT_MACHINE_KEYSET);
            Validate(entries.CryptAcquireContext(out var r,container,ProviderName,(Int32)ProviderType,(Int32)flags));
            Handle = r;
            ProviderType = (CRYPT_PROVIDER_TYPE)GetParameter<Int32>(CRYPT_PARAM.PP_PROVTYPE,0,null);
            Task.Factory.StartNew(() => { providername = GetParameter<String>(CRYPT_PARAM.PP_NAME, 0, Encoding.ASCII); });
            KeySpec = context.KeySpec;
            }
        #endregion
        #region ctor{CRYPT_PROVIDER_TYPE,CryptographicContextFlags}
        public CryptographicContextI(CRYPT_PROVIDER_TYPE providertype, CryptographicContextFlags flags)
            :this(null, null, providertype, flags)
            {
            }
        #endregion

        private const Int32 CRYPT_FIRST = 1;
        private const Int32 CRYPT_NEXT  = 2;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct PROV_ENUMALGS
            {
            public readonly ALG_ID AlgId;
            public readonly Int32 Length;
            public readonly Int32 NameLength;
            public readonly Byte Name;
            }

        #region M:ToString(Byte*,Int32,Encoding):String
        private static unsafe String ToString(Byte* source, Int32 size, Encoding encoding) {
            var r = new Byte[size];
            for (var i = 0; i < size; ++i) {
                r[i] = source[i];
                }
            return encoding.GetString(r);
            }
        #endregion
        #region M:EnsureEntries
        private CryptographicFunctions Entries;
        internal override void EnsureEntries(out CryptographicFunctions entries) {
            if (Entries == null) {
                Entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
                }
            entries = Entries;
            }
        #endregion
        #region M:GetLastWin32Error:Int32
        /// <summary>
        /// Returns the error code returned by the last unmanaged function that was called.
        /// using platform invoke that has the System.Runtime.InteropServices.DllImportAttribute.SetLastError flag set.
        /// </summary>
        /// <returns>The last error code set by a call to the Win32 SetLastError function.</returns>
        protected internal override Int32 GetLastWin32Error() {
            EnsureEntries(out var entries);
            return entries.GetLastError();
            }
        #endregion

        private String providername;
        }
    }