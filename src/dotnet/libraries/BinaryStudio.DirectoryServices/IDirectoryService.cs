using System;
using System.Collections.Generic;

namespace BinaryStudio.DirectoryServices
    {
    public interface IDirectoryService
        {
        /// <summary>Returns the file services (including their paths) in the directory service.</summary>
        /// <returns>A list of file services.</returns>
        IEnumerable<IFileService> GetFiles();
        }
    }