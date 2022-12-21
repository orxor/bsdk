using System;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Converters;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) privateKeyUsagePeriod(16)}
     * 2.5.29.16
     * /Joint-ISO-ITU-T/5/29/16
     * Private key usage period
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_PRIVATEKEY_USAGE_PERIOD)]
    internal class Asn1CertificatePrivateKeyUsagePeriodExtension : Asn1CertificateExtension
        {
        public DateTime? NotBefore { get; }
        public DateTime? NotAfter  { get; }

        public Asn1CertificatePrivateKeyUsagePeriodExtension(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    var contextspecifics = octet[0].Where(i => (i.Class == Asn1ObjectClass.ContextSpecific)).ToArray();
                    NotBefore = ToDateTime(contextspecifics.FirstOrDefault(i => ((Asn1ContextSpecificObject)i).Type == 0));
                    NotAfter  = ToDateTime(contextspecifics.FirstOrDefault(i => ((Asn1ContextSpecificObject)i).Type == 1));
                    }
                }
            }

        private static DateTime? ToDateTime(Asn1Object source) {
            if (source != null) {
                var value = source.Content.ToArray();
                if (value.Length > 0) {
                    var r = new Asn1GeneralTime(value);
                    return r.Value.LocalDateTime;
                    }
                }
            return null;
            }

        private static String ToString(DateTime? value) {
            return (value != null)
                ? Asn1DateTimeConverter.ToString(value.Value)
                : "*";
            }

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return $"[{ToString(NotBefore)}]-[{ToString(NotAfter)}]";
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.ScopeObject()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue("NotBefore", NotBefore);
                writer.WriteValue("NotAfter",  NotAfter);
                }
            }
        }
    }