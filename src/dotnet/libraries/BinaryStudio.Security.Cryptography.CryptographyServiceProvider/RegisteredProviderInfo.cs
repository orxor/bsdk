using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    public sealed class RegisteredProviderInfo
        {
        public CRYPT_PROVIDER_TYPE ProviderType { get; }
        public String ProviderName { get; }

        internal RegisteredProviderInfo(CRYPT_PROVIDER_TYPE providerType, String name) {
            ProviderType = providerType;
            ProviderName = name;
            }
        }
    }