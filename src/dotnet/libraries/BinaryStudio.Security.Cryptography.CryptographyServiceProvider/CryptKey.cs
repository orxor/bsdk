using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography
    {
    using CRYPT_DATA_BLOB=CRYPT_BLOB;
    public class CryptKey : CryptographicObject
        {
        public override IntPtr Handle { get { return handle; }}
        public KEY_SPEC_TYPE KeySpec { get; }
        public String Container { get; }
        public CryptographicContext Context { get { return context; }}

        public unsafe ALG_ID AlgId {get{
                var r = GetParameter(KEY_PARAM.KP_ALGID);
                fixed (Byte* o = r) {
                    return *(ALG_ID*)o;
                    }
                }}

        public String SignatureOID {get{
                var r = GetParameter(KEY_PARAM.KP_CP_SIGNATUREOID);
                return Encoding.ASCII.GetString(r).TrimEnd('\0');
                }}

        public String CipherOID {get{
                var r = GetParameter(KEY_PARAM.KP_CP_CIPHEROID);
                return Encoding.ASCII.GetString(r).TrimEnd('\0');
                }}

        public String HashOID {get{
                var r = GetParameter(KEY_PARAM.KP_CP_HASHOID);
                return Encoding.ASCII.GetString(r).TrimEnd('\0');
                }}

        public String DiffieHellmanOID {get{
                var r = GetParameter(KEY_PARAM.KP_CP_DHOID);
                return Encoding.ASCII.GetString(r).TrimEnd('\0');
                }}

        #region ctor{CryptographicContext,IntPtr}
        internal CryptKey(CryptographicContext context,IntPtr handle) {
            if (handle == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(handle)); }
            this.handle = handle;
            this.context = context;
            }
        #endregion
        #region ctor{IntPtr,KEY_SPEC_TYPE,String}
        internal CryptKey(CryptographicContext context, IntPtr handle, KEY_SPEC_TYPE keySpec, String container) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            if (handle == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(handle)); }
            this.handle = handle;
            this.context = context;
            KeySpec = keySpec;
            Container = container;
            }
        #endregion

        #region M:GenKey(CryptographicContext,ALG_ID,CryptGenKeyFlags):CryptKey
        public static CryptKey GenKey(CryptographicContext context,ALG_ID algid, CryptGenKeyFlags flags) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            Validate(((CryptographicFunctions)context.GetService(typeof(CryptographicFunctions))).CryptGenKey(context.Handle,algid,(Int32)flags,out var r));
            if (algid == ALG_ID.AT_KEYEXCHANGE) { return new CryptKey(context,r,KEY_SPEC_TYPE.AT_KEYEXCHANGE,context.Container); }
            if (algid == ALG_ID.AT_SIGNATURE)   { return new CryptKey(context,r,KEY_SPEC_TYPE.AT_SIGNATURE  ,context.Container); }
            return new CryptKey(context,r);
            }
        #endregion
        #region M:GetUserKey(CryptographicContext,KEY_SPEC_TYPE):CryptKey
        public static CryptKey GetUserKey(CryptographicContext context, KEY_SPEC_TYPE keyspec) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            ((CryptographicFunctions)context.GetService(typeof(CryptographicFunctions))).CryptGetUserKey(context.Handle,keyspec,out var r);
            return (r != IntPtr.Zero)
                ? new CryptKey(context,r)
                : null;
            }
        #endregion
        #region M:ExportPrivateKey(Stream,String)
        public unsafe void ExportPrivateKey(Stream OutputStream,String Password) {
            if (OutputStream == null) { throw new ArgumentNullException(nameof(OutputStream)); }
            var entries = (CryptographicFunctions)Context.GetService(typeof(CryptographicFunctions));
            var salt = new Byte[SALT_LENGTH];
            (new Random()).NextBytes(salt);
            var HashAlgId = ALG_ID.CALG_PBKDF2_94_256;
            switch (this.AlgId) {
                case ALG_ID.CALG_DH_EL_SF:            HashAlgId = ALG_ID.CALG_GR3411;   break;
                case ALG_ID.CALG_DH_GR3410_12_256_SF: HashAlgId = ALG_ID.CALG_PBKDF2_2012_256; break;
                case ALG_ID.CALG_DH_GR3410_12_512_SF: HashAlgId = ALG_ID.CALG_PBKDF2_2012_512; break;
                }
            OutputStream.Write(MAGIC_PRI0);
            OutputStream.Write((Int32)AlgId);
            OutputStream.Write((Int32)KeySpec);
            OutputStream.Write((Int32)HashAlgId);
            OutputStream.Write(salt.Length);
            OutputStream.Write(salt,0,salt.Length);
            using (var hash = new CryptHashAlgorithm(Context,HashAlgId)) {
                if (HashAlgId != ALG_ID.CALG_GR3411) {
                    Validate(hash.SetParameter(HP_PBKDF2_SALT,salt,CRYPT_PASS_THROUGHT_DATA_BLOB));
                    Validate(hash.SetParameter(HP_PBKDF2_PASSWORD,Encoding.ASCII.GetBytes(Password),CRYPT_PASS_THROUGHT_DATA_BLOB));
                    }
                Validate(entries.CryptDeriveKey(Context.Handle,ALG_ID.CALG_G28147,hash.Handle,CRYPT_EXPORTABLE,out var DerivedKeyU));
                using (var DerivedKey = new CryptKey(Context,DerivedKeyU)) {
                    Byte[] Data;
                    var Size = 0;
                    var AlgId = (HashAlgId == ALG_ID.CALG_GR3411)
                        ? ALG_ID.CALG_PRO_EXPORT
                        : ALG_ID.CALG_PRO12_EXPORT;
                    DerivedKey.SetParameter(KEY_PARAM.KP_ALGID,&AlgId);
                    DerivedKey.GetParameter(KEY_PARAM.KP_IV, out Byte[] SV);
                    OutputStream.Write((Int32)ALG_ID.CALG_G28147);
                    OutputStream.Write((Int32)AlgId);
                    OutputStream.Write(SV.Length);
                    OutputStream.Write(SV,0,SV.Length);
                    OutputStream.Write(PRIVATEKEYBLOB);
                    Validate(entries.CryptExportKey(Handle,DerivedKeyU,PRIVATEKEYBLOB,0,null, ref Size));
                    Validate(entries.CryptExportKey(Handle,DerivedKeyU,PRIVATEKEYBLOB,0,Data = new Byte[Size], ref Size));
                    OutputStream.Write(Size);
                    OutputStream.Write(Data,0,Size);
                    }
                }
            }
        #endregion

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
                                SetParameter(KEY_PARAM.KP_SIGNATURE_PIN,i);
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
                    SetParameter(KEY_PARAM.KP_SIGNATURE_PIN,IntPtr.Zero);
                    }
                }
            }
        #endregion
        #region P:Certificate:X509Certificate
        public X509Certificate Certificate {
            get {
                var r = GetParameter(KEY_PARAM.KP_CERTIFICATE);
                if (r != null)
                    {
                    return new X509Certificate(r);
                    }
                return null;
                }
            set
                {
                Validate(((CryptographicFunctions)context.GetService(typeof(CryptographicFunctions))).
                    CryptSetKeyParam(Handle,KEY_PARAM.KP_CERTIFICATE,value?.Bytes,0));
                }
            }
        #endregion
        #region M:GetParameter(KEY_PARAM):Byte[]
        internal Byte[] GetParameter(KEY_PARAM key) {
            return GetParameter(key, 0);
            }
        #endregion
        #region M:GetParameter(KEY_PARAM,Int32):Byte[]
        internal Byte[] GetParameter(KEY_PARAM key, Int32 flags) {
            for (var i = 0x200;;) {
                var r = new Byte[i];
                if (Context.CryptGetKeyParam(Handle, key, r, ref i, flags)) { return r; }
                var e = (Int32)Context.GetLastWin32Error();
                var FacilityI = ((Int32)e >> 16) & 0x1fff;
                var FacilityE = (FACILITY)FacilityI;
                if (FacilityE == FACILITY.WIN32) { e = ((Int32)e & 0xffff); }
                if ((Win32ErrorCode)e == Win32ErrorCode.ERROR_MORE_DATA) { continue; }
                if ((HRESULT)e == HRESULT.NTE_BAD_KEY) {
                    /*
                     * При вызове метода CryptGetKeyParam с буфером недостаточной длины GetLastError() возвращает ошибку NTE_BAD_KEY (0x80090003 Плохой ключ.),
                     * хотя ожидается ошибка ERROR_MORE_DATA. Стабильно воспроизводится при запросе сертификата (параметра KP_CERTIFICATE).
                     */
                    if (key == KEY_PARAM.KP_CERTIFICATE) {
                        if (Context.ProviderName.StartsWith("Crypto-Pro", StringComparison.OrdinalIgnoreCase) && (Context.Version.Major == 5)) {
                            continue;
                            }
                        }
                    }
                break;
                }
            return null;
            }
        #endregion
        #region M:GetParameter(KEY_PARAM,{out}Int32):Boolean
        public unsafe Boolean GetParameter(KEY_PARAM key,out Int32 value) {
            value = 0;
            var entries = (CryptographicFunctions)Context.GetService(typeof(CryptographicFunctions));
            var r = 0;
            var o = sizeof(Int32);
            if (entries.CryptGetKeyParam(Handle, key, (IntPtr)(&r), ref o, 0)) {
                value = r;
                return true;
                }
            return false;
            }
        #endregion
        #region M:GetParameter(KEY_PARAM,{out}Byte[]):Boolean
        public unsafe Boolean GetParameter(KEY_PARAM key,out Byte[] value) {
            value = null;
            for (var i = 0x200;;) {
                var r = new Byte[i];
                if (Context.CryptGetKeyParam(Handle, key, r, ref i, 0)) {
                    value = new Byte[i];
                    Array.Copy(r,value,i);
                    return true;
                    }
                var e = (Int32)Context.GetLastWin32Error();
                var FacilityI = ((Int32)e >> 16) & 0x1fff;
                var FacilityE = (FACILITY)FacilityI;
                if (FacilityE == FACILITY.WIN32) { e = ((Int32)e & 0xffff); }
                if ((Win32ErrorCode)e == Win32ErrorCode.ERROR_MORE_DATA) { continue; }
                if ((HRESULT)e == HRESULT.NTE_BAD_KEY) {
                    /*
                     * При вызове метода CryptGetKeyParam с буфером недостаточной длины GetLastError() возвращает ошибку NTE_BAD_KEY (0x80090003 Плохой ключ.),
                     * хотя ожидается ошибка ERROR_MORE_DATA. Стабильно воспроизводится при запросе сертификата (параметра KP_CERTIFICATE).
                     */
                    if (key == KEY_PARAM.KP_CERTIFICATE) {
                        if (Context.ProviderName.StartsWith("Crypto-Pro", StringComparison.OrdinalIgnoreCase) && (Context.Version.Major == 5)) {
                            continue;
                            }
                        }
                    }
                break;
                }
            return false;
            }
        #endregion
        #region M:SetParameter(KEY_PARAM,IntPtr):Byte[]
        internal void SetParameter(KEY_PARAM index, IntPtr value) {
            Validate(((CryptographicFunctions)context.GetService(typeof(CryptographicFunctions))).
                CryptSetKeyParam(Handle,index,value, 0));
            }
        #endregion
        #region M:SetParameter(KEY_PARAM,void*):Byte[]
        internal unsafe void SetParameter(KEY_PARAM index, void* value) {
            SetParameter(index,(IntPtr)value);
            }
        #endregion
        #region M:Dispose(Boolean)
        /// <summary>
        /// Releases the unmanaged resources used by the instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposing) {
            if (handle != IntPtr.Zero) {
                ((CryptographicFunctions)Context.GetService(typeof(CryptographicFunctions))).CryptDestroyKey(handle);
                handle = IntPtr.Zero;
                }
            context = null;
            base.Dispose(disposing);
            }
        #endregion
        #region M:ToString:String
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return $"{AlgId}:{Container}";
            }
        #endregion

        private IntPtr handle;
        private CryptographicContext context;
        private const Int32 SALT_LENGTH = 32;
        private const Int32 HP_PBKDF2_SALT     = 0x0017;
        private const Int32 HP_PBKDF2_PASSWORD = 0x0018;
        private const Int32 CRYPT_EXPORTABLE = 0x00000001;
        private const Int32 PLAINTEXTKEYBLOB = 0x8;
        private const Int32 PRIVATEKEYBLOB   = 0x7;
        private const Int32 MAGIC_PRI0 = 0x30495250;
        }
    }