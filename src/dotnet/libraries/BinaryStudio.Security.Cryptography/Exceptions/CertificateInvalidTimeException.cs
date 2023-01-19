using System;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    [CertificateChainErrorStatus(CertificateChainErrorStatus.TrustIsNotTimeValid)]
    public class CertificateInvalidTimeException : CertificateException
        {
        /// <summary>Initializes a new instance of the <see cref="CertificateInvalidTimeException"/> class with a specified message that describes the error.</summary>
        /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        public CertificateInvalidTimeException(String message)
            :base(message)
            {
            }

        /// <summary>Initializes a new instance of the <see cref="CertificateInvalidTimeException"/> class with a system-supplied message that describes the error.</summary>
        public CertificateInvalidTimeException()
            :this(Resources.ResourceManager.GetString(nameof(CertificateChainErrorStatus.TrustIsNotTimeValid)))
            {
            }
        }
    }