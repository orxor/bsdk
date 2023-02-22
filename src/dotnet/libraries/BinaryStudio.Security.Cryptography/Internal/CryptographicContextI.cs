using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Internal
    {
    internal class ECryptographicContext : CryptographicContext
        {
        public override IntPtr Handle { get; }
        public override unsafe IDictionary<ALG_ID,String> SupportedAlgorithms { get {
            EnsureEntries(out var entries);
            var r = new Dictionary<ALG_ID, String>();
            var sz = 1024;
            var buffer = new LocalMemory(sz);
            var cflags = CRYPT_FIRST;
            while (entries.CryptGetProvParam(Handle,(Int32)CRYPT_PARAM.PP_ENUMALGS, buffer, ref sz, cflags)) {
                var alg = (PROV_ENUMALGS*)buffer;
                r.Add(alg->AlgId, ToString(&(alg->Name), alg->NameLength, Encoding.ASCII));
                cflags = CRYPT_NEXT;
                }
            return r;
            }}

        public ECryptographicContext(IntPtr source)
            {
            if (source == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(source)); }
            Handle = source;
            }

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
        private ICryptoAPI Entries;
        internal override void EnsureEntries(out ICryptoAPI entries) {
            if (Entries == null) {
                Entries = (ICryptoAPI)DefaultContext.GetService(typeof(ICryptoAPI));
                }
            entries = Entries;
            }
        #endregion
        }
    }