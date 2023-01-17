using System;
using System.Collections.Generic;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public interface IX509CertificateStorage : IDisposable
        {
        IntPtr Handle { get; }
        IEnumerable<X509Certificate> Certificates { get; }
        IEnumerable<X509CertificateRevocationList> CertificateRevocationLists { get; }
        }
    }