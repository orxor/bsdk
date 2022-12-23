using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BinaryStudio.Security.Cryptography
    {
    internal static class Extensions
        {
        #region M:PositionScope({this}Stream):IDisposable
        private class P : IDisposable
            {
            private Stream source;
            private Int64 position;
            public P(Stream source)
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

        public static IDisposable PositionScope(this Stream source) {
            if (source == null)  { throw new ArgumentNullException(nameof(source)); }
            if (!source.CanSeek) { throw new ArgumentOutOfRangeException(nameof(source)); }
            return new P(source);
            }
        #endregion
        }
    }
