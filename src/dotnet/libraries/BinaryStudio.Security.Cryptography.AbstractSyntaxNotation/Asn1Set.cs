using System.IO;
using BinaryStudio.IO;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="SET"/> type.
    /// </summary>
    public sealed class Asn1Set : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Set"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Set; }}

        #region ctor
        internal Asn1Set()
            {
            }
        #endregion
        #region ctor{{params}Asn1Object[]}
        internal Asn1Set(params Asn1Object[] args)
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