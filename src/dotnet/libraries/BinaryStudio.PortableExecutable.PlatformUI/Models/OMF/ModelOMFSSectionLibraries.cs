using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionLibraries))]
    internal class ModelOMFSSectionLibraries : ModelOMFSSection<OMFSSectionLibraries>
        {
        public ObservableCollection<ModelOMFSSectionLibrary> Libraries { get; }
        public ModelOMFSSectionLibraries(OMFSSectionLibraries source)
            : base(source)
            {
            var order = 0;
            Libraries = new ObservableCollection<ModelOMFSSectionLibrary>(
                source.Libraries.Select(i => new ModelOMFSSectionLibrary(order++,i)));
            }
        }
    }