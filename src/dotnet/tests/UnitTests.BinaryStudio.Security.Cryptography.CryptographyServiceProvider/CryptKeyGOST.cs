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
using BinaryStudio.Security.Cryptography.Internal;

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
            var x = new CRYPT_OID_INFO();
            var container = $@"\\.\REGISTRY\{Guid.NewGuid().ToString("D").ToLowerInvariant()}";
            using (var context = CryptographicContext.AcquireContext(
                    CRYPT_PROVIDER_TYPE.PROV_GOST_2012_256, container,
                    CryptographicContextFlags.CRYPT_NEWKEYSET)) {
                context.SecureCode = CryptographicContext.GetSecureString("SomePassword");
                using (var key = CryptKey.GenKey(context, ALG_ID.AT_SIGNATURE, CryptGenKeyFlags.CRYPT_EXPORTABLE)) {
                    var AlgId = key.AlgId;
                    var cpro = new CPROSpecificCryptographicContext(context);
                    var certificate = cpro.CreateSelfSignCertificate("CN=ROOT, C=us");
                    }
                return;
                }
            }
        }
    }
