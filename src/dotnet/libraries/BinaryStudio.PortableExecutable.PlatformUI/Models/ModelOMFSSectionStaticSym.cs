using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionStaticSym))]
    internal class ModelOMFSSectionStaticSym : ModelOMFSSection<OMFSSectionStaticSym>
        {
        public ModelOMFSSectionStaticSym(OMFSSectionStaticSym source)
            : base(source)
            {
            }
        }
    }