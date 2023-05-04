using System;
using System.Diagnostics;
using System.IO;

namespace BinaryStudio.PortableExecutable
    {
    internal class TD32SSectionModule : OMFSSectionModule
        {
        protected Int32 NameIndex { get;set; }
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
                Segments.Add(new OMFSegmentInfo(
                    SegmentInfo->Segment,
                    SegmentInfo->Offset,
                    SegmentInfo->Size,
                    SegmentInfo->Flags));
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
                Segments.Count,
                InitialValue.NameIndex,
                Directory.Names[InitialValue.NameIndex - 1]);
            #endif
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            if (Writer == null) { throw new ArgumentNullException(nameof(Writer)); }
            Writer.WriteLine("{0}OverlayNumber:{{{1}}} LibraryIndex:{{{2}}} SegmentCount:{5} Name:{{{3}}}:{{{4}}}",
                LinePrefix,OverlayNumber.ToString("x4"),LibraryIndex.ToString("x4"),
                NameIndex.ToString("x8"),Directory.Names[NameIndex-1],
                Segments.Count.ToString("x4"));
            foreach (var Segment in Segments) {
                Writer.WriteLine("  {0}{{{1}}}:{2}-{3}",
                    LinePrefix,Segment.Index.ToString("x4"),
                    Segment.Offset.ToString("x8"),(Segment.Offset+Segment.Size).ToString("x8"));
                }
            }
        }
    }