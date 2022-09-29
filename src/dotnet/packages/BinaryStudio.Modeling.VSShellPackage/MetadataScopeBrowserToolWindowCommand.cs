using System;
using Microsoft.VisualStudio.Shell;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    public class MetadataScopeBrowserToolWindowCommand: EToolWindowCommand<MetadataScopeBrowserToolWindow>
        {
        public MetadataScopeBrowserToolWindowCommand(AsyncPackage package, OleMenuCommandService commandService)
            :base(package, commandService)
            {
            }

        public override Int32 CommandId { get { return 0x0101; }}
        public override Guid CommandSet { get { return new Guid("fdc929a3-8dfd-4def-b927-43ab6a8b7f8b"); }}
        }
    }