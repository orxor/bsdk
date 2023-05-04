using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    public class DigestEncryptionAlgorithmIdentifier : X509AlgorithmIdentifier
        {
        internal DigestEncryptionAlgorithmIdentifier(Asn1Sequence o)
            : base(o)
            {
            }
        }
    }