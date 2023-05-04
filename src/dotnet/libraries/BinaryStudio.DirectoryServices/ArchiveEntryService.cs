using System;
using System.IO;
using BinaryStudio.DiagnosticServices;
using SharpCompress.Archives;

namespace BinaryStudio.DirectoryServices
    {
    internal class ArchiveEntryService : IFileService
        {
        public IArchiveEntry ArchiveEntry { get; }
        public String FileName { get; }
        public String FullName { get; }

        public ArchiveEntryService(String filename, IArchiveEntry source) {
            ArchiveEntry = source;
            FullName = Path.Combine(filename, source.Key);
            FileName = source.Key;
            }

        #region M:ReadAllBytes:Byte[]
        public Byte[] ReadAllBytes() {
            using (var InputStream = OpenRead()) {
                using (var OutputStream = new MemoryStream()) {
                    InputStream.CopyTo(OutputStream);
                    return OutputStream.ToArray();
                    }
                }
            }
        #endregion

        /// <summary>Opens this file service for reading.</summary>
        /// <returns>A read-only <see cref="T:System.IO.Stream"/> for this file service content.</returns>
        public Stream OpenRead() {
            return ArchiveEntry.OpenEntryStream();
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            return FullName;
            }

        /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is allowed.</summary>
        /// <param name="target">The name of the destination file. This cannot be a directory.</param>
        /// <param name="overwrite"><see langword="true"/> if the destination file can be overwritten; otherwise, <see langword="false"/>.</param>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. -or-  <paramref name="target"/> is read-only.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="target"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.  -or-  <paramref name="target"/> specifies a directory.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="target"/> is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.IO.IOException"><paramref name="target"/> exists and <paramref name="overwrite"/> is <see langword="false"/>. -or- An I/O error has occurred.</exception>
        /// <exception cref="T:System.NotSupportedException"><paramref name="target"/> is in an invalid format.</exception>
        void IFileService.CopyTo(String target, Boolean overwrite) {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            try
                {
                using (var SourceStream = ((IFileService)this).OpenRead()) {
                    if (File.Exists(target)) {
                        if (!overwrite) { throw new IOException(); }
                        File.Delete(target);
                        }
                    var folder = Path.GetDirectoryName(target);
                    if ((!String.IsNullOrEmpty(folder) && !Directory.Exists(folder))) { Directory.CreateDirectory(folder); }
                    using (var TargetStream = File.OpenWrite(target)) {
                        SourceStream.CopyTo(TargetStream);
                        }
                    }
                }
            catch (Exception e)
                {
                e.Add("Target", target);
                e.Add("Overwrite", overwrite);
                throw;
                }
            }

        /// <summary>Move an existing file to a new file. Overwriting a file of the same name when <paramref name="overwrite"/> is <see langword="true"/>.</summary>
        /// <param name="target">The name of the destination file. This cannot be a directory.</param>
        /// <param name="overwrite"><see langword="true"/> if the destination file can be overwritten; otherwise, <see langword="false"/>.</param>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. -or-  <paramref name="target"/> is read-only.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="target"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.  -or-  <paramref name="target"/> specifies a directory.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="target"/> is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.IO.IOException"><paramref name="target"/> exists and <paramref name="overwrite"/> is <see langword="false"/>. -or- An I/O error has occurred.</exception>
        /// <exception cref="T:System.NotSupportedException"><paramref name="target"/> is in an invalid format.</exception>
        void IFileService.MoveTo(String target, Boolean overwrite) {
            ((IFileService)this).CopyTo(target, overwrite);
            }
        }
    }