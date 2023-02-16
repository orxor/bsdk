using System;
using System.Collections.Generic;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Internal;

namespace BinaryStudio.Security.Cryptography.Certificates.Internal
    {
    internal class DeviceCertificateStorage : MemoryCertificateStorage
        {
        private Boolean FetchedFromContext;
        public override String StoreName { get { return "Device"; }}
        public CRYPT_PROVIDER_TYPE ProviderType { get; }

        public DeviceCertificateStorage(CRYPT_PROVIDER_TYPE provider, X509StoreLocation location)
            : base(location)
            {
            ProviderType = provider;
            if (provider == CRYPT_PROVIDER_TYPE.AUTO) {
                ProviderType = CRYPT_PROVIDER_TYPE.PROV_GOST_2012_512;
                }
            }

        public override IEnumerable<X509Certificate> Certificates { get {
            if (FetchedFromContext) {
                var o = Entries.CertEnumCertificatesInStore(Handle, IntPtr.Zero);
                while (o != IntPtr.Zero) {
                    yield return new X509Certificate(o) {
                        IsMachineKeySet = (Location == X509StoreLocation.LocalMachine)
                        };
                    o = Entries.CertEnumCertificatesInStore(Handle, o);
                    }
                }
            else
                {
                throw new NotImplementedException();
                //using (var context = new ECryptographicContext(ProviderType,
                //    CryptographicContextFlags.CRYPT_VERIFYCONTEXT|
                //    ((Location == X509StoreLocation.CurrentUser)
                //        ? CryptographicContextFlags.CRYPT_NONE
                //        : CryptographicContextFlags.CRYPT_MACHINE_KEYSET)))
                //    {
                //    foreach (var key in context.Keys) {
                //        var r = key.GetParameter(KEY_PARAM.KP_CERTIFICATE);
                //        if (r != null) {
                //            var o = new X509Certificate(r, key);
                //            Validate(Entries.CertAddCertificateContextToStore(Handle, o.Handle, CERT_STORE_ADD.CERT_STORE_ADD_ALWAYS, IntPtr.Zero));
                //            yield return o;
                //            }
                //        }
                //    }
                FetchedFromContext = true;
                }
            }}
        }
    }
