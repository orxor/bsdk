﻿using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    public class CtlException : CryptographicException
        {
        /// <summary>Initializes a new instance of the <see cref="CtlException"/> class with a specified message that describes the error.</summary>
        /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        protected CtlException(String message)
            :base(message)
            {
            }
        }
    }