namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="BMPSTRING"/> type.
    /// </summary>
    internal sealed class Asn1UnicodeString : Asn1String
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.UnicodeString"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.UnicodeString; }}
        }
    }