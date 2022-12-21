using System;
using System.Collections.Generic;
using System.Globalization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {joint-iso-itu-t(2) country(16) us(840) organization(1) netscape(113730) cert-ext(1) cert-type(1)}
     * {2.16.840.1.113730.1.1}
     * {/Country/US/1/113730/1/1}
     * Netscape certificate type (a Rec. ITU-T X.509 v3 certificate extension used to identify whether the certificate subject is a Secure Sockets Layer (SSL) client, an SSL server or a Certificate Authority (CA))
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_NETSCAPE_CERT_TYPE)]
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

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return Type.ToString();
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.ScopeObject()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                var r = new List<String>();
                var n = (Int32)Type;
                if ((n & (Int32)NetscapeCertificateType.SslClient)       != 0) { r.Add(nameof(NetscapeCertificateType.SslClient      )); n &= ~(Int32)NetscapeCertificateType.SslClient;       }
                if ((n & (Int32)NetscapeCertificateType.SslServer)       != 0) { r.Add(nameof(NetscapeCertificateType.SslServer      )); n &= ~(Int32)NetscapeCertificateType.SslServer;       }
                if ((n & (Int32)NetscapeCertificateType.SMime)           != 0) { r.Add(nameof(NetscapeCertificateType.SMime          )); n &= ~(Int32)NetscapeCertificateType.SMime;           }
                if ((n & (Int32)NetscapeCertificateType.ObjectSigning)   != 0) { r.Add(nameof(NetscapeCertificateType.ObjectSigning  )); n &= ~(Int32)NetscapeCertificateType.ObjectSigning;   }
                if ((n & (Int32)NetscapeCertificateType.Reserved)        != 0) { r.Add(nameof(NetscapeCertificateType.Reserved       )); n &= ~(Int32)NetscapeCertificateType.Reserved;        }
                if ((n & (Int32)NetscapeCertificateType.SslCA)           != 0) { r.Add(nameof(NetscapeCertificateType.SslCA          )); n &= ~(Int32)NetscapeCertificateType.SslCA;           }
                if ((n & (Int32)NetscapeCertificateType.SMimeCA)         != 0) { r.Add(nameof(NetscapeCertificateType.SMimeCA        )); n &= ~(Int32)NetscapeCertificateType.SMimeCA;         }
                if ((n & (Int32)NetscapeCertificateType.ObjectSigningCA) != 0) { r.Add(nameof(NetscapeCertificateType.ObjectSigningCA)); n &= ~(Int32)NetscapeCertificateType.ObjectSigningCA; }
                if (!IsNullOrEmpty(r)) {
                    writer.WriteValue(nameof(Type), r);
                    }
                }
            }
        }
    }