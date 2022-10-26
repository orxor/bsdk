using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable
    {
    /// <summary>
    /// Subsection directory header structure.
    /// The directory header structure is followed by the directory entries
    /// which specify the subsection type, module index, file offset, and size.
    /// The subsection directory gives the location (LFO) and size of each subsection,
    /// as well as its type and module number if applicable.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TD32DirectoryEntry
        {
        public readonly TD32SubsectionType SubsectionType; // Subdirectory type
        public readonly Int16 ModuleIndex;                 // Module index
        public readonly Int32 Offset;                      // Offset from the base offset lfoBase
        public readonly Int32 Size;                        // Number of bytes in subsection
        }
    }