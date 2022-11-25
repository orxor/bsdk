using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionAlignSym))]
    internal class ModelOMFSSectionAlignSym : ModelOMFSSection<OMFSSectionAlignSym>
        {
        public ModelOMFSSectionAlignSym(OMFSSectionAlignSym source)
            : base(source)
            {
            }
        }
    }