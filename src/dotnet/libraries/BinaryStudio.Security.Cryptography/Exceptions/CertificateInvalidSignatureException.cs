using System;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    [CertificateChainErrorStatus(CertificateChainErrorStatus.CERT_TRUST_IS_NOT_SIGNATURE_VALID)]
    public class CertificateInvalidSignatureException : CertificateSignatureException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateInvalidSignatureException"/> class with a specified message that describes the error.</summary>
        /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        public CertificateInvalidSignatureException(String message)
            : base(message)
            {
            }

        /// <summary>Initializes a new instance of the <see cref="CertificateInvalidSignatureException"/> class with a system-supplied message that describes the error.</summary>
        [UsedImplicitly]
        public CertificateInvalidSignatureException()
            :this(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.CERT_TRUST_IS_NOT_SIGNATURE_VALID)))
            {
            }
        }
    }