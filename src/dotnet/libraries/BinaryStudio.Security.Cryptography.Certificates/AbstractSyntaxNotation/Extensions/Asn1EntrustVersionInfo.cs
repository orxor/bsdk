using System;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("1.2.840.113533.7.65.0")]
    internal class Asn1EntrustVersionInfo : Asn1CertificateExtension
        {
        public String Version { get; }
        public Asn1BitString Flags { get; }
        public Asn1EntrustVersionInfo(Asn1CertificateExtension source)
            :base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    Version = (Asn1String)octet[0][0];
                    Flags = (Asn1BitString)octet[0][1];
                    }
                }
            }
        }
    }