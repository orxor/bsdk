using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.Certificates.AbstractSyntaxNotation
    {
    public class Asn1CertificateRevocationList : Asn1LinkObject
        {
        public Asn1CertificateRevocationList(Asn1Object o) :
            base(o)
            {
            State |= ObjectState.Failed;
            }
        }
    }