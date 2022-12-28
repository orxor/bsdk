using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    public class DistributionPointName : Asn1LinkObject<Asn1ContextSpecificObject>
        {
        public IList<IX509GeneralName> FullName { get; }

        internal DistributionPointName(Asn1ContextSpecificObject source)
            : base(source)
            {
            var r = source.OfType<Asn1ContextSpecificObject>().ToArray();
            var c = r.FirstOrDefault(i => i.Type == 0);
            if (c != null)
                {
                FullName = new ReadOnlyCollection<IX509GeneralName>(c.
                    OfType<Asn1ContextSpecificObject>().
                    Select(X509GeneralName.From).
                    Where(i => !i.IsEmpty).
                    ToArray());
                }
            c = r.FirstOrDefault(i => i.Type == 1);
            if (c != null)
                {
                //NameRelativeToCRLIssuer = new RelativeDistinguishedName((Asn1Sequence)c[0]);
                }
            }

        public override String ToString()
            {
            return String.Join(Environment.NewLine, FullName);
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (FullName != null) {
                using (writer.Array()) {
                    foreach (var name in FullName) {
                        name.WriteTo(writer);
                        }
                    }
                }
            }
        }
    }