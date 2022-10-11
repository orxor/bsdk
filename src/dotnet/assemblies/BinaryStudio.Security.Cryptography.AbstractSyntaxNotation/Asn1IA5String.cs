namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="IA5STRING"/> type.
    /// </summary>
    internal sealed class Asn1IA5String : Asn1String
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.IA5String"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.IA5String; }}
        }
    }