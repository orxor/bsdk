using System;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("2.5.29.24")]
    public sealed class InvalidityDate : Asn1CertificateExtension
        {
        public DateTime Value { get; }
        public InvalidityDate(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    Value = (Asn1Time)octet[0];
                    }
                }
            }
        }
    }
