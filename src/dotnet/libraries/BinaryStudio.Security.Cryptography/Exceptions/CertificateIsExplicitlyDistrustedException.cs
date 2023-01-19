﻿using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_IS_EXPLICIT_DISTRUST)]
    public class CertificateIsExplicitlyDistrustedException : CertificateException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateIsExplicitlyDistrustedException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateIsExplicitlyDistrustedException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_IS_EXPLICIT_DISTRUST)))
            {
            }
        }
    }