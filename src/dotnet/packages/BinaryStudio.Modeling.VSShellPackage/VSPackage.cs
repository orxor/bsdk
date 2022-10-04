using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using BinaryStudio.PlatformUI;
using BinaryStudio.PortableExecutable.PlatformUI.Models;
using BinaryStudio.VSShellServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuidString)]
    [ProvideToolWindow(typeof(ModelBrowserToolWindow))]
    [ProvideToolWindow(typeof(MetadataScopeBrowserToolWindow))]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideEditorExtension(typeof(ModelEditorFactory), ".emx", 32000, EditorFactoryNotify = true, ProjectGuid = "{9049afb9-2d13-4270-83ba-fbacf999114a}")]
    [ProvideEditorExtension(typeof(MetadataObjectEditorFactory), ".dll", 32000, EditorFactoryNotify = true, ProjectGuid = "{2f5851e8-7c17-485a-85e1-ac6757c3f277}")]
    [ProvideEditorExtension(typeof(MetadataObjectEditorFactory), ".ocx", 32000, EditorFactoryNotify = true, ProjectGuid = "{2f5851e8-7c17-485a-85e1-ac6757c3f277}")]
    [ProvideEditorExtension(typeof(MetadataObjectEditorFactory), ".exe", 32000, EditorFactoryNotify = true, ProjectGuid = "{2f5851e8-7c17-485a-85e1-ac6757c3f277}")]
    [ProvideEditorLogicalView(typeof(ModelEditorFactory), "{d2f13359-2c06-401b-a2f9-818d2b73a1e1}")]
    [ProvideEditorLogicalView(typeof(MetadataObjectEditorFactory), "{c6718f2b-c1e1-462f-86db-6cdd5c92dc81}")]
    public sealed class VSPackage : ToolPackage
        {
         /// <summary>
        /// BinaryStudio.Modeling.VSShellPackagePackage GUID string.
        /// </summary>
        public const String PackageGuidString = "e3bb358c-9091-4550-b9c6-2d7eae8ce554";

        #region M:InitializeAsync(CancellationToken,IProgress<ServiceProgressData>):Task
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress) {
            await base.InitializeAsync(cancellationToken,progress);
            RegisterEditorFactory(new ModelEditorFactory());
            RegisterEditorFactory(new MetadataObjectEditorFactory());
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await UpdateMRUCommandsAsync(cancellationToken,GetGlobalService(typeof(SVsMRUItemsStore)) as IVsMRUItemsStore);
            EditorWindow.RegisterModelTypes(typeof(EMZMetadataObject).Assembly);
            Theme.Apply(Theme.Themes[3]);
            }
        #endregion

        public VSPackage()
            {
            return;
            }
        }
    }
