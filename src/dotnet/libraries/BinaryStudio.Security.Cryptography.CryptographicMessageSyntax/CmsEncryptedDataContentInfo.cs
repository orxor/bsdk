using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [CmsSpecific(ObjectIdentifiers.szOID_PKCS_7_ENCRYPTED)]
    [UsedImplicitly]
    public class CmsEncryptedDataContentInfo : CmsContentInfo
        {
        public CmsEncryptedDataContentInfo(Asn1Object source)
            : base(source)
            {
            }
        }
    }