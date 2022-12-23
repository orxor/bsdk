using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1CertificateExtensionCollection : ReadOnlyCollection<Asn1CertificateExtension>,IJsonSerializable
        {
        public Asn1CertificateExtensionCollection(IList<Asn1CertificateExtension> source)
            : base(source)
            {
            }

        public Asn1CertificateExtensionCollection(IEnumerable<Asn1CertificateExtension> source)
            : base(source.ToArray())
            {
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public void WriteTo(IJsonWriter writer) {
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(Count), Count);
                writer.WritePropertyName("{Self}");
                using (writer.ArrayObject()) {
                    foreach (var i in this) {
                        writer.WriteValue(i);
                        }
                    }
                }
            }
        }
    }