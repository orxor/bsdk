﻿using System.Text;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="VIDEOTEXSTRING"/> type.
    /// </summary>
    internal sealed class Asn1VideotexString : Asn1String
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.VideotexString"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.VideotexString; }}
        public override Encoding Encoding { get { return Encoding.ASCII; }}
        }
    }