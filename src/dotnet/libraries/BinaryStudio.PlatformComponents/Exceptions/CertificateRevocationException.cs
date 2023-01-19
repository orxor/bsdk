using HRESULT=BinaryStudio.PlatformComponents.Win32.HResult;

namespace BinaryStudio.Security.Cryptography
    {
    public class CertificateRevocationException : CertificateException
        {
        public CertificateRevocationException()
            :this(HRESULT.CERT_E_CHAINING)
            {
            }

        public CertificateRevocationException(HRESULT SCode)
            :base(SCode)
            {
            }
        }
    }
