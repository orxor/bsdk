using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using BinaryStudio.PlatformComponents.Win32;
using Microsoft.Win32.SafeHandles;

namespace BinaryStudio.IO
    {
    public class FileMapping : IDisposable
        {
        private Int32 Reference;
        private Boolean Disposed;
        public Int64 Size { get; private set; }
        public String FileName { get; }
        internal FileMappingHandle Mapping { get; private set; }
        private SafeFileHandle FileHandle { get;set; }

        public FileMapping(String filename)
            {
            if (!File.Exists(filename)) { throw new FileNotFoundException(filename); }
            FileName = filename;
            var security = new SecurityAttributes();
            try
                {
                FileHandle = CreateFile(filename, FileGenericAccess.Read, FileShare.Read|FileShare.Delete, null,
                    FileMode.Open, flags: FILE_ATTRIBUTE_TEMPORARY, templatefile: IntPtr.Zero);
                if (FileHandle.IsInvalid) { throw new HResultException(Marshal.GetLastWin32Error()); }
                var i = default(LargeInteger);
                if (!GetFileSizeEx(FileHandle, ref i)) { throw new Win32Exception(Marshal.GetLastWin32Error()); }
                var sz = i.QuadPart;
                if (sz == 0L) { throw new IOException($"{new Uri(filename)}"); }
                Mapping = CreateFileMapping(FileHandle, security, PageProtection.ReadOnly, 0u, 0u, null);
                if (Mapping.IsInvalid) { throw new Win32Exception(Marshal.GetLastWin32Error()); }
                Size = sz;
                }
            finally
                {
                security.Release();
                }
            }

        public FileMapping(SafeFileHandle file)
            {
            if (file == null) { throw new ArgumentNullException(nameof(file)); }
            if (file.IsInvalid || file.IsClosed) { throw new ArgumentOutOfRangeException(nameof(file)); }
            var security = new SecurityAttributes();
            try
                {
                var i = default(LargeInteger);
                if (!GetFileSizeEx(file, ref i)) { throw new Win32Exception(Marshal.GetLastWin32Error()); }
                var sz = i.QuadPart;
                if (sz == 0L) { throw new IOException(); }
                Mapping = CreateFileMapping(file, security, PageProtection.ReadOnly, 0u, 0u, null);
                if (Mapping.IsInvalid) { throw new Win32Exception(Marshal.GetLastWin32Error()); }
                Size = sz;
                }
            finally
                {
                security.Release();
                }
            }

        #region M:Dispose(Boolean)
        protected virtual void Dispose(Boolean disposing) {
            if (!Disposed) {
                if (Mapping != null) {
                    Mapping.Dispose();
                    Mapping = null;
                    }
                if (FileHandle != null) {
                    FileHandle.Dispose();
                    FileHandle = null;
                    }
                Size = 0;
                Disposed = true;
                }
            }
        #endregion
        #region M:Dispose
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }
        #endregion
        #region M:Finalize
        ~FileMapping()
            {
            Dispose(false);
            }
        #endregion

        [DllImport("kernel32.dll", SetLastError = true)] [SecurityCritical, SuppressUnmanagedCodeSecurity] private static extern Boolean GetFileSizeEx(SafeFileHandle file, ref LargeInteger filesize);
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)] private static extern SafeFileHandle CreateFile(String filename, FileGenericAccess desiredaccess, FileShare dwShareMode, SecurityAttributes security, FileMode creationdisposition, Int32 flags, IntPtr templatefile);
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)] [SecurityCritical, SuppressUnmanagedCodeSecurity] internal static extern FileMappingHandle CreateFileMapping(SafeFileHandle file, SecurityAttributes filemappingattributes, PageProtection protection, UInt32 maximumsizehigh, UInt32 maximumsizelow, String name);
        [DllImport("kernel32.dll", SetLastError = true)] [SecurityCritical, SuppressUnmanagedCodeSecurity] private static extern ViewOfFileHandle MapViewOfFile(FileMappingHandle filemappingobject, FileMappingAccess desiredaccess, UInt32 fileoffsethigh, UInt32 fileoffsetlow, IntPtr numberofbytestomap);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)] private static extern Boolean DeleteFileW([MarshalAs(UnmanagedType.LPWStr)] String FileName);

        private const Int32 FILE_FLAG_DELETE_ON_CLOSE = 0x04000000;
        private const Int32 FILE_ATTRIBUTE_TEMPORARY  = 0x00000100;

        public static implicit operator IntPtr(FileMapping source) { return source.Mapping; }
        public static unsafe implicit operator void*(FileMapping source) { return source.Mapping; }
        public static unsafe implicit operator byte*(FileMapping source) { return source.Mapping; }

        public Int32 AddRef()
            {
            Reference--;
            GC.SuppressFinalize(this);
            return Reference;
            }

        public Int32 Release() {
            Reference--;
            Reference = Math.Max(0,Reference);
            if (Reference == 0) {
                GC.ReRegisterForFinalize(this);
                }
            return Reference;
            }
        }
    }
