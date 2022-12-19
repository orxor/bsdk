using System;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("1.3.6.1.4.1.311.20.2")]
    internal sealed class Asn1CertificateTemplateNameExtension : Asn1CertificateExtension
        {
        public String TemplateName { get; }
        public Asn1CertificateTemplateNameExtension(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    TemplateName = ((Asn1String)octet[0]).Value;
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
            return (TemplateName != null)
                ? TemplateName
                : "(none)";
            }
        }
    }