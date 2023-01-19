using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    public class CertificateInvalidFormatException : CertificateException
        {
        public CertificateInvalidFormatException(String message)
            :base(message)
            {
            }
        }
    }