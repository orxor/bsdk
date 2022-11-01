using System;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.SegName)]
    internal class OMFSSectionSegName : OMFSSection
        {
        public OMFSSectionSegName(OMFDirectory Directory)
            : base(Directory)
            {
            }
        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.SegName; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            return this;
            }
        }
    }