using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionSegName))]
    internal class ModelOMFSSectionSegName : ModelOMFSSection<OMFSSectionSegName>
        {
        public ModelOMFSSectionSegName(OMFSSectionSegName source)
            : base(source)
            {
            }
        }
    }