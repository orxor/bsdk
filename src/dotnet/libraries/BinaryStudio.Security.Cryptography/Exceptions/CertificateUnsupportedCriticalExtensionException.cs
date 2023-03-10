﻿using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_SUPPORTED_CRITICAL_EXT)]
    public class CertificateUnsupportedCriticalExtensionException : CertificateInvalidExtensionException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateUnsupportedCriticalExtensionException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateUnsupportedCriticalExtensionException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_SUPPORTED_CRITICAL_EXT)))
            {
            }
        }
    }