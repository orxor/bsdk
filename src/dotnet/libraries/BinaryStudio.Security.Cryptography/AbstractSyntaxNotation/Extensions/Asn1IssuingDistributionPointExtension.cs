using System;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
{
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) issuingDistributionPoint(28)}
     * 2.5.29.28
     * /Joint-ISO-ITU-T/5/29/28
     * Issuing distribution point 
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_ISSUING_DIST_POINT)]
    internal class Asn1IssuingDistributionPointExtension : Asn1CertificateExtension
        {
        public DistributionPointName DistributionPoint { get; }
        public Boolean OnlyContainsUserCerts { get; }
        public Boolean OnlyContainsCACerts { get; }
        public Boolean IndirectCrl { get; }
        public ReasonFlags ReasonFlags { get; }

        public Asn1IssuingDistributionPointExtension(Asn1CertificateExtension u)
            :base(u)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count == 1) {
                    if (octet[0] is Asn1Sequence sequence) {
                        #region DistributionPoint [0]
                        var c = sequence.OfType<Asn1ContextSpecificObject>().FirstOrDefault(i => i.Type == 0);
                        if (c != null) {
                            DistributionPoint = new DistributionPointName(c);
                            }
                        #endregion
                        OnlyContainsUserCerts = ToBoolean(sequence.OfType<Asn1ContextSpecificObject>().FirstOrDefault(i => i.Type == 1));
                        OnlyContainsCACerts   = ToBoolean(sequence.OfType<Asn1ContextSpecificObject>().FirstOrDefault(i => i.Type == 2));
                        IndirectCrl           = ToBoolean(sequence.OfType<Asn1ContextSpecificObject>().FirstOrDefault(i => i.Type == 4));
                        ReasonFlags           = ToReasonFlags(sequence.OfType<Asn1ContextSpecificObject>().FirstOrDefault(i => i.Type == 3));
                        }
                    }
                }
            }

        public override String ToString()
            {
            return DistributionPoint.ToString();
            }

        private static ReasonFlags ToReasonFlags(Asn1ContextSpecificObject source) {
            if (source == null) { return ReasonFlags.Unused; }
            var r = source.Content.ToArray();
            return (ReasonFlags)r[0];
            }

        private static Boolean ToBoolean(Asn1ContextSpecificObject source) {
            if (source == null) { return false; }
            var r = source.Content.ToArray();
            return r[0] != 0;
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.ScopeObject()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue(nameof(ReasonFlags), ReasonFlags.ToString());
                writer.WriteValue(nameof(OnlyContainsUserCerts), OnlyContainsUserCerts);
                writer.WriteValue(nameof(OnlyContainsCACerts), OnlyContainsCACerts);
                writer.WriteValue(nameof(IndirectCrl), IndirectCrl);
                writer.WriteValue(nameof(DistributionPoint), DistributionPoint);
                }
            }
        }
    }