namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="INTEGER"/> type.
    /// </summary>
    public sealed class Asn1Integer : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Integer"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Integer; }}
        }
    }