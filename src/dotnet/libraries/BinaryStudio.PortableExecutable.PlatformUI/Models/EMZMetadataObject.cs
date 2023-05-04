using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(MZMetadataObject))]
    internal class EMZMetadataObject : NotifyPropertyChangedDispatcherObject<MZMetadataObject>
        {
        public ENEMetadataObject NEMetadataObject { get; }
        public EMZMetadataObject(MZMetadataObject source)
            : base(source)
            {
            NEMetadataObject = CreateModel(source.GetService(typeof(NEMetadataObject))) as ENEMetadataObject;
            }
        }
    }
