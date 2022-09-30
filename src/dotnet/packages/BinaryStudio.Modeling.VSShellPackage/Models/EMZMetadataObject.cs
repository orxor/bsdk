using BinaryStudio.PlatformUI.Models;
using BinaryStudio.PortableExecutable;

namespace BinaryStudio.Modeling.VSShellPackage.Models
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