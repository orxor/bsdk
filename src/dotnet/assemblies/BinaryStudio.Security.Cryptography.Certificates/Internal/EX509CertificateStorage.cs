using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.Certificates.Internal
    {
    internal class EX509CertificateStorage : X509Object
        {
        private IntPtr Store;
        public override IntPtr Handle { get { return Store; }}

        public EX509CertificateStorage(IntPtr store)
            {
            Store = store;
            }

        public IEnumerable<X509Certificate> Certificates { get {
            var o = CertEnumCertificatesInStore(Store, IntPtr.Zero);
            while (o != IntPtr.Zero) {
                yield return new X509Certificate(o);
                o = CertEnumCertificatesInStore(Store, o);
                }
            }}

        [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCertificatesInStore(IntPtr CertStore, IntPtr PrevCertContext);
        [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr CertEnumCRLsInStore(IntPtr CertStore, IntPtr PrevCrlContext);
        [DllImport("crypt32.dll", SetLastError = true)] internal static extern Boolean CertCloseStore(IntPtr handle, UInt32 flags);

        private const UInt32 CERT_CLOSE_STORE_FORCE_FLAG = 0x00000001;
        private const UInt32 CERT_CLOSE_STORE_CHECK_FLAG = 0x00000002;

        #region M:Dispose(Boolean)
        /// <summary>Releases the unmanaged resources used by the instance and optionally releases the managed resources.</summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposing) {
            lock(this) {
                if (!Disposed) {
                    base.Dispose(disposing);
                    Disposed = true;
                    if (Store != IntPtr.Zero) {
                        try
                            {
                            Validate(CertCloseStore(Store, CERT_CLOSE_STORE_CHECK_FLAG));
                            }
                        catch (Exception e)
                            {
                            Debug.Print($"Handled Exception:\n{Exceptions.ToString(e)}");
                            }
                        finally
                            {
                            Store = IntPtr.Zero;
                            }
                        }
                    }
                }
            }
        #endregion
        #region M:WriteTo(IJsonWriter)
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            var certificates = Certificates.ToArray();
            using (writer.ScopeObject()) {
                writer.WritePropertyName(nameof(Certificates));
                using (writer.ScopeObject()) {
                    writer.WriteValue("Count", certificates.Length);
                    writer.WriteValue("{Self}", certificates);
                    }
                }
            }
        #endregion
        }
    }