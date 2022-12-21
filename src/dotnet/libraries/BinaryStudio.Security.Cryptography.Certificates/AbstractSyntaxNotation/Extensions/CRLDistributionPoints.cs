using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /**
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) cRLDistributionPoints(31)}
     * 2.5.29.31
     * /Joint-ISO-ITU-T/5/29/31
     * Certificate Revocation List (CRL) distribution points
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.NSS_OID_X509_CRL_DIST_POINTS)]
    public class CRLDistributionPoints : Asn1CertificateExtension
        {
        public IList<DistributionPoint> DistributionPoints { get; }
        public CRLDistributionPoints(Asn1CertificateExtension u)
            :base(u)
            {
            DistributionPoints = new DistributionPoint[0];
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count == 1) {
                    if (octet[0] is Asn1Sequence sequence) {
                        DistributionPoints = sequence.
                            OfType<Asn1Sequence>().
                            Select(i => new DistributionPoint(i)).
                            ToArray();
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
                writer.WritePropertyName(nameof(DistributionPoints));
                using (writer.ArrayObject()) {
                    foreach (var i in DistributionPoints) {
                        i.WriteTo(writer);
                        }
                    }
                }
            }
        }
    }