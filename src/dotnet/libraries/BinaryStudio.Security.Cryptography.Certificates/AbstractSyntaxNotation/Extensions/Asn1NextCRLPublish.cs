using System;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {iso(1) identified-organization(3) dod(6) internet(1) private(4) enterprise(1) 311 21 4}
     * 1.3.6.1.4.1.311.21.4
     * /ISO/Identified-Organization/6/1/4/1/311/21/4
     * szOID_CRL_NEXT_PUBLISH
     * */
    [Asn1CertificateExtension("1.3.6.1.4.1.311.21.4")]
    internal class Asn1NextCRLPublish : Asn1CertificateExtension
        {
        public DateTime Value { get; }
        public Asn1NextCRLPublish(Asn1CertificateExtension source)
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
        }
    }