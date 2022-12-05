using BinaryStudio.PlatformUI.Models;
using BinaryStudio.PortableExecutable.CodeView;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    internal class ModelCodeViewSymbol<T> : NotifyPropertyChangedDispatcherObject<T>
        where T: CodeViewSymbol
        {
        public ModelCodeViewSymbol(T source)
            : base(source)
            {
            }
        }
    }