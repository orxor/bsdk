using System;
using System.Globalization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
{
    [Asn1CertificateExtension(ObjectIdentifiers.NSS_OID_X509_REASON_CODE)]
    public sealed class CRLReason : CertificateExtension
        {
        public X509CrlReason ReasonCode { get; }

        #region ctor{CertificateExtension}
        internal CRLReason(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    var content = octet[0].Content.ToArray();
                    ReasonCode = (X509CrlReason)(Int32)content[0];
                    }
                }
            }
        #endregion
        #region ctor{X509CrlReason}
        public CRLReason(X509CrlReason ReasonCode)
            : base(ObjectIdentifiers.NSS_OID_X509_REASON_CODE,false)
            {
            this.ReasonCode = ReasonCode;
            }
        #endregion

        protected override void BuildBody(ref Asn1OctetString o) {
            if (o == null) {
                o = new Asn1OctetString{
                    new Asn1Enum<X509CrlReason>(ReasonCode)
                    };
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
                writer.WriteValue(nameof(ReasonCode), ReasonCode);
                }
            }
        }
    }