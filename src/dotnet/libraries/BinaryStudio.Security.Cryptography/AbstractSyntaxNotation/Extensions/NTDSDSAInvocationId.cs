﻿namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [Asn1CertificateExtension("1.2.840.113556.1.5.284.1")]
    internal class NTDSDSAInvocationId : DRSObjectGuid
        {
        public NTDSDSAInvocationId(CertificateExtension source)
            : base(source)
            {
            }
        }
    }
