namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="OBJECT DESCRIPTOR"/> type.
    /// </summary>
    public sealed class Asn1ObjectDescriptor : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.ObjectDescriptor"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.ObjectDescriptor; }}
        }
    }