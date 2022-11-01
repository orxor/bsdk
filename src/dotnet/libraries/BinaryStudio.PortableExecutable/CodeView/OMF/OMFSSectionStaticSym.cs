using System;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.StaticSym)]
    internal class OMFSSectionStaticSym : OMFSSection
        {
        public OMFSSectionStaticSym(OMFDirectory Directory)
            : base(Directory)
            {
            }
        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.StaticSym; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            return this;
            }
        }
    }