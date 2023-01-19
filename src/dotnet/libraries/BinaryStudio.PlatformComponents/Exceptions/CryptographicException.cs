using System;
using System.Collections.Generic;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    public class CryptographicException : AggregateException
        {
        public CryptographicException(HResult SCode)
            :base(HResultException.FormatMessage(SCode))
            {
            HResult = (Int32)SCode;
            }

        public CryptographicException()
            {
            }

        public CryptographicException(String message)
            :base(message)
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
