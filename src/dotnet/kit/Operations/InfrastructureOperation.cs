using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography;
using Options;

namespace Operations
    {
    internal class InfrastructureOperation : Operation
        {
        public InfrastructureFlags Flags { get; }
        public Int32? ProviderType { get; }

        #region ctor{IList<OperationOption>}
        public InfrastructureOperation(IList<OperationOption> args)
            : base(args)
            {
            var flags = args.OfType<InfrastructureOption>().FirstOrDefault();
            ProviderType  = args.OfType<ProviderTypeOption>().FirstOrDefault()?.Type;
            if (flags != null) {
                if (flags.HasValue("csp"))   { Flags |= InfrastructureFlags.CSP;      }
                if (flags.HasValue("types")) { Flags |= InfrastructureFlags.CSPtypes; }
                if (flags.HasValue("keys"))  { Flags |= InfrastructureFlags.CSPkeys;  }
                if (flags.HasValue("algs"))  { Flags |= InfrastructureFlags.CSPalgs;  }
                }
            if (Flags == 0)
                {
                Flags = InfrastructureFlags.CSP;
                }
            }
        #endregion
        #region M:Execute
        public override void Execute() {
            if (Flags.HasFlag(InfrastructureFlags.CSP)) {
                WriteLine(Console.Out,ConsoleColor.White, "AvailableProviders:");
                foreach (var type in CryptographicContext.RegisteredProviders) {
                    Write(Console.Out,ConsoleColor.Gray,"  {");
                    Write(Console.Out,ConsoleColor.Yellow, $"{(Int32)type.ProviderType,2}");
                    Write(Console.Out,ConsoleColor.Gray,"} ");
                    Write(Console.Out,ConsoleColor.Gray,$"{type.ProviderName}:");
                    WriteLine(Console.Out,ConsoleColor.Yellow, $"{type.ProviderType}");
                    if (Flags.HasFlag(InfrastructureFlags.CSPalgs)) {
                        try
                            {
                            using (var context = CryptographicContext.AcquireContext(null, type.ProviderName, type.ProviderType, CryptographicContextFlags.CRYPT_SILENT|CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
                                foreach (var algid in context.SupportedAlgorithms) {
                                    WriteLine(Console.Out, ConsoleColor.Gray, $"    {algid.Key}:{algid.Value}");
                                    }
                                }
                            }
                        catch (Exception e)
                            {
                            Console.WriteLine(e);
                            }
                        }
                    }
                }
            if (Flags.HasFlag(InfrastructureFlags.CSPtypes)) {
                WriteLine(Console.Out,ConsoleColor.White, "AvailableProviderTypes:");
                foreach (var type in CryptographicContext.AvailableTypes) {
                    Write(Console.Out,ConsoleColor.Gray,"  {");
                    Write(Console.Out,ConsoleColor.Yellow, $"{(Int32)type.Key,2}");
                    Write(Console.Out,ConsoleColor.Gray,"} ");
                    Write(Console.Out,ConsoleColor.Yellow, $"{type.Key}");
                    WriteLine(Console.Out,ConsoleColor.Gray, $":{type.Value}");
                    }
                }
            }
        #endregion
        }
    }