using System;
using System.Globalization;
using System.Text;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
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
    [Asn1CertificateExtension(ObjectIdentifiers.NSS_OID_PKCS9_LOCAL_KEY_ID)]
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
        public override void WriteTo(IJsonWriter writer) {
            using (writer.ScopeObject()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue(nameof(LocalKeyId), LocalKeyId);
                }
            }
        }
    }