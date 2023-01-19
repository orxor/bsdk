using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
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
        }
    }
