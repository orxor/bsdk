using System;
using System.Linq;
using System.Text;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    //[Asn1SpecificObject("2.5.29.31")]
    internal sealed class Asn1CertificateCRLDistributionPointsExtension : Asn1CertificateExtension
        {
        public X509StringCollection Value { get; }
        public Asn1CertificateCRLDistributionPointsExtension(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    Value = new X509StringCollection(octet[0].
                        Where(i => (i is Asn1ContextSpecificObject) && (((Asn1ContextSpecificObject)i).Type == 6)).
                        OfType<Asn1ContextSpecificObject>().
                        Select(i => Encoding.UTF8.GetString(i.Content.ToArray())));
                    }
                }
            }

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return (Value != null)
                ? Value.ToString()
                : base.ToString();
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer)
            {
            base.WriteTo(writer);
            }
        }
    }