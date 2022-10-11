namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="BOOLEAN"/> type.
    /// </summary>
    public sealed class Asn1Boolean : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Boolean"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Boolean; }}
        }
    }