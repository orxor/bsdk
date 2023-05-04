using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [UsedImplicitly]
    [CmsSpecific(ObjectIdentifiers.NSS_OID_PKCS12_CERT_BAG)]
    public class PKCS12CertBag : PKCS12SafeBag
        {
        internal PKCS12CertBag(Asn1Object o)
            : base(o)
            {
            }
        }
    }