using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    public class CertificateException : CryptographicException
        {
        public CertificateException(HResult SCode)
            :base(SCode)
            {
            }
        }
    }
