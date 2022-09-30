using System.Runtime.InteropServices;
using BinaryStudio.VSShellServices;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    [Guid("e4722725-ccdb-466c-982b-33f0c87fd1e6")]
    [ProvideEditorWindow(typeof(ModelEditorWindow))]
    public class ModelEditorFactory : EditorFactory
        {
        }
    }