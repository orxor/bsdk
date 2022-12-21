using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_DELTA_CRL_INDICATOR)]
    internal class Asn1DeltaCRLIndicator : Asn1CertificateExtension
        {
        public BigInteger MinimumBaseCRLNumber { get; }
        public Asn1DeltaCRLIndicator(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    MinimumBaseCRLNumber = (Asn1Integer)octet[0];
                    }
                }
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.ScopeObject()) {
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue(nameof(MinimumBaseCRLNumber), String.Join(
                    String.Empty,
                    MinimumBaseCRLNumber.
                        ToByteArray().
                        Reverse().
                        Select(i => i.ToString("X2"))));
                }
            }
        }
    }