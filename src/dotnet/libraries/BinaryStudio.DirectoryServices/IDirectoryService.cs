using System;
using System.Collections.Generic;

namespace BinaryStudio.DirectoryServices
    {
    public interface IDirectoryService
        {
        IEnumerable<IFileService> GetFiles(String SearchPattern, DirectoryServiceSearchOptions SearchOption);
        }
    }