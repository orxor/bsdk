using System;
using System.Text;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="UTF8STRING"/> type.
    /// </summary>
    internal sealed class Asn1Utf8String : Asn1String
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Utf8String"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Utf8String; }}
        public override Encoding Encoding { get { return Encoding.UTF8; }}

        #region ctor
        internal Asn1Utf8String()
            {
            }
        #endregion
        #region ctor{String}
        public Asn1Utf8String(String value)
            :base(value)
            {
            }
        #endregion
        }
    }