using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    using CRYPT_INTEGER_BLOB = CRYPT_BLOB;
    using CERT_NAME_BLOB     = CRYPT_BLOB;

    public abstract class CryptographicObject : IDisposable, IServiceProvider
        {
        protected LocalMemoryManager LocalMemoryManager = new LocalMemoryManager();
        public abstract IntPtr Handle { get; }

        #region M:Dispose<T>([Ref]T)
        protected void Dispose<T>(ref T o)
            where T: class, IDisposable
            {
            if (o != null) {
                o.Dispose();
                o = null;
                }
            }
        #endregion
        #region M:Dispose(Boolean)
        /// <summary>
        /// Releases the unmanaged resources used by the instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing) {
            if (disposing) {
                Dispose(ref LocalMemoryManager);
                }
            }
        #endregion
        #region M:Dispose
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }
        #endregion
        #region M:Finalize
        /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
        ~CryptographicObject()
            {
            Dispose(false);
            }
        #endregion

        #region M:IsNullOrEmpty(ICollection):Boolean
        protected static Boolean IsNullOrEmpty(ICollection source)
            {
            return (source == null) || (source.Count == 0);
            }
        #endregion
        #region M:IsNullOrEmpty<T>(ICollection):Boolean
        protected static Boolean IsNullOrEmpty<T>(ICollection<T> source)
            {
            return (source == null) || (source.Count == 0);
            }
        #endregion

        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="service">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="service"/>.
        /// -or-
        /// <see langword="null"/> if there is no service object of type <paramref name="service"/>.</returns>
        public virtual Object GetService(Type service) {
            if (service == null) { return null; }
            if (service == GetType()) { return this; }
            if (service.IsAssignableFrom(GetType())) { return this; }
            return null;
            }


        #region M:Validate(LastErrorService,Boolean)
        protected static void Validate(LastErrorService service, Boolean status) {
            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            if (!status) {
                var e = HResultException.GetExceptionForHR((HRESULT)service.GetLastError());
                #if DEBUG
                Debug.Print($"Validate:{e.Message}");
                #endif
                throw e;
                }
            }
        #endregion
        #region M:Validate(Boolean,CryptographicObject)
        protected static void Validate(Boolean status,CryptographicObject o = null) {
            if (!status) {
                Exception e = HResultException.GetExceptionForHR((o != null)
                    ? (HRESULT)o.GetLastWin32Error()
                    : (HRESULT)Marshal.GetLastWin32Error());
                #if DEBUG
                Debug.Print($"Validate:{e.Message}");
                #endif
                throw e;
                }
            }
        #endregion
        #region M:Validate([Out]Exception,Boolean):Boolean
        protected virtual Boolean Validate(out Exception e, Boolean status) {
            e = null;
            if (!status) {
                e = HResultException.GetExceptionForHR((HRESULT)Marshal.GetLastWin32Error());
                #if DEBUG
                Debug.Print($"Validate:{e.Message}");
                #endif
                return false;
                }
            return true;
            }
        #endregion
        #region M:Validate(HRESULT)
        protected virtual void Validate(HRESULT hr) {
            if (hr != HRESULT.S_OK) {
                throw HResultException.GetExceptionForHR(hr);
                }
            }
        #endregion
        #region M:Validate<T>(T,Func<T,Boolean>)
        protected static T Validate<T>(T value, Func<T,Boolean> predicate) {
            return Validate(null,value,predicate);
            }
        #endregion
        #region M:Validate<T>(T,CryptographicObject,Func<T,Boolean>)
        protected static T Validate<T>(LastErrorService service, T value, Func<T,Boolean> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (!predicate(value)) {
                Exception e = HResultException.GetExceptionForHR((HRESULT)service.GetLastError());
                #if DEBUG
                Debug.Print($"Validate:{e.Message}");
                #endif
                throw e;
                }
            return value;
            }
        #endregion
        #region M:GetExceptionForHR(Int32):Exception
        protected virtual Exception GetExceptionForHR(Int32 hr)
            {
            return HResultException.GetExceptionForHR((HRESULT)hr);
            }
        #endregion
        #region M:GetExceptionForHR(HRESULT):Exception
        protected virtual Exception GetExceptionForHR(HRESULT hr)
            {
            return HResultException.GetExceptionForHR(hr);
            }
        #endregion
        #region M:GetHRForLastWin32Error:HRESULT
        protected virtual HRESULT GetHRForLastWin32Error()
            {
            return (HRESULT)(Marshal.GetHRForLastWin32Error());
            }
        #endregion
        #region M:GetLastWin32Error:Int32
        /// <summary>
        /// Returns the error code returned by the last unmanaged function that was called.
        /// using platform invoke that has the System.Runtime.InteropServices.DllImportAttribute.SetLastError flag set.
        /// </summary>
        /// <returns>The last error code set by a call to the Win32 SetLastError function.</returns>
        protected internal virtual Int32 GetLastWin32Error()
            {
            return Marshal.GetLastWin32Error();
            }
        #endregion
        #region M:NotZero(IntPtr):Boolean
        protected static Boolean NotZero(IntPtr value) {
            return value != IntPtr.Zero;
            }
        #endregion
        #region M:Yield
        protected static Boolean Yield()
            {
            #if NET35
            return SwitchToThread();
            #else
            return Thread.Yield();
            #endif
            }
        #endregion
        #region M:DecodeSerialNumberString({ref}CRYPT_INTEGER_BLOB):String
        internal static unsafe String DecodeSerialNumberString(ref CRYPT_INTEGER_BLOB source) {
            var c = source.Size;
            var r = new StringBuilder();
            var bytes = source.Data;
            for (var i = 0U; i < c; ++i) {
                r.AppendFormat("{0:x2}", bytes[c - i - 1]);
                }
            return r.ToString();
            }
        #endregion
        #region M:DecodeNameString(CryptographicFunctions,{ref}CERT_NAME_BLOB):String
        internal static String DecodeNameString(CryptographicFunctions API,ref CERT_NAME_BLOB source) {
            var r = API.CertNameToStrW(X509_ASN_ENCODING, ref source, CERT_X500_NAME_STR, IntPtr.Zero, 0);
            if (r != 0) {
                using (var buffer = new LocalMemory(r << 1)) {
                    if (API.CertNameToStrW(X509_ASN_ENCODING, ref source, CERT_X500_NAME_STR, buffer, r) > 0) {
                        return Marshal.PtrToStringUni(buffer);
                        }
                    }
                }
            return null;
            }
        #endregion

        #region {locks}
        protected static IDisposable ReadLock(ReaderWriterLockSlim o)            { return new ReadLockScope(o);            }
        protected static IDisposable WriteLock(ReaderWriterLockSlim o)           { return new WriteLockScope(o);           }
        protected static IDisposable UpgradeableReadLock(ReaderWriterLockSlim o) { return new UpgradeableReadLockScope(o); }

        private class ReadLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public ReadLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterReadLock();
                }

            public void Dispose()
                {
                o.ExitReadLock();
                o = null;
                }
            }

        private class UpgradeableReadLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public UpgradeableReadLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterUpgradeableReadLock();
                }

            public void Dispose()
                {
                o.ExitUpgradeableReadLock();
                o = null;
                }
            }

        private class WriteLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public WriteLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterWriteLock();
                }

            public void Dispose()
                {
                o.ExitWriteLock();
                o = null;
                }
            }
        #endregion

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

        protected const Int32 CERT_STORE_NO_CRYPT_RELEASE_FLAG              = 0x00000001;
        protected const Int32 CERT_STORE_SET_LOCALIZED_NAME_FLAG            = 0x00000002;
        protected const Int32 CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG   = 0x00000004;
        protected const Int32 CERT_STORE_DELETE_FLAG                        = 0x00000010;
        protected const Int32 CERT_STORE_UNSAFE_PHYSICAL_FLAG               = 0x00000020;
        protected const Int32 CERT_STORE_SHARE_STORE_FLAG                   = 0x00000040;
        protected const Int32 CERT_STORE_SHARE_CONTEXT_FLAG                 = 0x00000080;
        protected const Int32 CERT_STORE_MANIFOLD_FLAG                      = 0x00000100;
        protected const Int32 CERT_STORE_ENUM_ARCHIVED_FLAG                 = 0x00000200;
        protected const Int32 CERT_STORE_UPDATE_KEYID_FLAG                  = 0x00000400;
        protected const Int32 CERT_STORE_BACKUP_RESTORE_FLAG                = 0x00000800;
        protected const Int32 CERT_STORE_READONLY_FLAG                      = 0x00008000;
        protected const Int32 CERT_STORE_OPEN_EXISTING_FLAG                 = 0x00004000;
        protected const Int32 CERT_STORE_CREATE_NEW_FLAG                    = 0x00002000;
        protected const Int32 CERT_STORE_MAXIMUM_ALLOWED_FLAG               = 0x00001000;
        protected const Int32 CERT_SYSTEM_STORE_CURRENT_USER                = 0x00010000;
        protected const Int32 CERT_SYSTEM_STORE_LOCAL_MACHINE               = 0x00020000;
        protected const Int32 CERT_SYSTEM_STORE_CURRENT_SERVICE             = 0x00040000;
        protected const Int32 CERT_SYSTEM_STORE_SERVICES                    = 0x00050000;
        protected const Int32 CERT_SYSTEM_STORE_USERS                       = 0x00060000;
        protected const Int32 CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY   = 0x00070000;
        protected const Int32 CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY  = 0x00080000;
        protected const Int32 CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE    = 0x00090000;
        protected const Int32 X509_ASN_ENCODING   = 0x00000001;
        protected const Int32 PKCS_7_ASN_ENCODING = 0x00010000;
        protected const Int32 CERT_SIMPLE_NAME_STR = 1;
        protected const Int32 CERT_OID_NAME_STR    = CERT_SIMPLE_NAME_STR + 1;
        protected const Int32 CERT_X500_NAME_STR   = CERT_OID_NAME_STR    + 1;
        }
    }
