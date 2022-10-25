﻿using System;
using System.IO;
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
        }
    }