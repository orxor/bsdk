using System.Runtime.InteropServices;
using BinaryStudio.VSShellServices;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    [Guid("e4722725-ccdb-466c-982b-33f0c87fd1e6")]
    public class ModelEditorFactory : EditorFactory
        {
        /// <summary>Used by the editor factory architecture to create editors that support data/view separation.</summary>
        protected override EditorWindow CreateEditorInstance()
            {
            return new ModelEditorWindow(GetType().GUID);
            }
        }
    }