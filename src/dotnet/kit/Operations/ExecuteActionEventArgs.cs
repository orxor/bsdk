using System;
using BinaryStudio.DirectoryServices;

namespace Operations
    {
    internal class ExecuteActionEventArgs : EventArgs
        {
        public FileOperationStatus OperationStatus { get;set; }
        public IFileService FileService { get; }

        public ExecuteActionEventArgs(IFileService Service)
            {
            FileService = Service;
            }
        }
    }
