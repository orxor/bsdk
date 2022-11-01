using System;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.SrcLnSeg)]
    internal class OMFSSectionSrcLnSeg : OMFSSection
        {
        public OMFSSectionSrcLnSeg(OMFDirectory Directory)
            : base(Directory)
            {
            }
        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.SrcLnSeg; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            return this;
            }
        }
    }