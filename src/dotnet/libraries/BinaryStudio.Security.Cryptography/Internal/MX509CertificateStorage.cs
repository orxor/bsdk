using System;

namespace BinaryStudio.Security.Cryptography.Certificates.Internal
    {
    internal class MX509CertificateStorage : EX509CertificateStorage
        {
        public MX509CertificateStorage(X509StoreLocation location)
            :base(Entries.CertOpenStore(
                    CERT_STORE_PROV_MEMORY,
                    PKCS_7_ASN_ENCODING|X509_ASN_ENCODING,
                    IntPtr.Zero,
                    MapX509StoreFlags(location,X509OpenFlags.MaxAllowed|X509OpenFlags.ReadWrite),
                    IntPtr.Zero))
            {
            }

        private static readonly IntPtr CERT_STORE_PROV_MEMORY = new IntPtr(2);
        }
    }
