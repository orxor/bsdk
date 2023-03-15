using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [UsedImplicitly]
    [Asn1CertificateExtension("1.2.840.113556.1.5.284.1")]
    internal class NTDSDSAInvocationId : DRSObjectGuid
        {
        #region ctor{CertificateExtension}
        internal NTDSDSAInvocationId(CertificateExtension source)
            : base(source)
            {
            }
        #endregion
        }
    }
