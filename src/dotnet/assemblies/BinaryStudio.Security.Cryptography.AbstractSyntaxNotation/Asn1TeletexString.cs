namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="T61STRING"/> type.
    /// </summary>
    internal sealed class Asn1TeletexString : Asn1String
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.TeletexString"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.TeletexString; }}
        }
    }