using System;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.StaticSym)]
    internal class OMFSSectionStaticSym : OMFSSectionGlobalPub
        {
        public OMFSSectionStaticSym(OMFDirectory Directory)
            : base(Directory)
            {
            }
        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.StaticSym; }}
        }
    }