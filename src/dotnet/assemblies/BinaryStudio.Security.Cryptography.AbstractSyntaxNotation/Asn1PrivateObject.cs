using System;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1PrivateObject : Asn1Object
        {
        /// <summary>
        /// ASN.1 object class. Always returns <see cref="Asn1ObjectClass.Private"/>.
        /// </summary>
        public override Asn1ObjectClass Class { get { return Asn1ObjectClass.Private; }}
        public SByte Type { get; }

        public Asn1PrivateObject(SByte type)
            {
            Type = type;
            }
        }
    }