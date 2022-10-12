using System;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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

        [TestMethod]
        public void JsonS() {
            using (var store = new X509CertificateStorage(X509StoreName.My,X509StoreLocation.LocalMachine)) {
                using (var writer = new DefaultJsonWriter(new JsonTextWriter(Console.Out){
                    Formatting = Formatting.Indented,
                    Indentation = 2,
                    IndentChar = ' '
                    }))
                    {
                    store.WriteTo(writer);
                    }
                }
            }
        }
    }
