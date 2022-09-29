using Microsoft.VisualStudio.Shell;
using System;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ModelBrowserToolWindowCommand: EToolWindowCommand<ModelBrowserToolWindow>
        {
        public override Int32 CommandId { get { return 0x0100; }}
        public override Guid CommandSet { get { return new Guid("fdc929a3-8dfd-4def-b927-43ab6a8b7f8b"); }}

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBrowserToolWindowCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ModelBrowserToolWindowCommand(AsyncPackage package, OleMenuCommandService commandService)
            :base(package,commandService)
            {
            }
        }
    }
