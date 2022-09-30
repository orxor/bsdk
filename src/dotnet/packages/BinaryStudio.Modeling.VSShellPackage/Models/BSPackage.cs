using System.Collections.Generic;
using System.Linq;
using BinaryStudio.Modeling.UnifiedModelingLanguage;
using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.Modeling.VSShellPackage.Models
    {
    public abstract class BSPackageBase<T>: NotifyPropertyChangedDispatcherObject<T>
        where T:Package
        {
        protected BSPackageBase(T source)
            : base(source)
            {
            NestedPackages = source.NestedPackage.Select(i => new BSPackage(i)).ToArray();
            }

        public IList<BSPackage> NestedPackages { get; }
        }

    [Model(typeof(Package))]
    public class BSPackage : BSPackageBase<Package>
        {
        public BSPackage(Package source)
            : base(source)
            {
            }
        }
    }