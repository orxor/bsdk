using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [UsedImplicitly]
    [CmsSpecific(ObjectIdentifiers.NSS_OID_PKCS12_SAFE_CONTENTS_BAG)]
    public class PKCS12SafeContentBag : PKCS12SafeBag
        {
        internal PKCS12SafeContentBag(Asn1Object o)
            : base(o)
            {
            }
        }
    }