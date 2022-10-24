using System;
using System.Diagnostics;
using System.Text;

namespace BinaryStudio.PortableExecutable
    {
    internal static class Extensions
        {
        public static unsafe String GetString(this Encoding source, Byte* bytes, Int32 count) {
            try
                {
                var r = new Byte[count];
                for (var i = 0; i < count; i++) {
                    r[i] = bytes[i];
                    }
                return source.GetString(r);
                }
            catch (Exception e)
                {
                Debug.WriteLine(e);
                throw;
                }
            }
        }

    }