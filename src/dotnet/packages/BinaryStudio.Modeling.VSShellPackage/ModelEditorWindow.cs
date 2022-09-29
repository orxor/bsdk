using System;
using BinaryStudio.Modeling.VSShellPackage.Models;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Module = BinaryStudio.Modeling.UnifiedModelingLanguage.Module;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    public class ModelEditorWindow : EEditorWindow
        {
        protected override Int32 FormatIndex { get { return 0; }}
        protected override String FormatList { get { return "Model Files(*.emx)\n*.emx"; }}

        public ModelEditorWindow(Guid FactoryClassId)
            : base(FactoryClassId)
            {
            }

        #region M:LoadDataContext:Object
        protected override Object LoadDataContext()
            {
            return (new Module()).LoadModel(FileName);
            }
        #endregion
        #region M:Load(String,UInt32,Int32):Int32
        /// <summary>Opens a specified file and initializes an object from the file contents.</summary>
        /// <param name="filename">[in] Pointer to the name of the file to load, which, for an existing file, should always include the full path.</param>
        /// <param name="Mode">[in] File format mode. If zero, the object uses the usual defaults as if the user had opened the file.</param>
        /// <param name="ReadOnly">[in] <see langword="true"/> indicates that the file should be opened as read-only.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.</returns>
        public override Int32 Load(String filename, UInt32 Mode, Int32 ReadOnly) {
            ThreadHelper.ThrowIfNotOnUIThread();
            var r = base.Load(filename, Mode, ReadOnly);
            if (r != VSConstants.S_OK)
                {
                ModelBrowserToolWindowCommand.Instance.DataContext = new BSModelCollection { ModelContext ?? DataContext };
                }
            return r;
            }
        #endregion
        }
    }