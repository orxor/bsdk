namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="OCTET STRING"/> type.
    /// </summary>
    public sealed class Asn1OctetString : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.OctetString"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.OctetString; }}
        }
    }