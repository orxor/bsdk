namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="UTCTIME"/> type.
    /// </summary>
    internal sealed class Asn1UtcTime : Asn1Time
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.UtcTime"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.UtcTime; }}
        }
    }