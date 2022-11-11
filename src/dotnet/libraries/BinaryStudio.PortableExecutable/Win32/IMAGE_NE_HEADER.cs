using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [Flags]
    public enum IMAGE_WIN_NE_FLAGS : ushort
        {
        SingleAutodataSegment      = 0x0001,
        MultipleAutodataSegments   = 0x0002,
        PerProcessInitialization   = 0x0004,
        ProtectedModeOnly          = 0x0008,
        UsesEMSDirectly            = 0x0010,
        EMSBankInstance            = 0x0020,
        EMSGlobalMemory            = 0x0040,
        ProtectedModeApplication   = 0x0300,
        ProtectedModeCompatible    = 0x0200,
        ProtectedModeIncompatible  = 0x0100,
        Private                    = 0x4000,
        DynamicLinkLibrary         = 0x8000
        }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct NEImageFlags
        {
        [FieldOffset(0)] public readonly IMAGE_WIN_NE_FLAGS WindowsFlags;
        }

    public enum NETargetOperatingSystem : byte
        {
        OS2 = 1,
        WINDOWS = 2,
        DOS4 = 3,
        WIN386
        }

    [Flags]
    public enum NEImageOtherFlags : byte
        {
        OS2_LONG_FILE_NAMES   = 0x01,
        WIN_PROPORTIONAL_FONT = 0x02,
        WIN_CLEAN_MEMORY      = 0x04,
        WIN_GANGLOAD_PRESENT  = 0x08
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_NE_HEADER
        {
        public  readonly UInt16 MagicNumber;                            // Magic number
        public  readonly Byte   VersionNumber;                          // Version number
        public  readonly Byte   RevisionNumber;                         // Revision number
        public  readonly UInt16 EntryTableOffset;                       // Offset of Entry Table
        public  readonly UInt16 EntryTableSize;                         // Number of bytes in Entry Table
        public  readonly UInt32 Checksum;                               // Checksum of whole file
        public  readonly NEImageFlags Flags;                            // Flag word
        public  readonly UInt16 AutomaticDataSegmentNumber;             // Automatic data segment number
        public  readonly UInt16 InitialHeapAllocation;                  // Initial heap allocation
        public  readonly UInt16 InitialStackAllocation;                 // Initial stack allocation
        public  readonly UInt32 InitialCSIPSetting;                     // Initial CS:IP setting
        public  readonly UInt32 InitialSSSPSetting;                     // Initial SS:SP setting
        public  readonly UInt16 SegmentCount;                           // Count of file segments
        public  readonly UInt16 ModuleReferenceTableEntriesCount;       // Entries in Module Reference Table
        public  readonly UInt16 NonResidentNameTableSize;               // Size of non-resident name table
        public  readonly UInt16 SegmentTableOffset;                     // Offset of Segment Table
        public  readonly UInt16 ResourceTableOffset;                    // Offset of Resource Table
        public  readonly UInt16 ResidentNameTableOffset;                // Offset of resident name table
        public  readonly UInt16 ModuleReferenceTableOffset;             // Offset of Module Reference Table
        public  readonly UInt16 ImportedNamesTableOffset;               // Offset of Imported Names Table
        public  readonly UInt32 NonResidentNamesTableOffset;            // Offset of Non-resident Names Table
        public  readonly UInt16 MovableEntriesCount;                    // Count of movable entries
        public  readonly UInt16 SegmentAlignmentShiftCount;             // Segment alignment shift count
        public  readonly UInt16 ResourceSegmentsCount;                  // Count of resource segments
        public  readonly NETargetOperatingSystem TargetOperatingSystem; // Target Operating system
        public  readonly NEImageOtherFlags OtherFlags;                  // Other .EXE flags
        public  readonly UInt16 ReturnThunksOffset;                     // offset to return thunks
        public  readonly UInt16 ne_psegrefbytes;                        // offset to segment ref. bytes
        public  readonly UInt16 MinimumCodeSwapAreaSize;                // Minimum code swap area size
        private readonly UInt16 ExpectedWindowsVersionNumber;           // Expected Windows version number
        }
    }