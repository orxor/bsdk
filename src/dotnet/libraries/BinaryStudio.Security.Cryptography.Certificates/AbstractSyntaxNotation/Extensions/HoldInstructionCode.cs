using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("2.5.29.23")]
    public sealed class HoldInstructionCode : Asn1CertificateExtension
        {
        public Asn1ObjectIdentifier Value { get; }
        public HoldInstructionCode(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    Value = (Asn1ObjectIdentifier)octet[0];
                    }
                }
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer)
            {
            base.WriteTo(writer);
            }
        }
    }