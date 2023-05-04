using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [UsedImplicitly]
    [Asn1CertificateExtension("1.2.840.113556.1.5.284.4")]
    internal class DRSDomainObject : DRSObjectGuid
        {
        #region ctor{CertificateExtension}
        internal DRSDomainObject(CertificateExtension source)
            : base(source)
            {
            }
        #endregion
        }
    }
