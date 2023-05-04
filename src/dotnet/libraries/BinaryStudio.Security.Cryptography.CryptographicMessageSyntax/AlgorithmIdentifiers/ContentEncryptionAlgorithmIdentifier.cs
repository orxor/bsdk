using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    public class ContentEncryptionAlgorithmIdentifier : X509AlgorithmIdentifier
        {
        internal ContentEncryptionAlgorithmIdentifier(Asn1Sequence o)
            : base(o)
            {
            }
        }
    }