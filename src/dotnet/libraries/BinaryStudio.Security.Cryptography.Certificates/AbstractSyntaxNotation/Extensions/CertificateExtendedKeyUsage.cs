using System;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Converters;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) extKeyUsage(37)}
     * 2.5.29.37
     * /Joint-ISO-ITU-T/5/29/37
     * Extended key usage 
     * IETF RFC 5280
     * */
    [UsedImplicitly]
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_ENHANCED_KEY_USAGE)]
    public sealed class CertificateExtendedKeyUsage : CertificateExtension
        {
        public Asn1ObjectIdentifierCollection Value { get; }

        #region ctor{CertificateExtension}
        internal CertificateExtendedKeyUsage(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    Value = new Asn1ObjectIdentifierCollection(octet[0].OfType<Asn1ObjectIdentifier>());
                    }
                }
            }
        #endregion
        #region ctor{{param}String[]}
        public CertificateExtendedKeyUsage(params String[] identifiers)
            : base(ObjectIdentifiers.szOID_ENHANCED_KEY_USAGE,false)
            {
            Value = new Asn1ObjectIdentifierCollection(identifiers.Select(i => new Asn1ObjectIdentifier(i)));
            Body = new Asn1OctetString{
                new Asn1Sequence(Value)
                };
            }
        #endregion

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString() {
            if (Value != null) {
                return String.Join(";", ((Asn1ObjectIdentifierCollection)Value).Select(Asn1DecodedObjectIdentifierTypeConverter.ToString));
                }
            return "(none)";
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WritePropertyName("{Self}");
                using (writer.Array()) {
                    foreach (var i in Value)
                        {
                        writer.WriteValue(i.ToString());
                        }
                    }
                }
            }
        }
    }