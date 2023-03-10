using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using BinaryStudio.PlatformComponents;
using BinaryStudio.Security.Cryptography.Certificates;
using Options;

namespace Operations
    {
    internal class BatchOperation : Operation
        {
        private X509StoreLocation StoreLocation { get; }
        private String StoreName { get; }
        private BatchOperationFlags Flags { get; }

        #region ctor{IList<OperationOption>}
        public BatchOperation(IList<OperationOption> args)
            : base(args)
            {
            StoreLocation = (args.OfType<StoreLocationOption>().FirstOrDefault()?.Value).GetValueOrDefault(X509StoreLocation.CurrentUser);
            StoreName     = args.OfType<StoreNameOption>().FirstOrDefault()?.Value ?? nameof(X509StoreName.My);
            var options = (new HashSet<String>(args.OfType<BatchOption>().SelectMany(i => i.Values))).ToArray();
            if (options.Any(i => String.Equals(i, "rename",    StringComparison.OrdinalIgnoreCase))) { Flags |= BatchOperationFlags.Rename;    }
            if (options.Any(i => String.Equals(i, "serialize", StringComparison.OrdinalIgnoreCase))) { Flags |= BatchOperationFlags.Serialize; }
            if (options.Any(i => String.Equals(i, "extract",   StringComparison.OrdinalIgnoreCase))) { Flags |= BatchOperationFlags.Extract;   }
            if (options.Any(i => String.Equals(i, "group",     StringComparison.OrdinalIgnoreCase))) { Flags |= BatchOperationFlags.Group;     }
            if (options.Any(i => String.Equals(i, "install",   StringComparison.OrdinalIgnoreCase))) { Flags |= BatchOperationFlags.Install;   }
            if (options.Any(i => String.Equals(i, "uninstall", StringComparison.OrdinalIgnoreCase))) { Flags |= BatchOperationFlags.Uninstall; }
            if (options.Any(i => String.Equals(i, "report",    StringComparison.OrdinalIgnoreCase))) { Flags |= BatchOperationFlags.Report;    }
            if (options.Any(i => String.Equals(i, "force",     StringComparison.OrdinalIgnoreCase))) { Flags |= BatchOperationFlags.Force;     }
            if (options.Any(i => String.Equals(i, "asn",       StringComparison.OrdinalIgnoreCase))) { Flags |= BatchOperationFlags.AbstractSyntaxNotation; }
            }
        #endregion

        #region M:Execute
        public override void Execute() {
            if (Flags.HasFlag(BatchOperationFlags.Install) || Flags.HasFlag(BatchOperationFlags.Uninstall)) {
                if (StoreLocation == X509StoreLocation.LocalMachine) {
                    PlatformContext.ValidatePermission(WindowsBuiltInRole.Administrator);
                    }
                }
            }
        #endregion
        }
    }