using System;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {joint-iso-itu-t(2) country(16) us(840) organization(1) netscape(113730) cert-ext(1) cert-type(1)}
     * {2.16.840.1.113730.1.1}
     * {/Country/US/1/113730/1/1}
     * Netscape certificate type (a Rec. ITU-T X.509 v3 certificate extension used to identify whether the certificate subject is a Secure Sockets Layer (SSL) client, an SSL server or a Certificate Authority (CA))
     * */
    [Asn1CertificateExtension("2.16.840.1.113730.1.1")]
    public sealed class NetscapeCertificateTypeExtension : Asn1CertificateExtension
        {
        public NetscapeCertificateType Type { get; }
        public NetscapeCertificateTypeExtension(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    Type = (NetscapeCertificateType)octet[0].Content.ToArray()[0];
                    }
                }
            }

        public override String ToString()
            {
            return Type.ToString();
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer)
            {
            base.WriteTo(writer);
            }
        }
    }