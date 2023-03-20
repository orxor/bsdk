using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BinaryStudio.DirectoryServices;
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
        public String SourceFolder { get; }

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
                case ".der":
                case ".crl":
                case ".cer":
                    {
                    var r = e.FileService.ReadAllBytes();
                    var s = Encoding.UTF8.GetString(r);
                    s = s.Replace("-----BEGIN CERTIFICATE-----",String.Empty);
                    s = s.Replace("-----END CERTIFICATE-----",String.Empty);
                    s = s.Replace(" ",String.Empty);
                    var rebuild = false;
                    try
                        {
                        var o = Convert.FromBase64String(s);
                        r = o;
                        rebuild = true;
                        }
                    catch
                        {
                        /* do nothing */
                        }
                    using (var o = Asn1Object.Load(r).FirstOrDefault()) {
                        if ((o != null) && (!o.IsFailed)) {
                            Execute(o,e,rebuild);
                            }
                        }
                    }
                    break;
                }
            }
        #endregion
        #region M:Execute(Asn1Object,ExecuteActionEventArgs)
        private void Execute(Asn1Object InputObject, ExecuteActionEventArgs e, Boolean rebuild) {
            using (var cer = new Asn1Certificate(InputObject))               if (!cer.IsFailed) { Execute(cer,e,rebuild); return; }
            using (var crl = new Asn1CertificateRevocationList(InputObject)) if (!crl.IsFailed) { Execute(crl,e,rebuild); return; }
            using (var cms = new CmsMessage(InputObject))                    if (!cms.IsFailed) { Execute(cms,e,rebuild);         }
            }
        #endregion
        #region M:Execute(CmsMessage,ExecuteActionEventArgs,Boolean)
        private void Execute(CmsMessage InputObject,ExecuteActionEventArgs e, Boolean rebuild)
            {
            }
        #endregion
        #region M:Execute(Asn1Certificate,ExecuteActionEventArgs,Boolean)
        private void Execute(Asn1Certificate InputObject,ExecuteActionEventArgs e, Boolean rebuild) {
            if (Flags.HasFlag(BatchOperationFlags.Rename)) {
                var TargetFileName = $"{InputObject.FriendlyName}.cer";
                var TargetFolder = this.TargetFolder??Path.GetDirectoryName(e.FileService.FullName);
                if (Flags.HasFlag(BatchOperationFlags.Group)) { TargetFolder = Path.Combine(TargetFolder,InputObject.Country); }
                if (!String.IsNullOrEmpty(TargetFolder) && !Directory.Exists(TargetFolder)) { Directory.CreateDirectory(TargetFolder); }
                TargetFileName = Path.Combine(TargetFolder,TargetFileName);
                if (!String.Equals(e.FileService.FullName,TargetFileName)) {
                    if (rebuild)
                        {
                        File.WriteAllBytes(TargetFileName,InputObject.Body);
                        File.SetAttributes(e.FileService.FullName,File.GetAttributes(e.FileService.FullName) & (~FileAttributes.ReadOnly));
                        File.Delete(e.FileService.FullName);
                        }
                    else
                        {
                        e.FileService.MoveTo(TargetFileName,true);
                        }
                    e.OperationStatus = FileOperationStatus.Success;
                    }
                PathUtils.SetCreationTime(TargetFileName, InputObject.NotBefore);
                PathUtils.SetLastWriteTime(TargetFileName, InputObject.NotBefore);
                PathUtils.SetLastAccessTime(TargetFileName, InputObject.NotBefore);
                e.OperationStatus = FileOperationStatus.Success;
                }
            }
        #endregion
        #region M:Execute(Asn1CertificateRevocationList,ExecuteActionEventArgs,Boolean)
        private void Execute(Asn1CertificateRevocationList InputObject,ExecuteActionEventArgs e, Boolean rebuild) {
            if (Flags.HasFlag(BatchOperationFlags.Rename)) {
                var TargetFileName = $"{InputObject.FriendlyName}.crl";
                var TargetFolder = this.TargetFolder??Path.GetDirectoryName(e.FileService.FullName);
                if (Flags.HasFlag(BatchOperationFlags.Group)) { TargetFolder = Path.Combine(TargetFolder,InputObject.Country); }
                if (!String.IsNullOrEmpty(TargetFolder) && !Directory.Exists(TargetFolder)) { Directory.CreateDirectory(TargetFolder); }
                TargetFileName = Path.Combine(TargetFolder,TargetFileName);
                if (!String.Equals(e.FileService.FullName,TargetFileName)) {
                    e.FileService.MoveTo(TargetFileName,true);
                    e.OperationStatus = FileOperationStatus.Success;
                    }
                PathUtils.SetCreationTime(TargetFileName, InputObject.EffectiveDate);
                PathUtils.SetLastWriteTime(TargetFileName, InputObject.EffectiveDate);
                PathUtils.SetLastAccessTime(TargetFileName, InputObject.EffectiveDate);
                e.OperationStatus = FileOperationStatus.Success;
                }
            }
        #endregion
        }
    }