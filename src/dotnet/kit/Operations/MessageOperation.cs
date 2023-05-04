using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PlatformComponents.Win32;
using Options;

namespace Operations
    {
    internal abstract class MessageOperation : Operation
        {
        public CRYPT_PROVIDER_TYPE ProviderType { get; }
        protected MessageOperation(IList<OperationOption> args)
            : base(args)
            {
            ProviderType = (CRYPT_PROVIDER_TYPE)args.OfType<ProviderTypeOption>().First().Type;
            }
        }
    }
