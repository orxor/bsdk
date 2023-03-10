using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    public class CertificateConstraintException : CertificateException
        {
        public CertificateConstraintException(String message)
            : base(message)
            {
            }
        }
    }