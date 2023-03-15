using System;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class Asn1CertificateExtensionAttribute : Attribute
        {
        public String Key { get; }
        public Asn1CertificateExtensionAttribute(String key) {
            Key = key;
            }

        public override String ToString()
            {
            return Key;
            }
        }
    }
