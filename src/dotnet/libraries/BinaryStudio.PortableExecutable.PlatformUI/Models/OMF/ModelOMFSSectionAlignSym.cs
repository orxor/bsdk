using System.Collections;
using System.Linq;
using BinaryStudio.PlatformUI.Models;
using BinaryStudio.PortableExecutable.CodeView;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionAlignSym))]
    internal class ModelOMFSSectionAlignSym : ModelOMFSSection<OMFSSectionAlignSym>
        {
        public IEnumerable Symbols { get; }
        public ModelOMFSSectionAlignSym(OMFSSectionAlignSym source)
            : base(source)
            {
            //Symbols = source.Symbols.Where(i => i is S_COMPILE).ToArray();
            //Symbols = source.Symbols.Where(i => i is S_SSEARCH).ToArray();
            Symbols = source.Symbols.Select(Model.CreateModel).ToArray();
            }
        }
    }