using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionGlobalTypes))]
    internal class ModelOMFSSectionGlobalTypes : ModelOMFSSection<OMFSSectionGlobalTypes>
        {
        public ModelOMFSSectionGlobalTypes(OMFSSectionGlobalTypes source)
            : base(source)
            {
            }
        }
    }