using System;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.GlobalPub)]
    internal class OMFSSectionGlobalPub : OMFSSection
        {
        public OMFSSectionGlobalPub(OMFDirectory Directory)
            : base(Directory)
            {
            }

        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.GlobalPub; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            return this;
            }
        }
    }