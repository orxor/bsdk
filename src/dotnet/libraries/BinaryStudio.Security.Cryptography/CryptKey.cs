using System;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    public class CryptKey : CryptographicObject
        {
        public override IntPtr Handle { get { return handle; }}
        public KEY_SPEC_TYPE KeySpec { get; }
        public String Container { get; }
        public CryptographicContext Context { get { return context; }}

        #region ctor{CryptographicContext,IntPtr}
        internal CryptKey(CryptographicContext context,IntPtr handle)
            {
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

        public static CryptKey GenKey(CryptographicContext context,ALG_ID algid, Int32 flags) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            Validate(((KeyGenerationAndExchangeFunctions)context.GetService(typeof(KeyGenerationAndExchangeFunctions))).CryptGenKey(context.Handle,algid,flags,out var r));
            return new CryptKey(context,r);
            }

        public static CryptKey GetUserKey(CryptographicContext context, KEY_SPEC_TYPE keyspec) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            ((KeyGenerationAndExchangeFunctions)context.GetService(typeof(KeyGenerationAndExchangeFunctions))).CryptGetUserKey(context.Handle,keyspec,out var r);
            return (r != IntPtr.Zero)
                ? new CryptKey(context,r)
                : null;
            }

        #region P:Certificate:X509Certificate
        public X509Certificate Certificate { get{
            var r = GetParameter(KEY_PARAM.KP_CERTIFICATE);
            if (r != null)
                {
                return new X509Certificate(r);
                }
            return null;
            }}
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
        #region M:Dispose(Boolean)
        /// <summary>
        /// Releases the unmanaged resources used by the instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposing) {
            context = null;
            if (handle != IntPtr.Zero) {
                ((KeyGenerationAndExchangeFunctions)Context.GetService(typeof(KeyGenerationAndExchangeFunctions))).CryptDestroyKey(handle);
                handle = IntPtr.Zero;
                }
            base.Dispose(disposing);
            }
        #endregion

        private IntPtr handle;
        private CryptographicContext context;
        }
    }