using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.DirectoryServices;
using BinaryStudio.DiagnosticServices.Logging;
using BinaryStudio.PlatformComponents;
using Options;

namespace Operations
    {
    internal class FileOperation : Operation
        {
        public IList<String> InputFileName { get; }
        public String TargetFolder { get; }
        //public DirectoryServiceSearchOptions Options { get;set; }
        public String Pattern { get;set; }
        public event EventHandler<ExecuteActionEventArgs> ExecuteAction;
        private readonly MultiThreadOption MultiThreadOption;
        private readonly Boolean StopOnError;
        private readonly IList<String> Containers;

        public FileOperation(IList<OperationOption> args)
            :base(args)
            {
            if (args == null) { throw new ArgumentNullException(nameof(args)); }
            MultiThreadOption = args.OfType<MultiThreadOption>().FirstOrDefault() ??
                new MultiThreadOption{
                    NumberOfThreads = 32
                    };
            Containers = args?.OfType<ContainersOption>().FirstOrDefault()?.Values ?? EmptyArray<String>.Value;
            var TraceOption = args?.OfType<TraceOption>()?.FirstOrDefault();
            if (TraceOption != null) {
                StopOnError = TraceOption.HasValue("StopOnError");
                }
            InputFileName = args.OfType<InputFileOrFolderOption>().FirstOrDefault()?.Values;
            TargetFolder  = args.OfType<OutputFileOrFolderOption>().FirstOrDefault()?.Values?.FirstOrDefault();
            }

        #region M:Execute
        public override void Execute() {
            Execute(InputFileName);
            }
        #endregion
        #region M:Execute(IFileService):FileOperationStatus
        private FileOperationStatus Execute(IFileService service) {
            var status = FileOperationStatus.Skip;
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
            switch (status) {
                case FileOperationStatus.Success:
                    lock(ColorScope.SyncRoot) {
                        Write(Console.Out,ConsoleColor.Green, "{ok}");
                        Write(Console.Out,ConsoleColor.Gray, ":");
                        Write(Console.Out,ConsoleColor.Cyan, $"{{{timer.Elapsed.ToString("hh\\:mm\\:ss\\.fffff")}}}");
                        WriteLine(Console.Out,ConsoleColor.Gray, $":{service.FileName}");
                        }
                    break;
                case FileOperationStatus.Skip:
                    lock(ColorScope.SyncRoot) {
                        Write(Console.Out,ConsoleColor.Yellow, "{skip}");
                        Write(Console.Out,ConsoleColor.Gray, ":");
                        Write(Console.Out,ConsoleColor.Cyan, $"{{{timer.Elapsed.ToString("hh\\:mm\\:ss\\.fffff")}}}");
                        WriteLine(Console.Out,ConsoleColor.Gray, $":{service.FileName}");
                        }
                    break;;
                case FileOperationStatus.Warning:
                    lock(ColorScope.SyncRoot) {
                        Write(Console.Out,ConsoleColor.Cyan, "{warning}");
                        Write(Console.Out,ConsoleColor.Gray, ":");
                        Write(Console.Out,ConsoleColor.Cyan, $"{{{timer.Elapsed.ToString("hh\\:mm\\:ss\\.fffff")}}}");
                        WriteLine(Console.Out,ConsoleColor.Gray, $":{service.FileName}");
                        }
                    break;;
                case FileOperationStatus.Error:
                    lock(ColorScope.SyncRoot) {
                        Write(Console.Out,ConsoleColor.Red, "{error}");
                        Write(Console.Out,ConsoleColor.Gray, ":");
                        Write(Console.Out,ConsoleColor.Cyan, $"{{{timer.Elapsed.ToString("hh\\:mm\\:ss\\.fffff")}}}");
                        WriteLine(Console.Out,ConsoleColor.Gray, $":{service.FileName}");
                        }
                    break;
                    }
            return status;
            }
        #endregion
        #region M:Execute(IEnumerable<String>):FileOperationStatus
        private FileOperationStatus Execute(IEnumerable<String> InputSource) {
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
        private FileOperationStatus Execute(IEnumerable<IFileService> InputSource) {
            var status = FileOperationStatus.Skip;
            if (MultiThreadOption.NoMultiThread) {
                foreach (var service in InputSource) {
                    status = Max(status,Execute(service));
                    }
                }
            else
                {
                InputSource.AsParallel().ForAll(service=> {
                    status = Max(status,Execute(service));
                    });
                }
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