using System;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public abstract class X509Object : CryptographicObject,IJsonSerializable
        {
        internal static ICryptoAPI Entries;
        protected Boolean Disposed;

        public abstract void WriteTo(IJsonWriter writer);

        static X509Object()
            {
            Entries = (ICryptoAPI)CryptographicContext.DefaultContext.GetService(typeof(ICryptoAPI));
            }
        }
    }