using System;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("1.2.643.100.111")]
    internal sealed class Asn1CertificateSubjectDigitalSigningToolExtension : Asn1CertificateExtension
        {
        public String DigitalSigningTool { get; }
        public Asn1CertificateSubjectDigitalSigningToolExtension(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    DigitalSigningTool = ((Asn1String)octet[0]).Value;
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
            return DigitalSigningTool;
            }
        }
    }