using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates.Internal;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public class X509CertificateStorage : X509Object, IX509CertificateStorage
        {
        private IX509CertificateStorage Store;
        public override IntPtr Handle { get { return Store?.Handle ?? IntPtr.Zero; }}

        private X509CertificateStorage(IntPtr store) {
            if (store == null) { throw new ArgumentNullException(nameof(store)); }
            Store = new EX509CertificateStorage(store);
            }

        #region ctor{X509StoreName,X509StoreLocation}
        public X509CertificateStorage(X509StoreName name, X509StoreLocation location) {
            if ((location == X509StoreLocation.CurrentUser) || (location == X509StoreLocation.LocalMachine)) {
                String StoreName = null;
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
                    default: throw new ArgumentOutOfRangeException(nameof(name));
                    }
                Store = new EX509CertificateStorage(Validate(Entries.CertOpenStore(
                    CERT_STORE_PROV_SYSTEM_W,
                    PKCS_7_ASN_ENCODING|X509_ASN_ENCODING,
                    IntPtr.Zero,
                    MapX509StoreFlags(location,X509OpenFlags.MaxAllowed|X509OpenFlags.OpenExistingOnly),
                    StoreName),Entries as CryptographicObject, NotZero));
                return;
                }
            throw new ArgumentOutOfRangeException(nameof(location));
            }
        #endregion
        #region ctor{Uri,X509StoreLocation}
        public X509CertificateStorage(Uri uri, X509StoreLocation location) {
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

        [DllImport("kernel32.dll", SetLastError = true)] internal static extern unsafe IntPtr LocalFree(void* handle);
        [DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Unicode, SetLastError = true)] private static extern unsafe Boolean FormatMessage(UInt32 flags, IntPtr source,  Int32 dwMessageId, UInt32 dwLanguageId, void* lpBuffer, Int32 nSize, IntPtr[] arguments);

        protected static readonly IntPtr CERT_STORE_PROV_MSG                 = new IntPtr(1);
        protected static readonly IntPtr CERT_STORE_PROV_MEMORY              = new IntPtr(2);
        protected static readonly IntPtr CERT_STORE_PROV_FILE                = new IntPtr(3);
        protected static readonly IntPtr CERT_STORE_PROV_REG                 = new IntPtr(4);
        protected static readonly IntPtr CERT_STORE_PROV_PKCS7               = new IntPtr(5);
        protected static readonly IntPtr CERT_STORE_PROV_SERIALIZED          = new IntPtr(6);
        protected static readonly IntPtr CERT_STORE_PROV_FILENAME_A          = new IntPtr(7);
        protected static readonly IntPtr CERT_STORE_PROV_FILENAME_W          = new IntPtr(8);
        protected static readonly IntPtr CERT_STORE_PROV_FILENAME            = CERT_STORE_PROV_FILENAME_W;
        protected static readonly IntPtr CERT_STORE_PROV_SYSTEM_A            = new IntPtr(9);
        protected static readonly IntPtr CERT_STORE_PROV_SYSTEM_W            = new IntPtr(10);
        protected static readonly IntPtr CERT_STORE_PROV_SYSTEM              = CERT_STORE_PROV_SYSTEM_W;
        protected static readonly IntPtr CERT_STORE_PROV_COLLECTION          = new IntPtr(11);
        protected static readonly IntPtr CERT_STORE_PROV_SYSTEM_REGISTRY_A   = new IntPtr(12);
        protected static readonly IntPtr CERT_STORE_PROV_SYSTEM_REGISTRY_W   = new IntPtr(13);
        protected static readonly IntPtr CERT_STORE_PROV_SYSTEM_REGISTRY     = CERT_STORE_PROV_SYSTEM_REGISTRY_W;
        protected static readonly IntPtr CERT_STORE_PROV_PHYSICAL_W          = new IntPtr(14);
        protected static readonly IntPtr CERT_STORE_PROV_PHYSICAL            = CERT_STORE_PROV_PHYSICAL_W;
        protected static readonly IntPtr CERT_STORE_PROV_SMART_CARD_W        = new IntPtr(15);
        protected static readonly IntPtr CERT_STORE_PROV_SMART_CARD          = CERT_STORE_PROV_SMART_CARD_W;
        protected static readonly IntPtr CERT_STORE_PROV_LDAP_W              = new IntPtr(16);
        protected static readonly IntPtr CERT_STORE_PROV_LDAP                = CERT_STORE_PROV_LDAP_W;
        protected static readonly IntPtr CERT_STORE_PROV_PKCS12              = new IntPtr(17);
        protected static readonly String sz_CERT_STORE_PROV_MEMORY           = "Memory";
        protected static readonly String sz_CERT_STORE_PROV_FILENAME_W       = "File";
        protected static readonly String sz_CERT_STORE_PROV_FILENAME         = sz_CERT_STORE_PROV_FILENAME_W;
        protected static readonly String sz_CERT_STORE_PROV_SYSTEM_W         = "System";
        protected static readonly String sz_CERT_STORE_PROV_SYSTEM           = sz_CERT_STORE_PROV_SYSTEM_W;
        protected static readonly String sz_CERT_STORE_PROV_PKCS7            = "PKCS7";
        protected static readonly String sz_CERT_STORE_PROV_PKCS12           = "PKCS12";
        protected static readonly String sz_CERT_STORE_PROV_SERIALIZED       = "Serialized";
        protected static readonly String sz_CERT_STORE_PROV_COLLECTION       = "Collection";
        protected static readonly String sz_CERT_STORE_PROV_SYSTEM_REGISTRY_W = "SystemRegistry";
        protected static readonly String sz_CERT_STORE_PROV_SYSTEM_REGISTRY  = sz_CERT_STORE_PROV_SYSTEM_REGISTRY_W;
        protected static readonly String sz_CERT_STORE_PROV_PHYSICAL_W       = "Physical";
        protected static readonly String sz_CERT_STORE_PROV_PHYSICAL         = sz_CERT_STORE_PROV_PHYSICAL_W;
        protected static readonly String sz_CERT_STORE_PROV_SMART_CARD_W     = "SmartCard";
        protected static readonly String sz_CERT_STORE_PROV_SMART_CARD       = sz_CERT_STORE_PROV_SMART_CARD_W;
        protected static readonly String sz_CERT_STORE_PROV_LDAP_W           = "Ldap";
        protected static readonly String sz_CERT_STORE_PROV_LDAP             = sz_CERT_STORE_PROV_LDAP_W;

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

        private const UInt32 PKCS_7_ASN_ENCODING         = 0x00010000;
        private const UInt32 X509_ASN_ENCODING           = 0x00000001;

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
            Validate(Entries.CertEnumSystemStore((CERT_SYSTEM_STORE_FLAGS)flags, IntPtr.Zero, IntPtr.Zero, delegate(IntPtr systemstore, CERT_SYSTEM_STORE_FLAGS storeflags, ref CERT_SYSTEM_STORE_INFO pStoreInfo, IntPtr pvReserved, IntPtr pvArg) {
                if (storeflags.HasFlag(CERT_SYSTEM_STORE_FLAGS.CERT_SYSTEM_STORE_RELOCATE_FLAG))
                    {
                    throw new NotSupportedException();
                    }
                else
                    {
                    r.Add(Marshal.PtrToStringUni(systemstore));
                    }
                return true;
                }));
            return r.ToArray();
            }
        #endregion
        }
    }