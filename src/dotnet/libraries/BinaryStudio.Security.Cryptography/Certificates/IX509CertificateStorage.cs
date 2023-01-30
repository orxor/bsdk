﻿using System;
using System.Collections.Generic;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public interface IX509CertificateStorage : IDisposable
        {
        IntPtr Handle { get; }
        X509StoreLocation Location { get; }
        IEnumerable<X509Certificate> Certificates { get; }
        IEnumerable<X509CertificateRevocationList> CertificateRevocationLists { get; }
        unsafe X509Certificate Find(CERT_INFO* Info);
        }
    }