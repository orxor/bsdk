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

namespace Operations
    {
    internal class FileOperation : Operation
        {
        public IList<String> InputFileName { get; }
        public String TargetFolder { get;set; }
        public DirectoryServiceSearchOptions Options { get;set; }
        public String Pattern { get;set; }
        public Func<IFileService,DirectoryServiceSearchOptions,FileOperationArgs,FileOperationStatus> ExecuteAction { get;set; }
        public event EventHandler<DirectoryServiceRequestEventArgs> DirectoryServiceRequest;
        public event EventHandler DirectoryCompleted;
        public event EventHandler FileCompleted;
        public event EventHandler<NumberOfFilesNotifyEventArgs> NumberOfFilesNotify;
        private readonly Object console = new Object();
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
        public FileOperationStatus Execute(IEnumerable<IFileService> InputSource)
            {
            var status = FileOperationStatus.Skip;
            foreach (var InputItem in InputSource) {
                Console.WriteLine(InputItem);
                }
            return status;
            }
        #endregion
        //#region M:Execute(IDirectoryService,String):FileOperationStatus
        //protected virtual FileOperationStatus Execute(IDirectoryService service, String pattern) {
        //    if (service == null) { throw new ArgumentNullException(nameof(service)); }
        //    var status = new InterlockedInternal<FileOperationStatus>(FileOperationStatus.Skip);
        //    var files = service.GetFiles(pattern, Options).ToArray();
        //    NumberOfFiles = files.Length;
        //    NumberOfFilesNotify?.Invoke(this, new NumberOfFilesNotifyEventArgs{
        //        NumberOfFiles = NumberOfFiles
        //        });

        //    #if DEBUG4
        //    foreach (var i in files.OrderBy(i => i.FullName)) {
        //        status.Value = Max(status.Value, Execute(i, pattern));
        //        FileCompleted?.Invoke(this, EventArgs.Empty);
        //        }
        //    #else
        //    Parallel.ForEach(files,
        //        new ParallelOptions{
        //            MaxDegreeOfParallelism = MultiThreadOption.NumberOfThreads
        //            },
        //        i=>
        //            {
        //            status.Value = Max(status.Value, Execute(i, pattern));
        //            FileCompleted?.Invoke(this, EventArgs.Empty);
        //            });
        //    #endif
        //    DirectoryCompleted?.Invoke(this, EventArgs.Empty);
        //    return status.Value;
        //    }
        //#endregion
        //#region M:Execute(IFileService,String):FileOperationStatus
        //private FileOperationStatus Execute(IFileService service, String pattern) {
        //    if (service == null) { throw new ArgumentNullException(nameof(service)); }
        //    var status = FileOperationStatus.Skip;
        //    var targetfolder = (TargetFolder ?? Path.GetDirectoryName(service.FileName)) ?? String.Empty;
        //    var timer = new Stopwatch();
        //    timer.Start();
        //    try
        //        {
        //        return FileOperationStatus.Skip;
        //        switch (Path.GetExtension(service.FileName).ToLower()) {
        //            #region {.ldif}
        //            case ".ldif":
        //                if (Options.HasFlag(DirectoryServiceSearchOptions.Containers)) {
        //                    status = Execute(new LDIFFile(service), Pattern);
        //                    }
        //                break;
        //            #endregion
        //            //#region {.ml  }
        //            //case ".ml":
        //            //    if (Options.HasFlag(DirectoryServiceSearchOptions.Containers)) {
        //            //        using (var msg = new CmsMessage(service.ReadAllBytes())) {
        //            //            var masterList = (CSCAMasterList)msg.ContentInfo.GetService(typeof(CSCAMasterList));
        //            //            if (masterList != null) {
        //            //                status = Execute(masterList, Pattern);
        //            //                }
        //            //            }
        //            //        }
        //            //    break;
        //            //#endregion
        //            default:
        //                if (Options.HasFlag(DirectoryServiceSearchOptions.Containers)) {
        //                    var dirreq = new DirectoryServiceRequestEventArgs(service);
        //                    DirectoryServiceRequest?.Invoke(this, dirreq);
        //                    if (dirreq.Handled && (dirreq.Service != null)) {
        //                        status = Execute(dirreq.Service, Pattern);
        //                        if (status != FileOperationStatus.Skip) {
        //                            break;
        //                            }
        //                        }
        //                    }
        //                status = ExecuteAction(service, Options, new FileOperationArgs
        //                    {
        //                    TargetFolder = targetfolder,
        //                    Pattern = Pattern??"*.*"
        //                    });
        //                if (status == FileOperationStatus.Skip) {
        //                    if (Options.HasFlag(DirectoryServiceSearchOptions.Containers)) {
        //                        if (DirectoryService.GetService<IDirectoryService>(service, out var folder)) {
        //                            status = Execute(folder, Pattern);
        //                            }
        //                        }
        //                    }
        //                break;
        //            }
        //        }
        //    catch (Exception e)
        //        {
        //        e.Add("Service", new
        //            {
        //            Type = service.GetType().FullName,
        //            Self = service
        //            });
        //        status = FileOperationStatus.Error;
        //        Logger.Log(LogLevel.Warning, e);
        //        if (StopOnError)
        //            {
        //            throw;
        //            }
        //        }
        //    finally
        //        {
        //        timer.Stop();
        //        }
        //    switch (status) {
        //        case FileOperationStatus.Success:
        //            lock(console) {
        //                Write(Console.Out,ConsoleColor.Green, "{ok}");
        //                Write(Console.Out,ConsoleColor.Gray, ":");
        //                Write(Console.Out,ConsoleColor.Cyan, $"{{{timer.Elapsed}}}");
        //                WriteLine(Console.Out,ConsoleColor.Gray, $":{service.FileName}");
        //                }
        //            break;
        //        case FileOperationStatus.Warning:
        //            lock(console) {
        //                Write(Console.Out,ConsoleColor.Yellow, "{skip}");
        //                Write(Console.Out,ConsoleColor.Gray, ":");
        //                Write(Console.Out,ConsoleColor.Cyan, $"{{{timer.Elapsed}}}");
        //                WriteLine(Console.Out,ConsoleColor.Gray, $":{service.FileName}");
        //                }
        //            break;;
        //        case FileOperationStatus.Error:
        //            lock(console) {
        //                Write(Console.Out,ConsoleColor.Red, "{error}");
        //                Write(Console.Out,ConsoleColor.Gray, ":");
        //                Write(Console.Out,ConsoleColor.Cyan, $"{{{timer.Elapsed}}}");
        //                WriteLine(Console.Out,ConsoleColor.Gray, $":{service.FileName}");
        //                }
        //            break;
        //            }
        //    return status;
        //    }
        //#endregion
        #region M:Max(FileOperationStatus,FileOperationStatus)
        public static FileOperationStatus Max(FileOperationStatus x, FileOperationStatus y) {
            if (x == y) { return x; }
            return (FileOperationStatus)Math.Max(
                (Int32)x,
                (Int32)y);
            }
        #endregion

        public override object GetService(object source, Type service) {
            //if (service == typeof(IDirectoryService)) {
            //    var file = GetService<IFileService>(source);
            //    if (file != null) {
            //        switch (Path.GetExtension(file.FullName).ToLower()) {
            //            case ".hexcsv": { return new HexCSVGroupService(file); }
            //            }
            //        }
            //    }
            return base.GetService(source, service);
            }
        }
    }