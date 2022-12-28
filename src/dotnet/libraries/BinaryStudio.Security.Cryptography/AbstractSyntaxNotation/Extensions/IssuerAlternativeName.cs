using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
{
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) issuerAltName(18)}
     * 2.5.29.18
     * /Joint-ISO-ITU-T/5/29/18
     * Issuer alternative name
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_ISSUER_ALT_NAME2)]
    public class IssuerAlternativeName : Asn1CertificateExtension
        {
        public IList<IX509GeneralName> AlternativeName { get; }
        public IssuerAlternativeName(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0)
                    {
                    AlternativeName = new ReadOnlyCollection<IX509GeneralName>(octet[0].
                        OfType<Asn1ContextSpecificObject>().
                        Select(X509GeneralName.From).
                        Where(i => !i.IsEmpty).
                        ToArray());
                    }
                }
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return ((AlternativeName != null) && (AlternativeName.Count > 0))
                ? String.Join(";", AlternativeName.Select(i => $"{{{X509GeneralName.ToString(i.Type)}}}:{{{i}}}"))
                : "{none}";
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WritePropertyName(nameof(AlternativeName));
                if (!IsNullOrEmpty(AlternativeName)) {
                    using (writer.Array()) {
                        foreach (var name in AlternativeName.OfType<IJsonSerializable>()) {
                            name.WriteTo(writer);
                            }
                        }
                    }
                else
                    {
                    writer.WriteValue("(No alternative name)");
                    }
                }
            }
        }
    }