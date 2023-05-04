using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    /// <summary>
    /// [Resource Directory Table]
    /// This data structure should be considered the heading of a table because the table actually consists of
    /// directory entries.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_RESOURCE_DIRECTORY
        {
        private readonly UInt32 Characteristics;        /* Resource flags. This field is reserved for future use. It is currently set to zero.                                                                                          */
        private readonly UInt32 TimeDateStamp;          /* The time that the resource data was created by the resource compiler.                                                                                                        */
        private readonly UInt16 MajorVersion;           /* The major version number, set by the user.                                                                                                                                   */
        private readonly UInt16 MinorVersion;           /* The minor version number, set by the user.                                                                                                                                   */
        public  readonly UInt16 NumberOfNamedEntries;   /* The number of directory entries immediately following the table that use strings to identify [Type], [Name], or [Language] entries (depending on the level of the table).    */
        public  readonly UInt16 NumberOfIdEntries;      /* The number of directory entries immediately following the Name entries that use numeric IDs for [Type], [Name], or [Language] entries.                                       */
        }
    }