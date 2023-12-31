﻿using System.Text;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="PRINTABLESTRING"/> type.
    /// </summary>
    internal sealed class Asn1PrintableString : Asn1String
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.PrintableString"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.PrintableString; }}
        public override Encoding Encoding { get { return Encoding.ASCII; }}
        }
    }