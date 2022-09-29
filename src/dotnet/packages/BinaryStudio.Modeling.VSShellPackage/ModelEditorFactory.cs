using System.Runtime.InteropServices;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    [Guid("e4722725-ccdb-466c-982b-33f0c87fd1e6")]
    public class ModelEditorFactory : EEditorFactory
        {
        /// <summary>Used by the editor factory architecture to create editors that support data/view separation.</summary>
        protected override EEditorWindow CreateEditorInstance()
            {
            return new ModelEditorWindow(GetType().GUID);
            }
        }
    }