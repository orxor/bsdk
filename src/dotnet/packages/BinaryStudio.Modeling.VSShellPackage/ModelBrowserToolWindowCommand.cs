using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using BinaryStudio.Modeling.UnifiedModelingLanguage;
using Microsoft.VisualStudio;
using Action = System.Action;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Task = System.Threading.Tasks.Task;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ModelBrowserToolWindowCommand
        {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const Int32 CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("fdc929a3-8dfd-4def-b927-43ab6a8b7f8b");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;
        private Object DataContextProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBrowserToolWindowCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ModelBrowserToolWindowCommand(AsyncPackage package, OleMenuCommandService commandService)
            {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            Dispatcher = Dispatcher.CurrentDispatcher;

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
            }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ModelBrowserToolWindowCommand Instance { get;private set; }
        public Dispatcher Dispatcher { get; }
        public Boolean InvokeRequired { get { return Dispatcher.Thread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId; }}

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IAsyncServiceProvider ServiceProvider { get {
            return this.package;
            }}

        public Object DataContext {
            get { return DataContextProperty; }
            set
                {
                if (SetValue(ref DataContextProperty,value)) {
                    var window = package.FindToolWindow(typeof(ModelBrowserToolWindow), 0, true);
                    if (window != null) {
                        if (window.Content is FrameworkElement target) {
                            target.DataContext = value;
                            }
                        }
                    }
                }
            }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
            {
            // Switch to the main thread - the call to AddCommand in ModelBrowserToolWindowCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new ModelBrowserToolWindowCommand(package, commandService);
            }

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
            var window = package.FindToolWindow(typeof(ModelBrowserToolWindow), 0, true);
            if ((null != window) && (null != window.Frame)) {
                var windowFrame = (IVsWindowFrame)window.Frame;
                ErrorHandler.ThrowOnFailure(windowFrame.Show());
                }
            }
        #endregion
        #region M:OnPropertyChanged(String)
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyname = null) {
            var handler = PropertyChanged;
            if (handler != null) {
                var e = new PropertyChangedEventArgs(propertyname);
                if (InvokeRequired) {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(()=>{
                        handler.Invoke(this, e);
                        }));
                    }
                else
                    {
                    handler.Invoke(this, e);
                    }
                }
            }
        #endregion
        #region M:SetValue<T>(ref T,T,String):Boolean
        private Boolean SetValue<T>(ref T field, T value, [CallerMemberName] String propertyName = null) {
            var r = true;
            var equatable = value as IEquatable<T>;
            if (equatable != null) {
                r = equatable.Equals(field);
                }
            else if (typeof(T).IsSubclassOf(typeof(Enum)))
                {
                r = Equals(value, field);
                }
            else
                {
                r = Equals(value, field);
                }
            if (!r)
                {
                field = value;
                OnPropertyChanged(propertyName);
                }
            return !r;
            }
        #endregion
        }
    }
