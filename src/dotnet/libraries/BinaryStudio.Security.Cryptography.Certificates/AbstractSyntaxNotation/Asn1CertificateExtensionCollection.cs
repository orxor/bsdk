using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1CertificateExtensionCollection : ReadOnlyCollection<CertificateExtension>,IJsonSerializable
        {
        public Asn1CertificateExtensionCollection(IList<CertificateExtension> source)
            : base(source)
            {
            }

        public Asn1CertificateExtensionCollection(IEnumerable<CertificateExtension> source)
            : base(source.ToArray())
            {
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteValue(nameof(Count), Count);
                writer.WritePropertyName("{Self}");
                using (writer.Array()) {
                    foreach (var i in this) {
                        writer.WriteValue(i);
                        }
                    }
                }
            }
        }
    }