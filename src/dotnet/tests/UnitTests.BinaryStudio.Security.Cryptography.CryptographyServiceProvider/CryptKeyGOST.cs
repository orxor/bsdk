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
        public unsafe void GenKey() {
            var a = new X509Certificate(File.ReadAllBytes(@"C:\TFS\icao\repo\ru\CN=Document Signer,OU=СЗД,O=НТЦ Атлас,L=Moscow,C=RU,E=camail@stcnet.ru,SKI=[111F],AKI=[FA40],SN=[204ED309000000000049].cer"));
            var b = (CERT_CONTEXT*)a.Handle;
            var c = b->CertInfo->rgExtension;
            var container = $@"\\.\REGISTRY\{Guid.NewGuid().ToString("D").ToLowerInvariant()}";
            using (var contextS = CryptographicContext.AcquireContext(
                    CRYPT_PROVIDER_TYPE.PROV_GOST_2001_DH, container,
                    CryptographicContextFlags.CRYPT_NEWKEYSET)) {
                var contextT = new CryptoProCSPCryptographicContext(contextS);
                var rngs = contextT.RNGSources.ToArray();
                contextS.SecureCode = CryptographicContext.GetSecureString("SomePassword");
                using (var key = CryptKey.GenKey(contextS, ALG_ID.AT_SIGNATURE, CryptGenKeyFlags.CRYPT_EXPORTABLE)) {
                    var AlgId = key.AlgId;
                    var dt = DateTime.Now;
                    var certificate = contextT.CreateSelfSignCertificate("CN=R-CA, C=ru",dt.AddYears(-1),dt.AddYears(1),
                        new Asn1CertificateExtension[]
                            {
                            new CertificateSubjectKeyIdentifier(false,"111e03d866f14235829b5148ba5ff91774fa9e1f")
                            });
                    File.WriteAllBytes("cert.cer",certificate.Bytes);
                    }
                return;
                }
            }
        }
    }
