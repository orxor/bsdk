namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="VISIBLESTRING"/> type.
    /// </summary>
    internal sealed class Asn1VisibleString : Asn1String
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.VisibleString"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.VisibleString; }}
        }
    }