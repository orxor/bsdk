using System;
using System.Globalization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension(ObjectIdentifiers.szCPOID_IssuerSignTool)]
    internal sealed class Asn1CertificateIssuerDigitalSigningToolExtension : Asn1CertificateExtension
        {
        public String SoftwareUsedToCreateDigitalSignature { get; }
        public String CertificationAuthorityDescriptiveName { get; }
        public String ConformityPropertiesOfSoftwareUsedToCreateDigitalSignature { get; }
        public String ConformityPropertiesOfCertificationAuthority { get; }

        public Asn1CertificateIssuerDigitalSigningToolExtension(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (octet != null) {
                if (octet.Count > 0) {
                    var sequence = octet[0];
                    if (sequence.Count > 0) { SoftwareUsedToCreateDigitalSignature = ((Asn1String)sequence[0]).Value; }
                    if (sequence.Count > 1) { CertificationAuthorityDescriptiveName = ((Asn1String)sequence[1]).Value; }
                    if (sequence.Count > 2) { ConformityPropertiesOfSoftwareUsedToCreateDigitalSignature = ((Asn1String)sequence[2]).Value; }
                    if (sequence.Count > 3) { ConformityPropertiesOfCertificationAuthority = ((Asn1String)sequence[3]).Value; }
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
                if (!String.IsNullOrEmpty(SoftwareUsedToCreateDigitalSignature))                       { writer.WriteValue(nameof(SoftwareUsedToCreateDigitalSignature), SoftwareUsedToCreateDigitalSignature);                                             }
                if (!String.IsNullOrEmpty(CertificationAuthorityDescriptiveName))                      { writer.WriteValue(nameof(CertificationAuthorityDescriptiveName), CertificationAuthorityDescriptiveName);                                           }
                if (!String.IsNullOrEmpty(ConformityPropertiesOfSoftwareUsedToCreateDigitalSignature)) { writer.WriteValue(nameof(ConformityPropertiesOfSoftwareUsedToCreateDigitalSignature), ConformityPropertiesOfSoftwareUsedToCreateDigitalSignature); }
                if (!String.IsNullOrEmpty(ConformityPropertiesOfCertificationAuthority))               { writer.WriteValue(nameof(ConformityPropertiesOfCertificationAuthority), ConformityPropertiesOfCertificationAuthority);                             }
                }
            }
        }
    }