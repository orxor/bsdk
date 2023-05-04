using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    internal class EncryptionAlgorithmIdentifier : X509AlgorithmIdentifier
        {
        internal EncryptionAlgorithmIdentifier(Asn1Sequence o)
            : base(o)
            {
            }
        }
    }