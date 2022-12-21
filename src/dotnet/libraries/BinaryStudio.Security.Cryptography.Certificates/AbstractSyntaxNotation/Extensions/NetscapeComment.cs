using System;
using System.Globalization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.Certificates.AbstractSyntaxNotation.Extensions
    {
    /**
     * netscape OBJECT IDENTIFIER ::= { 2 16 840 1 113730 }
     * netscape-cert-extension OBJECT IDENTIFIER :: = { netscape 1 }
     * netscape-comment OBJECT IDENTIFIER ::= { netscape-cert-extension 13 }
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_NETSCAPE_COMMENT)]
    internal class NetscapeComment : Asn1CertificateExtension
        {
        public String Comment { get; }
        public NetscapeComment(Asn1Object source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    Comment = (octet[0] as Asn1String)?.Value;
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
                writer.WriteValueIfNotNull(nameof(Comment), Comment);
                }
            }
        }
    }
