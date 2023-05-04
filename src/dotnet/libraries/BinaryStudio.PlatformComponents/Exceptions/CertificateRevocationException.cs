using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    public class CertificateRevocationException : CertificateException
        {
        public CertificateRevocationException()
            :this(HRESULT.CERT_E_CHAINING)
            {
            }

        public CertificateRevocationException(String message)
            :this(HRESULT.CERT_E_CHAINING,message)
            {
            }

        public CertificateRevocationException(HRESULT SCode)
            :base(SCode)
            {
            }

        public CertificateRevocationException(HRESULT SCode,String message)
            :base(SCode,message)
            {
            }
        }
    }
