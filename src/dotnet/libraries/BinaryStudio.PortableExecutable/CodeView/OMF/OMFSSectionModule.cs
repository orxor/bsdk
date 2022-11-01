using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.Module)]
    internal class OMFSSectionModule : OMFSSection
        {
        public Int16 OverlayNumber { get;protected set; }
        public Int16 LibraryIndex { get;protected set; }
        protected Int32 NameIndex { get;set; }

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
            return this;
            }

        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            if (Writer == null) { throw new ArgumentNullException(nameof(Writer)); }
            Writer.WriteLine("{0}OverlayNumber:{{{1}}} LibraryIndex:{{{2}}} SegmentCount:{5} Name:{{{3}}}:{{{4}}}",
                LinePrefix,OverlayNumber.ToString("x4"),LibraryIndex.ToString("x4"),
                NameIndex.ToString("x8"),Directory.Names[NameIndex-1],
                InitialValue.Segments.Count.ToString("x4"));
            foreach (var Segment in InitialValue.Segments) {
                Writer.WriteLine("  {0}{{{1}}}:{2}-{3}",
                    LinePrefix,Segment.Segment.ToString("x4"),
                    Segment.Offset.ToString("x8"),(Segment.Offset+Segment.Size).ToString("x8"));
                }
            }
        }
    }