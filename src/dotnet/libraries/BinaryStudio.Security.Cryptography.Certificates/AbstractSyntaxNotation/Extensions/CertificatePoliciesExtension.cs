﻿using System;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) certificatePolicies(32)}
     * 2.5.29.32
     * /Joint-ISO-ITU-T/5/29/32
     * Certificate policies
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_CERT_POLICIES)]
    internal class CertificatePoliciesExtension : CertificateExtension
        {
        public Asn1ObjectIdentifierCollection CertificatePolicies { get; }

        #region ctor{CertificateExtension}
        internal CertificatePoliciesExtension(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    CertificatePolicies = new Asn1ObjectIdentifierCollection(
                        octet[0].OfType<Asn1Sequence>().
                        Select(i => (Asn1ObjectIdentifier)i[0]));
                    }
                }
            }
        #endregion
        #region ctor{{params}String[]}
        public CertificatePoliciesExtension(params String[] policies)
            : base(ObjectIdentifiers.szOID_CERT_POLICIES,false)
            {
            CertificatePolicies = new Asn1ObjectIdentifierCollection(policies.Select(i => new Asn1ObjectIdentifier(i)));
            }
        #endregion

        protected override void BuildBody(ref Asn1OctetString o) {
            if (o == null) {
                var r = new Asn1Sequence();
                foreach (var policy in CertificatePolicies) {
                    r.Add(new Asn1Sequence(policy));
                    }
                o = new Asn1OctetString(r);
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
                writer.WriteValue("CertificatePolicies", CertificatePolicies);
                }
            }
        }
    }