using System;

namespace BinaryStudio.Services
    {
    public class ServiceResolveEventArgs : EventArgs
        {
        public Object RequestingService { get; }
        public Object Service { get;set; }
        internal ServiceResolveEventArgs(Object service)
            {
            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            RequestingService = service;
            }
        }
    }
