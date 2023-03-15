using System;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("1.2.840.113533.7.65.0")]
    internal class Asn1EntrustVersionInfo : CertificateExtension
        {
        public String Version { get; }
        public Asn1BitString Flags { get; }
        public Asn1EntrustVersionInfo(CertificateExtension source)
            :base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    Version = (Asn1String)octet[0][0];
                    Flags = (Asn1BitString)octet[0][1];
                    }
                }
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue(nameof(Version), Version);
                writer.WriteValue(nameof(Flags), String.Join(String.Empty, Flags.Content.ToArray().Select(i => i.ToString("X2"))));
                }
            }
        }
    }