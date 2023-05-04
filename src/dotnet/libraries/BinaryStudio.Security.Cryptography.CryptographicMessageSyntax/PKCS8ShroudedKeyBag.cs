using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [UsedImplicitly]
    [CmsSpecific(ObjectIdentifiers.NSS_OID_PKCS12_PKCS8_SHROUDED_KEY_BAG)]
    public class PKCS8ShroudedKeyBag : PKCS12SafeBag
        {
        public EncryptedPrivateKeyInfo EncryptedPrivateKeyInfo { get; }
        internal PKCS8ShroudedKeyBag(Asn1Object o)
            : base(o)
            {
            State |= ObjectState.Failed;
            State &= ~ObjectState.DisposeUnderlyingObject;
            EncryptedPrivateKeyInfo = new EncryptedPrivateKeyInfo(o[1][0]);
            if (!EncryptedPrivateKeyInfo.IsFailed) {
                State &= ~ObjectState.Failed;
                State |= ObjectState.DisposeUnderlyingObject;
                }
            }
        }
    }