using System;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="BIT STRING"/> type.
    /// </summary>
    public sealed class Asn1BitString : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.BitString"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.BitString; }}
        public Int32 UnusedBits { get; private set; }

        protected override Boolean Decode() {
            if (IsDecoded) { return true; }
            if (IsIndefiniteLength) { return base.Decode(); }
            if (Length == 0) {
                State |= ObjectState.Failed;
                return false;
                }
            UnusedBits = Content.ReadByte();
            content = Content.Clone(Length - 1);
            length = Length - 1;
            base.Decode();
            State |= ObjectState.Decoded;
            State |= ObjectState.DisposeContent;
            return true;
            }
        }
    }