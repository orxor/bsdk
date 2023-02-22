using System;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    public class CryptKey : CryptographicObject
        {
        public override IntPtr Handle { get; }
        public KEY_SPEC_TYPE KeySpec { get; }
        public String Container { get; }
        public CryptographicContext Context { get; }

        #region ctor{IntPtr}
        public CryptKey(IntPtr handle)
            {
            if (handle == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(handle)); }
            Handle = handle;
            }
        #endregion
        #region ctor{IntPtr,KEY_SPEC_TYPE,String}
        public CryptKey(CryptographicContext context, IntPtr handle, KEY_SPEC_TYPE keySpec, String container) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            if (handle == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(handle)); }
            Handle = handle;
            KeySpec = keySpec;
            Container = container;
            Context = context;
            }
        #endregion

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
        }
    }