using System;
using System.IO;
using BinaryStudio.IO;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1ContextSpecificObject : Asn1PrivateObject
        {
        /// <summary>
        /// ASN.1 object class. Always returns <see cref="Asn1ObjectClass.ContextSpecific"/>.
        /// </summary>
        public override Asn1ObjectClass Class { get { return Asn1ObjectClass.ContextSpecific; }}

        #region ctor{SByte}
        public Asn1ContextSpecificObject(SByte type)
            :base(type)
            {
            }
        #endregion
        #region ctor{SByte,Byte[]}
        public Asn1ContextSpecificObject(SByte type,Byte[] content)
            :base(type)
            {
            if (content == null) { throw new ArgumentNullException(nameof(content)); }
            this.content = new ReadOnlyMemoryMappingStream(content);
            this.length = content.Length;
            this.size = length + GetHeader().Length;
            IsReadOnly = true;
            State |= ObjectState.Decoded;
            }
        #endregion
        #region ctor{SByte,{params}Asn1Object[]}
        public Asn1ContextSpecificObject(SByte type,params Asn1Object[] args)
            :base(type)
            {
            var size = 0L;
            using (var o = new MemoryStream()) {
                foreach (var i in args) {
                    Add(i);
                    size += i.Size;
                    i.WriteTo(o);
                    }
                this.length = size;
                this.content = new ReadOnlyMemoryMappingStream(o.ToArray());
                this.size = size + GetHeader().Length;
                }
            IsReadOnly = true;
            State |= ObjectState.Decoded|ObjectState.ExplicitConstructed;
            }
        #endregion
        }
    }