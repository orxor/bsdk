using System;
using System.IO;
using BinaryStudio.IO;

namespace BinaryStudio.DirectoryServices
    {
    internal class LocalFileService : IFileService
        {
        public String FileName { get; }
        public String FullName { get; }

        public LocalFileService(String filename) {
            FullName = filename;
            FileName = Path.GetFileName(filename);
            }

        #region M:OpenRead:Stream
        /// <summary>Opens this file service for reading.</summary>
        /// <returns>A read-only <see cref="T:System.IO.Stream"/> for this file service content.</returns>
        public Stream OpenRead()
            {
            return new ReadOnlyFileMappingStream(new FileMapping(FullName));
            }
        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            return FullName;
            }
        }
    }