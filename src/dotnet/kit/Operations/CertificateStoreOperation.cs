using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BinaryStudio.Reporting;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Reporting;
using Options;

namespace Operations
    {
    internal class CertificateStoreOperation : Operation
        {
        private X509StoreLocation StoreLocation { get; }
        private String StoreName { get; }
        private ReportOption ReportOption { get; }

        #region ctor{IList<OperationOption>}
        public CertificateStoreOperation(IList<OperationOption> args)
            : base(args)
            {
            StoreLocation = (args.OfType<StoreLocationOption>().FirstOrDefault()?.Value).GetValueOrDefault(X509StoreLocation.CurrentUser);
            StoreName     = args.OfType<StoreNameOption>().FirstOrDefault()?.Value ?? nameof(X509StoreName.My);
            ReportOption  = args.OfType<ReportOption>().FirstOrDefault();
            }
        #endregion
        #region M:Execute
        public override void Execute() {
            using (var store = new X509CertificateStorage(StoreName,StoreLocation)) {
                if (ReportOption != null) {
                    Type ReportType = null;
                    switch (ReportOption.ReportName.ToLowerInvariant()) {
                        case "gostcertificatesignature": { ReportType = typeof(GOSTCertificateSignature); break; }
                        default: throw new ArgumentOutOfRangeException(nameof(ReportOption));
                        }
                    var ReportSource = (ReportSource)Activator.CreateInstance(ReportType);
                    using (var output = File.Create("report.xml")) {
                        ReportSource.Build(store,output);
                        }
                    }
                }
            }
        #endregion
        }
    }