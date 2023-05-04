using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    public class DigestAlgorithmIdentifier : X509AlgorithmIdentifier
        {
        internal DigestAlgorithmIdentifier(Asn1Sequence o)
            : base(o)
            {
            }
        }
    }