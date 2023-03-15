using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) authorityKeyIdentifier(35)}
     * 2.5.29.35
     * /Joint-ISO-ITU-T/5/29/35
     * Authority key identifier
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.NSS_OID_X509_AUTH_KEY_ID)]
    public sealed class CertificateAuthorityKeyIdentifier : CertificateExtension
        {
        public Byte[] KeyIdentifier { get; }
        public String SerialNumber { get; }
        public IX509GeneralName CertificateIssuer { get; }

        #region ctor{CertificateExtension}
        internal CertificateAuthorityKeyIdentifier(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    var contextspecifics = octet[0].Where(i => (i.Class == Asn1ObjectClass.ContextSpecific)).ToArray();
                    #region [0]:KeyIdentifier
                    var specific = contextspecifics.FirstOrDefault(i => ((Asn1ContextSpecificObject)i).Type == 0);
                    if (specific != null) {
                        KeyIdentifier = specific.Content.ToArray();
                        }
                    #endregion
                    #region [1]:CertificateIssuer
                    specific = contextspecifics.FirstOrDefault(i => ((Asn1ContextSpecificObject)i).Type == 1);
                    if (specific != null) {
                        CertificateIssuer = X509GeneralName.From((Asn1ContextSpecificObject)specific[0]);
                        }
                    #endregion
                    #region [2]:SerialNumber
                    specific = contextspecifics.FirstOrDefault(i => ((Asn1ContextSpecificObject)i).Type == 2);
                    if (specific != null) {
                        SerialNumber = String.Join(String.Empty, specific.Content.ToArray().Select(i => i.ToString("x2")));
                        }
                    #endregion
                    }
                }
            }
        #endregion
        #region ctor{Asn1Certificate}
        public CertificateAuthorityKeyIdentifier(Asn1Certificate source)
            :base(new Oid(ObjectIdentifiers.NSS_OID_X509_AUTH_KEY_ID),false)
            {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var SKI = source.Extensions.
                    OfType<CertificateSubjectKeyIdentifier>().
                    FirstOrDefault();
            if (SKI == null) { throw new ArgumentOutOfRangeException(nameof(source)); }
            KeyIdentifier = SKI.KeyIdentifier;
            SerialNumber = source.SerialNumber;
            CertificateIssuer = source.Issuer;
            var args = new List<Asn1Object>();
            if (SKI.KeyIdentifier != null) { args.Add(new Asn1ContextSpecificObject(0,SKI.KeyIdentifier)); }
            args.Add(X509GeneralName.BuildContextSpecificObject(1,source.Subject));
            args.Add(new Asn1ContextSpecificObject(2,DecodeString(source.SerialNumber)));
            Body = new Asn1OctetString(new Asn1Sequence(args.ToArray()));
            }
        #endregion

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return (KeyIdentifier != null)
                ? KeyIdentifier.ToString("x")
                : base.ToString();
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue(nameof(KeyIdentifier), KeyIdentifier.ToString("x"));
                writer.WriteValueIfNotNull(nameof(SerialNumber), SerialNumber);
                writer.WriteValueIfNotNull(nameof(CertificateIssuer), CertificateIssuer);
                }
            }
        }
    }