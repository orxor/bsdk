using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    /// <summary>
    /// [Import Directory Table]
    /// The import information begins with the import directory table, which describes the remainder of the import
    /// information. The import directory table contains address information that is used to resolve fixup
    /// references to the entry points within a DLL image. The import directory table consists of an array of
    /// import directory entries, one entry for each DLL to which the image refers. The last directory entry is
    /// empty (filled with null values), which indicates the end of the directory table.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_IMPORT_DIRECTORY
        {
        public  readonly UInt32 ImportLookupTable;      /* The RVA of the import lookup table. This table contains a name or ordinal for each import.                                                         */
        private readonly UInt32 TimeDateStamp;          /* The stamp that is set to zero until the image is bound. After the image is bound, this field is set to the time/data stamp of the DLL.             */
        private readonly UInt32 ForwarderChain;         /* The index of the first forwarder reference.                                                                                                        */
        public  readonly UInt32 Name;                   /* The address of an ASCII string that contains the name of the DLL. This address is relative to the image base.                                      */
        public  readonly UInt32 ImportAddressTable;     /* The RVA of the import address table. The contents of this table are identical to the contents of the import lookup table until the image is bound. */
        }
    }