using System;
using System.Runtime.InteropServices;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    internal class CryptographicMessage : CryptographicObject
        {
        public override IntPtr Handle { get { return handle; }}

        #region ctor{IntPtr}
        internal CryptographicMessage(IntPtr source) {
            if (source == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(source)); }
            handle = source;
            }
        #endregion
        #region ctor{IntPtr,{ref}CMSG_STREAM_INFO}
        private CryptographicMessage(IntPtr source, ref CMSG_STREAM_INFO so)
            {
            if (source == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(source)); }
            handle = source;
            this.so = so;
            }
        #endregion

        #region M:Dispose(Boolean)
        protected override void Dispose(Boolean disposing) {
            if (handle != IntPtr.Zero) {
                Entries.CryptMsgClose(handle);
                handle = IntPtr.Zero;
                }
            base.Dispose(disposing);
            }
        #endregion
        #region M:Update(Byte[],Int32,Boolean)
        public unsafe void Update(Byte[] data, Int32 length, Boolean final) {
            fixed (Byte* bytes = data) {
                Update(bytes, length, final);
                }
            }
        #endregion
        #region M:Update(Byte*,Int32,Boolean)
        public unsafe void Update(Byte* data, Int32 length, Boolean final) {
            Validate(Entries.CryptMsgUpdate(Handle, (IntPtr)data, length, final));
            }
        #endregion
        #region M:Update(Byte[],Boolean)
        public void Update(Byte[] data, Boolean final) {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }
            Update(data, data.Length, final);
            }
        #endregion
        #region M:GetParameter(CMSG_PARAM,Int32,[Out]HRESULT):Byte[]
        internal Byte[] GetParameter(CMSG_PARAM parameter, Int32 signerindex, out HRESULT e) {
            var c = 0;
            if (!Entries.CryptMsgGetParam(Handle, parameter, signerindex, null, ref c)) {
                e = (HRESULT)Marshal.GetLastWin32Error();
                return EmptyArray<Byte>.Value;
                }
            var r = new Byte[c];
            if (!Entries.CryptMsgGetParam(Handle, parameter, signerindex, r, ref c)) {
                e = (HRESULT)Marshal.GetLastWin32Error();
                return EmptyArray<Byte>.Value;
                }
            e = 0;
            return r;
            }
        #endregion
        #region M:GetParameter(CMSG_PARAM,Int32):Byte[]
        internal Byte[] GetParameter(CMSG_PARAM parameter, Int32 signerindex) {
            HRESULT hr;
            var c = 0;
            if (!Entries.CryptMsgGetParam(Handle, parameter, signerindex, null, ref c)) {
                hr = (HRESULT)Marshal.GetLastWin32Error();
                if (hr != HRESULT.CRYPT_E_INVALID_INDEX) { Marshal.ThrowExceptionForHR((Int32)hr); }
                return EmptyArray<Byte>.Value;
                }
            var r = new Byte[c];
            if (!Entries.CryptMsgGetParam(Handle, parameter, signerindex, r, ref c)) {
                hr = (HRESULT)Marshal.GetLastWin32Error();
                if (hr != HRESULT.CRYPT_E_INVALID_INDEX) { Marshal.ThrowExceptionForHR((Int32)hr); }
                return EmptyArray<Byte>.Value;
                }
            return r;
            }
        #endregion
        #region M:GetParameter(CMSG_PARAM,Int32,{out}HRESULT,{out}Byte[]):Byte[]
        internal Boolean GetParameter(CMSG_PARAM parameter, Int32 signerindex, out HRESULT e, out Byte[] r) {
            var c = 0;
            r = EmptyArray<Byte>.Value;
            if (!Entries.CryptMsgGetParam(Handle, parameter, signerindex, null, ref c)) {
                e = (HRESULT)Entries.GetLastError();
                r = EmptyArray<Byte>.Value;
                return false;
                }
            r = new Byte[c];
            if (!Entries.CryptMsgGetParam(Handle, parameter, signerindex, r, ref c)) {
                e = (HRESULT)Entries.GetLastError();
                r = EmptyArray<Byte>.Value;
                return false;
                }
            e = 0;
            return true;
            }
        #endregion
        #region M:OpenToDecode(CRYPT_OPEN_MESSAGE_FLAGS):CryptographicMessage
        public static unsafe CryptographicMessage OpenToDecode(CRYPT_OPEN_MESSAGE_FLAGS flags) {
            var so = new CMSG_STREAM_INFO(CMSG_INDEFINITE_LENGTH,delegate(IntPtr arg, Byte* Data, UInt32 Size, Boolean Final) {
                var Bytes = new Byte[Size];
                for (var i = 0; i < Size; i++) {
                    Bytes[i] = Data[i];
                    }
                return true;
                }, IntPtr.Zero);
            return new CryptographicMessage(Entries.CryptMsgOpenToDecode(CRYPT_MSG_TYPE.PKCS_7_ASN_ENCODING, flags, CMSG_TYPE.CMSG_NONE, IntPtr.Zero, IntPtr.Zero, ref so),ref so);
            }
        #endregion
        #region M:OpenToDecode(Action<Byte[],Boolean>):CryptographicMessage
        public static unsafe CryptographicMessage OpenToDecode(Action<Byte[], Boolean> OutputHandler) {
            var so = new CMSG_STREAM_INFO(CMSG_INDEFINITE_LENGTH,delegate(IntPtr arg, Byte* Data, UInt32 Size, Boolean Final) {
                var Bytes = new Byte[Size];
                for (var i = 0; i < Size; i++) {
                    Bytes[i] = Data[i];
                    }
                OutputHandler(Bytes, Final);
                return true;
                }, IntPtr.Zero);
            return new CryptographicMessage(Entries.CryptMsgOpenToDecode(CRYPT_MSG_TYPE.PKCS_7_ASN_ENCODING, 0, CMSG_TYPE.CMSG_NONE, IntPtr.Zero, IntPtr.Zero, ref so),ref so);
            }
        #endregion
        #region M:OpenToEncode(Action<Byte[],Boolean>,UInt32,CRYPT_OPEN_MESSAGE_FLAGS,CMSG_TYPE,CMSG_SIGNED_ENCODE_INFO):CryptographicMessage
        public static unsafe CryptographicMessage OpenToEncode(Action<Byte[],Boolean> OutputHandler,UInt32 Length,CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,CMSG_SIGNED_ENCODE_INFO EncodeInfo) {
            if (OutputHandler == null) { throw new ArgumentNullException(nameof(OutputHandler)); }
            var so = new CMSG_STREAM_INFO(Length,delegate(IntPtr arg, Byte* Data, UInt32 Size, Boolean Final) {
                var Bytes = new Byte[Size];
                for (var i = 0; i < Size; i++) {
                    Bytes[i] = Data[i];
                    }
                try
                    {
                    OutputHandler(Bytes, Final);
                    }
                catch(Exception e)
                    {
                    //LastException = e;
                    return false;
                    }
                return true;
                }, IntPtr.Zero);
            try
                {
                return new CryptographicMessage(
                    Validate(Entries,Entries.CryptMsgOpenToEncode(CRYPT_MSG_TYPE.PKCS_7_ASN_ENCODING|CRYPT_MSG_TYPE.PKCS_7_ASN_ENCODING, Flags, Type,EncodeInfo,ref so),NotZero),
                    ref so);
                }
            catch (Exception e)
                {
                e.Add("MessageFlags",Flags);
                e.Add("MessageType",Type);
                throw;
                }
            }
        #endregion
        #region M:OpenToEncode(Action<Byte[],Boolean>,UInt32,CRYPT_OPEN_MESSAGE_FLAGS,CMSG_TYPE,{ref}CMSG_ENVELOPED_ENCODE_INFO):CryptographicMessage
        public static unsafe CryptographicMessage OpenToEncode(Action<Byte[],Boolean> OutputHandler,UInt32 Length,CRYPT_OPEN_MESSAGE_FLAGS Flags,CMSG_TYPE Type,ref CMSG_ENVELOPED_ENCODE_INFO EncodeInfo) {
            if (OutputHandler == null) { throw new ArgumentNullException(nameof(OutputHandler)); }
            var so = new CMSG_STREAM_INFO(Length,delegate(IntPtr arg, Byte* Data, UInt32 Size, Boolean Final) {
                var Bytes = new Byte[Size];
                for (var i = 0; i < Size; i++) {
                    Bytes[i] = Data[i];
                    }
                try
                    {
                    OutputHandler(Bytes, Final);
                    }
                catch(Exception e)
                    {
                    return false;
                    }
                return true;
                }, IntPtr.Zero);
            try
                {
                return new CryptographicMessage(
                    Validate(Entries,Entries.CryptMsgOpenToEncode(CRYPT_MSG_TYPE.PKCS_7_ASN_ENCODING|CRYPT_MSG_TYPE.PKCS_7_ASN_ENCODING, Flags, Type,ref EncodeInfo,ref so),NotZero),
                    ref so);
                }
            catch (Exception e)
                {
                e.Add("MessageFlags",Flags);
                e.Add("MessageType",Type);
                throw;
                }
            }
        #endregion
        #region M:Control(CRYPT_MESSAGE_FLAGS,CMSG_CTRL,IntPtr):Boolean
        public Boolean Control(CRYPT_MESSAGE_FLAGS Flags, CMSG_CTRL CtrlType, IntPtr CtrlPara)
            {
            return Entries.CryptMsgControl(Handle,Flags,CtrlType,CtrlPara);
            }
        #endregion

        protected const UInt32 CMSG_INDEFINITE_LENGTH = 0xFFFFFFFF;
        internal static CryptographicFunctions Entries;
        static CryptographicMessage()
            {
            Entries = (CryptographicFunctions)CryptographicContext.DefaultContext.GetService(typeof(CryptographicFunctions));
            }

        private CMSG_STREAM_INFO so;
        private IntPtr handle;
        }
    }