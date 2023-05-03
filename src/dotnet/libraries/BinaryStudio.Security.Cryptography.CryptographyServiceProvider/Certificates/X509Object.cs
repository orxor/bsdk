using System;
using BinaryStudio.Serialization;
using BinaryStudio.Services;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public abstract class X509Object : CryptographicObject,IJsonSerializable
        {
        internal static CryptographicFunctions Entries;
        protected Boolean Disposed;

        public abstract void WriteTo(IJsonWriter writer);

        static X509Object()
            {
            Entries = (CryptographicFunctions)CryptographicContext.DefaultContext.GetService(typeof(CryptographicFunctions));
            }
        }
    }