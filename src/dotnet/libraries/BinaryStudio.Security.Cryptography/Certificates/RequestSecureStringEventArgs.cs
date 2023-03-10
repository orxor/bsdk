using System;
using System.Security;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public class RequestSecureStringEventArgs : EventArgs
        {
        public SecureString SecureString { get;set; }
        public Boolean Canceled { get;set; }
        public Boolean StoreSecureString { get;set; }
        public String Info { get;set; }
        public String Container { get;set; }

        #region M:GetSecureString(String):SecureString
        public unsafe static SecureString GetSecureString(String value) {
            fixed (Char* c = value) {
                return new SecureString(c, value.Length);
                }
            }
        #endregion
        }

    public delegate void RequestSecureStringEventHandler(Object sender, RequestSecureStringEventArgs e);
    public delegate void RequestSecureStringCallback(RequestSecureStringEventArgs e);
    }