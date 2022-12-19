using BinaryStudio.PlatformComponents.Win32;
using Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operations
    {
    internal class MessageOperation : Operation
        {
        public CRYPT_PROVIDER_TYPE ProviderType { get; }
        public MessageOperation(IList<OperationOption> args)
            : base(args)
            {
            ProviderType = (CRYPT_PROVIDER_TYPE)args.OfType<ProviderTypeOption>().First().Type;
            }

        public override void Execute(TextWriter output)
            {
            }
        }
    }
