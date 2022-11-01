using System;
using System.Collections.Generic;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.Module)]
    internal class OMFSSectionModule : OMFSSection
        {
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
        }
    }