using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    /// <summary>
    /// The subsection directory is prefixed with a directory header structure
    /// indicating size and number of subsection directory entries that follow.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TD32DirectoryHeader
        {
        public readonly Int16 Size;           // Length of this structure
        public readonly Int16 DirEntrySize;   // Length of each directory entry
        public readonly Int32 DirEntryCount;  // Number of directory entries
        public readonly Int32 lfoNextDir;     // Offset from lfoBase of next directory.
        public readonly Int32 Flags;          // Flags describing directory and subsection tables.
        //TD32DirectoryEntry DirEntries[DirEntryCount];
        }
    }