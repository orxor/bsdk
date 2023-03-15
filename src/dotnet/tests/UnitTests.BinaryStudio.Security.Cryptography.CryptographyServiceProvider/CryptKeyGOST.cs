using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using UnitTests.BinaryStudio.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryStudio.Security.Cryptography;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Specific.CryptoProCSP;

namespace UnitTests.BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    [TestClass]
    public class CryptKeyGOST : ClassT
        {
        [TestInitialize]
        public void Initialize()
            {
            }

        [TestCleanup]
        public void Cleanup()
            {
            }

        #region M:MakeSelfSignedCert({out}X509Certificate,SecureString,Boolean)
        private static void MakeSelfSignedCert(out X509Certificate Certificate,SecureString SecureCode, Boolean DeletePrivateKey) {
            var dt = DateTime.Now;
            CryptographicContext.MakeCertificate(ALG_ID.CALG_GR3410EL,"CN=R-CA, C=ru","010203",
                dt.AddYears(-1),dt.AddYears(1),
                new CertificateExtension[] {
                    new CertificateSubjectKeyIdentifier(false,"89abcdeffedcba98765432100123456789abcdef")
                    },
                Stream.Null, SecureCode,
                out Certificate,DeletePrivateKey);
            }
        #endregion

        #region T:MakeSelfSignedCert
        [TestMethod]
        [Ignore]
        public void MakeSelfSignedCert() {
            MakeSelfSignedCert(out var Certificate,
                CryptographicContext.GetSecureString("SomePassword"),
                true);
            }
        #endregion
        #region T:MakeCertSN
        [TestMethod]
        public void MakeCertSN() {
            var SecureCode = CryptographicContext.GetSecureString("SomePassword");
            MakeSelfSignedCert(out var IssuerCertificate,SecureCode,false);
            File.WriteAllBytes("i1.cer",IssuerCertificate.Bytes);
            try
                {
                var dt = DateTime.Now;
                CryptographicContext.MakeCertificate(ALG_ID.CALG_GR3410EL, "CN=I-CA, C=ru", "040506",
                    dt.AddYears(-1), dt.AddYears(1), null,
                    Stream.Null, SecureCode,
                    out var SubjectCertificate,
                    IssuerCertificate, true);
                }
            finally
                {
                CryptographicContext.DeleteContainer(CryptographicContext.ProviderTypeFromAlgId(ALG_ID.CALG_GR3410EL),IssuerCertificate.Container);
                }
            
            }
        #endregion
        }
    }
