namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="SEQUENCE"/> type.
    /// </summary>
    public sealed class Asn1Sequence : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Sequence"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Sequence; }}
        }
    }