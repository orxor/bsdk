using System;
using BinaryStudio.PlatformComponents.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    [UsedImplicitly]
    public class CertificateInvalidFormatException : CertificateException
        {
        public CertificateInvalidFormatException(String message)
            :base(message)
            {
            }
        }
    }