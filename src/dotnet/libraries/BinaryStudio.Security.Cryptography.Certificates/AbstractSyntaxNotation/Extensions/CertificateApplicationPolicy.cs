using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.Certificates.AbstractSyntaxNotation.Extensions
    {
    [UsedImplicitly]
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_APPLICATION_CERT_POLICIES)]
    internal class CertificateApplicationPolicy : CertificatePoliciesExtension
        {
        #region ctor{CertificateExtension}
        internal CertificateApplicationPolicy(CertificateExtension source)
            : base(source)
            {
            }
        #endregion
        }
    }
