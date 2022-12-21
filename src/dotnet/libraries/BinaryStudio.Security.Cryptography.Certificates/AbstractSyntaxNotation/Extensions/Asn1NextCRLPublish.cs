using System;
using System.Globalization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {iso(1) identified-organization(3) dod(6) internet(1) private(4) enterprise(1) 311 21 4}
     * 1.3.6.1.4.1.311.21.4
     * /ISO/Identified-Organization/6/1/4/1/311/21/4
     * szOID_CRL_NEXT_PUBLISH
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_CRL_NEXT_PUBLISH)]
    internal class Asn1NextCRLPublish : Asn1CertificateExtension
        {
        public DateTime Value { get; }
        public Asn1NextCRLPublish(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0)
                    {
                    Value = (Asn1Time)octet[0];
                    }
                }
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.ScopeObject()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue(nameof(Value), Value);
                }
            }
        }
    }