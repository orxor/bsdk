using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryStudio.IO;
using BinaryStudio.Security.Cryptography;
using System.IO;

namespace UnitTests.BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    [TestClass]
    public class CryptographicContextT
        {
        [TestMethod]
        public void DecryptFT()
            {
            (new CryptographicContext()).DecryptMessage(
                new ReadOnlyFileMappingStream(@"C:\DEV\ARM27_20221221095536.dat"),
                new MemoryStream(), out var certificate);
            }
        }
    }