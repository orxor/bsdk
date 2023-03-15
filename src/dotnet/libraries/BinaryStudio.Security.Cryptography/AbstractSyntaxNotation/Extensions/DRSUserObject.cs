using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [UsedImplicitly]
    [Asn1CertificateExtension("1.2.840.113556.1.5.284.3")]
    internal class DRSUserObject : DRSObjectGuid
        {
        #region ctor{CertificateExtension}
        internal DRSUserObject(CertificateExtension source)
            : base(source)
            {
            }
        #endregion
        }
    }
