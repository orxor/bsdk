using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    /// <summary>
    /// [Export Directory Table]
    /// The export symbol information begins with the export directory table, which describes the remainder of the
    /// export symbol information. The export directory table contains address information that is used to resolve
    /// imports to the entry points within this image.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_EXPORT_DIRECTORY
        {
        private readonly UInt32 ExportFlags;            /* Reserved, must be 0.                                                                                                                                         */
        private readonly UInt32 TimeDateStamp;          /* The time and date that the export data was created.                                                                                                          */
        private readonly UInt16 MajorVersion;           /* The major version number. The major and minor version numbers can be set by the user.                                                                        */
        private readonly UInt16 MinorVersion;           /* The minor version number.                                                                                                                                    */
        private readonly UInt32 NameRVA;                /* The address of the ASCII string that contains the name of the DLL. This address is relative to the image base.                                               */
        public  readonly UInt32 OrdinalBase;            /* The starting ordinal number for exports in this image. This field specifies the starting ordinal number for the export address table. It is usually set to 1.*/
        public  readonly UInt32 AddressTableEntries;    /* The number of entries in the export address table.                                                                                                           */
        public  readonly UInt32 NumberOfNamePointers;   /* The number of entries in the name pointer table. This is also the number of entries in the ordinal table.                                                    */
        public  readonly UInt32 ExportAddressTableRVA;  /* The address of the export address table, relative to the image base.                                                                                         */
        public  readonly UInt32 NamePointerRVA;         /* The address of the export name pointer table, relative to the image base. The table size is given by the [NumberOfNamePointers] field.                       */
        public  readonly UInt32 OrdinalTableRVA;        /* The address of the ordinal table, relative to the image base.                                                                                                */
        }
    }