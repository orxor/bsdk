using System.Numerics;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("2.5.29.27")]
    internal class Asn1DeltaCRLIndicator : Asn1CertificateExtension
        {
        public BigInteger MinimumBaseCRLNumber { get; }
        public Asn1DeltaCRLIndicator(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    MinimumBaseCRLNumber = (Asn1Integer)octet[0];
                    }
                }
            }
        }
    }