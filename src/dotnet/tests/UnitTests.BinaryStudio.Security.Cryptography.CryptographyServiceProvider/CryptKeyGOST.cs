using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.BinaryStudio.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryStudio.Security.Cryptography;
using BinaryStudio.PlatformComponents.Win32;

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
            using (var context = CryptographicContext.AcquireContext(
                    CRYPT_PROVIDER_TYPE.PROV_GOST_2012_512, container,
                    CryptographicContextFlags.CRYPT_NEWKEYSET|CryptographicContextFlags.CRYPT_SILENT)) {
                var K = CryptKey.GenKey(context,)
                var A = CryptKey.GetUserKey(context,KEY_SPEC_TYPE.AT_SIGNATURE);
                var B = CryptKey.GetUserKey(context,KEY_SPEC_TYPE.AT_KEYEXCHANGE);
                return;
                }
            }
        }
    }
