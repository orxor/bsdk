using BinaryStudio.DirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
