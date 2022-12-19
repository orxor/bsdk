using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    [Asn1CertificateExtension("2.5.29.9")]
    internal class Asn1SubjectDirectoryAttributes : Asn1CertificateExtension
        {
        public IList<Asn1CertificateAttribute> Attributes { get; }
        public Asn1SubjectDirectoryAttributes(Asn1CertificateExtension source)
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
        public override void WriteTo(IJsonWriter writer)
            {
            base.WriteTo(writer);
            }
        }
    }