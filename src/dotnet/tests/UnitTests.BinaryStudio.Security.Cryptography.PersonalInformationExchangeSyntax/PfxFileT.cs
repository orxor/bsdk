using System;
using System.IO;
using BinaryStudio.Security.Cryptography.PersonalInformationExchangeSyntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.BinaryStudio.Security.Cryptography.PersonalInformationExchangeSyntax
    {
    [TestClass]
    public class PfxFileT
        {
        [TestInitialize]
        public void Setup()
            {
            }

        [TestMethod]
        public void PfxFileToPEM() {
            var Source = new PfxFile(File.ReadAllBytes(@"C:\TFS\bsdk\src\dotnet\tests\UnitTestData\pfx\cryptopro00.pfx"));
            }
        }
    }
