using System;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.GlobalSym)]
    internal class OMFSSectionGlobalSym : OMFSSection
        {
        public OMFSSectionGlobalSym(OMFDirectory Directory)
            : base(Directory)
            {
            }

        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.GlobalSym; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            return this;
            }
        }
    }