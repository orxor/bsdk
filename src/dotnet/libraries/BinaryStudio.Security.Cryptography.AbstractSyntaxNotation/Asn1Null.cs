namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="NULL"/> type.
    /// </summary>
    public sealed class Asn1Null : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Null"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Null; }}
        }
    }