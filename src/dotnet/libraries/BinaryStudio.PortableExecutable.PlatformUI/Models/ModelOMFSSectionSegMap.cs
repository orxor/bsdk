using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionSegMap))]
    internal class ModelOMFSSectionSegMap : ModelOMFSSection<OMFSSectionSegMap>
        {
        public ModelOMFSSectionSegMap(OMFSSectionSegMap source)
            : base(source)
            {
            }
        }
    }