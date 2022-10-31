using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct OffsetPair
            {
            public readonly Int32 StartOffset;
            public readonly Int32 EndOffset;
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
        #region M:LoadDirectory(IntPtr,IntPtr,IMAGE_DEBUG_DIRECTORY)
        protected override unsafe void LoadDirectory(Byte* BaseAddress,Byte* VirtualAddress, IMAGE_DEBUG_DIRECTORY* ImageDebugDirectory) {
            if (ImageDebugDirectory == null) { throw new ArgumentNullException(nameof(ImageDebugDirectory)); }
            if (VirtualAddress == null) { throw new ArgumentNullException(nameof(VirtualAddress)); }
            var BegOfDebugData = VirtualAddress + ImageDebugDirectory->AddressOfRawData;
            var Signature = (TD32FileSignature*)BegOfDebugData;
            if (!IsTD32(Signature)) { throw new ArgumentOutOfRangeException(nameof(ImageDebugDirectory)); }
            var Header = (TD32DirectoryHeader*)(BegOfDebugData + Signature->Offset);
            var Entry = (TD32DirectoryEntry*)(Header + 1);
            var Modules = new List<ModuleInfo>();
            var Names = new List<String>();
            #if DEBUGG
            for (var i = 0; i < Header->DirEntryCount; i++) {
                Debug.Print("ModuleIndex:{0:x4} Offset:{1:x8} FileOffset:{2:x8} Size:{3:x8} Type:{4}",
                    (Entry + i)->ModuleIndex,
                    (Entry + i)->Offset, BegOfDebugData + (Entry + i)->Offset - BaseAddress,
                    (Entry + i)->Size,
                    (Entry + i)->SubsectionType);
                }
            #endif
            for (var i = 0; i < Header->DirEntryCount; i++) {
                LoadDirectoryEntry(BaseAddress,BegOfDebugData,Entry,Modules,Names);
                Entry++;
                }

            foreach (var Module in Modules) {
                Module.Name = Names[Module.NameIndex - 1];
                }
            return;
            }
        #endregion
        private unsafe void LoadDirectoryEntry(Byte* BaseAddress,Byte* VirtualAddress,TD32DirectoryEntry* Entry,
            IList<ModuleInfo> Modules, IList<String> Names) {
            if (Entry == null) { throw new ArgumentNullException(nameof(Entry)); }
            if (VirtualAddress == null) { throw new ArgumentNullException(nameof(VirtualAddress)); }
            var EntryData = VirtualAddress + Entry->Offset;
            switch (Entry->SubsectionType) {
                case TD32SubsectionType.SUBSECTION_TYPE_MODULE:        { LoadModule(BaseAddress,(TD32ModuleInfo*)EntryData,Entry->Size,Modules); } break;
                case TD32SubsectionType.SUBSECTION_TYPE_ALIGN_SYMBOLS: { LoadAlignSymbols(BaseAddress,(TD32SymbolInfoList*)EntryData,Entry->Size); } break;
                case TD32SubsectionType.SUBSECTION_TYPE_SOURCE_MODULE: { LoadSourceModule(BaseAddress,(TD32SourceModuleInfo*)EntryData,Entry->Size); } break;
                case TD32SubsectionType.SUBSECTION_TYPE_GLOBAL_TYPES:  { LoadGlobalTypes(BaseAddress,(TD32GlobalTypeInfo*)EntryData,Entry->Size); } break;
                case TD32SubsectionType.SUBSECTION_TYPE_NAMES:         { LoadNames(BaseAddress,EntryData,Entry->Size,Names); } break;
                case TD32SubsectionType.SUBSECTION_TYPE_TYPES:
                case TD32SubsectionType.SUBSECTION_TYPE_SYMBOLS:
                case TD32SubsectionType.SUBSECTION_TYPE_GLOBAL_SYMBOLS: break;
                default: throw new ArgumentOutOfRangeException();
                }
            return;
            }

        #region M:LoadNames(IntPtr,IntPtr,Int32,IList<String>)
        private unsafe void LoadNames(Byte* BaseAddress, Byte* EntryData, Int32 Size,IList<String> Names) {
            var Count = ReadInt32(ref EntryData);
            for (var i = 0; i < Count; i++) {
                ReadByte(ref EntryData);
                Names.Add(ReadZeroTerminatedString(ref EntryData, Encoding.ASCII));
                }
            }
        #endregion
        #region M:LoadGlobalTypes(IntPtr,TD32GlobalTypeInfo,Size)
        private unsafe void LoadGlobalTypes(Byte* BaseAddress, TD32GlobalTypeInfo* Source, Int32 Size) {
            var Offsets = (Int32*)(Source + 1);
            Debug.Print("TypeCount:{0:x4}", Source->TypeCount);
            for (var i = 0; i < Source->TypeCount;i++) {
                var SymbolTypeInfo = (TD32SymbolTypeInfo*)((Byte*)Source + Offsets[i]);
                #if TD32DEBUG
                Debug.Print("FileOffset:{2:x8} Offset:{0:x8} Size:{1:x4} Type:{4:x4} Leaf:{3}",
                    Offsets[i],SymbolTypeInfo->Size,
                    (Byte*)SymbolTypeInfo - BaseAddress,
                    SymbolTypeInfo->Leaf,
                    i + 0x1000);
                #endif
                }
            return;
            }
        #endregion
        #region M:LoadAlignSymbols(IntPtr,TD32SymbolInfoList,Int32)
        private unsafe void LoadAlignSymbols(Byte* BaseAddress,TD32SymbolInfoList* Source, Int32 Size) {
            var SymbolList = (TD32SymbolInfo*)(Source + 1);
            Size -= sizeof(TD32SymbolInfo);
            while (Size > 0) {
                var Offset = (Byte*)SymbolList - BaseAddress;
                #if TD32DEBUG
                Debug.Print("FileOffset:{0:x8} Size:{1:x4} SymbolType:{2}",
                    Offset,SymbolList->Size,
                    SymbolList->SymbolType);
                #endif
                Size -= SymbolList->Size + 2;
                SymbolList = (TD32SymbolInfo*)((Byte*)SymbolList + SymbolList->Size + 2);
                }
            }
        #endregion
        #region M:LoadSourceModule(IntPtr,TD32SourceModuleInfo,Int32)
        private unsafe void LoadSourceModule(Byte* BaseAddress,TD32SourceModuleInfo* Source, Int32 Size) {
            var BaseSrcFiles = (Int32*)(Source + 1);
            var SegmentAdrss = (OffsetPair*)(BaseSrcFiles + Source->FileCount);
            var SegmentIndex = (Int16*)(SegmentAdrss + Source->SegmentCount);
            #if TD32DEBUG
            Debug.Print("FileCount:{0:x4} SegmentCount:{1:x4}",
                Source->FileCount,
                Source->SegmentCount);
            #endif
            if (Source->SegmentCount > 0) {
                #if TD32DEBUG
                Debug.Print("  Segments:");
                #endif
                for (var i = 0; i < Source->SegmentCount; i++) {
                    #if TD32DEBUG
                    Debug.Print("    {0:x4}:{1:x8}-{2:x8}",
                        SegmentIndex[i],
                        SegmentAdrss[i].StartOffset,
                        SegmentAdrss[i].EndOffset);
                    #endif
                    }
                }
            if (Source->FileCount > 0) {
                #if TD32DEBUG
                Debug.Print("  Files:");
                #endif
                for (var i = 0; i < Source->FileCount; i++) {
                    var SrcFile = (TD32SourceFileEntry*)((Byte*)Source + BaseSrcFiles[i]);
                    var SrcFileBaseSrcFiles = (Int32*)(SrcFile + 1);
                    #if TD32DEBUG
                    Debug.Print("    Offset:{0:x8} FileOffset:{1:x8} FileNameIndex:{2:x8}",
                        BaseSrcFiles[i],
                        (Byte*)Source - BaseAddress + BaseSrcFiles[i],
                        SrcFile->NameIndex);
                    #endif
                    for (var j = 0; j < SrcFile->SegmentCount; j++) {
                        var LineEntry = (TD32LineMappingEntry*)((Byte*)Source + SrcFileBaseSrcFiles[j]);
                        var LineEntryOffsets = (Int32*)(LineEntry + 1);
                        var LineEntryLnNmbrs = (Int16*)(LineEntryOffsets + LineEntry->PairCount);
                        #if TD32DEBUG
                        var DebugString = new StringBuilder("      Line numbers:\n     ");
                        #endif
                        for (var l = 0; l < LineEntry->PairCount; l++) {
                            #if TD32DEBUG
                            if ((l % 4 == 0) && (l > 0)) {
                                DebugString.AppendLine();
                                DebugString.Append("     ");
                                }
                            DebugString.AppendFormat(" {0:d5}:{1:x8}",
                                LineEntryLnNmbrs[l],
                                LineEntryOffsets[l]);
                            #endif
                            }
                        #if TD32DEBUG
                        Debug.Print(DebugString.ToString());
                        #endif
                        }
                    }
                }
            return;
            }
        #endregion
        #region M:LoadModule(IntPtr,TD32ModuleInfo,Int32,IList<ModuleInfo>)
        private unsafe void LoadModule(Byte* BaseAddress,TD32ModuleInfo* Source, Int32 Size, IList<ModuleInfo> Modules) {
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            var ModuleInfo = new ModuleInfo {
                DebuggingStyle = Source->DebuggingStyle,
                LibraryIndex   = Source->LibraryIndex,
                NameIndex      = Source->NameIndex,
                OverlayNumber  = Source->OverlayNumber
                };
            #if TD32DEBUG
            Debug.Print("OverlayNumber:{0:x4} LibraryIndex:{1:x4} SegmentCount:{2}",
                Source->OverlayNumber,
                Source->LibraryIndex,
                Source->SegmentCount);
            #endif
            var SegmentInfo = (TD32SegmentInfo*)(Source + 1);
            for (var i = 0; i < Source->SegmentCount; i++) {
                ModuleInfo.Segments.Add(new SegmentInfo{
                    Offset  = SegmentInfo->Offset,
                    Size    = SegmentInfo->Size,
                    Flags   = SegmentInfo->Flags,
                    Segment = SegmentInfo->Segment
                    });
                #if TD32DEBUG
                Debug.Print("  {0:x4}:{1:x8}-{2:x8} Flags:{3:x4}",
                    SegmentInfo->Segment,
                    SegmentInfo->Offset,
                    SegmentInfo->Offset+SegmentInfo->Size-1,
                    SegmentInfo->Flags);
                #endif
                SegmentInfo++;
                }
            Modules.Add(ModuleInfo);
            }
        #endregion
        }
    }