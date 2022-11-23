using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionLibraries))]
    internal class ModelOMFSSectionLibraries : ModelOMFSSection<OMFSSectionLibraries>
        {
        public ModelOMFSSectionLibraries(OMFSSectionLibraries source)
            : base(source)
            {
            }
        }
    }