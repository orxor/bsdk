using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(NEMetadataObject))]
    internal class ENEMetadataObject : NotifyPropertyChangedDispatcherObject<NEMetadataObject>
        {
        public ObservableCollection<IModelOMFSSection> DebugSections { get; }
        public ENEMetadataObject(NEMetadataObject source)
            : base(source)
            {
            DebugSections = new ObservableCollection<IModelOMFSSection>(
                source.DebugDirectory.Sections.
                Where(i => i.SectionIndex != OMFSSectionIndex.AlignSym).
                Where(i => i.SectionIndex != OMFSSectionIndex.Module).
                Where(i => i.SectionIndex != OMFSSectionIndex.SrcModule).
                Select(i => (IModelOMFSSection)CreateModel(i)));
            }
        }
    }