using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_KEY_PROV_INFO
        {
        public unsafe IntPtr ContainerName;
        public unsafe IntPtr ProviderName;
        public unsafe CRYPT_PROVIDER_TYPE ProviderType;
        public unsafe CryptographicContextFlags ProviderFlags;
        public unsafe Int32 ProviderParameterCount;
        public unsafe CRYPT_KEY_PROV_PARAM* ProviderParameters;
        public unsafe KEY_SPEC_TYPE KeySpec;
        }
    }