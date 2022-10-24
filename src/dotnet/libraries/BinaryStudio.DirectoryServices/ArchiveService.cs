using System;
using System.Collections.Generic;
using SharpCompress.Archives;

namespace BinaryStudio.DirectoryServices
    {
    internal class ArchiveService : DirectoryService
        {
        public IArchive Archive { get; }
        public String FileName { get; }

        public ArchiveService(String filename, IArchive archive)
            {
            FileName = filename;
            Archive = archive;
            }

        public override IEnumerable<IFileService> GetFiles(String SearchPattern, DirectoryServiceSearchOptions SearchOption) {
            SearchPattern = SearchPattern ?? "*.*";
            foreach (var entry in Archive.Entries) {
                if (!entry.IsDirectory) {
                    if (PathUtils.IsMatch(SearchPattern, entry.Key)) {
                        yield return new ArchiveEntryService(FileName, entry);
                        }
                    }
                }
            }
        }
    }