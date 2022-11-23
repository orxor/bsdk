using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(NEMetadataObject))]
    public class ENEMetadataObject : NotifyPropertyChangedDispatcherObject<NEMetadataObject>
        {
        public ObservableCollection<EOMFSSection> DebugSections { get; }
        public ENEMetadataObject(NEMetadataObject source)
            : base(source)
            {
            DebugSections = new ObservableCollection<EOMFSSection>(source.DebugDirectory.Sections.Select(i => new EOMFSSection(i)));
            }
        }
    }