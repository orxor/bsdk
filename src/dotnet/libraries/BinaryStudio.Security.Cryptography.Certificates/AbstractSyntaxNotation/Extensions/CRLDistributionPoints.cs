using System.Collections.Generic;
using System.Linq;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /**
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) cRLDistributionPoints(31)}
     * 2.5.29.31
     * /Joint-ISO-ITU-T/5/29/31
     * Certificate Revocation List (CRL) distribution points
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension("2.5.29.31")]
    public class CRLDistributionPoints : Asn1CertificateExtension
        {
        public IList<DistributionPoint> DistributionPoints { get; }
        public CRLDistributionPoints(Asn1CertificateExtension u)
            :base(u)
            {
            DistributionPoints = new DistributionPoint[0];
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count == 1) {
                    if (octet[0] is Asn1Sequence sequence) {
                        DistributionPoints = sequence.
                            OfType<Asn1Sequence>().
                            Select(i => new DistributionPoint(i)).
                            ToArray();
                        }
                    }
                }
            }
        }
    }