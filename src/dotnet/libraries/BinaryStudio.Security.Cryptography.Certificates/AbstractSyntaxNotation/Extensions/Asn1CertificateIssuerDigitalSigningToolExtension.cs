using System;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("1.2.643.100.112")]
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
        }
    }