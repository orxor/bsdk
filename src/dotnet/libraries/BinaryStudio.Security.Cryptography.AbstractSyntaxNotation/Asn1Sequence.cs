using System.IO;
using BinaryStudio.IO;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="SEQUENCE"/> type.
    /// </summary>
    public sealed class Asn1Sequence : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Sequence"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Sequence; }}

        #region ctor
        internal Asn1Sequence()
            {
            }
        #endregion
        #region ctor{{params}Asn1Object[]}
        internal Asn1Sequence(params Asn1Object[] args)
            {
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
            State |= ObjectState.Decoded|ObjectState.ExplicitConstructed;
            }
        #endregion
        }
    }