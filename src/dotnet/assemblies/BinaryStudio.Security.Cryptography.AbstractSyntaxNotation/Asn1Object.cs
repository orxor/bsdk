using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Threading;
using BinaryStudio.IO;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public abstract class Asn1Object : IServiceProvider,IDisposable
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
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Int64 length;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private ObjectState state;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected ReadOnlyMappingStream content;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private List<Asn1Object> sequence = new List<Asn1Object>();

        public abstract Asn1ObjectClass Class { get; }
        #if NET35
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected internal virtual Boolean IsDecoded { get { return ((state & ObjectState.Decoded)==ObjectState.Decoded); }}
        [Browsable(false)] public virtual Boolean IsFailed  { get { return ((state & ObjectState.Failed)==ObjectState.Failed);  }}
        [Browsable(false)] public virtual Boolean IsExplicitConstructed  { get { return ((state & ObjectState.ExplicitConstructed)==ObjectState.ExplicitConstructed); }}
        [Browsable(false)] public virtual Boolean IsImplicitConstructed  { get { return ((state & ObjectState.ImplicitConstructed)==ObjectState.ImplicitConstructed); }}
        [Browsable(false)] public virtual Boolean IsIndefiniteLength     { get { return ((state & ObjectState.Indefinite)==ObjectState.Indefinite); }}
        [Browsable(false)] protected internal virtual Boolean IsDisposed { get { return ((state & ObjectState.Disposed)==ObjectState.Disposed);     }}
        #else
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected internal virtual Boolean IsDecoded { get { return state.HasFlag(ObjectState.Decoded); }}
        [Browsable(false)] public virtual Boolean IsFailed  { get { return state.HasFlag(ObjectState.Failed);  }}
        [Browsable(false)] public virtual Boolean IsExplicitConstructed  { get { return state.HasFlag(ObjectState.ExplicitConstructed); }}
        [Browsable(false)] public virtual Boolean IsImplicitConstructed  { get { return state.HasFlag(ObjectState.ImplicitConstructed); }}
        [Browsable(false)] public virtual Boolean IsIndefiniteLength     { get { return state.HasFlag(ObjectState.Indefinite);          }}
        [Browsable(false)] protected internal virtual Boolean IsDisposed { get { return state.HasFlag(ObjectState.Disposed);            }}
        #endif
        public virtual ReadOnlyMappingStream Content { get { return content; }}
        public virtual Int64 Size   { get {
            if (state.HasFlag(ObjectState.SealedSize)) { return size; }
            return size;
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
            o.state |= state;
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
        protected internal virtual Boolean Decode()
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
                            state |= ObjectState.Failed;
                            return false;
                            }
                        content = content.Clone(offset, length);
                        state |= ObjectState.DisposeContent;
                        if (Decode(r))
                            {
                            sequence.AddRange(r);
                            state |= ObjectState.Decoded;
                            if (!IsExplicitConstructed)
                                {
                                state |= ObjectState.ImplicitConstructed;
                                }
                            return true;
                            }
                        }
                    if (!IsExplicitConstructed)
                        {
                        state |= ObjectState.Decoded;
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
                            state |= ObjectState.Decoded;
                            if (!IsExplicitConstructed)
                                {
                                state |= ObjectState.ImplicitConstructed;
                                }
                            return true;
                            }
                        }
                    if (!IsExplicitConstructed)
                        {
                        state |= ObjectState.Decoded;
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
                        state |= ObjectState.Failed;
                        return false;
                        }
                    }
                state |= ObjectState.Failed;
                return false;
                }
            catch
                {
                state |= ObjectState.Failed;
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
            state |= ObjectState.Failed | ObjectState.SealedLength | ObjectState.SealedSize;
            offset = source.Position - 1;
            length = DecodeLength(source);
                 if (length == -1) { return; }
            else if (length == -2) {
                state |= ObjectState.Indefinite;
                size = 2;
                offset += forceoffset;
                content = source;
                state &= ~ObjectState.Failed;
                }
            else
                {
                size = source.Position - offset + length;
                if (offset + size > source.Length) { return; }
                offset += forceoffset;
                content = source.Clone(length);
                source.Seek(length, SeekOrigin.Current);
                state &= ~ObjectState.Failed;
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
        /// <summary>
        /// Releases the unmanaged resources used by the instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing) {
            if (!IsDisposed) {
                lock(this) {
                    state |= ObjectState.Disposed;
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
        }
    }