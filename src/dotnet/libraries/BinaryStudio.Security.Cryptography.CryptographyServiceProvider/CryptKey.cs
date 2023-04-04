using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography
    {
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
        #region M:GetParameter(KEY_PARAM,UInt32):Byte[]
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
        #region M:SetParameter(KEY_PARAM,IntPtr):Byte[]
        internal void SetParameter(KEY_PARAM index, IntPtr value) {
            Validate(((CryptographicFunctions)context.GetService(typeof(CryptographicFunctions))).
                CryptSetKeyParam(Handle,index,value, 0));
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
        }
    }