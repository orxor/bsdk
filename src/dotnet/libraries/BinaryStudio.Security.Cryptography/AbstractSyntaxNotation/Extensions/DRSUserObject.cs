namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("1.2.840.113556.1.5.284.3")]
    internal class DRSUserObject : DRSObjectGuid
        {
        public DRSUserObject(Asn1CertificateExtension source)
            : base(source)
            {
            }
        }
    }
