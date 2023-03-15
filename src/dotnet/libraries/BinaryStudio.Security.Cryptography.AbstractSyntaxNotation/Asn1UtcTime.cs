using System;
using System.Text;
using BinaryStudio.IO;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="UTCTIME"/> type.
    /// </summary>
    internal sealed class Asn1UtcTime : Asn1Time
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.UtcTime"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.UtcTime; }}
        public override DateTimeKind Kind { get { return DateTimeKind.Utc; }}

        #region ctor
        internal Asn1UtcTime()
            {
            }
        #endregion
        #region ctor{Byte[]}
        public Asn1UtcTime(Byte[] source)
            {
            Value = Parse(Encoding.ASCII.GetString(source), Asn1ObjectType.UtcTime).GetValueOrDefault();
            State |= ObjectState.Decoded;
            }
        #endregion
        #region ctor{DateTime}
        public Asn1UtcTime(DateTime source)
            :base(source)
            {
            }
        #endregion

        #region M:BuildContent
        protected override void BuildContent() {
            var InputContent = Encoding.ASCII.GetBytes($"{Value.ToString("yyMMddHHmmss")}Z");
            length = InputContent.Length;
            content = new ReadOnlyMemoryMappingStream(InputContent);
            size = length + GetHeader().Length;
            }
        #endregion
        #region M:Decode:Boolean
        protected override Boolean Decode()
            {
            if (IsDecoded) { return true; }
            if (IsIndefiniteLength) { return false; }
            var r = new Byte[Length];
            Content.Read(r, 0, r.Length);
            Value = Parse(Encoding.ASCII.GetString(r), Asn1ObjectType.UtcTime).GetValueOrDefault();
            State |= ObjectState.Decoded;
            return true;
            }
        #endregion
        }
    }