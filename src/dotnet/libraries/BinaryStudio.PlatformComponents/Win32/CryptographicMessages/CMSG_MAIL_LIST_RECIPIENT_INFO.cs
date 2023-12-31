﻿using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace BinaryStudio.PlatformComponents.Win32
    {
    using CRYPT_DATA_BLOB = CRYPT_BLOB;
    [StructLayout(LayoutKind.Sequential)]
    public struct CMSG_MAIL_LIST_RECIPIENT_INFO
        {
        int dwVersion;
        CRYPT_DATA_BLOB KeyId;
        CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;
        CRYPT_DATA_BLOB EncryptedKey;
        FILETIME Date;
        unsafe CRYPT_ATTRIBUTE_TYPE_VALUE* pOtherAttr;
        }
    }
