using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    public abstract class PKCS12SafeBag : CmsContentInfo
        {
        protected PKCS12SafeBag(Asn1Object o)
            : base(o)
            {
            }
        }
    }