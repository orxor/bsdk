﻿using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace BinaryStudio.IO
    {
    public sealed class ReadOnlyFileMappingStream : ReadOnlyMappingStream
        {
        protected Boolean DeleteOnDispose;
        public override Int64 Offset { get; }

        /**
         * <summary>When overridden in a derived class, gets the length in bytes of the stream.</summary>
         * <returns>A long value representing the length of the stream in bytes.</returns>
         * <exception cref="NotSupportedException">A class derived from Stream does not support seeking.</exception>
         * <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
         * <filterpriority>1</filterpriority>
         * */
        public override Int64 Length { get; }

        #region M:Read(Byte[],Int32,Int32):Int32
        /**
         * <summary>When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
         * <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
         * <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
         * <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
         * <param name="count">The maximum number of bytes to be read from the current stream.</param>
         * <exception cref="ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length.</exception>
         * <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
         * <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
         * <exception cref="IOException">An I/O error occurs.</exception>
         * <exception cref="NotSupportedException">The stream does not support reading.</exception>
         * <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
         * <filterpriority>1</filterpriority>
         * */
        public override unsafe Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
            {
            if (buffer == null) { throw new ArgumentNullException(nameof(buffer)); }
            if (offset < 0) { throw new ArgumentOutOfRangeException(nameof(offset)); }
            if (count < 0)  { throw new ArgumentOutOfRangeException(nameof(count));  }
            if (buffer.Length - offset < count) { throw new ArgumentOutOfRangeException(nameof(offset)); }
            if (Disposed) { throw new ObjectDisposedException(nameof(Mapping)); }
                {
                var sz = Length - (Position + count);
                if (sz < 0) { count += (Int32)sz; }
                var n = default(LargeInteger);
                n.QuadPart = Offset + Position;
                var r = (n.QuadPart / AllocationGranularity) * AllocationGranularity;
                var g = (n.QuadPart % AllocationGranularity) + count;
                var j = n.QuadPart - r;
                n.QuadPart = r;
                #if LINUX
                var view = MapViewOfFile(IntPtr.Zero, new IntPtr(g), PageProtection.Read, MAP_PRIVATE, (Int32)Mapping.FileHandle.DangerousGetHandle(), (IntPtr)n.QuadPart);
                if (view.IsInvalid) { throw new Win32Exception(Marshal.GetLastWin32Error()); }
                view.Length = g;
                #else
                var view = MapViewOfFile(Mapping.Mapping, FileMappingAccess.Read, n.UHighPart, n.ULowPart, new IntPtr(g));
                if (view.IsInvalid) { throw new Win32Exception(Marshal.GetLastWin32Error()); }
                #endif
                var ptr = (Byte*)view.Memory;
                for (var i = offset; i < count; i++)
                    {
                    buffer[i] = ptr[j + i];
                    }
                Seek(count, SeekOrigin.Current);
                return count;
                }
            }
        #endregion

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEM_INFO
            {
            private readonly UInt16 wProcessorArchitecture;
            private readonly UInt16 wReserved;
            private readonly UInt32 dwPageSize;
            private readonly IntPtr lpMinimumApplicationAddress;
            private readonly IntPtr lpMaximumApplicationAddress;
            private readonly IntPtr dwActiveProcessorMask;
            private readonly UInt32 dwNumberOfProcessors;
            private readonly UInt32 dwProcessorType;
            public  readonly UInt32 dwAllocationGranularity;
            private readonly UInt16 wProcessorLevel;
            private readonly UInt16 wProcessorRevision;
            }

        private static readonly Int64 AllocationGranularity;
        static ReadOnlyFileMappingStream()
            {
            #if LINUX
            AllocationGranularity = GetPageSize();
            #else
            var si = new SYSTEM_INFO();
            GetSystemInfo(ref si);
            AllocationGranularity = si.dwAllocationGranularity;
            #endif
            }

        #if LINUX
        [DllImport("c", EntryPoint = "getpagesize")] private static extern Int32 GetPageSize();
        [DllImport("c", EntryPoint = "mmap")] private static extern ViewOfFileHandle MapViewOfFile(IntPtr addr, IntPtr length, PageProtection protection, Int32 flags, Int32 fd, IntPtr offset);
        private const Int32 MAP_PRIVATE = 0x02;
        #else
        [DllImport("kernel32.dll", SetLastError = true)] private static extern void GetSystemInfo(ref SYSTEM_INFO systeminfo);
        [DllImport("kernel32.dll", SetLastError = true)][SecurityCritical, SuppressUnmanagedCodeSecurity] private static extern ViewOfFileHandle MapViewOfFile(FileMappingHandle filemappingobject, FileMappingAccess desiredaccess, UInt32 fileoffsethigh, UInt32 fileoffsetlow, IntPtr numberofbytestomap);
        #endif

        #region M:Dispose(Boolean)
        /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"/> and optionally releases the managed resources.</summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposing) {
            lock(this) {
                if (!Disposed) {
                    Disposed = true;
                    Mapping = null;
                    }
                base.Dispose(disposing);
                }
            }
        #endregion

        internal ReadOnlyFileMappingStream(ReadOnlyFileMappingStream source, Int64 offset, Int64 length)
            {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (offset < 0) { throw new ArgumentOutOfRangeException(nameof(offset)); }
            if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
            if ((offset + length) > source.Mapping.Size) { throw new ArgumentOutOfRangeException(nameof(length)); }
            Mapping = source.Mapping;
            Offset = offset;
            Length = length;
            }

        public ReadOnlyFileMappingStream(String FileName)
            {
            if (FileName == null) { throw new ArgumentNullException(nameof(FileName)); }
            #if NET35
            if (String.IsNullOrEmpty(FileName)) { throw new ArgumentOutOfRangeException(nameof(FileName)); }
            #else
            if (String.IsNullOrWhiteSpace(FileName)) { throw new ArgumentOutOfRangeException(nameof(FileName)); }
            #endif
            Mapping = new FileMapping(FileName);
            Length = Mapping.Size;
            Offset = 0;
            }

        internal ReadOnlyFileMappingStream(String FileName, Boolean DeleteOnDispose)
            {
            if (FileName == null) { throw new ArgumentNullException(nameof(FileName)); }
            #if NET35
            if (String.IsNullOrEmpty(FileName)) { throw new ArgumentOutOfRangeException(nameof(FileName)); }
            #else
            if (String.IsNullOrWhiteSpace(FileName)) { throw new ArgumentOutOfRangeException(nameof(FileName)); }
            #endif
            Mapping = new FileMapping(FileName);
            Length = Mapping.Size;
            this.DeleteOnDispose = DeleteOnDispose;
            Offset = 0;
            }

        public ReadOnlyFileMappingStream(FileMapping mapping)
            {
            if (mapping == null) { throw new ArgumentNullException(nameof(mapping)); }
            Mapping = mapping;
            Offset = 0;
            Length = mapping.Size;
            }

        internal ReadOnlyFileMappingStream(FileMapping mapping, Int64 offset, Int64 length)
            {
            if (mapping == null) { throw new ArgumentNullException(nameof(mapping)); }
            if (offset < 0) { throw new ArgumentOutOfRangeException(nameof(offset)); }
            if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
            if ((offset + length) > mapping.Size) { throw new ArgumentOutOfRangeException(nameof(length)); }
            Mapping = mapping;
            Offset = offset;
            Length = length;
            }

        internal ReadOnlyFileMappingStream(ReadOnlyFileMappingStream source, Int64 length)
            {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (length < 0) {
                throw new ArgumentOutOfRangeException(nameof(length));
                }
            Mapping = source.Mapping;
            Offset = source.Offset + source.Position;
            if ((Offset + length) > Mapping.Size) { throw new ArgumentOutOfRangeException(nameof(length)); }
            Length = length;
            }

        public override ReadOnlyMappingStream Clone()
            {
            var r = new ReadOnlyFileMappingStream(Mapping, Offset, Length);
            r.Seek(Position, SeekOrigin.Begin);
            return r;
            }

        public override ReadOnlyMappingStream Clone(Int64 offset, Int64 length) { return new ReadOnlyFileMappingStream(this, offset, length); }
        public override ReadOnlyMappingStream Clone(Int64 length)               { return new ReadOnlyFileMappingStream(this, length);         }

        private FileMapping Mapping;
        }
    }