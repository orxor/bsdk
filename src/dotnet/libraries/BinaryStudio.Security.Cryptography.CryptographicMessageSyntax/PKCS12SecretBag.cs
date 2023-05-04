using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [UsedImplicitly]
    [CmsSpecific(ObjectIdentifiers.NSS_OID_PKCS12_SECRET_BAG)]
    public class PKCS12SecretBag : PKCS12SafeBag
        {
        internal PKCS12SecretBag(Asn1Object o)
            : base(o)
            {
            }
        }
    }