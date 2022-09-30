using BinaryStudio.Modeling.UnifiedModelingLanguage;
using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.Modeling.VSShellPackage.Models
    {
    [Model(typeof(Model))]
    public class BSModel : BSPackageBase<Model>
        {
        public BSModel(Model source)
            : base(source)
            {
            IsExpanded = true;
            }
        }
    }