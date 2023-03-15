using System;
using System.IO;
using BinaryStudio.IO;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="OCTET STRING"/> type.
    /// </summary>
    public sealed class Asn1OctetString : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.OctetString"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.OctetString; }}

        #region ctor
        internal Asn1OctetString()
            {
            }
        #endregion
        #region ctor{{params}Asn1Object[]}
        internal Asn1OctetString(params Asn1Object[] args) {
            var size = 0L;
            using (var o = new MemoryStream()) {
                foreach (var i in args) {
                    Add(i);
                    size += i.Size;
                    i.WriteTo(o,true);
                    }
                this.length = size;
                this.content = new ReadOnlyMemoryMappingStream(o.ToArray());
                this.size = size + GetHeader().Length;
                }
            IsReadOnly = true;
            State |= ObjectState.Decoded;
            }
        #endregion
        #region ctor{Byte[]}
        internal Asn1OctetString(Byte[] content)
            {
            if (content == null) { throw new ArgumentNullException(nameof(content)); }
            this.content = new ReadOnlyMemoryMappingStream(content);
            this.length = content.Length;
            this.size = length + GetHeader().Length;
            IsReadOnly = true;
            State |= ObjectState.Decoded;
            }
        #endregion
        }
    }