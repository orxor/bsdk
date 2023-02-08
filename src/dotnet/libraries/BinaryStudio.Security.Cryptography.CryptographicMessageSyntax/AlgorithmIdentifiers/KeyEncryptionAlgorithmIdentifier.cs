using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    public class KeyEncryptionAlgorithmIdentifier : X509AlgorithmIdentifier
        {
        internal KeyEncryptionAlgorithmIdentifier(Asn1Sequence o)
            : base(o)
            {
            }
        }
    }