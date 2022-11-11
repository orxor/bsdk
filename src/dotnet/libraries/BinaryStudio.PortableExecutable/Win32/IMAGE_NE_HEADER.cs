using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_NE_HEADER
        {
        public  readonly UInt16 MagicNumber;                      // Magic number
        public  readonly Byte   VersionNumber;                    // Version number
        public  readonly Byte   RevisionNumber;                   // Revision number
        public  readonly UInt16 EntryTableOffset;                 // Offset of Entry Table
        public  readonly UInt16 EntryTableSize;                   // Number of bytes in Entry Table
        public  readonly UInt32 Checksum;                         // Checksum of whole file
        public  readonly UInt16 Flags;                            // Flag word
        public  readonly UInt16 AutomaticDataSegmentNumber;       // Automatic data segment number
        public  readonly UInt16 InitialHeapAllocation;            // Initial heap allocation
        public  readonly UInt16 InitialStackAllocation;           // Initial stack allocation
        public  readonly UInt32 InitialCSIPSetting;               // Initial CS:IP setting
        public  readonly UInt32 InitialSSSPSetting;               // Initial SS:SP setting
        public  readonly UInt16 SegmentCount;                     // Count of file segments
        public  readonly UInt16 ModuleReferenceTableEntriesCount; // Entries in Module Reference Table
        public  readonly UInt16 NonResidentNameTableSize;         // Size of non-resident name table
        public  readonly UInt16 SegmentTableOffset;               // Offset of Segment Table
        public  readonly UInt16 ResourceTableOffset;              // Offset of Resource Table
        public  readonly UInt16 ResidentNameTableOffset;          // Offset of resident name table
        public  readonly UInt16 ModuleReferenceTableOffset;       // Offset of Module Reference Table
        public  readonly UInt16 ImportedNamesTableOffset;         // Offset of Imported Names Table
        public  readonly UInt32 NonResidentNamesTableOffset;      // Offset of Non-resident Names Table
        public  readonly UInt16 MovableEntriesCount;              // Count of movable entries
        public  readonly UInt16 SegmentAlignmentShiftCount;       // Segment alignment shift count
        public  readonly UInt16 ResourceSegmentsCount;            // Count of resource segments
        public  readonly Byte   TargetOperatingsystem;            // Target Operating system
        public  readonly Byte   ne_flagsothers;                   // Other .EXE flags
        public  readonly UInt16 ReturnThunksOffset;               // offset to return thunks
        public  readonly UInt16 ne_psegrefbytes;                  // offset to segment ref. bytes
        public  readonly UInt16 MinimumCodeSwapAreaSize;          // Minimum code swap area size
        private readonly UInt16 ExpectedWindowsVersionNumber;     // Expected Windows version number
        }
    }