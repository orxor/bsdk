namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1ApplicationObject : Asn1Object
        {
        /// <summary>
        /// ASN.1 object class. Always returns <see cref="Asn1ObjectClass.Application"/>.
        /// </summary>
        public override Asn1ObjectClass Class { get { return Asn1ObjectClass.Application; }}
        }
    }