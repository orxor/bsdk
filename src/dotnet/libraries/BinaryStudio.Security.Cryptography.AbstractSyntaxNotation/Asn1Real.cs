namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="REAL"/> type.
    /// </summary>
    public sealed class Asn1Real : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Real"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Real; }}
        }
    }