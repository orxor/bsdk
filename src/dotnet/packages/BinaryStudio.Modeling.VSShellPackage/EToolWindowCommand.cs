using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Action = System.Action;
using Task = System.Threading.Tasks.Task;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    public abstract class EToolWindowCommand<T>
        {
        public Dispatcher Dispatcher { get; }
        public Boolean InvokeRequired { get { return Dispatcher.Thread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId; }}
        public abstract Int32 CommandId { get; }
        public abstract Guid CommandSet { get; }

        #region P:DataContext:Object
        public Object DataContext {
            get { return datacontext; }
            set
                {
                if (SetValue(ref datacontext,value)) {
                    var window = ServiceProvider.FindToolWindow(typeof(T), 0, true);
                    if (window != null) {
                        if (window.Content is FrameworkElement target) {
                            target.DataContext = value;
                            }
                        }
                    }
                }
            }
        #endregion

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static EToolWindowCommand<T> Instance { get;private set; }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        protected AsyncPackage ServiceProvider { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EToolWindowCommand&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        protected EToolWindowCommand(AsyncPackage package, OleMenuCommandService commandService)
            {
            ServiceProvider = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            Dispatcher = Dispatcher.CurrentDispatcher;
            var MenuCommandID = new CommandID(CommandSet, CommandId);
            var MenuItem = new MenuCommand(Execute, MenuCommandID);
            commandService.AddCommand(MenuItem);
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
            var window = ServiceProvider.FindToolWindow(typeof(T), 0, true);
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
        #region M:InitializeAsync(AsyncPackage):Task
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
            var type = typeof(T);
            var ctor = type.GetConstructor(BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public,null,
                new []{
                    typeof(AsyncPackage),
                    typeof(OleMenuCommandService)
                    },null);
            if (ctor == null) {
                throw new MissingMemberException();
                }
            Instance = (EToolWindowCommand<T>)ctor.Invoke(new Object[]{
                package,
                commandService
                });
            }
        #endregion

        private Object datacontext;
        }
    }