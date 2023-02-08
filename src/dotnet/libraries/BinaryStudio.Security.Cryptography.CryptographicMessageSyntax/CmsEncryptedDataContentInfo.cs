﻿using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [UsedImplicitly]
    [CmsSpecific(ObjectIdentifiers.szOID_PKCS_7_ENCRYPTED)]
    public class CmsEncryptedDataContentInfo : CmsContentInfo
        {
        internal CmsEncryptedDataContentInfo(Asn1Object source)
            : base(source)
            {
            }
        }
    }