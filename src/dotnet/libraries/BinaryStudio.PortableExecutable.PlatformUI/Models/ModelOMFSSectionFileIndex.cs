using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionFileIndex))]
    internal class ModelOMFSSectionFileIndex : ModelOMFSSection<OMFSSectionFileIndex>
        {
        public ModelOMFSSectionFileIndex(OMFSSectionFileIndex source)
            : base(source)
            {
            }
        }
    }