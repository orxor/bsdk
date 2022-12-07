using System.Collections;
using System.Linq;
using BinaryStudio.PlatformUI.Models;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSSectionAlignSym))]
    internal class ModelOMFSSectionAlignSym : ModelOMFSSection<OMFSSectionAlignSym>
        {
        public IEnumerable Symbols { get; }
        public ModelOMFSSectionAlignSym(OMFSSectionAlignSym source)
            : base(source)
            {
            Symbols = source.Symbols.
                //Where(i => i.Type == DEBUG_SYMBOL_INDEX.S_SSEARCH).
                Select(Model.CreateModel).ToArray();
            }
        }
    }