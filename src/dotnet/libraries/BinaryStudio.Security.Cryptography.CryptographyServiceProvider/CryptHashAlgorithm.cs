using System;
using System.IO;
using System.Security.Cryptography;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Services;

namespace BinaryStudio.Security.Cryptography
    {
    public class CryptHashAlgorithm : CryptographicObject
        {
        private IntPtr handle;
        private const Int32 BLOCK_SIZE_64K = 64*1024;
        private Int32 HashSizeValue = -1;
        private Byte[] HashValue;
        public override IntPtr Handle { get {
            EnsureHandle();
            return handle;
            }}
        internal CryptographicContext Context { get; }
        internal ALG_ID Algorithm { get; }
        
        #region ctor{CryptographicContext,ALG_ID}
        public CryptHashAlgorithm(CryptographicContext context, ALG_ID algorithm)
            {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            Algorithm = algorithm;
            Context = context;
            }
        #endregion
        #region ctor{CryptographicContext,Oid}
        public CryptHashAlgorithm(CryptographicContext context, Oid algorithm) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            Algorithm = CryptographicContext.GetAlgId(algorithm);
            Context = context;
            }
        #endregion

        #region M:Compute(Stream):Byte[]
        public Byte[] Compute(Stream stream) {
            return Compute(stream,null);
            }
        #endregion
        #region M:Compute(Stream,Action<Byte[],Int32>):Byte[]
        internal Byte[] Compute(Stream stream,Action<Byte[],Int32> predicate) {
            if (stream == null) { throw new ArgumentNullException(nameof(stream)); }
            if (!stream.CanRead) { throw new ArgumentOutOfRangeException(nameof(stream)); }
            var Block = new Byte[BLOCK_SIZE_64K];
            EnsureHandle();
            for (;;) {
                var Size = stream.Read(Block, 0, Block.Length);
                if (Size == 0) { break; }
                Yield();
                Validate(Entries.CryptHashData(Handle,Block,Size));
                predicate?.Invoke(Block, Size);
                }
            HashValue = new Byte[HashSizeValue];
            Validate(Entries.CryptGetHashParam(Handle,HP_HASHVAL,HashValue,ref HashSizeValue));
            return HashValue;
            }
        #endregion
        #region M:Compute(Byte[]):Byte[]
        public Byte[] Compute(Byte[] source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            using (var stream = new MemoryStream(source)) {
                return Compute(stream);
                }
            }
        #endregion

        #region M:EnsureHandle
        private void EnsureHandle() {
            if (handle == IntPtr.Zero) {
                Validate(EnsureEntries().CryptCreateHash(Context.Handle,Algorithm,IntPtr.Zero,out handle));
                Validate(EnsureEntries().CryptGetHashParam(handle, HP_HASHSIZE, out HashSizeValue));
                }
            }
        #endregion
        #region M:EnsureEntries:CryptographicFunctions
        private CryptographicFunctions Entries;
        private CryptographicFunctions EnsureEntries() {
            if (Entries == null) {
                Entries = (CryptographicFunctions)Context.GetService(typeof(CryptographicFunctions));
                }
            return Entries;
            }
        #endregion
        #region M:Dispose(Boolean)
        protected override void Dispose(Boolean disposing) {
            base.Dispose(disposing);
            lock (this) {
                if (handle != IntPtr.Zero) {
                    Entries.CryptDestroyHash(handle);
                    handle = IntPtr.Zero;
                    }
                }
            }
        #endregion

        #region M:SignHash(KEY_SPEC_TYPE,{out}Digest,{out}Signature)
        public void SignHash(KEY_SPEC_TYPE KeySpec,out Byte[] Digest, out Byte[] Signature) {
            if (HashValue == null) { throw new InvalidOperationException(); }
            Digest = HashValue;
            var SignatureLength = 0;
            Validate(Entries.CryptSignHash(Handle,KeySpec, null, ref SignatureLength));
            Validate(Entries.CryptSignHash(Handle,KeySpec, Signature = new Byte[SignatureLength], ref SignatureLength));
            }
        #endregion
        #region M:SetParameter(Int32,Byte[],Int32):Boolean
        public Boolean SetParameter(Int32 Param,Byte[] Data,Int32 Flags) {
            EnsureHandle();
            return (EnsureEntries().CryptSetHashParam(Handle,Param,Data,Flags));
            }
        #endregion

        private const Int32 HP_ALGID                = 0x0001;  // Hash algorithm
        private const Int32 HP_HASHVAL              = 0x0002;  // Hash value
        private const Int32 HP_HASHSIZE             = 0x0004;  // Hash value size
        }
    }