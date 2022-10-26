using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable
    {
    using static COFFDataOperations;
    public class TD32DebugDirectoryLoader : COFFDebugDirectoryLoader
        {
        private const Int32 Borland32BitSymbolFileSignatureForDelphi = 0x39304246; // 'FB09'
        private const Int32 Borland32BitSymbolFileSignatureForBCB    = 0x41304246; // 'FB0A'

        private class SegmentInfo
            {
            public Int16 Segment; // Segment that this structure describes
            public Int16 Flags;   // Attributes for the logical segment.
            public Int32 Offset;  // Offset in segment where the code starts
            public Int32 Size;    // Count of the number of bytes of code in the segment
            }

        private class ModuleInfo
            {
            public Int16 OverlayNumber;  // Overlay number
            public Int16 LibraryIndex;   // Index into sstLibraries subsection if this module was linked from a library
            public Int16 DebuggingStyle; // Debugging style  for this  module.
            public Int32 NameIndex;      // Name index of module.
            public String Name;
            public readonly IList<SegmentInfo> Segments = new List<SegmentInfo>();
            public override String ToString()
                {
                return String.IsNullOrEmpty(Name)
                    ? $"NameIndex:{NameIndex}"
                    : Name;
                }
            }

        #region M:IsTD32(TD32FileSignature):Boolean
        private static unsafe Boolean IsTD32(TD32FileSignature* Signature) {
            return (Signature->Signature == Borland32BitSymbolFileSignatureForDelphi) ||
                   (Signature->Signature == Borland32BitSymbolFileSignatureForBCB);
            }
        #endregion
        #region M:IsTD32(IntPtr,IMAGE_DEBUG_DIRECTORY):Boolean
        public static unsafe Boolean IsTD32(Byte* VirtualAddress,IMAGE_DEBUG_DIRECTORY* ImageDebugDirectory) {
            var BegOfDebugData = VirtualAddress + ImageDebugDirectory->AddressOfRawData;
            var EndOfDebugData = BegOfDebugData + ImageDebugDirectory->SizeOfData;
            var Signature = (TD32FileSignature*)(EndOfDebugData - sizeof(TD32FileSignature));
            if (IsTD32(Signature)) {
                Signature = (TD32FileSignature*)(EndOfDebugData - Signature->Offset);
                return IsTD32(Signature);
                }
            return false;
            }
        #endregion
        #region M:LoadDirectory(IntPtr,IMAGE_DEBUG_DIRECTORY)
        protected override unsafe void LoadDirectory(Byte* VirtualAddress, IMAGE_DEBUG_DIRECTORY* ImageDebugDirectory) {
            if (ImageDebugDirectory == null) { throw new ArgumentNullException(nameof(ImageDebugDirectory)); }
            if (VirtualAddress == null) { throw new ArgumentNullException(nameof(VirtualAddress)); }
            var BegOfDebugData = VirtualAddress + ImageDebugDirectory->AddressOfRawData;
            var Signature = (TD32FileSignature*)BegOfDebugData;
            if (!IsTD32(Signature)) { throw new ArgumentOutOfRangeException(nameof(ImageDebugDirectory)); }
            var Header = (TD32DirectoryHeader*)(BegOfDebugData + Signature->Offset);
            var Entry = (TD32DirectoryEntry*)(Header + 1);
            var Modules = new List<ModuleInfo>();
            var Names = new List<String>();
            for (var i = 0; i < Header->DirEntryCount; i++) {
                LoadDirectoryEntry(BegOfDebugData,Entry,Modules,Names);
                Entry++;
                }

            foreach (var Module in Modules) {
                Module.Name = Names[Module.NameIndex - 1];
                }
            return;
            }
        #endregion
        private unsafe void LoadDirectoryEntry(Byte* VirtualAddress,TD32DirectoryEntry* Entry, IList<ModuleInfo> Modules, IList<String> Names) {
            if (Entry == null) { throw new ArgumentNullException(nameof(Entry)); }
            if (VirtualAddress == null) { throw new ArgumentNullException(nameof(VirtualAddress)); }
            var EntryData = VirtualAddress + Entry->Offset;
            switch (Entry->SubsectionType) {
                case TD32SubsectionType.SUBSECTION_TYPE_MODULE:
                    {
                    LoadModule((TD32ModuleInfo*)EntryData,Entry->Size,out var Module);
                    Modules.Add(Module);
                    }
                    break;
                case TD32SubsectionType.SUBSECTION_TYPE_TYPES: break;
                case TD32SubsectionType.SUBSECTION_TYPE_SYMBOLS: break;
                case TD32SubsectionType.SUBSECTION_TYPE_ALIGN_SYMBOLS: break;
                case TD32SubsectionType.SUBSECTION_TYPE_SOURCE_MODULE: break;
                case TD32SubsectionType.SUBSECTION_TYPE_GLOBAL_SYMBOLS: break;
                case TD32SubsectionType.SUBSECTION_TYPE_GLOBAL_TYPES: break;
                case TD32SubsectionType.SUBSECTION_TYPE_NAMES:
                    {
                    var Count = ReadInt32(ref EntryData);
                    for (var i = 0; i < Count; i++) {
                        ReadByte(ref EntryData);
                        Names.Add(ReadZeroTerminatedString(ref EntryData, Encoding.ASCII));
                        }
                    }
                    break;
                default: throw new ArgumentOutOfRangeException();
                }
            return;
            }

        private unsafe void LoadModule(TD32ModuleInfo* Source, Int32 Size, out ModuleInfo Target) {
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            Target = new ModuleInfo {
                DebuggingStyle = Source->DebuggingStyle,
                LibraryIndex   = Source->LibraryIndex,
                NameIndex      = Source->NameIndex,
                OverlayNumber  = Source->OverlayNumber
                };
            var SegmentInfo = (TD32SegmentInfo*)(Source + 1);
            for (var i = 0; i < Source->SegmentCount; i++) {
                Target.Segments.Add(new SegmentInfo{
                    Offset  = SegmentInfo->Offset,
                    Size    = SegmentInfo->Size,
                    Flags   = SegmentInfo->Flags,
                    Segment = SegmentInfo->Segment
                    });
                SegmentInfo++;
                }
            }
        }
    }