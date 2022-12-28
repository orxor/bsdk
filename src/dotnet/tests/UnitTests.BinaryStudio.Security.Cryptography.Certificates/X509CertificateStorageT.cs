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
        public void Certificates()
            {
            }

        #region M:GetSystemStores
        private void GetSystemStores(X509StoreLocation location) {
            using (IJsonWriter writer = new DefaultJsonWriter(new JsonTextWriter(Console.Out){
                Formatting = Formatting.Indented,
                Indentation = 2,
                IndentChar = ' '
                }))
                {
                using (writer.Constructor("GetSystemStores")) {
                    using (writer.Object()) {
                        writer.WritePropertyName("Output");
                        using (writer.Array()) {
                            foreach (var i in X509CertificateStorage.GetSystemStores(location)) {
                                writer.WriteValue(i);
                                }
                            }
                        }
                    }
                }
            ;
            }
        #endregion
        #region M:GetSystemStoresLocalMachine
        [TestMethod]
        public void GetSystemStoresLocalMachine() {
            GetSystemStores(X509StoreLocation.LocalMachine);
            }
        #endregion
        #region M:CERTIFICATES_LOCALMACHINE_MY
        [TestMethod]
        [Ignore]
        public void CERTIFICATES_LOCALMACHINE_MY() {
            using (var store = new X509CertificateStorage(X509StoreName.My,X509StoreLocation.LocalMachine)) {
                foreach (var certificate in store.Certificates) {
                    Assert.AreNotEqual(IntPtr.Zero, certificate.Handle);
                    certificate.Dispose();
                    }
                }
            }
        #endregion
        #region M:CERTIFICATES_CURRENTUSER_MY
        [TestMethod]
        [Ignore]
        public void CERTIFICATES_CURRENTUSER_MY() {
            using (var store = new X509CertificateStorage(X509StoreName.My,X509StoreLocation.CurrentUser)) {
                foreach (var certificate in store.Certificates) {
                    Assert.AreNotEqual(IntPtr.Zero, certificate.Handle);
                    }
                }
            }
        #endregion

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
