using System;
#if FEATURE_SECURE_STRING_PASSWORD
using System.Security;
#endif

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public class RequestSecureStringEventArgs : EventArgs
        {
        #if FEATURE_SECURE_STRING_PASSWORD
        public SecureString SecureString { get;set; }
        #else
        public String SecureString { get;set; }
        #endif
        public Boolean Canceled { get;set; }
        public Boolean StoreSecureString { get;set; }
        public String Info { get;set; }
        public String Container { get;set; }

        #if FEATURE_SECURE_STRING_PASSWORD
        #region M:GetSecureString(String):SecureString
        public unsafe static SecureString GetSecureString(String value) {
            fixed (Char* c = value) {
                return new SecureString(c, value.Length);
                }
            }
        #endregion
        #else
        #region M:GetSecureString(String):String
        public static String GetSecureString(String value) {
            return value;
            }
        #endregion
        #endif
        }

    public delegate void RequestSecureStringEventHandler(Object sender, RequestSecureStringEventArgs e);
    public delegate void RequestSecureStringCallback(RequestSecureStringEventArgs e);
    }