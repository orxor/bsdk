using System;
using System.Globalization;
using System.Security.Cryptography;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
{
    [Asn1CertificateExtension(ObjectIdentifiers.NSS_OID_X509_SUBJECT_KEY_ID)]
    public sealed class CertificateSubjectKeyIdentifier : CertificateExtension
        {
        public Byte[] KeyIdentifier { get; }

        #region ctor{CertificateExtension}
        internal CertificateSubjectKeyIdentifier(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    octet = (Asn1OctetString)octet[0];
                    KeyIdentifier = octet.Content.ToArray();
                    }
                else
                    {
                    KeyIdentifier = octet.Content.ToArray();
                    }
                }
            }
        #endregion
        #region ctor{Boolean,String}
        public CertificateSubjectKeyIdentifier(Boolean critical, String value)
            :base(new Oid(ObjectIdentifiers.NSS_OID_X509_SUBJECT_KEY_ID),critical)
            {
            KeyIdentifier = DecodeString(value);
            Body = new Asn1OctetString(new Asn1OctetString(KeyIdentifier));
            }
        #endregion

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return KeyIdentifier.ToString("x");
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue("KeyIdentifier", KeyIdentifier.ToString("x"));
                }
            }
        }
    }