using System;
using System.Runtime.InteropServices;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Certificates.Internal;

namespace BinaryStudio.Security.Cryptography.Internal
    {
    internal class MessageCertificateStorage : EX509CertificateStorage
        {
        public override X509StoreLocation Location { get { return X509StoreLocation.CurrentUser; }}
        public MessageCertificateStorage(IntPtr message)
            :base(IntPtr.Zero, "Message")
            {
            throw new NotImplementedException();
            }

        #region M:Find(CERT_INFO*):X509Certificate
        public override unsafe X509Certificate Find(CERT_INFO* Info) {
            if (Info == null) { throw new ArgumentNullException(nameof(Info)); }
            var r = Entries.CertGetSubjectCertificateFromStore(Store,PKCS_7_ASN_ENCODING|X509_ASN_ENCODING,Info);
            if (r == IntPtr.Zero) {
                var e = (HRESULT)Marshal.GetHRForLastWin32Error();
                var CertificateSerialNumber = DecodeSerialNumberString(ref Info->SerialNumber);
                var CertificateIssuer = DecodeNameString(Entries,ref Info->Issuer);
                foreach (var certificate in Certificates) {
                    if (CertificateSerialNumber == certificate.SerialNumber) {
                        return certificate;
                        }
                    }
                throw HResultException.GetExceptionForHR(e)
                    .Add("CertificateSerialNumber",CertificateSerialNumber)
                    .Add("CertificateIssuer",CertificateIssuer);
                }
            return new X509Certificate(r);
            }
        #endregion

        private static readonly IntPtr CERT_STORE_PROV_MSG = new IntPtr(1);
        }
    }