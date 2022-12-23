using System;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public abstract class X509Object : CryptographicObject,IJsonSerializable
        {
        protected Boolean Disposed;

        public abstract void WriteTo(IJsonWriter writer);
        }
    }