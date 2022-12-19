using System;
using System.Linq;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /**
     * {iso(1) identified-organization(3) dod(6) internet(1) private(4) enterprise(1) 311 21 2}
     * {1.3.6.1.4.1.311.21.2}
     * {/ISO/Identified-Organization/6/1/4/1/311/21/2}
     * szOID_CERTSRV_PREVIOUS_CERT_HASH
     */
    [Asn1CertificateExtension("1.3.6.1.4.1.311.21.2")]
    internal class Asn1PreviousCACertificateHash : Asn1CertificateExtension
        {
        public String HashValue { get; }
        public Asn1PreviousCACertificateHash(Asn1CertificateExtension source)
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
        }
    }