using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace BinaryStudio.VSShellServices
    {
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    public class ToolPackage : AsyncPackage
        {
        #region M:InitializeAsync(CancellationToken,IProgress<ServiceProgressData>):Task
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress) {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            var ToolWindowId = 0;
            var CommandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            foreach (var type in GetType().Assembly.GetTypes().Where(i => !i.IsAbstract)) {
                foreach (var attribute in type.GetCustomAttributes(typeof(ToolWindowCommandAttribute), false).OfType<ToolWindowCommandAttribute>()) {
                    var ToolWindowType = type;
                    CommandService.AddCommand(new ToolMenuCommand(Execute, new CommandID(Guid.Parse(attribute.CommandSet), attribute.CommandId), ToolWindowType));
                    ToolWindowId++;
                    }
                }
            EditorWindow.RegisterModelTypes(GetType().Assembly);
            }
        #endregion
        #region M:CreateToolWindow(Type,Int32,Object):WindowPane
        /// <summary>
        /// Create a tool window of the specified type with the specified ID.
        /// </summary>
        /// <param name="toolWindowType">Type of the window to be created</param>
        /// <param name="id">Instance ID or 0 for single instance toolwindows</param>
        /// <param name="context">Tool window creation context (passed to <see cref="T:Microsoft.VisualStudio.Shell.ToolWindowPane" /> constructor)</param>
        /// <returns>An instance of a class derived from <see cref="T:Microsoft.VisualStudio.Shell.ToolWindowPane" /></returns>
        protected override WindowPane CreateToolWindow(Type toolWindowType, Int32 id, Object context)
            {
            if (toolWindowType == null) { throw new ArgumentNullException(nameof(toolWindowType)); }
            if (!toolWindowType.IsSubclassOf(typeof(WindowPane))) { throw new ArgumentOutOfRangeException(nameof(toolWindowType)); }
            if (id < 0) { throw new ArgumentOutOfRangeException(nameof(id)); }
            var r = base.CreateToolWindow(toolWindowType, id, context);
            if (r == null) {
                r = (WindowPane)Activator.CreateInstance(toolWindowType);
                }
            return r;
            }
        #endregion
        #region M:CreateToolWindow({ref}Guid,Int32):Int32
        /// <devdoc>
        /// Create a tool window of the specified type with the specified ID.
        /// </devdoc>
        /// <param name="toolWindowType">Type of the window to be created</param>
        /// <param name="id">Instance ID or 0 for single instance toolwindows</param>
        /// <returns>HRESULT for toolwindow creation</returns>
        protected override Int32 CreateToolWindow(ref Guid toolWindowType, Int32 id)
            {
            return base.CreateToolWindow(ref toolWindowType, id);
            }
        #endregion
        #region M:Execute(Object,EventArgs)
        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(Object sender, EventArgs e) {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (sender is ToolMenuCommand command) {
                var window = FindToolWindow(command.ToolWindowType, 0, true);
                if ((null != window) && (null != window.Frame)) {
                    var windowFrame = (IVsWindowFrame)window.Frame;
                    ErrorHandler.ThrowOnFailure(windowFrame.Show());
                    }
                }
            }
        #endregion
        #region M:UpdateMRUCommandsAsync(CancellationToken,IVsMRUItemsStore):Task
        protected async Task UpdateMRUCommandsAsync(CancellationToken cancellationToken,IVsMRUItemsStore MRUItemsStore) {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            if (MRUItemsStore != null) {
                var MruExtns = new Dictionary<String,Guid>();
                foreach (var attribute in GetType().GetCustomAttributes(typeof(ProvideEditorExtensionAttribute),false).OfType<ProvideEditorExtensionAttribute>()) {
                    MruExtns[attribute.Extension] = attribute.Factory;
                    }
                var MruFiles = new String[VSConstants.VSStd97CmdID.MRUFile25 - VSConstants.VSStd97CmdID.MRUFile1 + 1];
                var MruFilesGuid = VSConstants.MruList.Files;
                MRUItemsStore.GetMRUItems(ref MruFilesGuid,String.Empty,(UInt32)MruFiles.Length,MruFiles);
                for (var i = 0; i < MruFiles.Length; i++) {
                    if (MruFiles[i] != null) {
                        var Entries = MruFiles[i].Split(new []{ '|' }, StringSplitOptions.None);
                        if (Entries.Length >= 2) {
                            if (MruExtns.TryGetValue(Path.GetExtension(Entries[0]).ToLowerInvariant(), out var EditorFactoryGuid)) {
                                Entries[1] = EditorFactoryGuid.ToString("B");
                                }
                            MruFiles[i] = String.Join("|", Entries);
                            }
                        }
                    }
                MRUItemsStore.DeleteMRUItems(ref MruFilesGuid);
                foreach (var i in MruFiles.Where(i => i != null)) {
                    MRUItemsStore.AddMRUItem(ref MruFilesGuid, i);
                    }
                }
            }
        #endregion

        static ToolPackage()
            {
            }
        }
    }