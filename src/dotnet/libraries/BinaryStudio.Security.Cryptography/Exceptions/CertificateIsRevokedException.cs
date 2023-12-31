﻿using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_IS_REVOKED)]
    public class CertificateIsRevokedException : CertificateRevocationException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateIsRevokedException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateIsRevokedException()
            :base(HRESULT.CRYPT_E_REVOKED,Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_IS_REVOKED)))
            {
            }
        }
    }