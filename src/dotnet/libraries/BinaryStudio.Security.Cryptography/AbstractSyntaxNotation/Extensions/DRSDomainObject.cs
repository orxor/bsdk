namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("1.2.840.113556.1.5.284.4")]
    internal class DRSDomainObject : DRSObjectGuid
        {
        public DRSDomainObject(CertificateExtension source)
            : base(source)
            {
            }
        }
    }
