using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;

namespace BinaryStudio.Security.Cryptography.Certificates.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_APPLICATION_CERT_POLICIES)]
    internal class CertificateApplicationPolicy : CertificatePoliciesExtension
        {
        public CertificateApplicationPolicy(Asn1CertificateExtension source)
            : base(source)
            {
            }
        }
    }
