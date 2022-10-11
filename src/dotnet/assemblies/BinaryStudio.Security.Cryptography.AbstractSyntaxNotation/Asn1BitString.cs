namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="BIT STRING"/> type.
    /// </summary>
    public sealed class Asn1BitString : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.BitString"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.BitString; }}
        }
    }