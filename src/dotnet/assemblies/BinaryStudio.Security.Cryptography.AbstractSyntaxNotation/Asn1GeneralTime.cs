namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="GENERALIZEDTIME"/> type.
    /// </summary>
    internal sealed class Asn1GeneralTime : Asn1Time
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.GeneralTime"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.GeneralTime; }}
        }
    }