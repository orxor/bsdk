using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [UsedImplicitly]
    [CmsSpecific(ObjectIdentifiers.szOID_PKCS_7_SIGNEDANDENVELOPED)]
    internal class CmsSignEnvDataContentInfo : CmsContentInfo
        {
        internal CmsSignEnvDataContentInfo(Asn1Object source)
            : base(source)
            {
            }
        }
    }