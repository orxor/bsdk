using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    [Guid("b48b966f-ca8f-4f44-9c4e-495aa8993905")]
    public class MetadataScopeBrowserToolWindow : ToolWindowPane
        {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBrowserToolWindow"/> class.
        /// </summary>
        public MetadataScopeBrowserToolWindow() : base(null)
            {
            Caption = "Metadata Browser";
            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            }
        }
    }