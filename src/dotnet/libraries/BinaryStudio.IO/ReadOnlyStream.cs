using BinaryStudio.DirectoryServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BinaryStudio.IO
    {
    public class ReadOnlyStream : ReadOnlyMappingStream
        {
        private Stream Source;
        private readonly Boolean Closable;
        private static readonly Int32 ProcessId = Process.GetCurrentProcess().Id;
        private const Int32 BlockSize = 10;

        public override Int64 Offset { get; }
        public override Int64 Length { get { return Source.Length; }}

        public override ReadOnlyMappingStream Clone()
            {
            throw new NotImplementedException();
            }

        public override ReadOnlyMappingStream Clone(long offset, long length)
            {
            throw new NotImplementedException();
            }

        public ReadOnlyStream(Stream source)
            :this(source,false)
            {
            }

        private ReadOnlyStream(Stream source, Boolean closable) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            Source = source;
            Closable = closable;
            }

        /// <summary>When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length.</exception>
        /// <exception cref="T:System.ArgumentNullException"> <paramref name="buffer"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override int Read(byte[] buffer, int offset, int count) {
            if (buffer == null) { throw new ArgumentNullException(nameof(buffer)); }
            if (offset < 0) { throw new ArgumentOutOfRangeException(nameof(offset)); }
            if (count  < 0) { throw new ArgumentOutOfRangeException(nameof(count));  }
            if (buffer.Length - offset < count) { throw new ArgumentOutOfRangeException(nameof(offset)); }
            if (Disposed) { throw new ObjectDisposedException("Object already disposed."); }
            return Source.Read(buffer, offset, count);
            }

        /// <summary>When overridden in a derived class, gets or sets the position within the current stream.</summary>
        /// <returns>The current position within the stream.</returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override Int64 Position {
            get { return Source.Position;  }
            set { Source.Position = value; }
            }

        #region M:Clone(Stream):ReadOnlyMappingStream
        public static ReadOnlyMappingStream Clone(Stream source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (source.CanSeek) { return new ReadOnlyStream(source); }
            var assembly = Assembly.GetEntryAssembly();
            var Folder = Path.Combine(Path.GetTempPath(), $"{{{assembly.FullName},{ProcessId}}}");
            if (!Directory.Exists(Folder)) { Directory.CreateDirectory(Folder); }
            var FileName = PathUtils.GetTempFileName(Folder, "str");
            var Block = new Byte[BlockSize];
            using (var Output = File.OpenWrite(FileName = Path.Combine(Folder,Path.GetFileName(FileName)))) {
                while (true) {
                    var Count = source.Read(Block,0,Block.Length);
                    if (Count == 0) { break; }
                    Output.Write(Block,0,Count);
                    }
                }
            return new ReadOnlyFileMappingStream(FileName, true);
            }
        #endregion

        public override ReadOnlyMappingStream Clone(Int64 length) {
            if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
            if (CanSeek) {
                var offset = Position;
                var assembly = Assembly.GetEntryAssembly();
                var folder = Path.Combine(Path.GetTempPath(), $"{{{assembly.FullName},{ProcessId}}}");
                if (!Directory.Exists(folder)) { Directory.CreateDirectory(folder); }
                var filename = PathUtils.GetTempFileName(folder, "str");
                var block = new Byte[BlockSize];
                using (var output = File.OpenWrite(filename = Path.Combine(folder, Path.GetFileName(filename)))) {
                    while (length > 0) {
                        var blockcount = (Int32)Math.Min(block.Length, length);
                        var sourcecount = Source.Read(block, 0, blockcount);
                        if (sourcecount == 0) { break; }
                        output.Write(block, 0, sourcecount);
                        length -= sourcecount;
                        }
                    }
                Source.Seek(offset, SeekOrigin.Begin);
                return new ReadOnlyFileMappingStream(filename, true);
                }
            else
                {
                var assembly = Assembly.GetEntryAssembly();
                var folder = Path.Combine(Path.GetTempPath(), $"{{{assembly.FullName},{ProcessId}}}");
                if (!Directory.Exists(folder)) { Directory.CreateDirectory(folder); }
                var filename = PathUtils.GetTempFileName(folder, "str");
                var block = new Byte[BlockSize];
                using (var output = File.OpenWrite(filename = Path.Combine(folder, Path.GetFileName(filename))))
                    {
                    while (length > 0) {
                        var blockcount = (Int32)Math.Min(block.Length, length);
                        var sourcecount = Source.Read(block, 0, blockcount);
                        if (sourcecount == 0) { break; }
                        output.Write(block, 0, sourcecount);
                        length -= sourcecount;
                        }
                    }
                return new ReadOnlyFileMappingStream(filename, true);
                }
            }
        }
    }
