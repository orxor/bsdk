using System;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [Flags]
    public enum CRYPT_OID_INFO_KEY_TYPE
        {
        CRYPT_OID_INFO_OID_KEY           = 1,
        CRYPT_OID_INFO_NAME_KEY          = 2,
        CRYPT_OID_INFO_ALGID_KEY         = 3,
        CRYPT_OID_INFO_SIGN_KEY          = 4,
        CRYPT_OID_INFO_CNG_ALGID_KEY     = 5,
        CRYPT_OID_INFO_CNG_SIGN_KEY      = 6,
        CRYPT_OID_INFO_PUBKEY_SIGN_KEY_FLAG    = unchecked((Int32)0x80000000),
        CRYPT_OID_INFO_PUBKEY_ENCRYPT_KEY_FLAG = unchecked((Int32)0x40000000)
        }
    }