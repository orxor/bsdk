using System;
using System.Diagnostics;

namespace BinaryStudio.PortableExecutable
    {
    internal class TD32SSectionModule : OMFSSectionModule
        {
        public TD32SSectionModule(OMFDirectory Directory)
            : base(Directory)
            {
            }

        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            Analyze((TD32ModuleInfo*)Source);
            return this;
            }

        private unsafe void Analyze(TD32ModuleInfo* Source) {
            if (InitialValue != null) { throw new InvalidOperationException(); }
            LibraryIndex = Source->LibraryIndex;
            NameIndex = Source->NameIndex;
            OverlayNumber = Source->OverlayNumber;
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
            var SegmentInfo = (TD32SegInfo*)(Source + 1);
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
            InitialValue = ModuleInfo;
            }

        public override void ResolveReferences(OMFDirectory Directory)
            {
            if (InitialValue == null) { throw new InvalidOperationException(); }
            #if DEBUG
            Debug.Print("OverlayNumber:{0:x4} LibraryIndex:{1:x4} SegmentCount:{2} Name:{3:x4}:{4}",
                InitialValue.OverlayNumber,
                InitialValue.LibraryIndex,
                InitialValue.Segments.Count,
                InitialValue.NameIndex,
                Directory.Names[InitialValue.NameIndex - 1]);
            #endif
            }
        }
    }