using System;
using BinaryStudio.IO;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="BOOLEAN"/> type.
    /// </summary>
    public sealed class Asn1Boolean : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Boolean"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Boolean; }}
        public Boolean Value { get;private set; }

        #region ctor
        internal Asn1Boolean()
            {
            }
        #endregion
        #region ctor{Boolean}
        public Asn1Boolean(Boolean value)
            {
            Value = value;
            State |= ObjectState.Decoded;
            }
        #endregion

        public static implicit operator Boolean(Asn1Boolean source) {
            return source.Value;
            }

        #region M:Decode:Boolean
        protected override Boolean Decode() {
            Value = Content.ReadByte() != 0;
            return true;
            }
        #endregion
        #region M:BuildContent
        protected override void BuildContent() {
            var InputContent = new Byte[] {
                Value
                    ? (Byte)0xff
                    : (Byte)0x00
                    };
            length = InputContent.Length;
            content = new ReadOnlyMemoryMappingStream(InputContent);
            size = length + GetHeader().Length;
            }
        #endregion
        }
    }