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

        protected const Int32 X509_ASN_ENCODING   = 0x00000001;
        protected const Int32 PKCS_7_ASN_ENCODING = 0x00010000;
        }
    }