using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;

namespace Microsoft.Win32
    {
    #region E:IMAGE_FILE_CHARACTERISTIC
    [Flags]
    public enum IMAGE_FILE_CHARACTERISTIC : ushort
        {
        IMAGE_FILE_NONE                     = 0x0000,
        IMAGE_FILE_RELOCS_STRIPPED          = 0x0001,
        IMAGE_FILE_EXECUTABLE_IMAGE         = 0x0002,
        IMAGE_FILE_LINE_NUMS_STRIPPED       = 0x0004,
        IMAGE_FILE_LOCAL_SYMS_STRIPPED      = 0x0008,
        IMAGE_FILE_AGGRESSIVE_WS_TRIM       = 0x0010,
        IMAGE_FILE_LARGE_ADDRESS_AWARE      = 0x0020,
        IMAGE_FILE_RESERVED                 = 0x0040,
        IMAGE_FILE_BYTES_REVERSED_LO        = 0x0080,
        IMAGE_FILE_32BIT_MACHINE            = 0x0100,
        IMAGE_FILE_DEBUG_STRIPPED           = 0x0200,
        IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP  = 0x0400,
        IMAGE_FILE_NET_RUN_FROM_SWAP        = 0x0800,
        IMAGE_FILE_SYSTEM                   = 0x1000,
        IMAGE_FILE_DLL                      = 0x2000,
        IMAGE_FILE_UP_SYSTEM_ONLY           = 0x4000,
        IMAGE_FILE_BYTES_REVERSED_HI        = 0x8000
        }
    #endregion
    #region T:IMAGE_DATA_DIRECTORY
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{VirtualAddress}:{Size}")]
    public struct IMAGE_DATA_DIRECTORY
        {
        public readonly UInt32 VirtualAddress;
        public readonly UInt32 Size;
        }
    #endregion
    #region T:IMAGE_DIRECTORY_ENTRY_RESOURCE
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct IMAGE_DIRECTORY_ENTRY_RESOURCE
        {
        [FieldOffset(0)] public readonly UInt32 NameOffset;
        [FieldOffset(0)] public readonly UInt32 IntegerId;
        [FieldOffset(4)] public readonly UInt32 DataEntryOffset;
        }
    #endregion

    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    public struct ANON_OBJECT_HEADER
        {
        public readonly UInt16 Sig1;            // Must be IMAGE_FILE_MACHINE_UNKNOWN
        public readonly UInt16 Sig2;            // Must be 0xffff
        public readonly UInt16 Version;         // >= 1 (implies the CLSID field is present)
        public readonly IMAGE_FILE_MACHINE Machine;
        public readonly UInt32 TimeDateStamp;
        public Guid   ClassID;         // Used to invoke CoCreateInstance
        public readonly UInt32 SizeOfData;      // Size of data that follows the header
        }

    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    internal struct ANON_OBJECT_HEADER_V2
        {
        public readonly UInt16 Sig1;            // Must be IMAGE_FILE_MACHINE_UNKNOWN
        public readonly UInt16 Sig2;            // Must be 0xffff
        public readonly UInt16 Version;         // >= 2 (implies the Flags field is present - otherwise V1)
        public readonly IMAGE_FILE_MACHINE Machine;
        public readonly UInt32 TimeDateStamp;
        public readonly Guid   ClassID;         // Used to invoke CoCreateInstance
        public readonly UInt32 SizeOfData;      // Size of data that follows the header
        public readonly UInt32 Flags;           // 0x1 -> contains metadata
        public readonly UInt32 MetaDataSize;    // Size of CLR metadata
        public readonly UInt32 MetaDataOffset;  // Offset of CLR metadata
        }

    [StructLayout(LayoutKind.Explicit,Pack = 1,Size = 52)]
    internal struct ANON_OBJECT_HEADER_BIGOBJ_V1
        {
        [FieldOffset( 0)] public readonly UInt16 Sig1;            // Must be IMAGE_FILE_MACHINE_UNKNOWN
        [FieldOffset( 2)] public readonly UInt16 Sig2;            // Must be 0xffff
        [FieldOffset( 4)] public readonly UInt16 Version;         // >= 2 (implies the Flags field is present)
        [FieldOffset( 6)] public readonly IMAGE_FILE_MACHINE Machine;         // Actual machine - IMAGE_FILE_MACHINE_xxx
        [FieldOffset( 8)] public readonly UInt32 TimeDateStamp;
        [FieldOffset(12)] public readonly Guid   ClassID;
        [FieldOffset(28)] public readonly UInt32 SizeOfData;      // Size of data that follows the header
        [FieldOffset(32)] public readonly UInt16 Flags;           // 0x1 -> contains metadata
        [FieldOffset(34)] public readonly UInt16 NumberOfSections; // extended from WORD
        [FieldOffset(36)] public readonly UInt32 NumberOfSymbols;
        [FieldOffset(40)] public readonly UInt32 PointerToSymbolTable;
        }

    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    internal struct ANON_OBJECT_HEADER_BIGOBJ_V2
        {
        /* same as ANON_OBJECT_HEADER_V2 */
        public readonly UInt16 Sig1;            // Must be IMAGE_FILE_MACHINE_UNKNOWN
        public readonly UInt16 Sig2;            // Must be 0xffff
        public readonly UInt16 Version;         // >= 2 (implies the Flags field is present)
        public readonly IMAGE_FILE_MACHINE Machine;         // Actual machine - IMAGE_FILE_MACHINE_xxx
        public readonly UInt32 TimeDateStamp;
        public readonly Guid   ClassID;         // {D1BAA1C7-BAEE-4ba9-AF20-FAF66AA4DCB8}
        public readonly UInt32 SizeOfData;      // Size of data that follows the header
        public readonly UInt32 Flags;           // 0x1 -> contains metadata
        public readonly UInt32 MetaDataSize;    // Size of CLR metadata
        public readonly UInt32 MetaDataOffset;  // Offset of CLR metadata

        /* bigobj specifics */
        public readonly UInt32 NumberOfSections; // extended from WORD
        public readonly UInt32 PointerToSymbolTable;
        public readonly UInt32 NumberOfSymbols;
        }
    }