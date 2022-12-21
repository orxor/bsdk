using System.Collections.Generic;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1ObjectIdentifierCollection : Asn1ReadOnlyCollection<Asn1ObjectIdentifier>,IJsonSerializable
        {
        public Asn1ObjectIdentifierCollection(IList<Asn1ObjectIdentifier> source)
            : base(source)
            {
            }

        public Asn1ObjectIdentifierCollection(IEnumerable<Asn1ObjectIdentifier> source)
            : base(source)
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
                        writer.WriteValue(i.ToString());
                        }
                    }
                }
            }
        }
    }
