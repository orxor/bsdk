using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.DirectoryServices;
using BinaryStudio.DiagnosticServices.Logging;
using BinaryStudio.PlatformComponents;
using BinaryStudio.Security.Cryptography.CryptographicMessageSyntax;
using BinaryStudio.Security.Cryptography.Interchange;
using Options;
using System.Timers;

namespace Operations
    {
    internal class FileOperation : Operation
        {
        public IList<String> InputFileName { get; }
        public String TargetFolder { get;set; }
        public DirectoryServiceSearchOptions Options { get;set; }
        public String Pattern { get;set; }
        //public Func<IFileService,DirectoryServiceSearchOptions,FileOperationArgs,FileOperationStatus> ExecuteAction { get;set; }
        public event EventHandler<DirectoryServiceRequestEventArgs> DirectoryServiceRequest;
        public event EventHandler DirectoryCompleted;
        public event EventHandler<ExecuteActionEventArgs> ExecuteAction;
        public event EventHandler<NumberOfFilesNotifyEventArgs> NumberOfFilesNotify;
        private Int32 NumberOfFiles = 1;
        private readonly MultiThreadOption MultiThreadOption;
        private readonly TraceOption TraceOption;
        private readonly Boolean StopOnError = false;
        private readonly IList<String> Containers;

        public FileOperation(IList<OperationOption> args)
            :base(new OperationOption[0])
            {
            MultiThreadOption = args?.OfType<MultiThreadOption>().FirstOrDefault() ??
                new MultiThreadOption{
                    NumberOfThreads = 32
                    };
            Containers = args?.OfType<ContainersOption>().FirstOrDefault()?.Values ?? EmptyArray<String>.Value;
            TraceOption = args?.OfType<TraceOption>()?.FirstOrDefault();
            if (TraceOption != null) {
                StopOnError = TraceOption.HasValue("StopOnError");
                }
            InputFileName = args.OfType<InputFileOrFolderOption>().FirstOrDefault()?.Values;
            }

        public override void Execute(TextWriter output) {
            Execute(InputFileName);
            }

        #region M:Execute(IEnumerable<String>):FileOperationStatus
        public FileOperationStatus Execute(IEnumerable<String> InputSource) {
            var Status = new InterlockedInternal<FileOperationStatus>(FileOperationStatus.Skip);
            foreach (var Source in InputSource) {
                if (Source.Contains("*")) {
                    var SearchPattern = Path.GetFileName(Source);
                    var Folder = Path.GetDirectoryName(Source);
                    if (!SearchPattern.Contains("*")) { SearchPattern = "*.*"; }
                    Status.Value = Max(Status.Value,
                        Execute(DirectoryService.GetFiles(DirectoryService.GetService<IDirectoryService>(
                            new Uri($"folder://{Folder}")),SearchPattern,Containers)));
                    }
                else
                    {
                    Status.Value = Max(Status.Value, Execute(new[]{
                        DirectoryService.GetService<IFileService>(
                        new Uri($"file://{Source}"))
                        }));
                    }
                }
            return Status.Value;
            }
        #endregion
        #region M:Execute(IDirectoryService):FileOperationStatus
        public FileOperationStatus Execute(IEnumerable<IFileService> InputSource) {
            var status = FileOperationStatus.Skip;
            InputSource.AsParallel().ForAll(service=> {
                var timer = new Stopwatch();
                var e = new ExecuteActionEventArgs(service)
                    {
                    OperationStatus = FileOperationStatus.Skip
                    };
                timer.Start();
                try
                    {
                    ExecuteAction?.Invoke(this,e);
                    }
                catch (Exception x)
                    {
                    x.Add("Service", new
                        {
                        Type = service.GetType().FullName,
                        Self = service.FullName
                        });
                    status = FileOperationStatus.Error;
                    Logger.Log(LogLevel.Warning, x);
                    if (StopOnError)
                        {
                        throw;
                        }
                    }
                finally
                    {
                    timer.Stop();
                    }
                status = Max(status,e.OperationStatus);
                switch (e.OperationStatus) {
                    case FileOperationStatus.Success:
                        lock(console) {
                            Write(Console.Out,ConsoleColor.Green, "{ok}");
                            Write(Console.Out,ConsoleColor.Gray, ":");
                            Write(Console.Out,ConsoleColor.Cyan, $"{{{timer.Elapsed.ToString("hh\\:mm\\:ss\\.fffff")}}}");
                            WriteLine(Console.Out,ConsoleColor.Gray, $":{service.FileName}");
                            }
                        break;
                    case FileOperationStatus.Skip:
                        lock(console) {
                            Write(Console.Out,ConsoleColor.Yellow, "{skip}");
                            Write(Console.Out,ConsoleColor.Gray, ":");
                            Write(Console.Out,ConsoleColor.Cyan, $"{{{timer.Elapsed.ToString("hh\\:mm\\:ss\\.fffff")}}}");
                            WriteLine(Console.Out,ConsoleColor.Gray, $":{service.FileName}");
                            }
                        break;;
                    case FileOperationStatus.Warning:
                        lock(console) {
                            Write(Console.Out,ConsoleColor.Cyan, "{warning}");
                            Write(Console.Out,ConsoleColor.Gray, ":");
                            Write(Console.Out,ConsoleColor.Cyan, $"{{{timer.Elapsed.ToString("hh\\:mm\\:ss\\.fffff")}}}");
                            WriteLine(Console.Out,ConsoleColor.Gray, $":{service.FileName}");
                            }
                        break;;
                    case FileOperationStatus.Error:
                        lock(console) {
                            Write(Console.Out,ConsoleColor.Red, "{error}");
                            Write(Console.Out,ConsoleColor.Gray, ":");
                            Write(Console.Out,ConsoleColor.Cyan, $"{{{timer.Elapsed.ToString("hh\\:mm\\:ss\\.fffff")}}}");
                            WriteLine(Console.Out,ConsoleColor.Gray, $":{service.FileName}");
                            }
                        break;
                        }
                });
            return status;
            }
        #endregion
        #region M:Max(FileOperationStatus,FileOperationStatus)
        public static FileOperationStatus Max(FileOperationStatus x, FileOperationStatus y) {
            if (x == y) { return x; }
            return (FileOperationStatus)Math.Max(
                (Int32)x,
                (Int32)y);
            }
        #endregion

        public override object GetService(object source, Type service) {
            return base.GetService(source, service);
            }
        }
    }