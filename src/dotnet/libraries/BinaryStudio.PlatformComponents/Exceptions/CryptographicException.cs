using System;
using System.Collections.Generic;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    public class CryptographicException : AggregateException
        {
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
        }
    }
