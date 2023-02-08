using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using BinaryStudio.IO;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public abstract class Asn1LinkObject<T> : Asn1Object
        where T: Asn1Object
        {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)][Browsable(false)] private T U;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)][Browsable(false)] public virtual Asn1Object UnderlyingObject { get { return U; }}
        [DebuggerBrowsable(DebuggerBrowsableState.Never)][Browsable(false)] public sealed override Asn1ObjectClass Class { get { return UnderlyingObject.Class; }}
        [DebuggerBrowsable(DebuggerBrowsableState.Never)][Browsable(false)] public sealed override Int64 Offset { get { return UnderlyingObject.Offset; }}
        [DebuggerBrowsable(DebuggerBrowsableState.Never)][Browsable(false)] public sealed override Int64 Length { get { return UnderlyingObject.Length; }}
        [DebuggerBrowsable(DebuggerBrowsableState.Never)][Browsable(false)] public sealed override Int64 Size   { get { return UnderlyingObject.Size;   }}
        [DebuggerBrowsable(DebuggerBrowsableState.Never)][Browsable(false)] public sealed override Int32 Count  { get { return UnderlyingObject.Count;  }}
        [DebuggerBrowsable(DebuggerBrowsableState.Never)][Browsable(false)] public sealed override ReadOnlyMappingStream Content { get { return UnderlyingObject?.Content; }}
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]protected internal override Object TypeCode { get { return $"Complex{{{U.TypeCode}}}"; }}

        protected Asn1LinkObject(T o)
            {
            if (ReferenceEquals(o, null)) { throw new ArgumentNullException(nameof(o)); }
            if (o.IsFailed) { throw new ArgumentException(nameof(o)); }
            U = o;
            }

        #region P:IList<Asn1Object>.this[Int32]:Asn1Object
        /**
         * <summary>Gets or sets the element at the specified index.</summary>
         * <param name="index">The zero-based index of the element to get or set.</param>
         * <returns>The element at the specified index.</returns>
         * <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
         * <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
         * */
        public override Asn1Object this[Int32 index]
            {
            get { return UnderlyingObject[index];  }
            set { UnderlyingObject[index] = value; }
            }
        #endregion
        #region M:Dispose(Boolean)
        /// <summary>Releases the unmanaged resources used by the instance and optionally releases the managed resources.</summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposing) {
            lock(this) {
                if (!IsDisposed) {
                    U = null;
                    base.Dispose(disposing);
                    }
                }
            }
        #endregion
        #region M:WriteTo(IJsonWriter)
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(Class), Class.ToString());
                writer.WriteValue(nameof(Type), TypeCode);
                if (Offset >= 0) { writer.WriteValue(nameof(Offset), Offset); }
                var c = Count;
                if (c > 0) {
                    writer.WritePropertyName("{Self}");
                    using (writer.Array()) {
                        foreach (var Value in UnderlyingObject) {
                            Value.WriteTo(writer);
                            }
                        }
                    }
                else
                    {
                    writer.WriteValue(nameof(Content),Convert.ToBase64String(Content.ToArray(), Base64FormattingOptions.InsertLineBreaks).Split('\n'));
                    }
                }
            }
        #endregion
        #region M:WriteTo(Stream)
        public override void WriteTo(Stream target) {
            UnderlyingObject.WriteTo(target);
            }
        #endregion
        #region M:WriteHeader(Stream)
        protected internal override void WriteHeader(Stream target) {
            UnderlyingObject.WriteHeader(target);
            }
        #endregion
        }

    public abstract class Asn1LinkObject : Asn1LinkObject<Asn1Object>
        {
        protected Asn1LinkObject(Asn1Object o)
            : base(o)
            {
            }
        }
    }