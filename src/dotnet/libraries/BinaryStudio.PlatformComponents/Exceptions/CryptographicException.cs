using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    public class CryptographicException : AggregateException
        {
        private StackTrace ExternalStackTrace;
        private String OriginalMessage;

        public CryptographicException(HRESULT SCode)
            :base(HResultException.FormatMessage(SCode))
            {
            OriginalMessage = base.Message;
            HResult = (Int32)SCode;
            }

        public CryptographicException()
            :this(HRESULT.CORSEC_E_CRYPTO)
            {
            }

        public CryptographicException(HRESULT SCode,String message)
            :base(message)
            {
            HResult = (Int32)SCode;
            OriginalMessage = message;
            }

        public CryptographicException(String message)
            :this(HRESULT.CORSEC_E_CRYPTO,message)
            {
            }

        public CryptographicException(String message, Exception innerException)
            :base(message, innerException)
            {
            OriginalMessage = message;
            }

        public CryptographicException(String message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions)
            {
            OriginalMessage = message;
            }

        #region ctor{HRESULT,IEnumerable<Exception>}
        public CryptographicException(HRESULT scode, IEnumerable<Exception> innerExceptions)
            : base(HResultException.FormatMessage(scode),innerExceptions)
            {
            OriginalMessage = HResultException.FormatMessage(scode);
            HResult = (Int32)scode;
            }
        #endregion

        #region P:StackTrace:String
        public override String StackTrace { get{
            return (ExternalStackTrace != null)
                ? ExternalStackTrace.ToString()
                : base.StackTrace;
            }}
        #endregion
        #region M:Message:String
        public override String Message { get {
            return String.Join(" ", GetMessage().Select(i => i + "."));
            }}
        #endregion

        #region M:SetStackTrace(StackTrace):CryptographicException
        public CryptographicException SetStackTrace(StackTrace source)
            {
            ExternalStackTrace = source;
            return this;
            }
        #endregion
        #region M:GetMessage:IEnumerable<String>
        private IEnumerable<String> GetMessage() {
            var r = new HashSet<String>{ };
            var o = OriginalMessage.TrimEnd('.',' ');
            yield return o;
            r.Add(o);
            foreach (var i in InnerExceptions) {
                o = i.Message.TrimEnd('.',' ');
                if (r.Add(o)) {
                    yield return o;
                    }
                }
            }
        #endregion
        }
    }
