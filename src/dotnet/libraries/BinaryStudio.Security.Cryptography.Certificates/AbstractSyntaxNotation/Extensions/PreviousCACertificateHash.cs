using System;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /**
     * {iso(1) identified-organization(3) dod(6) internet(1) private(4) enterprise(1) 311 21 2}
     * {1.3.6.1.4.1.311.21.2}
     * {/ISO/Identified-Organization/6/1/4/1/311/21/2}
     * szOID_CERTSRV_PREVIOUS_CERT_HASH
     */
    [UsedImplicitly]
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_CERTSRV_PREVIOUS_CERT_HASH)]
    internal class PreviousCACertificateHash : CertificateExtension
        {
        public String HashValue { get; }

        #region ctor{CertificateExtension}
        internal PreviousCACertificateHash(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (ReferenceEquals(octet, null)) { throw new ArgumentOutOfRangeException(nameof(source), "Extension should contains [OctetString]"); }
            if (octet.Count == 0)             { throw new ArgumentOutOfRangeException(nameof(source), "Extension should contains [Sequence]");    }
            if (octet[0] is Asn1OctetString hashvalue) {
                HashValue = String.Join(
                    String.Empty,
                    hashvalue.Content.ToArray().Select(i => i.ToString("X2")));
                }
            else
                {
                throw new ArgumentOutOfRangeException(nameof(source), "Extension should contains [OctetString]");
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
                writer.WriteValue(nameof(HashValue), HashValue);
                }
            }
        }
    }