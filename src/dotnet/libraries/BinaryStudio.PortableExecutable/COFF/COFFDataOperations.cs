using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryStudio.PortableExecutable
    {
    internal static class COFFDataOperations
        {
        #region M:ReadInt32({ref}IntPtr):Int32
        public static unsafe Int32 ReadInt32(ref Byte* source) {
            var r = *(Int32*)source;
            source += sizeof(Int32);
            return r;
            }
        #endregion
        #region M:ReadByte({ref}IntPtr):Byte
        public static unsafe Byte ReadByte(ref Byte* source) {
            return *source++;
            }
        #endregion
        #region M:ReadZeroTerminatedString({ref}IntPtr,Encoding):String
        public static unsafe String ReadZeroTerminatedString(ref Byte* source,Encoding encoding) {
            if (source == null) { return null; }
            var r = new List<Byte>();
            while (*source != 0) {
                r.Add(*source++);
                }
            source++;
            return encoding.GetString(r.ToArray());
            }
        #endregion
        }
    }