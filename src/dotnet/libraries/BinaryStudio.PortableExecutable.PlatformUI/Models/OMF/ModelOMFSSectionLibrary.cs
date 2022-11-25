using System;
using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    internal class ModelOMFSSectionLibrary : NotifyPropertyChangedDispatcherObject<String>
        {
        public Int32 Order { get; }
        public ModelOMFSSectionLibrary(Int32 order, String source)
            : base(source)
            {
            Order = order;
            }
        }
    }