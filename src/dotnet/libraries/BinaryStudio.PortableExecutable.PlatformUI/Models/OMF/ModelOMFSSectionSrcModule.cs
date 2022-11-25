using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionSrcModule))]
    internal class ModelOMFSSectionSrcModule : ModelOMFSSection<OMFSSectionSrcModule>
        {
        public ModelOMFSSectionSrcModule(OMFSSectionSrcModule source)
            : base(source)
            {
            }
        }
    }