using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
{
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) subjectAltName(17)}
     * 2.5.29.17
     * /Joint-ISO-ITU-T/5/29/17
     * Subject alternative name ("subjectAltName" extension)
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.NSS_OID_X509_SUBJECT_ALT_NAME)]
    public class SubjectAlternativeName : IssuerAlternativeName
        {
        public SubjectAlternativeName(Asn1CertificateExtension source)
            : base(source)
            {
            }
        }
    }