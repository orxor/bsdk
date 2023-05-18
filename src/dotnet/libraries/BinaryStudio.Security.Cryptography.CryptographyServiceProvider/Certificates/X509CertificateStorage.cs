using System;
using System.Collections.Generic;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates.Internal;
using BinaryStudio.Serialization;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    /// <summary>
    /// Represents an X.509 store, where certificates are persisted and managed.
    /// </summary>
    public class X509CertificateStorage : X509Object, IX509CertificateStorage
        {
        private IX509CertificateStorage Store;
        public virtual X509StoreLocation Location { get; }
        public override IntPtr Handle { get { return Store?.Handle ?? IntPtr.Zero; }}

        #region ctor{IntPtr}
        /// <summary>
        /// Initializes a new instance of the <see cref="X509CertificateStorage"/> class using an <see cref="IntPtr"/>
        /// handle to an <see langword="HCERTSTORE"/> store.
        /// </summary>
        /// <param name="store">A handle to an <see langword="HCERTSTORE"/> store.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="store"/> parameter is <see langword="null"/>.</exception>
        private X509CertificateStorage(IntPtr store) {
            if (store == null) { throw new ArgumentNullException(nameof(store)); }
            throw new NotImplementedException();
            }
        #endregion
        #region ctor{X509StoreName}
        /// <summary>
        /// Initializes a new instance of the <see cref="X509CertificateStorage"/> class using the specified store name
        /// from the current user's certificate stores (<see cref="X509StoreLocation.CurrentUser"/>).
        /// </summary>
        /// <param name="storename">One of the enumeration values that specifies the name of the X.509 certificate store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="storename"/> is not a valid name.</exception>
        public X509CertificateStorage(X509StoreName storename)
            :this(storename,X509StoreLocation.CurrentUser)
            {
            }
        #endregion
        #region ctor{X509StoreName,X509StoreLocation}
        /// <summary>
        /// Initializes a new instance of the <see cref="X509CertificateStorage"/> class using the specified
        /// <see cref="X509StoreName"/> and <see cref="X509StoreLocation"/> values.
        /// </summary>
        /// <param name="storename">One of the enumeration values that specifies the name of the X.509 certificate store.</param>
        /// <param name="location">One of the enumeration values that specifies the location of the X.509 certificate store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="location"/> is not a valid location or <paramref name="storename"/> is not a valid name.</exception>
        public X509CertificateStorage(X509StoreName storename, X509StoreLocation location) {
            Location = location;
            if ((location == X509StoreLocation.CurrentUser) || (location == X509StoreLocation.LocalMachine)) {
                String StoreName;
                switch (storename) {
                    case X509StoreName.AddressBook:          { StoreName = "AddressBook";      } break;
                    case X509StoreName.AuthRoot:             { StoreName = "AuthRoot";         } break;
                    case X509StoreName.CertificateAuthority: { StoreName = "CA";               } break;
                    case X509StoreName.Disallowed:           { StoreName = "Disallowed";       } break;
                    case X509StoreName.My:                   { StoreName = "My";               } break;
                    case X509StoreName.Root:                 { StoreName = "Root";             } break;
                    case X509StoreName.TrustedPeople:        { StoreName = "TrustedPeople";    } break;
                    case X509StoreName.TrustedPublisher:     { StoreName = "TrustedPublisher"; } break;
                    case X509StoreName.TrustedDevices:       { StoreName = "TrustedDevices";   } break;
                    case X509StoreName.Device: { Store = new DeviceCertificateStorage(CRYPT_PROVIDER_TYPE.AUTO,location); return; }
                    case X509StoreName.Memory: { Store = new MemoryCertificateStorage(location); return; }
                    default: throw new ArgumentOutOfRangeException(nameof(storename))
                            .Add("StoreName",storename);
                    }
                Store = new EX509CertificateStorage(Validate(Entries.CertOpenStore(
                    CERT_STORE_PROV_SYSTEM_A,
                    PKCS_7_ASN_ENCODING|X509_ASN_ENCODING,
                    IntPtr.Zero,
                    MapX509StoreFlags(location,X509OpenFlags.MaxAllowed|X509OpenFlags.OpenExistingOnly),
                    StoreName),NotZero),StoreName);
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
                    Store = new FolderCertificateStorage(source.Substring(9), location);
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
        #region ctor{String,X509StoreLocation}
        /// <summary>
        /// Initializes a new instance of the <see cref="X509CertificateStorage"/> class using the specified
        /// <paramref name="storename"/> and <see cref="X509StoreLocation"/> values.
        /// </summary>
        /// <param name="storename">Specifies the name of the X.509 certificate store.</param>
        /// <param name="location">One of the enumeration values that specifies the location of the X.509 certificate store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="location"/> is not a valid location or <paramref name="storename"/> is not a valid name.</exception>
        public X509CertificateStorage(String storename, X509StoreLocation location) {
            if (storename == null) { throw new ArgumentNullException(nameof(storename)); }
            if (String.IsNullOrWhiteSpace(storename)) { throw new ArgumentOutOfRangeException(nameof(storename)); }
            Location = location;
            if (storename.StartsWith("folder://", StringComparison.OrdinalIgnoreCase)) { Store = new FolderCertificateStorage(storename.Substring(9), location); return; }
            if ((location == X509StoreLocation.CurrentUser) || (location == X509StoreLocation.LocalMachine)) {
                if (String.Equals(storename,"Device",StringComparison.OrdinalIgnoreCase)) { Store = new DeviceCertificateStorage(CRYPT_PROVIDER_TYPE.AUTO,location); return; }
                if (String.Equals(storename,"Memory",StringComparison.OrdinalIgnoreCase)) { Store = new MemoryCertificateStorage(location); return; }
                Store = new EX509CertificateStorage(Validate(Entries.CertOpenStore(
                    CERT_STORE_PROV_SYSTEM_A,
                    PKCS_7_ASN_ENCODING|X509_ASN_ENCODING,
                    IntPtr.Zero,
                    MapX509StoreFlags(location,X509OpenFlags.MaxAllowed|X509OpenFlags.OpenExistingOnly),
                    storename),NotZero),storename);
                return;
                }
            throw new ArgumentOutOfRangeException(nameof(location));
            }
        #endregion

        /// <summary>
        /// Returns a collection of certificates located in an X.509 certificate store.
        /// </summary>
        public IEnumerable<X509Certificate> Certificates { get {
            foreach (var o in Store.Certificates) {
                yield return o;
                }
            }}

        /// <summary>
        /// Returns a collection of certificate revocation list (CRL) located in an X.509 certificate store.
        /// </summary>
        public IEnumerable<X509CertificateRevocationList> CertificateRevocationLists { get {
            foreach (var o in Store.CertificateRevocationLists) {
                yield return o;
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
        public unsafe X509Certificate Find(CERT_INFO* Info)
            {
            return Store.Find(Info);
            }
        #endregion
        #region M:Add(X509Certificate o)
        /// <summary>
        /// Adds a certificate to an X.509 certificate store.
        /// </summary>
        /// <param name="o">The certificate to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="o"/> is <see langword="null"/>.</exception>
        public void Add(X509Certificate o) {
            if (o == null) {throw new ArgumentNullException(nameof(o)); }
            Store.Add(o);
            }
        #endregion
        #region M:Add(X509CertificateRevocationList)
        /// <summary>
        /// Adds a certificate revocation list (CRL) to an X.509 certificate store.
        /// </summary>
        /// <param name="o">The certificate revocation list (CRL) to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="o"/> is null.</exception>
        public void Add(X509CertificateRevocationList o) {
            if (o == null) {throw new ArgumentNullException(nameof(o)); }
            Store.Add(o);
            }
        #endregion
        #region M:Remove(X509Certificate)
        /// <summary>
        /// Removes a certificate from an X.509 certificate store.
        /// </summary>
        /// <param name="o">The certificate to remove.</param>
        /// <exception cref="ArgumentNullException"><paramref name="o"/> is null.</exception>
        public void Remove(X509Certificate o) {
            if (o == null) {throw new ArgumentNullException(nameof(o)); }
            Store.Remove(o);
            }
        #endregion
        #region M:Remove(X509CertificateRevocationList)
        /// <summary>
        /// Removes a certificate revocation list (CRL) from an X.509 certificate store.
        /// </summary>
        /// <param name="o">The certificate revocation list (CRL) to remove.</param>
        /// <exception cref="ArgumentNullException"><paramref name="o"/> is null.</exception>
        public void Remove(X509CertificateRevocationList o) {
            if (o == null) {throw new ArgumentNullException(nameof(o)); }
            Store.Remove(o);
            }
        #endregion

        #region M:MapX509StoreFlags(X509StoreLocation,X509OpenFlags):UInt32
        internal static Int32 MapX509StoreFlags(X509StoreLocation storeLocation, X509OpenFlags flags) {
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