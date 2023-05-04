using System;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [Flags]
    public enum CryptGenKeyFlags
        {
        CRYPT_NONE              = 0x00000000,
        CRYPT_EXPORTABLE        = 0x00000001,
        CRYPT_USER_PROTECTED    = 0x00000002,
        CRYPT_CREATE_SALT       = 0x00000004,
        CRYPT_UPDATE_KEY        = 0x00000008,
        CRYPT_NO_SALT           = 0x00000010,
        CRYPT_PREGEN            = 0x00000040,
        CRYPT_RECIPIENT         = 0x00000010,
        CRYPT_INITIATOR         = 0x00000040,
        CRYPT_ONLINE            = 0x00000080,
        CRYPT_SF                = 0x00000100,
        CRYPT_CREATE_IV         = 0x00000200,
        CRYPT_KEK               = 0x00000400,
        CRYPT_DATA_KEY          = 0x00000800,
        CRYPT_VOLATILE          = 0x00001000,
        CRYPT_SGCKEY            = 0x00002000,
        CRYPT_ARCHIVABLE        = 0x00004000,
        CP_CRYPT_DH_ALLOWED     = 0x00002000,
        RSA1024BIT_KEY          = 0x04000000,
        RSA512BIT_KEY           = 0x02000000,
        RSA256BIT_KEY           = 0x01000000,
        CRYPT_FORCE_KEY_PROTECTION_HIGH = 0x00008000,
        }
    }
