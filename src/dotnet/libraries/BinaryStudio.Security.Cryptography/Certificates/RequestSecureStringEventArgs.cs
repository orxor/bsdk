using System;
using System.Security;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public class RequestSecureStringEventArgs : EventArgs
        {
        public SecureString SecureString { get; set; }
        public bool Canceled { get; set; }
        public bool StoreSecureString { get; set; }
        public string Container { get; set; }
        }

    public delegate void RequestSecureStringEventHandler(object sender, RequestSecureStringEventArgs e);
    }