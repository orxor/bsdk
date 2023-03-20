using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.CryptographicMessageSyntax;
using Options;

namespace Operations
    {
    internal class BatchOperation : Operation
        {
        private X509StoreLocation StoreLocation { get; }
        private String StoreName { get; }
        private BatchOperationFlags Flags { get; }
        public String TargetFolder { get; }

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
            TargetFolder  = args.OfType<OutputFileOrFolderOption>().FirstOrDefault()?.Values?.FirstOrDefault();
            }
        #endregion

        #region M:Execute
        public override void Execute() {
            //if (Flags.HasFlag(BatchOperationFlags.Install) || Flags.HasFlag(BatchOperationFlags.Uninstall)) {
            //    if (StoreLocation == X509StoreLocation.LocalMachine) {
            //        PlatformContext.ValidatePermission(WindowsBuiltInRole.Administrator);
            //        }
            //    }
            try
                {
                using (var core = new FileOperation(Options)) {
                    core.ExecuteAction += Execute;
                    core.Execute();
                    core.ExecuteAction -= Execute;
                    }
                }
            finally
                {
                
                }
            }
        #endregion
        #region M:Execute(Object,ExecuteActionEventArgs)
        private void Execute(Object sender, ExecuteActionEventArgs e) {
            switch (Path.GetExtension(e.FileService.FileName)?.ToLowerInvariant()) {
                case ".crl":
                case ".cer":
                    {
                    using (var o = Asn1Object.Load(e.FileService.ReadAllBytes()).FirstOrDefault()) {
                        if ((o != null) && (!o.IsFailed)) {
                            Execute(o,e);
                            }
                        }
                    }
                    break;
                }
            }
        #endregion
        #region M:Execute(Asn1Object,ExecuteActionEventArgs)
        private void Execute(Asn1Object InputObject, ExecuteActionEventArgs e) {
            using (var cer = new Asn1Certificate(InputObject))               if (!cer.IsFailed) { Execute(cer,e); return; }
            using (var crl = new Asn1CertificateRevocationList(InputObject)) if (!crl.IsFailed) { Execute(crl,e); return; }
            using (var cms = new CmsMessage(InputObject))                    if (!cms.IsFailed) { Execute(cms,e);         }
            }
        #endregion
        #region M:Execute(CmsMessage,ExecuteActionEventArgs)
        private void Execute(CmsMessage InputObject,ExecuteActionEventArgs e)
            {
            }
        #endregion
        #region M:Execute(Asn1Certificate,ExecuteActionEventArgs)
        private void Execute(Asn1Certificate InputObject,ExecuteActionEventArgs e) {
            if (Flags.HasFlag(BatchOperationFlags.Rename)) {
                var TargetFileName = $"{InputObject.FriendlyName}.cer";
                TargetFileName = Path.Combine(TargetFolder,TargetFileName);
                e.FileService.MoveTo(TargetFileName,true);
                e.OperationStatus = FileOperationStatus.Success;
                }
            }
        #endregion
        #region M:Execute(Asn1CertificateRevocationList,ExecuteActionEventArgs)
        private void Execute(Asn1CertificateRevocationList InputObject,ExecuteActionEventArgs e)
            {
            }
        #endregion
        }
    }