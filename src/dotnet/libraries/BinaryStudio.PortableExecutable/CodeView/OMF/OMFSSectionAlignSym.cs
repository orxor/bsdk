using System;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.AlignSym)]
    internal class OMFSSectionAlignSym : OMFSSection
        {
        public OMFSSectionAlignSym(OMFDirectory Directory)
            : base(Directory)
            {
            }

        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.AlignSym; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            return this;
            }
        }
    }