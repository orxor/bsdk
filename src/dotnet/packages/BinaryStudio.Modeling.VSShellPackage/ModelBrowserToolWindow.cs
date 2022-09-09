using System;
using System.Runtime.InteropServices;
using System.Windows;
using BinaryStudio.Modeling.UnifiedModelingLanguage;
using Microsoft.VisualStudio.Shell;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    [Guid("57f64175-f95b-4e5a-b588-8c212db72594")]
    public class ModelBrowserToolWindow : ToolWindowPane
        {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBrowserToolWindow"/> class.
        /// </summary>
        public ModelBrowserToolWindow() : base(null)
            {
            Caption = "Model Browser";
            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            Content = new ModelBrowserControl();
            }
        }
    }