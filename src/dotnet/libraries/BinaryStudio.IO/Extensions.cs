using System;
using System.IO;

namespace BinaryStudio.IO
    {
    public static class Extensions
        {
        #region M:CopyTo({this}Stream,Stream)
        #if NET35
        public static void CopyTo(this Stream source, Stream target)
            {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            var buffer = new Byte[8192];
            while (true) {
                var size = source.Read(buffer, 0, 8192);
                if (size == 0) { break; }
                target.Write(buffer, 0, size);
                }
            }
        #endif
        #endregion
        #region M:CopyTo({this}Stream,Stream.Int32,Int64)
        public static void CopyTo(this Stream source, Stream target, Int32 buffersize, Int64 length) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            if (buffersize < 1) { throw new ArgumentOutOfRangeException(nameof(buffersize)); }
            if (length < 1)     { throw new ArgumentOutOfRangeException(nameof(length));     }
            var buffer = new Byte[buffersize];
            while (length > 0) {
                var size = source.Read(buffer, 0, (Int32)Math.Min((Int64)buffersize,length));
                if (size == 0) { break; }
                target.Write(buffer, 0, size);
                length -= size;
                }
            }
        #endregion
        #region M:Write({this}Stream,Byte*,Int32)
        public static unsafe void Write(this Stream target, Byte* buffer, Int32 length) {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            if (buffer == null) { throw new ArgumentNullException(nameof(buffer)); }
            if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
            var r = new Byte[length];
            for (var i = 0; i < length; i++) {
                r[i] = buffer[i];
                }
            target.Write(r,0,length);
            }
        #endregion
        #region M:Write({this}Stream,Int32)
        public static unsafe void Write(this Stream target, Int32 value) {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            var r = new Byte[sizeof(Int32)];
            fixed (Byte* o = r) {
                *((Int32*)o) = value;
                target.Write(r,0,r.Length);
                }
            }
        #endregion

        public static IDisposable StorePosition(this BinaryReader source)
            {
            return new PositionScope(source.BaseStream);
            }

        public static IDisposable StorePosition(this Stream source)
            {
            return new PositionScope(source);
            }

        private class PositionScope : IDisposable
            {
            private Stream source;
            private readonly Int64 position;
            public PositionScope(Stream source)
                {
                this.source = source;
                position = source.Position;
                }

            public void Dispose()
                {
                source.Position = position;
                source = null;
                }
            }
        }
    }