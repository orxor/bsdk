using System;
using Microsoft.Win32;

namespace BinaryStudio.Security.Cryptography.Specific.CryptoProCSP
    {
    public class CryptoProCSPRNGSource
        {
        #if NET5_0
        #else
        internal CryptoProCSPRNGSource(RegistryKey source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            }
        #endif
        }
    }