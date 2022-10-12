using System;
using BinaryStudio.Security.Cryptography.Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.BinaryStudio.Security.Cryptography.Certificates
    {
    [TestClass]
    public class X509CertificateStorageT
        {
        [TestInitialize]
        public void Setup()
            {
            }

        [TestMethod]
        public void Certificates() {
            using (var store = new X509CertificateStorage(X509StoreName.My,X509StoreLocation.LocalMachine)) {
                foreach (var certificate in store.Certificates) {
                    Assert.AreNotEqual(IntPtr.Zero, certificate.Handle);
                    certificate.Dispose();
                    }
                }
            }
        }
    }
