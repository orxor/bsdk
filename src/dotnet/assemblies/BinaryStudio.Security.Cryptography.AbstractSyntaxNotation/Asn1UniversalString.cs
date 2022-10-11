namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="UNIVERSALSTRING"/> type.
    /// </summary>
    internal sealed class Asn1UniversalString : Asn1String
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.UniversalString"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.UniversalString; }}
        }
    }