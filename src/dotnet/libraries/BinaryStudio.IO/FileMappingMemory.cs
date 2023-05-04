using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace BinaryStudio.IO
    {
    [DebuggerDisplay("{ToString(),nq}")]
    public class FileMappingMemory : IDisposable
        {
        private ViewOfFileHandle Handle { get; }
        public FileMapping Mapping { get; }
        public FileMappingMemory(FileMapping mapping)
            {
            if (mapping == null) { throw new ArgumentNullException(nameof(mapping)); }
            Mapping = mapping;
            #if LINUX
            Handle = MapViewOfFile(IntPtr.Zero,new IntPtr(mapping.Size),PageProtection.Read,MAP_PRIVATE,(Int32)mapping.FileHandle.DangerousGetHandle(),IntPtr.Zero);
            #else
            Handle = MapViewOfFile(mapping.Mapping, FileMappingAccess.Read, 0,0, IntPtr.Zero);
            #endif
            }

        #region M:Dispose(Boolean)
        protected virtual void Dispose(Boolean disposing) {
            if (disposing) {
                }
            }
        #endregion-
        #region M:Dispose
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }
        #endregion
        #region M:Finalize
        ~FileMappingMemory()
            {
            Dispose(false);
            }
        #endregion

        public static unsafe explicit operator void*(FileMappingMemory source)
            {
            return (source.Handle != null)
                ? (void*)source.Handle
                : null;
            }

        public static unsafe explicit operator Byte*(FileMappingMemory source)
            {
            return (source.Handle != null)
                ? (Byte*)source.Handle
                : null;
            }

        public static unsafe explicit operator IntPtr(FileMappingMemory source)
            {
            return (source.Handle != null)
                ? (IntPtr)(void*)source.Handle
                : IntPtr.Zero;
            }

        #if LINUX
        [DllImport("c", EntryPoint = "mmap")] private static extern ViewOfFileHandle MapViewOfFile(IntPtr addr, IntPtr length, PageProtection protection, Int32 flags, Int32 fd, IntPtr offset);
        private const Int32 MAP_PRIVATE = 0x02;
        #else
        [DllImport("kernel32.dll", SetLastError = true)][SecurityCritical, SuppressUnmanagedCodeSecurity] private static extern ViewOfFileHandle MapViewOfFile(FileMappingHandle filemappingobject, FileMappingAccess desiredaccess, UInt32 fileoffsethigh, UInt32 fileoffsetlow, IntPtr numberofbytestomap);
        #endif

        public override String ToString()
            {
            return Handle.ToString();
            }
        }
    }