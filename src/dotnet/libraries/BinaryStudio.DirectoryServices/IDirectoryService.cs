using System;
using System.Collections.Generic;

namespace BinaryStudio.DirectoryServices
    {
    public interface IDirectoryService
        {
        /// <summary>Returns the names of file services (including their paths) that match the specified search pattern in the specified directory service, using a value to determine whether to search subdirectories.</summary>
        /// <param name="SearchPattern">The search string to match against the names of file services.</param>
        /// <param name="SearchOption">One of the <see cref="DirectoryServiceSearchOptions"/> values that specifies whether the search operation should include all subdirectories or only the current directory service.</param>
        /// <returns>A list containing the names of file services in the specified directory service that match the specified search pattern. File service names include the full path.</returns>
        IEnumerable<IFileService> GetFiles(String SearchPattern, DirectoryServiceSearchOptions SearchOption);
        }
    }