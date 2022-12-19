using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("2.5.29.29")]
    public sealed class CertificateIssuer : Asn1CertificateExtension
        {
        public IX509GeneralName Value { get; }
        public CertificateIssuer(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    Value = X509GeneralName.From((Asn1ContextSpecificObject)octet[0][0]);
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
