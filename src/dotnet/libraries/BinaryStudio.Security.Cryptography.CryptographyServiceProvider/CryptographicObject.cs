﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    using HRESULT=HResult;
    using CRYPT_INTEGER_BLOB = CRYPT_BLOB;
    using CERT_NAME_BLOB     = CRYPT_BLOB;

    public abstract class CryptographicObject : IDisposable, IServiceProvider
        {
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

        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="service">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="service" />.
        /// -or-
        /// <see langword="null" /> if there is no service object of type <paramref name="service" />.</returns>
        public virtual Object GetService(Type service) {
            if (service == null) { return null; }
            if (service == GetType()) { return this; }
            if (service.IsAssignableFrom(GetType())) { return this; }
            return null;
            }

        #region M:Validate(Boolean)
        protected virtual void Validate(Boolean status) {
            if (!status) {
                Exception e = HResultException.GetExceptionForHR(Marshal.GetLastWin32Error());
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
                e = HResultException.GetExceptionForHR(Marshal.GetLastWin32Error());
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
                throw HResultException.GetExceptionForHR((Int32)hr);
                }
            }
        #endregion
        #region M:GetExceptionForHR(Int32):Exception
        protected virtual Exception GetExceptionForHR(Int32 hr)
            {
            return HResultException.GetExceptionForHR(hr);
            }
        #endregion
        #region M:GetExceptionForHR(HRESULT):Exception
        protected virtual Exception GetExceptionForHR(HRESULT hr)
            {
            return HResultException.GetExceptionForHR((Int32)hr);
            }
        #endregion
        #region M:GetHRForLastWin32Error:HRESULT
        protected static HRESULT GetHRForLastWin32Error()
            {
            return (HRESULT)(Marshal.GetHRForLastWin32Error());
            }
        #endregion
        #region M:GetLastWin32Error:Int32
        protected static Int32 GetLastWin32Error()
            {
            return Marshal.GetLastWin32Error();
            }
        #endregion
        #region M:DecodeSerialNumberString(ref CRYPT_INTEGER_BLOB):String
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
        #region M:DecodeNameString(ref CERT_NAME_BLOB):String
        internal static String DecodeNameString(ref CERT_NAME_BLOB source) {
            var r = CertNameToStr(X509_ASN_ENCODING, ref source, CERT_X500_NAME_STR, IntPtr.Zero, 0);
            if (r != 0) {
                using (var buffer = new LocalMemory(r << 1)) {
                    if (CertNameToStr(X509_ASN_ENCODING, ref source, CERT_X500_NAME_STR, buffer, r) > 0) {
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

        protected const UInt32 X509_ASN_ENCODING         = 0x00000001;
        protected const UInt32 PKCS_7_ASN_ENCODING       = 0x00010000;
        private const UInt32 CERT_SIMPLE_NAME_STR        = 1;
        private const UInt32 CERT_OID_NAME_STR           = CERT_SIMPLE_NAME_STR + 1;
        private const UInt32 CERT_X500_NAME_STR          = CERT_OID_NAME_STR    + 1;

        [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern UInt32 CertNameToStr([In] UInt32 dwCertEncodingType, [In] ref CERT_NAME_BLOB pName, [In] UInt32 dwStrType, [In] [Out] IntPtr psz, [In] UInt32 csz);
        }
    }
