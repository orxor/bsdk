using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.Certificates.Internal
    {
    internal class EX509CertificateStorage : X509Object, IX509CertificateStorage
        {
        private IntPtr Store;
        public override IntPtr Handle { get { return Store; }}

        public EX509CertificateStorage(IntPtr store)
            {
            Store = store;
            }

        public virtual IEnumerable<X509Certificate> Certificates { get {
            var o = Entries.CertEnumCertificatesInStore(Store, IntPtr.Zero);
            while (o != IntPtr.Zero) {
                yield return new X509Certificate(o);
                o = Entries.CertEnumCertificatesInStore(Store, o);
                }
            }}

        private const UInt32 CERT_CLOSE_STORE_FORCE_FLAG = 0x00000001;
        private const UInt32 CERT_CLOSE_STORE_CHECK_FLAG = 0x00000002;
        protected const UInt32 CERT_STORE_NO_CRYPT_RELEASE_FLAG                 = 0x00000001;
        protected const UInt32 CERT_STORE_SET_LOCALIZED_NAME_FLAG               = 0x00000002;
        protected const UInt32 CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG      = 0x00000004;
        protected const UInt32 CERT_STORE_DELETE_FLAG                           = 0x00000010;
        protected const UInt32 CERT_STORE_UNSAFE_PHYSICAL_FLAG                  = 0x00000020;
        protected const UInt32 CERT_STORE_SHARE_STORE_FLAG                      = 0x00000040;
        protected const UInt32 CERT_STORE_SHARE_CONTEXT_FLAG                    = 0x00000080;
        protected const UInt32 CERT_STORE_MANIFOLD_FLAG                         = 0x00000100;
        protected const UInt32 CERT_STORE_ENUM_ARCHIVED_FLAG                    = 0x00000200;
        protected const UInt32 CERT_STORE_UPDATE_KEYID_FLAG                     = 0x00000400;
        protected const UInt32 CERT_STORE_BACKUP_RESTORE_FLAG                   = 0x00000800;
        protected const UInt32 CERT_STORE_READONLY_FLAG                         = 0x00008000;
        protected const UInt32 CERT_STORE_OPEN_EXISTING_FLAG                    = 0x00004000;
        protected const UInt32 CERT_STORE_CREATE_NEW_FLAG                       = 0x00002000;
        protected const UInt32 CERT_STORE_MAXIMUM_ALLOWED_FLAG                  = 0x00001000;
        protected const UInt32 CERT_SYSTEM_STORE_CURRENT_USER                   = 0x00010000;
        protected const UInt32 CERT_SYSTEM_STORE_LOCAL_MACHINE                  = 0x00020000;
        protected const UInt32 CERT_SYSTEM_STORE_CURRENT_SERVICE                = 0x00040000;
        protected const UInt32 CERT_SYSTEM_STORE_SERVICES                       = 0x00050000;
        protected const UInt32 CERT_SYSTEM_STORE_USERS                          = 0x00060000;
        protected const UInt32 CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY      = 0x00070000;
        protected const UInt32 CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY     = 0x00080000;
        protected const UInt32 CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE       = 0x00090000;

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
        protected internal static UInt32 MapX509StoreFlags(X509StoreLocation storeLocation, X509OpenFlags flags) {
            var r = 0U;
            var mode = ((UInt32)flags) & 0x3;
            switch (mode)
                {
                case (UInt32)X509OpenFlags.ReadOnly:    { r |= CERT_STORE_READONLY_FLAG; }          break;
                case (UInt32)X509OpenFlags.MaxAllowed:  { r |= CERT_STORE_MAXIMUM_ALLOWED_FLAG; }   break;
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