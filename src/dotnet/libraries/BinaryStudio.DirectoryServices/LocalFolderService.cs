using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryStudio.DirectoryServices
    {
    internal class LocalFolderService : DirectoryService
        {
        public String Folder { get; }

        public LocalFolderService(String folder)
            {
            Folder = folder;
            }

        private static IEnumerable<String> GetFiles(String Folder, String SearchPattern, SearchOption SearchOption) {
            try
                {
                return Directory.GetFiles(Folder,SearchPattern,SearchOption);
                }
            catch (Exception e)
                {
                e.Data["Folder"] = Folder;
                e.Data["SearchPattern"] = SearchPattern;
                throw;
                }
            }

        public override IEnumerable<IFileService> GetFiles() {
            foreach (var file in GetFiles(Folder,"*.*",SearchOption.AllDirectories)) {
                yield return new LocalFileService(file);
                }
            }
        }
    }