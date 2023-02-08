using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [UsedImplicitly]
    [CmsSpecific(ObjectIdentifiers.szOID_PKCS_7_DIGESTED)]
    public class CmsDigestedDataContentInfo : CmsContentInfo
        {
        internal CmsDigestedDataContentInfo(Asn1Object source)
            : base(source)
            {
            }
        }
    }