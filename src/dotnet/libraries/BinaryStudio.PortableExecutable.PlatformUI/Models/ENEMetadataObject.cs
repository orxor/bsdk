using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(NEMetadataObject))]
    public class ENEMetadataObject : NotifyPropertyChangedDispatcherObject<NEMetadataObject>
        {
        public ENEMetadataObject(NEMetadataObject source)
            : base(source)
            {
            }
        }
    }