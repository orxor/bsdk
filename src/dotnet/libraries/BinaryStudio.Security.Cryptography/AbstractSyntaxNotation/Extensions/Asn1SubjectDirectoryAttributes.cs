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
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) subjectDirectoryAttributes(9)}
     * 2.5.29.9
     * /Joint-ISO-ITU-T/5/29/9
     * Subject directory attributes certificate extension
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.NSS_OID_X509_SUBJECT_DIRECTORY_ATTRIBUTES)]
    internal class Asn1SubjectDirectoryAttributes : CertificateExtension
        {
        public IList<Asn1CertificateAttribute> Attributes { get; }
        public Asn1SubjectDirectoryAttributes(CertificateExtension source)
            : base(source)
            {
            Attributes = new List<Asn1CertificateAttribute>();
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    if (octet[0] is Asn1Sequence sequence) {
                        foreach (var i in sequence.OfType<Asn1Sequence>()) {
                            Attributes.Add(new Asn1CertificateAttribute(i));
                            }
                        }
                    }
                }
            Attributes = new ReadOnlyCollection<Asn1CertificateAttribute>(Attributes);
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                if (!IsNullOrEmpty(Attributes)) {
                    writer.WritePropertyName(nameof(Attributes));
                    using (writer.Array()) {
                        foreach (var i in Attributes) {
                            i.WriteTo(writer);
                            }
                        }
                    }
                }
            }
        }
    }