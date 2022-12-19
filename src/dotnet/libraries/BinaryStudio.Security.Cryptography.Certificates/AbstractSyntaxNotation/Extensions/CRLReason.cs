using System;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("2.5.29.21")]
    public sealed class CRLReason : Asn1CertificateExtension
        {
        public X509CrlReason ReasonCode { get; }
        public CRLReason(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    var content = octet[0].Content.ToArray();
                    ReasonCode = (X509CrlReason)(Int32)content[0];
                    }
                }
            }
        }
    }