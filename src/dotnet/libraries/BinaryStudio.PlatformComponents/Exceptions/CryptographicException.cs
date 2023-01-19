using System;
using System.Collections.Generic;
using System.Diagnostics;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    public class CryptographicException : AggregateException
        {
        private StackTrace ExternalStackTrace;
        public CryptographicException(HRESULT SCode)
            :base(HResultException.FormatMessage(SCode))
            {
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
            }

        public CryptographicException(String message)
            :this(HRESULT.CORSEC_E_CRYPTO,message)
            {
            }

        public CryptographicException(String message, Exception innerException)
            :base(message, innerException)
            {
            }

        public CryptographicException(String message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions)
            {
            }

        #region ctor{HRESULT,IEnumerable<Exception>}
        public CryptographicException(HRESULT scode, IEnumerable<Exception> innerExceptions)
            : base(HResultException.FormatMessage(scode),innerExceptions)
            {
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
        #region M:SetStackTrace(StackTrace):CryptographicException
        public CryptographicException SetStackTrace(StackTrace source)
            {
            ExternalStackTrace = source;
            return this;
            }
        #endregion
        }
    }
