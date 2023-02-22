using System;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [Flags]
    public enum CryptographicContextFlags
        {
        CRYPT_NONE = 0,
        CRYPT_VERIFYCONTEXT                 = unchecked((Int32)0xF0000000),
        CRYPT_NEWKEYSET                     = 0x00000008,
        CRYPT_DELETEKEYSET                  = 0x00000010,
        CRYPT_MACHINE_KEYSET                = 0x00000020,
        CRYPT_SILENT                        = 0x00000040,
        CRYPT_DEFAULT_CONTAINER_OPTIONAL    = 0x00000080
        }
    }