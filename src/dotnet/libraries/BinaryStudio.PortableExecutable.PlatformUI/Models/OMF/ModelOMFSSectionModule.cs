using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionModule))]
    internal class ModelOMFSSectionModule : ModelOMFSSection<OMFSSectionModule>
        {
        public ObservableCollection<ModelOMFSegmentInfo> Segments { get; }
        public ModelOMFSSectionModule(OMFSSectionModule source)
            : base(source)
            {
            var order = 0;
            Segments = new ObservableCollection<ModelOMFSegmentInfo>(
                source.Segments.Select(i => new ModelOMFSegmentInfo(order++,i)));
            return;
            }
        }
    }