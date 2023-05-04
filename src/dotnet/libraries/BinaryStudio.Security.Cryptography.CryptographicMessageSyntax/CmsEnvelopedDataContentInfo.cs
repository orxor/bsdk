using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [UsedImplicitly]
    [CmsSpecific(ObjectIdentifiers.szOID_PKCS_7_ENVELOPED)]
    public class CmsEnvelopedDataContentInfo : CmsContentInfo
        {
        internal CmsEnvelopedDataContentInfo(Asn1Object source)
            : base(source)
            {
            }
        }
    }