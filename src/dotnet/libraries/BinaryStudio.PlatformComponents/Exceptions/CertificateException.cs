using System;
using System.Collections.Generic;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    public class CertificateException : CryptographicException
        {
        public CertificateException(HRESULT SCode)
            :base(SCode)
            {
            }

        public CertificateException(HRESULT SCode,String message)
            :base(SCode,message)
            {
            }

        public CertificateException(String message)
            :base(message)
            {
            }

        #region ctor{HRESULT,IEnumerable<Exception>}
        public CertificateException(HRESULT scode, IEnumerable<Exception> innerExceptions)
            :base(scode,innerExceptions)
            {
            }
        #endregion
        }
    }
