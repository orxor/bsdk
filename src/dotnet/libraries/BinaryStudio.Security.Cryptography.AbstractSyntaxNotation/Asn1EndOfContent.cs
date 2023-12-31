﻿namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="EOC"/> type.
    /// </summary>
    public sealed class Asn1EndOfContent : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.EndOfContent"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.EndOfContent; }}
        }
    }