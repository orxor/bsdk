using System;
using System.Collections.Generic;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates.Internal;
using BinaryStudio.Serialization;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public class X509CertificateStorage : X509Object, IX509CertificateStorage
        {
        private IX509CertificateStorage Store;
        public virtual X509StoreLocation Location { get; }
        public override IntPtr Handle { get { return Store?.Handle ?? IntPtr.Zero; }}

        private X509CertificateStorage(IntPtr store) {
            if (store == null) { throw new ArgumentNullException(nameof(store)); }
            throw new NotImplementedException();
            }

        #region ctor{X509StoreName,X509StoreLocation}
        public X509CertificateStorage(X509StoreName name, X509StoreLocation location) {
            Location = location;
            if ((location == X509StoreLocation.CurrentUser) || (location == X509StoreLocation.LocalMachine)) {
                String StoreName;
                switch (name) {
                    case X509StoreName.AddressBook:          { StoreName = "AddressBook";      } break;
                    case X509StoreName.AuthRoot:             { StoreName = "AuthRoot";         } break;
                    case X509StoreName.CertificateAuthority: { StoreName = "CA";               } break;
                    case X509StoreName.Disallowed:           { StoreName = "Disallowed";       } break;
                    case X509StoreName.My:                   { StoreName = "My";               } break;
                    case X509StoreName.Root:                 { StoreName = "Root";             } break;
                    case X509StoreName.TrustedPeople:        { StoreName = "TrustedPeople";    } break;
                    case X509StoreName.TrustedPublisher:     { StoreName = "TrustedPublisher"; } break;
                    case X509StoreName.TrustedDevices:       { StoreName = "TrustedDevices";   } break;
                    default: throw new ArgumentOutOfRangeException(nameof(name))
                            .Add("StoreName",name);
                    }
                throw new NotImplementedException();
                return;
                }
            throw new ArgumentOutOfRangeException(nameof(location));
            }
        #endregion
        #region ctor{Uri,X509StoreLocation}
        public X509CertificateStorage(Uri uri, X509StoreLocation location) {
            Location = location;
            if ((location == X509StoreLocation.CurrentUser) || (location == X509StoreLocation.LocalMachine)) {
                if (String.Equals(uri.Scheme,"folder",StringComparison.OrdinalIgnoreCase)) {
                    var source = uri.ToString();
                    Store = new FX509CertificateStorage(source.Substring(9), location);
                    return;
                    }
                throw new NotSupportedException(nameof(uri));
                }
            throw new ArgumentOutOfRangeException(nameof(location));
            }
        #endregion
        #region ctor{Uri}
        public X509CertificateStorage(Uri uri)
            :this(uri,X509StoreLocation.CurrentUser)
            {
            }
        #endregion

        public IEnumerable<X509Certificate> Certificates { get {
            foreach (var o in Store.Certificates) {
                yield return o;
                }
            }}

        public IEnumerable<X509CertificateRevocationList> CertificateRevocationLists { get {
            foreach (var o in Store.CertificateRevocationLists) {
                yield return o;
                }
            }}

        #region M:Find(CERT_INFO*):X509Certificate
        public unsafe X509Certificate Find(CERT_INFO* Info)
            {
            return Store.Find(Info);
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
        #region M:Dispose(Boolean)
        /// <summary>Releases the unmanaged resources used by the instance and optionally releases the managed resources.</summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposing) {
            lock(this) {
                if (!Disposed) {
                    base.Dispose(disposing);
                    Disposed = true;
                    Dispose(ref Store);
                    }
                }
            }
        #endregion
        #region M:WriteTo(IJsonWriter)
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            (Store as IJsonSerializable)?.WriteTo(writer);
            }
        #endregion
        #region M:GetSystemStores(X509StoreLocation):String[]
        public static String[] GetSystemStores(X509StoreLocation flags) {
            var r = new List<String>();
            Validate(Entries.CertEnumSystemStore((CERT_SYSTEM_STORE_FLAGS)flags, IntPtr.Zero, IntPtr.Zero, delegate(String SystemStoreName, CERT_SYSTEM_STORE_FLAGS Storeflags, ref CERT_SYSTEM_STORE_INFO StoreInfo, IntPtr Reserved, IntPtr Arg) {
                if (Storeflags.HasFlag(CERT_SYSTEM_STORE_FLAGS.CERT_SYSTEM_STORE_RELOCATE_FLAG))
                    {
                    throw new NotSupportedException();
                    }
                else
                    {
                    r.Add(SystemStoreName);
                    }
                return true;
                }));
            return r.ToArray();
            }
        #endregion
        }
    }