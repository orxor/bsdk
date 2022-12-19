using BinaryStudio.DiagnosticServices.Logging;
using BinaryStudio.DirectoryServices;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.CryptographicMessageSyntax;
using Operations;
using Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operations
    {
    internal class VerifyMessageOperation : MessageOperation
        {
        public String Policy { get; }
        private FileOperation UnderlyingObject { get; }

        public VerifyMessageOperation(IList<OperationOption> args)
            : base(args)
            {
            UnderlyingObject = new FileOperation(args);
            Policy = args.OfType<PolicyOption>().FirstOrDefault()?.Value;
            }

        #region M:Execute(TextWriter)
        public override void Execute(TextWriter output) {
            UnderlyingObject.ExecuteAction += OnExecuteAction;
            UnderlyingObject.Execute(output);
            UnderlyingObject.ExecuteAction -= OnExecuteAction;
            }
        #endregion
        #region M:OnExecuteAction(Object,ExecuteActionEventArgs)
        private void OnExecuteAction(Object sender,ExecuteActionEventArgs e) {
            switch (Path.GetExtension(e.FileService.FileName).ToLower()) {
                case ".cer":
                case ".crl":
                case ".der":
                case ".ber":
                case ".p7b":
                    {
                    using (var stream = e.FileService.OpenRead()) {
                        using (var o = Asn1Object.Load(stream).FirstOrDefault()) {
                            using (var cer = new Asn1Certificate(o))               if (!cer.IsFailed) { e.OperationStatus = Execute(e.FileService,cer); break; }
                            using (var crl = new Asn1CertificateRevocationList(o)) if (!crl.IsFailed) { e.OperationStatus = Execute(e.FileService,crl); break; }
                            using (var cms = new CmsMessage(o))                    if (!cms.IsFailed) { e.OperationStatus = Execute(e.FileService,cms); break; }
                            e.OperationStatus = FileOperationStatus.Skip;
                            }
                        }
                    }
                    break;
                }
            }
        #endregion
        #region M:Execute(IFileService,Asn1Certificate):FileOperationStatus
        private FileOperationStatus Execute(IFileService FileService, Asn1Certificate Source) {
            return FileOperationStatus.Skip;
            }
        #endregion
        #region M:Execute(IFileService,Asn1CertificateRevocationList):FileOperationStatus
        private FileOperationStatus Execute(IFileService FileService, Asn1CertificateRevocationList Source) {
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder)) {
                JsonSerialize(Source,writer);
                }
            Logger.Log(LogLevel.Debug,builder.ToString());
            return FileOperationStatus.Success;
            }
        #endregion
        #region M:Execute(IFileService,CmsMessage):FileOperationStatus
        private FileOperationStatus Execute(IFileService FileService, CmsMessage Source) {
            return FileOperationStatus.Skip;
            }
        #endregion
        }
    }
