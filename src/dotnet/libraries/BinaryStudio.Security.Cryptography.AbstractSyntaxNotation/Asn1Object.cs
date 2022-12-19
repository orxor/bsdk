using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using BinaryStudio.IO;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public abstract class Asn1Object : IServiceProvider,IDisposable,IList<Asn1Object>,IJsonSerializable
        {
        [Flags]
        protected internal enum ObjectState : ushort
            {
            None                    = 0x000,
            ExplicitConstructed     = 0x001,
            ImplicitConstructed     = 0x002,
            Failed                  = 0x004,
            Indefinite              = 0x008,
            Decoded                 = 0x010,
            Disposed                = 0x020,
            SealedLength            = 0x040,
            SealedSize              = 0x080,
            DisposeUnderlyingObject = 0x100,
            DisposeContent          = 0x200,
            Incompleted             = 0x400
            }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Int64 offset;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Int64 size;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected Int64 length;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected ObjectState State;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected ReadOnlyMappingStream content;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private List<Asn1Object> sequence = new List<Asn1Object>();

        public abstract Asn1ObjectClass Class { get; }
        #if NET35
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected internal virtual Boolean IsDecoded { get { return ((State & ObjectState.Decoded)==ObjectState.Decoded); }}
        [Browsable(false)] public virtual Boolean IsFailed  { get { return ((State & ObjectState.Failed)==ObjectState.Failed);  }}
        [Browsable(false)] public virtual Boolean IsExplicitConstructed  { get { return ((State & ObjectState.ExplicitConstructed)==ObjectState.ExplicitConstructed); }}
        [Browsable(false)] public virtual Boolean IsImplicitConstructed  { get { return ((State & ObjectState.ImplicitConstructed)==ObjectState.ImplicitConstructed); }}
        [Browsable(false)] public virtual Boolean IsIndefiniteLength     { get { return ((State & ObjectState.Indefinite)==ObjectState.Indefinite); }}
        [Browsable(false)] protected internal virtual Boolean IsDisposed { get { return ((State & ObjectState.Disposed)==ObjectState.Disposed);     }}
        #else
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected internal virtual Boolean IsDecoded { get { return State.HasFlag(ObjectState.Decoded); }}
        [Browsable(false)] public virtual Boolean IsFailed  { get { return State.HasFlag(ObjectState.Failed);  }}
        [Browsable(false)] public virtual Boolean IsExplicitConstructed  { get { return State.HasFlag(ObjectState.ExplicitConstructed); }}
        [Browsable(false)] public virtual Boolean IsImplicitConstructed  { get { return State.HasFlag(ObjectState.ImplicitConstructed); }}
        [Browsable(false)] public virtual Boolean IsIndefiniteLength     { get { return State.HasFlag(ObjectState.Indefinite);          }}
        [Browsable(false)] protected internal virtual Boolean IsDisposed { get { return State.HasFlag(ObjectState.Disposed);            }}
        #endif

        protected internal abstract Object TypeCode { get; }
        public virtual ReadOnlyMappingStream Content { get { return content; }}
        public virtual Int64 Offset { get{ return offset; }}
        public virtual Int64 Size   { get {
            if (State.HasFlag(ObjectState.SealedSize)) { return size; }
            return size;
            }}

        public virtual Int64 Length { get {
            if (State.HasFlag(ObjectState.SealedLength)) { return length; }
            var c = Count;
            var r = 0L;
            if (c > 0)
                {
                foreach (var i in this)
                    {
                    r += i.Size;
                    }
                }
            else
                {
                r = content.Length;
                }
            length = r;
            State |= ObjectState.SealedLength;
            return length;
            }}

        #region M:IServiceProvider.GetService(Type):Object
        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="service">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="service"/>.-or- null if there is no service object of type <paramref name="service"/>.</returns>
        /// <filterpriority>2</filterpriority>
        public virtual Object GetService(Type service) {
            if (service == null) { return null; }
            if (service == GetType()) { return this; }
            if (service == typeof(Asn1Object)) { return this; }
            return null;
            }
        #endregion
        #region M:Load(ReadOnlyMappingStream):IEnumerable<Asn1Object>
        public static IEnumerable<Asn1Object> Load(ReadOnlyMappingStream source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            for (;;) {
                var offset = source.Position;
                var o = ReadNext(source, source.Position);
                if (o == null) { break; }
                if (!o.IsDecoded) {
                    if (o.IsFailed || !o.Decode()) {
                        break;
                        }
                    }
                yield return o;
                source.Position = offset + o.Size;
                }
            }
        #endregion
        #region M:Load(Stream):IEnumerable<Asn1Object>
        public static IEnumerable<Asn1Object> Load(Stream source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (source is ReadOnlyMappingStream ro) { return Load(ro); }
            return Load(ReadOnlyStream.Clone(source));
            }
        #endregion
        #region M:ReadNext(ReadOnlyMappingStream,Int64):Asn1Object
        protected static Asn1Object ReadNext(ReadOnlyMappingStream source, Int64 forceoffset) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            Asn1Object o = null;
            if (source.Position == source.Length) { return null; }
            var p = source.Position;
            var r = source.ReadByte();
            var c = (Asn1ObjectClass)((r & 0xc0) >> 6);
            var state = ((r & 0x20) == 0x20) ? ObjectState.ExplicitConstructed : ObjectState.None;
            switch (c) {
                case Asn1ObjectClass.Universal:
                    {
                    switch ((Asn1ObjectType)(r & 0x1f)) {
                        case Asn1ObjectType.EndOfContent:               { o = new Asn1EndOfContent            (); } break;
                        case Asn1ObjectType.Boolean:                    { o = new Asn1Boolean                 (); } break;
                        case Asn1ObjectType.Integer:                    { o = new Asn1Integer                 (); } break;
                        case Asn1ObjectType.BitString:                  { o = new Asn1BitString               (); } break;
                        case Asn1ObjectType.OctetString:                { o = new Asn1OctetString             (); } break;
                        case Asn1ObjectType.Null:                       { o = new Asn1Null                    (); } break;
                        case Asn1ObjectType.ObjectIdentifier:           { o = new Asn1ObjectIdentifier        (); } break;
                        case Asn1ObjectType.ObjectDescriptor:           { o = new Asn1ObjectDescriptor        (); } break;
                        case Asn1ObjectType.External:                   { o = new Asn1External                (); } break;
                        case Asn1ObjectType.Real:                       { o = new Asn1Real                    (); } break;
                        case Asn1ObjectType.Enum:                       { o = new Asn1Enum                    (); } break;
                        case Asn1ObjectType.EmbeddedPDV:                { o = new Asn1EmbeddedPDV             (); } break;
                        case Asn1ObjectType.RelativeObjectIdentifier:   { o = new Asn1RelativeObjectIdentifier(); } break;
                        case Asn1ObjectType.Sequence:                   { o = new Asn1Sequence                (); } break;
                        case Asn1ObjectType.Set:                        { o = new Asn1Set                     (); } break;
                        case Asn1ObjectType.Utf8String:                 { o = new Asn1Utf8String              (); } break;
                        case Asn1ObjectType.NumericString:              { o = new Asn1NumericString           (); } break;
                        case Asn1ObjectType.PrintableString:            { o = new Asn1PrintableString         (); } break;
                        case Asn1ObjectType.TeletexString:              { o = new Asn1TeletexString           (); } break;
                        case Asn1ObjectType.VideotexString:             { o = new Asn1VideotexString          (); } break;
                        case Asn1ObjectType.IA5String:                  { o = new Asn1IA5String               (); } break;
                        case Asn1ObjectType.UtcTime:                    { o = new Asn1UtcTime                 (); } break;
                        case Asn1ObjectType.GeneralTime:                { o = new Asn1GeneralTime             (); } break;
                        case Asn1ObjectType.GraphicString:              { o = new Asn1GraphicString           (); } break;
                        case Asn1ObjectType.VisibleString:              { o = new Asn1VisibleString           (); } break;
                        case Asn1ObjectType.GeneralString:              { o = new Asn1GeneralString           (); } break;
                        case Asn1ObjectType.UniversalString:            { o = new Asn1UniversalString         (); } break;
                        case Asn1ObjectType.UnicodeString:              { o = new Asn1UnicodeString           (); } break;
                        }
                    }
                    break;
                case Asn1ObjectClass.Application:     { o = new Asn1ApplicationObject    ((SByte)(r & 0x1f)); } break;
                case Asn1ObjectClass.ContextSpecific: { o = new Asn1ContextSpecificObject((SByte)(r & 0x1f)); } break;
                case Asn1ObjectClass.Private:         { o = new Asn1PrivateObject        ((SByte)(r & 0x1f)); } break;
                }
            if ((o == null) || (o.IsFailed)) {
                source.Seek(p, SeekOrigin.Begin);
                return null;
                }
            o.Load(source, forceoffset);
            if (o.IsFailed) {
                source.Seek(p, SeekOrigin.Begin);
                return null;
                }
            o.State |= state;
            if (o.IsIndefiniteLength) {
                if (!o.Decode()) {
                    source.Seek(p, SeekOrigin.Begin);
                    return null;
                    }
                }
            return o;
            }
        #endregion
        #region M:Decode:Boolean
        protected virtual Boolean Decode()
            {
            if (IsDecoded) { return true;  }
            if (IsFailed)  { return false; }
            try
                {
                if (IsIndefiniteLength)
                    {
                    var r = new List<Asn1Object>();
                    var sz = size;
                    var ln = 0L;
                    var flag = true;
                    for (;;)
                        {
                        var i = ReadNext(content, 0);
                        if (i == null)
                            {
                            flag = false;
                            break;
                            }
                        i.offset += offset;
                        if (i is Asn1EndOfContent)
                            {
                            sz += i.Size;
                            r.Add(i);
                            flag = true;
                            break;
                            }
                        sz += i.Size;
                        ln += i.Size;
                        r.Add(i);
                        }
                    if (flag) {
                        size   = sz;
                        length = ln;
                        if ((offset + length) > content.Length) {
                            State |= ObjectState.Failed;
                            return false;
                            }
                        content = content.Clone(offset, length);
                        State |= ObjectState.DisposeContent;
                        if (Decode(r))
                            {
                            sequence.AddRange(r);
                            State |= ObjectState.Decoded;
                            if (!IsExplicitConstructed)
                                {
                                State |= ObjectState.ImplicitConstructed;
                                }
                            return true;
                            }
                        }
                    if (!IsExplicitConstructed)
                        {
                        State |= ObjectState.Decoded;
                        return true;
                        }
                    }
                else
                    {
                    var r = new List<Asn1Object>();
                    var forceoffset = offset + size - length;
                    var flag = true;
                    var sz = length;
                    while (sz != 0)
                        {
                        var i = ReadNext(content, forceoffset);
                        if ((i == null) || (i is Asn1EndOfContent))
                            {
                            flag = false;
                            break;
                            }
                        r.Add(i);
                        sz -= i.Size;
                        if (sz < 0)
                            {
                            flag = false;
                            break;
                            }
                        }
                    if (flag)
                        {
                        if (Decode(r))
                            {
                            sequence.AddRange(r);
                            State |= ObjectState.Decoded;
                            if (!IsExplicitConstructed)
                                {
                                State |= ObjectState.ImplicitConstructed;
                                }
                            return true;
                            }
                        }
                    if (!IsExplicitConstructed)
                        {
                        State |= ObjectState.Decoded;
                        return true;
                        }
                    else
                        {
                        #if FEATURE_ASN1_INCOMPLETED
                        if (Decode(r))
                            {
                            sequence.AddRange(r);
                            state |= ObjectState.Decoded;
                            state |= ObjectState.ImplicitConstructed;
                            state |= ObjectState.Incompleted;
                            return true;
                            }
                        #endif
                        State |= ObjectState.Failed;
                        return false;
                        }
                    }
                State |= ObjectState.Failed;
                return false;
                }
            catch
                {
                State |= ObjectState.Failed;
                return false;
                }
            }
        #endregion
        #region M:Decode(IEnumerable<Asn1Object>):Boolean
        private static Boolean Decode(IEnumerable<Asn1Object> items)
            {
            #if FEATURE_MULTI_THREAD_PROCESSING && !NET35
            var r = new List<Task<Boolean>>();
            foreach (var item in items)
                {
                r.Add(Task.Factory.StartNew(item.Decode));
                }
            Task.WaitAll(r.OfType<Task>().ToArray());
            return r.All(i => i.Result);
            #else
            return items.All(item => item.Decode());
            #endif
            }
        #endregion
        #region M:DecodeLength(Stream):Int64
        internal static Int64 DecodeLength(Stream source) {
            var r = (Int64)source.ReadByte();
            if (r == 0) { return 0; }
            if ((r & 0x80) == 0x80) {
                var c = r & 0x7f;
                if (c == 0) { return -2; }
                if (c > 7)  { return -1; }
                r = 0L;
                for (var i = 0; i < c; ++i) {
                    r <<= 8;
                    r |= (Int64)source.ReadByte();
                    }
                }
            return r;
            }
        #endregion
        #region M:Load(ReadOnlyMappingStream,Int64)
        protected virtual void Load(ReadOnlyMappingStream source, Int64 forceoffset) {
            State |= ObjectState.Failed | ObjectState.SealedLength | ObjectState.SealedSize;
            offset = source.Position - 1;
            length = DecodeLength(source);
                 if (length == -1) { return; }
            else if (length == -2) {
                State |= ObjectState.Indefinite;
                size = 2;
                offset += forceoffset;
                content = source;
                State &= ~ObjectState.Failed;
                }
            else
                {
                size = source.Position - offset + length;
                if (offset + size > source.Length) { return; }
                offset += forceoffset;
                content = source.Clone(length);
                source.Seek(length, SeekOrigin.Current);
                State &= ~ObjectState.Failed;
                }
            }
        #endregion
        #region M:Dispose<T>({ref}T)
        protected static void Dispose<T>(ref T o)
            where T: IDisposable
            {
            if (o != null) {
                o.Dispose();
                o = default;
                }
            }
        #endregion
        #region M:Dispose<T>({ref}T[])
        protected static void Dispose<T>(ref T[] o)
            where T: IDisposable
            {
            if (o != null) {
                for (var i = 0; i < o.Length; i++) {
                    Dispose(ref o[i]);
                    }
                o = default;
                }
            }
        #endregion
        #region M:Dispose(Boolean)
        /// <summary>Releases the unmanaged resources used by the instance and optionally releases the managed resources.</summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing) {
            if (!IsDisposed) {
                lock(this) {
                    State |= ObjectState.Disposed;
                    if (sequence != null) {
                        for (var i = 0; i < sequence.Count; i++) {
                            sequence[i].Dispose();
                            sequence[i] = null;
                            }
                        sequence.Clear();
                        sequence = null;
                        }
                    content = null;
                    }
                }
            }
        #endregion
        #region M:Dispose
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }
        #endregion
        #region M:Finalize
        /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
        ~Asn1Object()
            {
            Dispose(false);
            }
        #endregion
        #region M:ToString:String
        /**
         * <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</summary>
         * <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
         */
        public override String ToString()
            {
            return Count > 0
                ? $"Count = {Count}"
                : $"Size  = {Length}";
            }
        #endregion

        #region M:IEnumerable<Asn1Object>.GetEnumerator:IEnumerator<Asn1Object>
        /**
         * <summary>Returns an enumerator that iterates through the collection.</summary>
         * <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
         * <filterpriority>1</filterpriority>
         * */
        public virtual IEnumerator<Asn1Object> GetEnumerator() { return sequence.GetEnumerator(); }
        #endregion
        #region M:IEnumerable.GetEnumerator:IEnumerator
        /**
         * <summary>Returns an enumerator that iterates through a collection.</summary>
         * <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
         * <filterpriority>2</filterpriority>
         * */
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        #endregion
        #region M:ICollection<Asn1Object>.Add(Asn1Object)
        protected void Add(Asn1Object item)
            {
            if (IsReadOnly) { throw new InvalidOperationException(); }
            sequence.Add(item);
            }

        /**
         * <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
         * <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
         * <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
         * */
        void ICollection<Asn1Object>.Add(Asn1Object item)
            {
            Add(item);
            }
        #endregion
        #region M:ICollection<Asn1Object>.Clear
        protected void Clear() {
            if (IsReadOnly) { throw new InvalidOperationException(); }
            sequence.Clear();
            }

        /**
         * <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
         * <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
         * */
        void ICollection<Asn1Object>.Clear()
            {
            Clear();
            }
        #endregion
        #region M:ICollection<Asn1Object>.Contains(IAsn1Object):Boolean
        /**
         * <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.</summary>
         * <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
         * <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.</returns>
         * */
        Boolean ICollection<Asn1Object>.Contains(Asn1Object item)
            {
            return sequence.Contains(item);
            }
        #endregion
        #region M:ICollection<Asn1Object>.CopyTo(IAsn1Object[],Int32)
        /**
         * <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.</summary>
         * <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="Array"/> must have zero-based indexing.</param>
         * <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
         * <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
         * <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
         * <exception cref="ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
         * */
        void ICollection<Asn1Object>.CopyTo(Asn1Object[] array, Int32 arrayIndex)
            {
            sequence.CopyTo(array, arrayIndex);
            }
        #endregion
        #region M:ICollection<Asn1Object>.Remove(IAsn1Object):Boolean
        /**
         * <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
         * <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
         * <returns>true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
         * <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
         * */
        Boolean ICollection<Asn1Object>.Remove(Asn1Object item)
            {
            if (IsReadOnly) { throw new NotSupportedException(); }
            return sequence.Remove(item);
            }
        #endregion
        #region P:ICollection<Asn1Object>.Count:Int32
        /**
         * <summary>Gets the number of elements in the collection.</summary>
         * <returns>The number of elements in the collection.</returns>
         * */
        public virtual Int32 Count { get {
            if (IsDisposed) { throw new ObjectDisposedException("this"); }
            return sequence.Count;
            }}
        #endregion
        #region P:ICollection<Asn1Object>.IsReadOnly:Boolean
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected Boolean IsReadOnly { get;set; }

        /**
         * <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</summary>
         * <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
         * */
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] Boolean ICollection<Asn1Object>.IsReadOnly { get {
            return IsReadOnly;
            }}
        #endregion
        #region M:IList<Asn1Object>.IndexOf(IAsn1Object):Int32
        /**
         * <summary>Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.</summary>
         * <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
         * <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
         * */
        Int32 IList<Asn1Object>.IndexOf(Asn1Object item)
            {
            return sequence.IndexOf(item);
            }
        #endregion
        #region M:IList<Asn1Object>.Insert(Int32,IAsn1Object)
        /**
         * <summary>Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.</summary>
         * <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
         * <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
         * <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
         * <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
         * */
        void IList<Asn1Object>.Insert(Int32 index, Asn1Object item)
            {
            if (IsReadOnly) { throw new NotSupportedException(); }
            sequence.Insert(index, item);
            }
        #endregion
        #region M:IList<Asn1Object>.RemoveAt(Int32)
        /**
         * <summary>Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.</summary>
         * <param name="index">The zero-based index of the item to remove.</param>
         * <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
         * <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
         * */
        void IList<Asn1Object>.RemoveAt(Int32 index)
            {
            if (IsReadOnly) { throw new NotSupportedException(); }
            sequence.RemoveAt(index);
            State &= ~ObjectState.SealedSize;
            State &= ~ObjectState.SealedLength;
            }
        #endregion
        #region P:IList<Asn1Object>.this[Int32]:IAsn1Object
        /**
         * <summary>Gets or sets the element at the specified index.</summary>
         * <param name="index">The zero-based index of the element to get or set.</param>
         * <returns>The element at the specified index.</returns>
         * <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
         * <exception cref="NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
         * */
        public virtual Asn1Object this[Int32 index] {
            get
                {
                if (IsDisposed) { throw new ObjectDisposedException("this"); }
                return sequence[index];
                }
            set
                {
                if (IsReadOnly) { throw new NotSupportedException(); }
                sequence[index] = value;
                State &= ~ObjectState.SealedSize;
                State &= ~ObjectState.SealedLength;
                }
            }
        #endregion
        #region M:WriteTo(IJsonWriter)
        public virtual void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(Class), Class.ToString());
                writer.WriteValue(nameof(Type), TypeCode);
                if (Offset >= 0) { writer.WriteValue(nameof(Offset), Offset); }
                var c = Count;
                if (c > 0) {
                    writer.WritePropertyName("{Self}");
                    using (writer.ArrayObject()) {
                        foreach (var Value in this) {
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
        public virtual void WriteTo(Stream target) {
            if (IsIndefiniteLength)
                {
                Content.Seek(0, SeekOrigin.Begin);
                Content.CopyTo(target);
                target.WriteByte(0);
                target.WriteByte(0);
                target.WriteByte(0);
                target.WriteByte(0);
                }
            else
                {
                WriteHeader(target);
                WriteContent(target);
                }
            }
        #endregion
        #region M:WriteContent(Stream)
        protected virtual void WriteContent(Stream target) {
            Content.Seek(0, SeekOrigin.Begin);
            Content.CopyTo(target);
            if (IsIndefiniteLength)
                {
                target.WriteByte(0);
                target.WriteByte(0);
                target.WriteByte(0);
                target.WriteByte(0);
                }
            }
        #endregion
        #region M:WriteHeader(Stream target,Boolean,Asn1ObjectClass,SByte,Int64)
        protected static void WriteHeader(Stream target, Boolean constructed, Asn1ObjectClass @class, SByte type, Int64 length)
            {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            var r = constructed ? 0x20 : 0x00;
            r |= ((Byte)@class) << 6;
            r |= (Byte)type;
            target.WriteByte((Byte)r);
            if (length < 0)     {
                target.WriteByte((Byte)0x80);
                return;
                }
            if (length < 0x80) { target.WriteByte((Byte)length); }
            else
                {
                var n = new List<Byte>();
                while (length > 0) {
                    n.Add((Byte)(length & 0xFF));
                    length >>= 8;
                    }
                var c = n.Count;
                target.WriteByte((Byte)(c | 0x80));
                for (var i = c - 1;i >= 0; i--) {
                    target.WriteByte(n[i]);
                    }
                }
            }
        #endregion

        protected internal static IDisposable ReadLock(ReaderWriterLockSlim o)            { return new ReadLockScope(o);            }
        protected internal static IDisposable WriteLock(ReaderWriterLockSlim o)           { return new WriteLockScope(o);           }
        protected internal static IDisposable UpgradeableReadLock(ReaderWriterLockSlim o) { return new UpgradeableReadLockScope(o); }

        private class ReadLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public ReadLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterReadLock();
                }

            public void Dispose()
                {
                o.ExitReadLock();
                o = null;
                }
            }

        private class UpgradeableReadLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public UpgradeableReadLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterUpgradeableReadLock();
                }

            public void Dispose()
                {
                o.ExitUpgradeableReadLock();
                o = null;
                }
            }

        private class WriteLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public WriteLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterWriteLock();
                }

            public void Dispose()
                {
                o.ExitWriteLock();
                o = null;
                }
            }

        protected internal abstract void WriteHeader(Stream target);
        }
    }