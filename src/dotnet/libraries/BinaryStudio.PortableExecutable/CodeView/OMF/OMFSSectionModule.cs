using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.Module)]
    internal class OMFSSectionModule : OMFSSection
        {
        public Int16 OverlayNumber { get;protected set; }
        public Int16 LibraryIndex { get;protected set; }
        protected String Name { get;set; }

        protected class SegmentInfo
            {
            public Int16 Segment; // Segment that this structure describes
            public Int16 Flags;   // Attributes for the logical segment.
            public Int32 Offset;  // Offset in segment where the code starts
            public Int32 Size;    // Count of the number of bytes of code in the segment
            }

        protected class ModuleInfo
            {
            public Int16 OverlayNumber;  // Overlay number
            public Int16 LibraryIndex;   // Index into sstLibraries subsection if this module was linked from a library
            public Int16 DebuggingStyle; // Debugging style  for this  module.
            public Int32 NameIndex;
            public String Name;
            public readonly IList<SegmentInfo> Segments = new List<SegmentInfo>();
            public override String ToString()
                {
                return Name;
                }
            }

        protected ModuleInfo InitialValue;

        public OMFSSectionModule(OMFDirectory Directory)
            : base(Directory)
            {
            }
        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.Module; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            Analyze((OMFModuleInfo*)Source);
            return this;
            }

        private unsafe void Analyze(OMFModuleInfo* Source) {
            if (InitialValue != null) { throw new InvalidOperationException(); }
            LibraryIndex = Source->LibraryIndex;
            OverlayNumber = Source->OverlayNumber;
            var ModuleInfo = new ModuleInfo {
                DebuggingStyle = Source->DebuggingStyle,
                LibraryIndex   = Source->LibraryIndex,
                OverlayNumber  = Source->OverlayNumber
                };
            var SegmentInfo = (OMFSegInfo*)(Source + 1);
            for (var i = 0; i < Source->SegmentCount; i++) {
                ModuleInfo.Segments.Add(new SegmentInfo{
                    Offset  = SegmentInfo->Offset,
                    Size    = SegmentInfo->Size,
                    Flags   = SegmentInfo->Flags,
                    Segment = SegmentInfo->Segment
                    });
                SegmentInfo++;
                }
            Name = ToString(Encoding.ASCII, (Byte*)(SegmentInfo), true);
            ModuleInfo.Name = Name;
            InitialValue = ModuleInfo;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            if (Writer == null) { throw new ArgumentNullException(nameof(Writer)); }
            Writer.WriteLine("{0}OverlayNumber:{{{1}}} LibraryIndex:{{{2}}} SegmentCount:{4} Name:{{{3}}}",
                LinePrefix,OverlayNumber.ToString("x4"),LibraryIndex.ToString("x4"),
                Name,InitialValue.Segments.Count.ToString("x4"));
            foreach (var Segment in InitialValue.Segments) {
                Writer.WriteLine("  {0}{{{1}}}:{2}-{3}",
                    LinePrefix,Segment.Segment.ToString("x4"),
                    Segment.Offset.ToString("x8"),(Segment.Offset+Segment.Size - 1).ToString("x8"));
                }
            }
        }
    }