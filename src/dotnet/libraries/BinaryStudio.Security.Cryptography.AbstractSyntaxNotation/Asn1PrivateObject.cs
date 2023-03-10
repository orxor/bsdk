using System;
using System.IO;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1PrivateObject : Asn1Object
        {
        /// <summary>
        /// ASN.1 object class. Always returns <see cref="Asn1ObjectClass.Private"/>.
        /// </summary>
        public override Asn1ObjectClass Class { get { return Asn1ObjectClass.Private; }}
        public SByte Type { get; }
        protected internal override Object TypeCode { get { return Type; }}
        protected internal override SByte  ByteCode { get { return Type; }}

        public Asn1PrivateObject(SByte type)
            {
            Type = type;
            }

        #region M:WriteHeader(Stream)
        protected internal override void WriteHeader(Stream target) {
            WriteHeader(target, IsExplicitConstructed, Class, Type,
                IsIndefiniteLength
                 ? - 1
                 : Length);
            }
        #endregion
        }
    }