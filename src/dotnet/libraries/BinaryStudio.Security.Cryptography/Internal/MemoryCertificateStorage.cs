using System;

namespace BinaryStudio.Security.Cryptography.Certificates.Internal
    {
    internal class MemoryCertificateStorage : EX509CertificateStorage
        {
        public override X509StoreLocation Location { get; }
        public MemoryCertificateStorage(X509StoreLocation location)
            :base(Entries.CertOpenStoreA(
                    CERT_STORE_PROV_MEMORY,
                    PKCS_7_ASN_ENCODING|X509_ASN_ENCODING,
                    IntPtr.Zero,
                    MapX509StoreFlags(location,X509OpenFlags.MaxAllowed|X509OpenFlags.ReadWrite),
                    IntPtr.Zero),"Memory")
            {
            Location = location;
            }

        private static readonly IntPtr CERT_STORE_PROV_MEMORY = new IntPtr(2);
        }
    }
