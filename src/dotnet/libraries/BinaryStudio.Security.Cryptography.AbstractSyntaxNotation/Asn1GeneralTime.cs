using System;
using System.IO;
using System.Text;
using BinaryStudio.IO;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="GENERALIZEDTIME"/> type.
    /// </summary>
    internal sealed class Asn1GeneralTime : Asn1Time
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.GeneralTime"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.GeneralTime; }}
        public override DateTimeKind Kind { get { return DateTimeKind.Local; }}

        #region ctor
        internal Asn1GeneralTime()
            {
            }
        #endregion
        #region ctor{Byte[]}
        public Asn1GeneralTime(Byte[] source)
            {
            Value = Parse(Encoding.ASCII.GetString(source), Asn1ObjectType.GeneralTime).GetValueOrDefault();
            State |= ObjectState.Decoded;
            this.content = new ReadOnlyMemoryMappingStream(source);
            this.length = source.Length;
            this.size = length + GetHeader().Length;
            }
        #endregion
        #region ctor{DateTime}
        public Asn1GeneralTime(DateTime source)
            {
            Value = new DateTimeOffset(source);
            State |= ObjectState.Decoded;
            var content = Encoding.ASCII.GetBytes($"{source.ToString("yyyyMMddHHmmss")}Z");
            this.content = new ReadOnlyMemoryMappingStream(content);
            this.length = content.Length;
            this.size = length + GetHeader().Length;
            }
        #endregion

        protected override Boolean Decode()
            {
            if (IsDecoded) { return true; }
            if (IsIndefiniteLength) { return false; }
            var r = new Byte[Length];
            Content.Seek(0, SeekOrigin.Begin);
            Content.Read(r, 0, r.Length);
            Value = Parse(Encoding.ASCII.GetString(r), Asn1ObjectType.GeneralTime).GetValueOrDefault();
            State |= ObjectState.Decoded;
            return true;
            }
        }
    }