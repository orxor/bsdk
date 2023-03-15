using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
{
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) freshestCRL(46)}
     * 2.5.29.46
     * /Joint-ISO-ITU-T/5/29/46
     * "id-ce-freshestCRL"
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_FRESHEST_CRL)]
    internal class FreshestCRL : CRLDistributionPoints
        {
        public FreshestCRL(CertificateExtension u)
            :base(u)
            {
            }
        }
    }