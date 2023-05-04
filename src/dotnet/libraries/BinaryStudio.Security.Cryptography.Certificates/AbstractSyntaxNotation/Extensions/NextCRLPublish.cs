using System;
using System.Globalization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {iso(1) identified-organization(3) dod(6) internet(1) private(4) enterprise(1) 311 21 4}
     * 1.3.6.1.4.1.311.21.4
     * /ISO/Identified-Organization/6/1/4/1/311/21/4
     * szOID_CRL_NEXT_PUBLISH
     * */
    [UsedImplicitly]
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_CRL_NEXT_PUBLISH)]
    internal class NextCRLPublish : CertificateExtension
        {
        public DateTime Value { get; }

        #region ctor{CertificateExtension}
        internal NextCRLPublish(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0)
                    {
                    Value = (Asn1Time)octet[0];
                    }
                }
            }
        #endregion

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue(nameof(Value), Value);
                }
            }
        }
    }