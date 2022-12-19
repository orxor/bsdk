﻿using System.Linq;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) certificatePolicies(32)}
     * 2.5.29.32
     * /Joint-ISO-ITU-T/5/29/32
     * Certificate policies
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension("2.5.29.32")]
    internal class Asn1CertificatePoliciesExtension : Asn1CertificateExtension
        {
        public Asn1ObjectIdentifierCollection CertificatePolicies { get; }
        public Asn1CertificatePoliciesExtension(Asn1CertificateExtension source)
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
        }
    }