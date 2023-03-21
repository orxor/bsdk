using System;
using System.Collections.Generic;
using System.IO;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates.Internal
    {
    internal class FolderCertificateStorage : MemoryCertificateStorage
        {
        private readonly String Folder;
        private Boolean FetchedFromFolderCER;
        private Boolean FetchedFromFolderCRL;

        #region P:Certificates:IEnumerable<X509Certificate>
        public override IEnumerable<X509Certificate> Certificates { get {
            if (FetchedFromFolderCER) {
                var o = Entries.CertEnumCertificatesInStore(Handle, IntPtr.Zero);
                while (o != IntPtr.Zero) {
                    yield return new X509Certificate(o);
                    o = Entries.CertEnumCertificatesInStore(Handle, o);
                    }
                }
            else try
                {
                foreach (var filename in Directory.GetFiles(Folder)) {
                    var e = Path.GetExtension(filename)?.ToLowerInvariant()??String.Empty;
                    switch (e) {
                        case ".cer":
                            {
                            var o = new X509Certificate(File.ReadAllBytes(filename));
                            Validate(Entries.CertAddCertificateContextToStore(Handle, o.Handle, CERT_STORE_ADD.CERT_STORE_ADD_ALWAYS));
                            yield return o;
                            }
                            break;
                        }
                    }
                }
            finally
                {
                FetchedFromFolderCER = true;
                }
            }}
        #endregion
        #region P:CertificateRevocationLists:IEnumerable<X509CertificateRevocationList>
        public override IEnumerable<X509CertificateRevocationList> CertificateRevocationLists { get {
            if (FetchedFromFolderCRL) {
                var o = Entries.CertEnumCRLsInStore(Handle, IntPtr.Zero);
                while (o != IntPtr.Zero) {
                    yield return new X509CertificateRevocationList(o);
                    o = Entries.CertEnumCRLsInStore(Handle, o);
                    }
                }
            else try
                {
                foreach (var filename in Directory.GetFiles(Folder)) {
                    var e = Path.GetExtension(filename)?.ToLowerInvariant()??String.Empty;
                    switch (e) {
                        case ".crl":
                            {
                            var o = new X509CertificateRevocationList(File.ReadAllBytes(filename));
                            Validate(Entries.CertAddCRLContextToStore(Handle, o.Handle, CERT_STORE_ADD.CERT_STORE_ADD_ALWAYS,IntPtr.Zero));
                            yield return o;
                            }
                            break;
                        }
                    }
                }
            finally
                {
                FetchedFromFolderCRL = true;
                }
            }}
        #endregion

        public FolderCertificateStorage(String folder)
            :this(folder,X509StoreLocation.CurrentUser)
            {
            }

        public FolderCertificateStorage(String folder, X509StoreLocation location)
            :base(location)
            {
            if (folder == null) { throw new ArgumentNullException(nameof(folder)); }
            if (!Directory.Exists(folder)) { throw new ArgumentOutOfRangeException(nameof(folder)); }
            Folder = folder;
            }
        }
    }
