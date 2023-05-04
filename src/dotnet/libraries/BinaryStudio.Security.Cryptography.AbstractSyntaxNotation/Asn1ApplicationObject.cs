using System;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1ApplicationObject : Asn1PrivateObject
        {
        /// <summary>
        /// ASN.1 object class. Always returns <see cref="Asn1ObjectClass.Application"/>.
        /// </summary>
        public override Asn1ObjectClass Class { get { return Asn1ObjectClass.Application; }}

        public Asn1ApplicationObject(SByte type)
            :base(type)
            {
            }
        }
    }