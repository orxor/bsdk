using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    public class CertificateChainErrorStatusAttribute : Attribute
        {
        public CertificateChainErrorStatus Status { get; }
        public CertificateChainErrorStatusAttribute(CertificateChainErrorStatus status)
            {
            Status = status;
            }
        }
    }