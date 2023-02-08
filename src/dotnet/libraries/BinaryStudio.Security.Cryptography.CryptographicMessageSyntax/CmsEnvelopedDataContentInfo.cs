using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [CmsSpecific(ObjectIdentifiers.szOID_PKCS_7_ENVELOPED)]
    [UsedImplicitly]
    public class CmsEnvelopedDataContentInfo : CmsContentInfo
        {
        public CmsEnvelopedDataContentInfo(Asn1Object source)
            : base(source)
            {
            }
        }
    }