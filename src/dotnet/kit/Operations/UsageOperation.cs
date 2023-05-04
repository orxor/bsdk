using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Options;

namespace Operations
    {
    internal class UsageOperation : Operation
        {
        public UsageOperation(IList<OperationOption> args)
            : base(args)
            {
            }

        #region M:Execute
        public override void Execute() {
            var i = 0;
            var version = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine("# Version: {{{0}}}:{{{1}}}", version,
                (new DateTime(2000, 1, 1).
                    AddDays(version.Build).
                    AddSeconds(version.Revision * 2)).ToString("s"));
            Console.WriteLine($"# Is64BitProcess:{Environment.Is64BitProcess}");
            Console.WriteLine("# Available options:");
            foreach (var descriptor in descriptors.OrderBy(j => j.OptionName)) { 
                if (i > 0) {
                    Console.WriteLine();
                    }
                Console.Write("  ");
                descriptor.Usage(Console.Out);
                i++;
                }
            Console.Write("\n# Samples:");
            Console.WriteLine(@"
  infrastructure:csp,types
  input:{file-name}.ldif output:{folder} batch:extract
  input:{file-name}.ldif output:{folder} batch:extract,group
  input:{file-name}.ldif filter:*.cer batch:{un}install storelocation:LocalMachine storename:Root
  input:{file-name}.ldif filter:*.crl batch:{un}install storelocation:LocalMachine storename:CA
  input:*.cer batch:{un}install storelocation:LocalMachine storename:Root
  input:*.crl batch:{un}install storelocation:LocalMachine storename:CA
  input:{file-name} hash algid:{algid}
  input:{file-name} hash algid:{algid} providertype:{number}
  input:{file-name} message verify
  input:*.crl [output:{folder}] batch:rename,group
  input:*.rar [output:{folder}] batch:rename,group filter:*.crl
  input:{file-name}.rar output:{folder} batch:extract,group
  input:{file-name}.rar output:{folder} batch:extract,group filter:*.crl
  input:{file-name}.rar\*.crl output:{folder} batch:extract,group
  input:*.cer verify policy:icao datetime:{datetime}
  input:{file-name} certificate:{thumbprint} message create storename:device
");
            }
        #endregion
        }
    }