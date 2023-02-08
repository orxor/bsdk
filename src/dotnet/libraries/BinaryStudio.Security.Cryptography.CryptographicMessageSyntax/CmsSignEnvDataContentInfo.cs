using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [CmsSpecific(ObjectIdentifiers.szOID_PKCS_7_SIGNEDANDENVELOPED)]
    [UsedImplicitly]
    internal class CmsSignEnvDataContentInfo : CmsContentInfo
        {
        public CmsSignEnvDataContentInfo(Asn1Object source)
            : base(source)
            {
            }
        }
    }