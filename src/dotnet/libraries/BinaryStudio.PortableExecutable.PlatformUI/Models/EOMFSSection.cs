using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSection))]
    public class EOMFSSection : NotifyPropertyChangedDispatcherObject<OMFSSection>
        {
        public EOMFSSection(OMFSSection source)
            : base(source)
            {
            }
        }
    }