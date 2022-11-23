using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionModule))]
    internal class ModelOMFSSectionModule : ModelOMFSSection<OMFSSectionModule>
        {
        public ModelOMFSSectionModule(OMFSSectionModule source)
            : base(source)
            {
            }
        }
    }