using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [CmsSpecific(ObjectIdentifiers.szOID_PKCS_7_DIGESTED)]
    [UsedImplicitly]
    public class CmsDigestedDataContentInfo : CmsContentInfo
        {
        public CmsDigestedDataContentInfo(Asn1Object source)
            : base(source)
            {
            }
        }
    }