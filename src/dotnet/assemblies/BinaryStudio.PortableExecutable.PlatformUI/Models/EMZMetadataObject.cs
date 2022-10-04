using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(MZMetadataObject))]
    public class EMZMetadataObject : NotifyPropertyChangedDispatcherObject<MZMetadataObject>
        {
        public EMZMetadataObject(MZMetadataObject source)
            : base(source)
            {
            }
        }
    }
