using System;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.GlobalSym)]
    internal class OMFSSectionGlobalSym : OMFSSectionGlobalPub
        {
        public OMFSSectionGlobalSym(OMFDirectory Directory)
            : base(Directory)
            {
            }

        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.GlobalSym; }}
        }
    }