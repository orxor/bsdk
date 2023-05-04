using System.Globalization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [UsedImplicitly]
    [Asn1CertificateExtension(ObjectIdentifiers.NSS_OID_X509_CERTIFICATE_ISSUER)]
    public sealed class CertificateIssuer : CertificateExtension
        {
        public IX509GeneralName Value { get; }

        #region ctor{CertificateExtension}
        internal CertificateIssuer(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    Value = X509GeneralName.From((Asn1ContextSpecificObject)octet[0][0]);
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
                writer.WriteValue(nameof(Value), Value.ToString());
                }
            }
        }
    }
