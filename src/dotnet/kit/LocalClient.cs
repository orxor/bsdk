using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinaryStudio.DiagnosticServices.Logging;
using BinaryStudio.Security.Cryptography.Certificates;
using log4net;
using Operations;
using Options;

public class LocalClient : ILocalClient
    {
    private static readonly ILogger Logger = new ClientLogger(LogManager.GetLogger(nameof(LocalClient)));
    private InterlockedInternal<Operation> operation = new InterlockedInternal<Operation>();

    public Int32 Main(String[] args)
        {
        try
            {
            var options = Operation.Parse(args);
            operation.Value = new UsageOperation(options);
            if (!HasOption(options, typeof(MultiThreadOption))) {
                options.Add(new MultiThreadOption
                    {
                    NumberOfThreads = 64
                    });
                }
            if (!HasOption(options, typeof(ProviderTypeOption)))  { options.Add(new ProviderTypeOption(80));                             }
            if (!HasOption(options, typeof(StoreLocationOption))) { options.Add(new StoreLocationOption(X509StoreLocation.CurrentUser)); }
            if (!HasOption(options, typeof(StoreNameOption)))     { options.Add(new StoreNameOption(nameof(X509StoreName.My)));          }
            if (!HasOption(options, typeof(PinCodeRequestType)))  { options.Add(new PinCodeRequestType(PinCodeRequestTypeKind.Default)); }
            if (!HasOption(options, typeof(OutputTypeOption)))    { options.Add(new OutputTypeOption("none"));                           }
            if (!HasOption(options, typeof(DateTimeOption)))      { options.Add(new DateTimeOption(DateTime.Now));                       }
            operation.Value.ValidatePermission();
            var trace = options.OfType<TraceOption>().FirstOrDefault()?.Values;
            if ((trace != null) && trace.Any(i => String.Equals(i, "suspend",StringComparison.OrdinalIgnoreCase))) {
                Console.WriteLine("Press [ENTER] to resume...");
                Console.ReadLine();
                }
            Task.Factory.StartNew(()=>{
                try
                    {
                    operation.Value.Execute(Console.Out);
                    }
                finally
                    {
                    }
                }).Wait();
            return 0;
            }
        catch (Exception e)
            {
            Logger.Log(LogLevel.Critical, e);
            }
        return -1;
        }

    #region M:HasOption(IList<OperationOption>,Type):Boolean
    private static Boolean HasOption(IList<OperationOption> source, Type type) {
        if (type == null) { throw new ArgumentNullException(nameof(type)); }
        if (source == null) { throw new ArgumentNullException(nameof(source)); }
        return source.Any(i => i.GetType() == type);
        }
    #endregion
    #region M:Dispose
    public void Dispose()
        {
        }
    #endregion
    }
