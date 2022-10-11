﻿namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="ENUMERATED"/> type.
    /// </summary>
    public sealed class Asn1Enum : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Enum"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Enum; }}
        }
    }