﻿using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_DEFINED_NAME_CONSTRAINT)]
    public class CertificateUndefinedNameConstraintException : CertificateConstraintException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateUndefinedNameConstraintException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateUndefinedNameConstraintException()
            :base(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_HAS_NOT_DEFINED_NAME_CONSTRAINT)))
            {
            }
        }
    }