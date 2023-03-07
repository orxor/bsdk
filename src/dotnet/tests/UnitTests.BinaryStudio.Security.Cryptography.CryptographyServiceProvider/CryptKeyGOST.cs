using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using UnitTests.BinaryStudio.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryStudio.Security.Cryptography;
using BinaryStudio.PlatformComponents.Win32;
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
        public void GenKey() {
            var container = $@"\\.\REGISTRY\{Guid.NewGuid().ToString("D").ToLowerInvariant()}";
            using (var contextS = CryptographicContext.AcquireContext(
                    CRYPT_PROVIDER_TYPE.PROV_GOST_2012_256, container,
                    CryptographicContextFlags.CRYPT_NEWKEYSET|CryptographicContextFlags.CRYPT_SILENT)) {
                var contextT = new CryptoProCSPCryptographicContext(contextS);
                var rngs = contextT.RNGSources.ToArray();
                contextS.SecureCode = CryptographicContext.GetSecureString("SomePassword");
                using (var key = CryptKey.GenKey(contextS, ALG_ID.AT_SIGNATURE, CryptGenKeyFlags.CRYPT_EXPORTABLE)) {
                    var AlgId = key.AlgId;
                    var certificate = contextT.CreateSelfSignCertificate("CN=ROOT, C=us");
                    }
                return;
                }
            }
        }
    }
