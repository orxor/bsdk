using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable;
using BinaryStudio.VSShellServices;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    [Guid("7c0314d7-b7eb-4a8d-b401-f1df1ced064e")]
    [ProvideEditorWindow(typeof(MetadataObjectEditorWindow))]
    public class MetadataObjectEditorFactory : EditorFactory
        {
        private static MetadataScope scope;
        public static MetadataScope Scope { get { return scope = scope ?? new MetadataScope(); }}
        }
    }