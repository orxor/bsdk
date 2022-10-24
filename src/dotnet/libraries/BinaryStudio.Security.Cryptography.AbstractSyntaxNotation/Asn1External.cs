namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="EXTERNAL"/> type.
    /// </summary>
    public sealed class Asn1External : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.External"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.External; }}
        }
    }