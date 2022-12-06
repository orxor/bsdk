using System.Collections;
using System.Linq;
using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionAlignSym))]
    internal class ModelOMFSSectionAlignSym : ModelOMFSSection<OMFSSectionAlignSym>
        {
        public IEnumerable Symbols { get; }
        public ModelOMFSSectionAlignSym(OMFSSectionAlignSym source)
            : base(source)
            {
            Symbols = source.Symbols.Select(Model.CreateModel).ToArray();
            }
        }
    }