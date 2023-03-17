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
    internal class EX509CertificateStorage : X509Object, IX509CertificateStorage
        {
        protected IntPtr Store;
        public override IntPtr Handle { get { return Store; }}
        public virtual String StoreName { get; }
        public virtual X509StoreLocation Location { get { return X509StoreLocation.CurrentService; }}

        #region ctor{IntPtr,String}
        public EX509CertificateStorage(IntPtr store, String storeName)
            {
            Store = store;
            StoreName = storeName;
            }
        #endregion

        public virtual IEnumerable<X509Certificate> Certificates { get {
            var o = Entries.CertEnumCertificatesInStore(Store, IntPtr.Zero);
            while (o != IntPtr.Zero) {
                yield return new X509Certificate(o);
                o = Entries.CertEnumCertificatesInStore(Store, o);
                }
            }}

        public IEnumerable<X509CertificateRevocationList> CertificateRevocationLists { get {
            var o = Entries.CertEnumCRLsInStore(Store, IntPtr.Zero);
            while (o != IntPtr.Zero) {
                yield return new X509CertificateRevocationList(o);
                o = Entries.CertEnumCRLsInStore(Store, o);
                }
            }}

        #region M:Find(CERT_INFO*):X509Certificate
        /**
         * <summary>
         * Finds a subject certificate context uniquely identified by its issuer and serial number in certificate store.
         * </summary>
         * <param name="Info">A pointer to a <see cref="CERT_INFO"/> structure. Only the <see cref="CERT_INFO.Issuer"/> and <see cref="CERT_INFO.SerialNumber"/> members are used.</param>
         * <returns>The certificate if succeeds, otherwise <see langword="null"/>.</returns>
         * <seealso cref="CryptographicFunctions.CertGetSubjectCertificateFromStore"/>
         */
        public virtual unsafe X509Certificate Find(CERT_INFO* Info) {
            if (Info == null) { throw new ArgumentNullException(nameof(Info)); }
            var r = Entries.CertGetSubjectCertificateFromStore(Store,PKCS_7_ASN_ENCODING|X509_ASN_ENCODING,Info);
            if (r == IntPtr.Zero) {
                var e = (HRESULT)Marshal.GetHRForLastWin32Error();
                var CertificateSerialNumber = DecodeSerialNumberString(ref Info->SerialNumber);
                var CertificateIssuer = DecodeNameString(Entries,ref Info->Issuer);
                throw HResultException.GetExceptionForHR(e)
                    .Add("CertificateSerialNumber",CertificateSerialNumber)
                    .Add("CertificateIssuer",CertificateIssuer);
                }
            return new X509Certificate(r);
            }
        #endregion
        #region M:Add(X509Certificate o)
        public void Add(X509Certificate o) {
            if (o == null) {throw new ArgumentNullException(nameof(o)); }
            Validate(Entries.CertAddCertificateContextToStore(Store,o.Handle,CERT_STORE_ADD.CERT_STORE_ADD_ALWAYS));
            }
        #endregion
        #region M:Add(X509CertificateRevocationList)
        public void Add(X509CertificateRevocationList o) {
            if (o == null) {throw new ArgumentNullException(nameof(o)); }
            Validate(Entries.CertAddCRLContextToStore(Store,o.Handle,CERT_STORE_ADD.CERT_STORE_ADD_ALWAYS,IntPtr.Zero));
            }
        #endregion
        #region M:Remove(X509Certificate)
        public void Remove(X509Certificate o) {
            if (o == null) {throw new ArgumentNullException(nameof(o)); }
            Validate(Entries.CertAddCertificateContextToStore(Store,o.Handle,CERT_STORE_ADD.CERT_STORE_ADD_ALWAYS,out var r));
            Validate(Entries.CertDeleteCertificateFromStore(r));
            }
        #endregion
        #region M:Remove(X509CertificateRevocationList)
        public void Remove(X509CertificateRevocationList o) {
            if (o == null) {throw new ArgumentNullException(nameof(o)); }
            Validate(Entries.CertAddCRLContextToStore(Store,o.Handle,CERT_STORE_ADD.CERT_STORE_ADD_ALWAYS,out var r));
            Validate(Entries.CertDeleteCRLFromStore(r));
            }
        #endregion

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
                            Validate(Entries.CertCloseStore(Store, CERT_CLOSE_STORE_CHECK_FLAG));
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
            using (writer.Object()) {
                writer.WritePropertyName(nameof(Certificates));
                using (writer.Object()) {
                    writer.WriteValue("Count", certificates.Length);
                    writer.WriteValue("{Self}", certificates);
                    }
                }
            }
        #endregion

        #region M:MapX509StoreFlags(X509StoreLocation,X509OpenFlags):UInt32
        protected internal static Int32 MapX509StoreFlags(X509StoreLocation storeLocation, X509OpenFlags flags) {
            var r = 0;
            var mode = ((Int32)flags) & 0x3;
            switch (mode)
                {
                case (Int32)X509OpenFlags.ReadOnly:    { r |= CERT_STORE_READONLY_FLAG; }          break;
                case (Int32)X509OpenFlags.MaxAllowed:  { r |= CERT_STORE_MAXIMUM_ALLOWED_FLAG; }   break;
                }
            if ((flags & X509OpenFlags.OpenExistingOnly) == X509OpenFlags.OpenExistingOnly) { r |= CERT_STORE_OPEN_EXISTING_FLAG; }
            if ((flags & X509OpenFlags.IncludeArchived)  == X509OpenFlags.IncludeArchived)  { r |= CERT_STORE_ENUM_ARCHIVED_FLAG; }
                 if (storeLocation == X509StoreLocation.LocalMachine) { r |= CERT_SYSTEM_STORE_LOCAL_MACHINE; }
            else if (storeLocation == X509StoreLocation.CurrentUser)  { r |= CERT_SYSTEM_STORE_CURRENT_USER;  }
            return r;
            }
        #endregion
        }
    }