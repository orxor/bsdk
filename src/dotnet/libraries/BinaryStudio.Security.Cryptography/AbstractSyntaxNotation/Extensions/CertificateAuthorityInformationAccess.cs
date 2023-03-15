using System;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
{
    /*
     * {iso(1) identified-organization(3) dod(6) internet(1) security(5) mechanisms(5) pkix(7) pe(1) authorityInfoAccess(1)}
     * 1.3.6.1.5.5.7.1.1
     * /ISO/Identified-Organization/6/1/5/5/7/1/1
     * Certificate authority information access
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_AUTHORITY_INFO_ACCESS)]
    internal sealed class Asn1CertificateAuthorityInformationAccessExtension : CertificateExtension
        {
        public Asn1CertificateAuthorityInformationAccessCollection AccessDescriptions { get; }
        public Asn1CertificateAuthorityInformationAccessExtension(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    var sequence = octet[0];
                    AccessDescriptions = new Asn1CertificateAuthorityInformationAccessCollection(
                        sequence.Select(i => new Asn1CertificateAuthorityInformationAccess(i)));
                    }
                }
            }

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return AccessDescriptions.ToString();
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue(nameof(AccessDescriptions), AccessDescriptions);
                }
            }
        }
    }