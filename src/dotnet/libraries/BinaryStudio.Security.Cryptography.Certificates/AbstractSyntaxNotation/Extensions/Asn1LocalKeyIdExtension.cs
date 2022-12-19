using System;
using System.Text;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /**
     * {iso(1) member-body(2) us(840) rsadsi(113549) pkcs(1) pkcs-9(9) localKeyID(21)}
     * {1.2.840.113549.1.9.21}
     * {/ISO/Member-Body/US/113549/1/9/21}
     * Public-Key Cryptography Standards (PKCS) #9 localKeyID (for PKCS #12)
     *
     * localKeyId ATTRIBUTE ::=
     * {
     *   WITH SYNTAX OCTET STRING
     *   EQUALITY MATCHING RULE octetStringMatch
     *   SINGLE VALUE TRUE
     *   ID pkcs-9-at-localKeyId
     * }
     */
    [Asn1CertificateExtension("1.2.840.113549.1.9.21")]
    internal class Asn1LocalKeyIdExtension : Asn1CertificateExtension
        {
        public String LocalKeyId { get; }
        public Asn1LocalKeyIdExtension(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (ReferenceEquals(octet, null)) { throw new ArgumentOutOfRangeException(nameof(source), "Extension should contains [OctetString]"); }
            LocalKeyId = (octet.Count == 0)
                ? Encoding.ASCII.GetString(octet.Content.ToArray())
                : Encoding.ASCII.GetString(octet[0].Content.ToArray());
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer)
            {
            base.WriteTo(writer);
            }
        }
    }