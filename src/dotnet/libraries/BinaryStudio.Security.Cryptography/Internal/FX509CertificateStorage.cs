using System;
using System.Collections.Generic;
using System.IO;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates.Internal
    {
    internal class FX509CertificateStorage : MemoryCertificateStorage
        {
        private String Folder;
        private Boolean FetchedFromFolder;

        public override IEnumerable<X509Certificate> Certificates { get {
            if (FetchedFromFolder) {
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
                            Validate(Entries.CertAddCertificateContextToStore(Handle, o.Handle, CERT_STORE_ADD.CERT_STORE_ADD_ALWAYS, IntPtr.Zero));
                            yield return o;
                            }
                            break;
                        }
                    }
                }
            finally
                {
                FetchedFromFolder = true;
                }
            }}

        public FX509CertificateStorage(String folder)
            :this(folder,X509StoreLocation.CurrentUser)
            {
            }

        public FX509CertificateStorage(String folder, X509StoreLocation location)
            :base(location)
            {
            if (folder == null) { throw new ArgumentNullException(nameof(folder)); }
            if (!Directory.Exists(folder)) { throw new ArgumentOutOfRangeException(nameof(folder)); }
            Folder = folder;
            }
        }
    }
