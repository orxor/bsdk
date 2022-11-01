using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    /// <summary>
    /// The subsection directory is prefixed with a directory header structure
    /// indicating size and number of subsection directory entries that follow.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CodeViewSubsectionDirectoryHeader
        {
        public readonly Int16 Size;           // Length of this structure
        public readonly Int16 DirEntrySize;   // Length of each directory entry
        public readonly Int32 DirEntryCount;  // Number of directory entries
        public readonly Int32 lfoNextDir;     // Offset from lfoBase of next directory.
        /// <summary>
        /// Flags describing directory and subsection tables. No values have been defined for this field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 Flags;
        // TD32DirectoryEntry DirEntries[DirEntryCount];
        }
    }