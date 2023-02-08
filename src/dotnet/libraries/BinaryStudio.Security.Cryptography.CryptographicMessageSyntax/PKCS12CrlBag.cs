using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [UsedImplicitly]
    [CmsSpecific(ObjectIdentifiers.NSS_OID_PKCS12_CRL_BAG)]
    public class PKCS12CrlBag : PKCS12SafeBag
        {
        internal PKCS12CrlBag(Asn1Object o)
            : base(o)
            {
            }
        }
    }