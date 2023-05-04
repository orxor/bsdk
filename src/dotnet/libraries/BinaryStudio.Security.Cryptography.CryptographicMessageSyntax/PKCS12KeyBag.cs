using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [UsedImplicitly]
    [CmsSpecific(ObjectIdentifiers.NSS_OID_PKCS12_KEY_BAG)]
    public class PKCS12KeyBag : PKCS12SafeBag
        {
        internal PKCS12KeyBag(Asn1Object o)
            : base(o)
            {
            }
        }
    }