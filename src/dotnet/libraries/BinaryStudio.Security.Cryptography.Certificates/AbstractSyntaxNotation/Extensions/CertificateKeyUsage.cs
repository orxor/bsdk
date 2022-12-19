using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) keyUsage(15)}
     * 2.5.29.15
     * /Joint-ISO-ITU-T/5/29/15
     * Key usage
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension("2.5.29.15")]
    internal sealed class CertificateKeyUsage : Asn1CertificateExtension
        {
        public X509KeyUsageFlags KeyUsage { get; }
        public CertificateKeyUsage(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    var bitstring = octet[0] as Asn1BitString;
                    if (!ReferenceEquals(bitstring, null)) {
                        if (bitstring.Content.Length > 0) {
                            bitstring.Content.Seek(0, SeekOrigin.Begin);
                            var i = bitstring.Content.ReadByte();
                            KeyUsage = (X509KeyUsageFlags)i;
                            }
                        }
                    }
                }
            }
        }
    }