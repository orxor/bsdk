using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    internal class CryptographicMessage : CryptographicObject
        {
        public override IntPtr Handle { get; }
        internal CryptographicMessage(IntPtr source) {
            if (source == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(source)); }
            Handle = source;
            }

        #region M:Update(Byte[],Int32,Boolean)
        public unsafe void Update(Byte[] data, Int32 length, Boolean final) {
            fixed (Byte* bytes = data) {
                Update(bytes, length, final);
                }
            }
        #endregion
        #region M:Update(Byte*,Int32,Boolean)
        public unsafe void Update(Byte* data, Int32 length, Boolean final) {
            Validate(CryptMsgUpdate(Handle, (IntPtr)data, length, final));
            }
        #endregion
        #region M:Update(Byte[],Boolean)
        public void Update(Byte[] data, Boolean final) {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }
            Update(data, data.Length, final);
            }
        #endregion
        #region M:GetParameter(CMSG_PARAM,Int32,[Out]HRESULT):Byte[]
        internal Byte[] GetParameter(CMSG_PARAM parameter, Int32 signerindex, out HRESULT hr) {
            var c = 0;
            if (!CryptMsgGetParam(Handle, parameter, signerindex, null, ref c)) {
                hr = (HRESULT)Marshal.GetLastWin32Error();
                return EmptyArray<Byte>.Value;
                }
            var r = new Byte[c];
            if (!CryptMsgGetParam(Handle, parameter, signerindex, r, ref c)) {
                hr = (HRESULT)Marshal.GetLastWin32Error();
                return EmptyArray<Byte>.Value;
                }
            hr = 0;
            return r;
            }
        #endregion
        #region M:GetParameter(CMSG_PARAM,Int32):Byte[]
        internal Byte[] GetParameter(CMSG_PARAM parameter, Int32 signerindex) {
            HRESULT hr;
            var c = 0;
            if (!CryptMsgGetParam(Handle, parameter, signerindex, null, ref c)) {
                hr = (HRESULT)Marshal.GetLastWin32Error();
                if (hr != HRESULT.CRYPT_E_INVALID_INDEX) { Marshal.ThrowExceptionForHR((Int32)hr); }
                return EmptyArray<Byte>.Value;
                }
            var r = new Byte[c];
            if (!CryptMsgGetParam(Handle, parameter, signerindex, r, ref c)) {
                hr = (HRESULT)Marshal.GetLastWin32Error();
                if (hr != HRESULT.CRYPT_E_INVALID_INDEX) { Marshal.ThrowExceptionForHR((Int32)hr); }
                return EmptyArray<Byte>.Value;
                }
            return r;
            }
        #endregion

        [DllImport("crypt32.dll", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgUpdate(IntPtr hcryptmsg, IntPtr pbdata, Int32 cbdata, Boolean final);
        [DllImport("crypt32.dll", BestFitMapping = false, CharSet = CharSet.None, SetLastError = true)] private static extern Boolean CryptMsgGetParam(IntPtr msg, CMSG_PARAM parameter, Int32 signerindex, [MarshalAs(UnmanagedType.LPArray)] Byte[] data, ref Int32 size);
        }
    }
