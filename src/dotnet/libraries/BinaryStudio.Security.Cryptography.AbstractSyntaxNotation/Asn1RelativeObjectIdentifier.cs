namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="RELATIVE-OID"/> type.
    /// </summary>
    public sealed class Asn1RelativeObjectIdentifier : Asn1ObjectIdentifier
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.RelativeObjectIdentifier"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.RelativeObjectIdentifier; }}
        }
    }