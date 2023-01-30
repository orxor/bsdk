using System;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Certificates.Internal;

namespace BinaryStudio.Security.Cryptography.Internal
    {
    internal class MessageCertificateStorage : EX509CertificateStorage
        {
        public override X509StoreLocation Location { get { return X509StoreLocation.CurrentUser; }}
        public MessageCertificateStorage(IntPtr message)
            :base(Entries.CertOpenStore(
                    CERT_STORE_PROV_MSG,
                    PKCS_7_ASN_ENCODING|X509_ASN_ENCODING,
                    IntPtr.Zero,
                    MapX509StoreFlags(X509StoreLocation.CurrentUser,X509OpenFlags.MaxAllowed|X509OpenFlags.OpenExistingOnly),
                    message))
            {
            }

        private static readonly IntPtr CERT_STORE_PROV_MSG = new IntPtr(1);
        }
    }