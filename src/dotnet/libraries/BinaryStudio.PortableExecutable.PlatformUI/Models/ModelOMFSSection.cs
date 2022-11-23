using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    internal interface IModelOMFSSection
        {
        
        }

    [Model(typeof(OMFSSection))]
    internal class ModelOMFSSection : ModelOMFSSection<OMFSSection>
        {
        public ModelOMFSSection(OMFSSection source)
            : base(source)
            {
            }
        }

    internal class ModelOMFSSection<T> : NotifyPropertyChangedDispatcherObject<T>,IModelOMFSSection
        {
        protected ModelOMFSSection(T source)
            : base(source)
            {
            }
        }
    }