﻿using System;
using System.IO;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    public class CryptHashAlgorithm : CryptographicObject
        {
        private IntPtr handle;
        private const Int32 BLOCK_SIZE_64K = 64*1024;
        private Int32 HashSizeValue = -1;
        private Byte[] HashValue;
        public override IntPtr Handle { get { return handle; }}
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
        #region M:Compute(Stream):Byte[]
        public Byte[] Compute(Stream stream) {
            if (stream == null) { throw new ArgumentNullException(nameof(stream)); }
            if (!stream.CanRead) { throw new ArgumentOutOfRangeException(nameof(stream)); }
            var Block = new Byte[BLOCK_SIZE_64K];
            EnsureHandle();
            for (;;) {
                var Size = stream.Read(Block, 0, Block.Length);
                if (Size == 0) { break; }
                Yield();
                Validate(Entries.CryptHashData(Handle,Block,Size));
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
                var Size = sizeof(Int32);
                Validate(EnsureEntries().CryptCreateHash(Context.Handle,Algorithm,IntPtr.Zero,out handle));
                Validate(EnsureEntries().CryptGetHashParam(handle, HP_HASHSIZE, out HashSizeValue, ref Size));
                }
            }
        #endregion
        #region M:EnsureEntries:ICryptoAPI
        private ICryptoAPI Entries;
        private ICryptoAPI EnsureEntries() {
            if (Entries == null) {
                Entries = (ICryptoAPI)GetService(typeof(ICryptoAPI));
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

        private const Int32 HP_ALGID                = 0x0001;  // Hash algorithm
        private const Int32 HP_HASHVAL              = 0x0002;  // Hash value
        private const Int32 HP_HASHSIZE             = 0x0004;  // Hash value size
        }
    }