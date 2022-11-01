using System;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.MPC)]
    internal class OMFSSectionMPC : OMFSSection
        {
        public OMFSSectionMPC(OMFDirectory Directory)
            : base(Directory)
            {
            }

        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.MPC; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            return this;
            }
        }
    }