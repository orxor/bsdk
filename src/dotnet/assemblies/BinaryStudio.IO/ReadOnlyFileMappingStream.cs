using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace BinaryStudio.IO
    {
    public class ReadOnlyFileMappingStream : Stream
        {
        public override Int32 ReadTimeout  { get { return 0; }}
        public override Int32 WriteTimeout { get { return 0; }}
        public Int64 Offset { get; }

        /**
         * <summary>When overridden in a derived class, gets the length in bytes of the stream.</summary>
         * <returns>A long value representing the length of the stream in bytes.</returns>
         * <exception cref="NotSupportedException">A class derived from Stream does not support seeking.</exception>
         * <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
         * <filterpriority>1</filterpriority>
         * */
        public override Int64 Length { get; }

        #region P:CanRead:Boolean
        /**
         * <summary>
         * When overridden in a derived class, gets a value indicating whether the current stream supports reading.
         * </summary>
         * <returns>true if the stream supports reading; otherwise, false.</returns>
         * <filterpriority>1</filterpriority>
         * */
        public override Boolean CanRead { get { return true; }}
        #endregion
        #region P:CanSeek:Boolean
        /**
         * <summary>
         * When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
         * </summary>
         * <returns>true if the stream supports seeking; otherwise, false.</returns>
         * <filterpriority>1</filterpriority>
         * */
        public override Boolean CanSeek { get { return true; }}
        #endregion
        #region P:CanWrite:Boolean
        /**
         * <summary>
         * When overridden in a derived class, gets a value indicating whether the current stream supports writing.
         * </summary>
         * <returns>true if the stream supports writing; otherwise, false.</returns>
         * <filterpriority>1</filterpriority>
         * */
        public override Boolean CanWrite { get { return false; }}
        #endregion
        #region P:Position:Int64
        /**
         * <summary>When overridden in a derived class, gets or sets the position within the current stream.</summary>
         * <returns>The current position within the stream.</returns>
         * <exception cref="IOException">An I/O error occurs.</exception>
         * <exception cref="NotSupportedException">The stream does not support seeking.</exception>
         * <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
         * <filterpriority>1</filterpriority>
         * */
        public override Int64 Position
            {
            get { return position; }
            set
                {
                if (value < 0) { throw new ArgumentOutOfRangeException(nameof(value)); }
                Seek(value, SeekOrigin.Begin);
                }
            }
        #endregion

        #region M:Flush
        /**
         * <summary>
         * When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be
         * written to the underlying device.
         * </summary>
         * <exception cref="IOException">An I/O error occurs.</exception>
         * <filterpriority>2</filterpriority>
         * */
        public override void Flush()
            {
            }
        #endregion
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
            if (Disposed) { throw new ObjectDisposedException(nameof(mapping)); }
                {
                var sz = Length - (Position + count);
                if (sz < 0) { count += (Int32)sz; }
                var n = default(LargeInteger);
                n.QuadPart = Offset + Position;
                var r = (n.QuadPart / AllocationGranularity) * AllocationGranularity;
                var g = (n.QuadPart % AllocationGranularity) + count;
                var j = n.QuadPart - r;
                n.QuadPart = r;
                var view = MapViewOfFile(mapping.Mapping, FileMappingAccess.Read, n.UHighPart, n.ULowPart, new IntPtr(g));
                if (view.IsInvalid) { throw new Win32Exception(Marshal.GetLastWin32Error()); }
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
        #region M:Write(Byte[],Int32,Int32)
        /**
         * <summary>When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
         * <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
         * <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
         * <param name="count">The number of bytes to be written to the current stream.</param>
         * <exception cref="T:System.ArgumentException">The sum of <paramref name="offset">offset</paramref> and <paramref name="count">count</paramref> is greater than the buffer length.</exception>
         * <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
         * <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset">offset</paramref> or <paramref name="count">count</paramref> is negative.</exception>
         * <exception cref="T:System.IO.IOException">An I/O error occured, such as the specified file cannot be found.</exception>
         * <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
         * <exception cref="T:System.ObjectDisposedException"><see cref="M:System.IO.Stream.Write(System.Byte[],System.Int32,System.Int32)"></see> was called after the stream was closed.</exception>
         * */
        public override void Write(Byte[] buffer, Int32 offset, Int32 count)
            {
            throw new NotSupportedException();
            }
        #endregion
        #region M:Seek(Int64,SeekOrigin):Int64
        /**
         * <summary>When overridden in a derived class, sets the position within the current stream.</summary>
         * <returns>The new position within the current stream.</returns>
         * <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
         * <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
         * <exception cref="IOException">An I/O error occurs.</exception>
         * <exception cref="NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output.</exception>
         * <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
         * <filterpriority>1</filterpriority>
         * */
        public override Int64 Seek(Int64 offset, SeekOrigin origin) {
            lock (this) {
                if ((origin < SeekOrigin.Begin) || (origin > SeekOrigin.End)) { throw new ArgumentOutOfRangeException(nameof(origin)); }
                if (Disposed) { throw new ObjectDisposedException(nameof(Disposed)); }
                switch (origin) {
                    case SeekOrigin.Begin:
                        {
                        if (offset < 0)      { throw new ArgumentOutOfRangeException(nameof(offset)); }
                        if (offset > Length) { throw new ArgumentOutOfRangeException(nameof(offset)); }
                        position = offset;
                        return position;
                        }
                    case SeekOrigin.Current: { return Seek(position + offset, SeekOrigin.Begin); }
                    case SeekOrigin.End:     { return Seek(Length + offset, SeekOrigin.Begin);   }
                    default: throw new ArgumentOutOfRangeException(nameof(origin));
                    }
                }
            }
        #endregion
        #region M:SetLength(Int64)
        /**
         * <summary>When overridden in a derived class, sets the length of the current stream.</summary>
         * <param name="value">The desired length of the current stream in bytes.</param>
         * <exception cref="IOException">An I/O error occurs.</exception>
         * <exception cref="NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.</exception>
         * <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
         * <filterpriority>2</filterpriority>
         * */
        public override void SetLength(Int64 value)
            {
            throw new NotSupportedException();
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
            var si = new SYSTEM_INFO();
            GetSystemInfo(ref si);
            AllocationGranularity = si.dwAllocationGranularity;
            }

        [DllImport("kernel32.dll", SetLastError = true)] private static extern void GetSystemInfo(ref SYSTEM_INFO systeminfo);
        [DllImport("kernel32.dll", SetLastError = true)][SecurityCritical, SuppressUnmanagedCodeSecurity] private static extern ViewOfFileHandle MapViewOfFile(FileMappingHandle filemappingobject, FileMappingAccess desiredaccess, UInt32 fileoffsethigh, UInt32 fileoffsetlow, IntPtr numberofbytestomap);

        #region M:Dispose(Boolean)
        /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"/> and optionally releases the managed resources.</summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposing) {
            lock(this) {
                if (!Disposed) {
                    Disposed = true;
                    mapping = null;
                    }
                base.Dispose(disposing);
                }
            }
        #endregion
        #region M:ToString:String
        public override String ToString()
            {
            return $"{Position}:{Length}";
            }
        #endregion

        protected Boolean Disposed;
        private Int64 position;
        private FileMapping mapping;
        }
    }