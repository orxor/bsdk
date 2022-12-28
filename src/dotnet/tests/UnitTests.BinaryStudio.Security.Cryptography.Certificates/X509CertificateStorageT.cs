﻿using System;
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
            using (var store = new X509CertificateStorage(new Uri("folder://./UnitTestData/cer"))) {
                foreach (var certificate in store.Certificates) {
                    //Console.WriteLine($"{(Int32)certificate.Handle:x8}:{certificate.Thumbprint}");
                    Assert.AreNotEqual(IntPtr.Zero, certificate.Handle);
                    }
                }
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
                Console.Out.WriteLine();
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
        #region M:CertificatesLocalMachineMy
        [TestMethod]
        public void CertificatesLocalMachineMy() {
            using (var store = new X509CertificateStorage(X509StoreName.My,X509StoreLocation.LocalMachine)) {
                foreach (var certificate in store.Certificates) {
                    Assert.AreNotEqual(IntPtr.Zero, certificate.Handle);
                    certificate.Dispose();
                    }
                }
            }
        #endregion
        #region M:CertificatesCurrentUserMy
        [TestMethod]
        public void CertificatesCurrentUserMy() {
            using (var store = new X509CertificateStorage(X509StoreName.My,X509StoreLocation.CurrentUser)) {
                foreach (var certificate in store.Certificates) {
                    Assert.AreNotEqual(IntPtr.Zero, certificate.Handle);
                    }
                }
            }
        #endregion

        [TestMethod]
        [Ignore]
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
