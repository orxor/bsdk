using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionGlobalSym))]
    internal class ModelOMFSSectionGlobalSym : ModelOMFSSection<OMFSSectionGlobalSym>
        {
        public ModelOMFSSectionGlobalSym(OMFSSectionGlobalSym source)
            : base(source)
            {
            }
        }
    }