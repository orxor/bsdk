﻿using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    public class CertificatePrivateKeyMissingException : CertificateException
        {
        #region ctor{String}
        /// <summary>Initializes a new instance of the <see cref="CertificatePrivateKeyMissingException"/> class with a specified message that describes the error.</summary>
        /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        public CertificatePrivateKeyMissingException(String message)
            : base(message)
            {
            }
        #endregion
        #region ctor{HRESULT}
        public CertificatePrivateKeyMissingException(HRESULT scode)
            : base(scode)
            {
            }
        #endregion
        }
    }