using System;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    internal class Asn1CertificateAuthorityInformationAccess : Asn1LinkObject
        {
        public Asn1ObjectIdentifier AccessMethod { get; }
        public IX509GeneralName AccessLocation { get; }

        private SByte ContextSpecificObjectType { get; }
        public Asn1CertificateAuthorityInformationAccess(Asn1Object source)
            : base(source)
            {
            var sequence = (Asn1Sequence)source;
            AccessMethod = (Asn1ObjectIdentifier)sequence[0];
            AccessLocation = X509GeneralName.From((Asn1ContextSpecificObject)sequence[1]);
            }
        }
    }