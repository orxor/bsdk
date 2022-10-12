using System;
using System.Runtime.InteropServices;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public abstract class X509Object : IDisposable,IServiceProvider
        {
        protected Boolean Disposed;
        public abstract IntPtr Handle { get; }

        #region M:Dispose<T>({ref}T)
        protected static void Dispose<T>(ref T o)
            where T: IDisposable
            {
            if (o != null) {
                o.Dispose();
                o = default;
                }
            }
        #endregion
        #region M:Dispose<T>({ref}T[])
        protected static void Dispose<T>(ref T[] o)
            where T: IDisposable
            {
            if (o != null) {
                for (var i = 0; i < o.Length; i++) {
                    Dispose(ref o[i]);
                    }
                o = default;
                }
            }
        #endregion
        #region M:Dispose(Boolean)
        /// <summary>Releases the unmanaged resources used by the instance and optionally releases the managed resources.</summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing) {
            lock(this) {
                if (!Disposed) {
                    Disposed = true;
                    }
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
        ~X509Object()
            {
            Dispose(false);
            }
        #endregion
        #region M:IServiceProvider.GetService(Type):Object
        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="service">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="service"/>.-or- null if there is no service object of type <paramref name="service"/>.</returns>
        /// <filterpriority>2</filterpriority>
        public virtual Object GetService(Type service) {
            if (service == null) { return null; }
            if (service == GetType()) { return this; }
            if (service == typeof(X509Object)) { return this; }
            return null;
            }
        #endregion
        #region M:Validate(Boolean)
        protected virtual void Validate(Boolean status) {
            if (!status) {
                throw HResultException.GetExceptionForHR(GetHRForLastWin32Error());
                }
            }
        #endregion
        #region M:GetHRForLastWin32Error:Int32
        protected static Int32 GetHRForLastWin32Error()
            {
            #if CAPILITE
            var e = (Int32)GetLastWin32Error();
            if (((long)e & 0x80000000L) == 0x80000000L) { return e; }
            return e & (Int32)UInt16.MaxValue | (unchecked((Int32)0x80070000));
            #else
            return (Int32)(Marshal.GetHRForLastWin32Error());
            #endif
            }
        #endregion
        }
    }