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

        [TestMethod]
        [Ignore]
        public void MakeCert() {
            var container = $@"\\.\REGISTRY\{Guid.NewGuid().ToString("D").ToLowerInvariant()}";
            using (var contextS = CryptographicContext.AcquireContext(
                    CRYPT_PROVIDER_TYPE.PROV_GOST_2001_DH, container,
                    CryptographicContextFlags.CRYPT_NEWKEYSET)) {
                contextS.SecureCode = CryptographicContext.GetSecureString("SomePassword");
                var contextT = new CryptoProCSPCryptographicContext(contextS);
                using (var key = CryptKey.GenKey(contextS, ALG_ID.AT_SIGNATURE, CryptGenKeyFlags.CRYPT_EXPORTABLE)) {
                    var dt = DateTime.Now;
                    var certificate = contextT.CreateSelfSignCertificate("CN=R-CA, C=ru",dt.AddYears(-1),dt.AddYears(1),
                        new Asn1CertificateExtension[]
                            {
                            new CertificateSubjectKeyIdentifier(false,"111e03d866f14235829b5148ba5ff91774fa9e1f")
                            });
                    File.WriteAllBytes("cert.cer",certificate.Bytes);
                    }
                }
            }

        [TestMethod]
        public void MakeCertSN() {
            var dt = DateTime.Now;
            CryptographicContext.MakeCert(ALG_ID.CALG_GR3410EL,"CN=R-CA, C=ru","010203",
                dt.AddYears(-1),dt.AddYears(1),
                new Asn1CertificateExtension[]
                    {
                    new CertificateSubjectKeyIdentifier(false,"111e03d866f14235829b5148ba5ff91774fa9e1f")
                    },
                Stream.Null, CryptographicContext.GetSecureString("SomePassword"),
                out var Certificate,true);
            /*var container = $@"\\.\REGISTRY\{Guid.NewGuid().ToString("D").ToLowerInvariant()}";
            using (var contextS = CryptographicContext.AcquireContext(
                    CRYPT_PROVIDER_TYPE.PROV_GOST_2001_DH, container,
                    CryptographicContextFlags.CRYPT_NEWKEYSET)) {
                contextS.SecureCode = CryptographicContext.GetSecureString("SomePassword");
                var contextT = new CryptoProCSPCryptographicContext(contextS);
                using (var key = CryptKey.GenKey(contextS, ALG_ID.AT_SIGNATURE, CryptGenKeyFlags.CRYPT_EXPORTABLE)) {
                    var dt = DateTime.Now;
                    var certificate = contextT.CreateSelfSignCertificate("CN=R-CA, C=ru","010203", dt.AddYears(-1),dt.AddYears(1),
                        new Asn1CertificateExtension[]
                            {
                            new CertificateSubjectKeyIdentifier(false,"111e03d866f14235829b5148ba5ff91774fa9e1f")
                            });
                    key.Certificate = certificate;
                    contextT.ExportCertificate(certificate, "cert.pfx",CryptographicContext.GetSecureString("SomePassword"));
                    contextT.ExportCertificate(certificate, "cert.cer",null);
                    }
                return;
                }
            */
            }
        }
    }
