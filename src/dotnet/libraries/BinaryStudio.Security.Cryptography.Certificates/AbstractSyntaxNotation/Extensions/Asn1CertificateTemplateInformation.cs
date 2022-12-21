using System;
using System.Globalization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /**
     * {iso(1) identified-organization(3) dod(6) internet(1) private(4) enterprise(1) 311 21 7}
     * {1.3.6.1.4.1.311.21.7}
     * {/ISO/Identified-Organization/6/1/4/1/311/21/7}
     * [MS-WCCE]
     * Internal Name: szOID_CERTIFICATE_TEMPLATE
     * Contains the information about the template. This extension value MUST be DER-encoded. The critical field for this extension SHOULD be set to FALSE.
     *
     * CertificateTemplateOID ::= SEQUENCE
     * {
     *   templateID              OBJECT IDENTIFIER,
     *   templateMajorVersion    INTEGER (0..4294967295) OPTIONAL,
     *   templateMinorVersion    INTEGER (0..4294967295) OPTIONAL
     * }
     */
    [Asn1CertificateExtension("1.3.6.1.4.1.311.21.7")]
    internal class Asn1CertificateTemplateInformation : Asn1CertificateExtension
        {
        public Asn1ObjectIdentifier TemplateId { get; }
        public Version Version { get; }
        public Asn1CertificateTemplateInformation(Asn1CertificateExtension source)
            : base(source)
            {
            Version = new Version(0,0);
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    if (octet[0] is Asn1Sequence sequence) {
                        var c = sequence.Count;
                        if (c == 0) { throw new ArgumentOutOfRangeException(nameof(source)); }
                        TemplateId = (Asn1ObjectIdentifier)sequence[0];
                        var major = 0;
                        var minor = 0;
                        if (c > 1) { major = (Asn1Integer)sequence[1]; }
                        if (c > 2) { minor = (Asn1Integer)sequence[2]; }
                        Version = new Version(major,minor);
                        }
                    }
                }
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.ScopeObject()) {
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue(nameof(TemplateId), TemplateId.ToString());
                writer.WriteValue(nameof(Version), Version.ToString());
                }
            }
        }
    }