using System;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("2.5.29.14")]
    public sealed class CertificateSubjectKeyIdentifier : Asn1CertificateExtension
        {
        public Byte[] KeyIdentifier { get; }
        public CertificateSubjectKeyIdentifier(Asn1CertificateExtension source)
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

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return KeyIdentifier.ToString("x");
            }
        }
    }