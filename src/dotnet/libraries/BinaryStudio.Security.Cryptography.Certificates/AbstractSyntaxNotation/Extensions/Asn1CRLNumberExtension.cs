using System;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) cRLNumber(20)}
     * 2.5.29.20
     * /Joint-ISO-ITU-T/5/29/20
     * Certificate Revocation List (CRL) number
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_CRL_NUMBER)]
    internal class Asn1CRLNumberExtension : Asn1CertificateExtension
        {
        public String Value { get; }
        public Asn1CRLNumberExtension(Asn1CertificateExtension source)
            :base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count == 1) {
                    Value = String.Join(String.Empty, octet[0].Content.ToArray().Select(i => i.ToString("X2")));
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