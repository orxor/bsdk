namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="OBJECT IDENTIFIER"/> type.
    /// </summary>
    public class Asn1ObjectIdentifier : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.ObjectIdentifier"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.ObjectIdentifier; }}
        }
    }