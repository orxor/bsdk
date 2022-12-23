using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1CertificateRevocationListEntry : Asn1LinkObject
        {
        public String SerialNumber { get; }
        public DateTime RevocationDate { get; }
        public IList<Asn1CertificateExtension> Extensions { get; }
        internal Asn1CertificateRevocationListEntry(Asn1Object source)
            : base(source)
            {
            SerialNumber = String.Join(String.Empty, ((Asn1Integer)source[0]).Value.ToByteArray().Select(i => i.ToString("X2")));
            RevocationDate = (Asn1Time)source[1];
            if (source.Count > 2) {
                Extensions = source[2].Select(
                    i => Asn1CertificateExtension.From(
                        new Asn1CertificateExtension(i))).ToArray();
                }
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return SerialNumber;
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(SerialNumber), SerialNumber);
                writer.WriteValue(nameof(RevocationDate), RevocationDate.ToString("O"));
                writer.WriteValueIfNotNull(nameof(Extensions),Extensions);
                }
            }
        }
    }
