using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BinaryStudio.PlatformComponents
    {
    public class LocalMemoryManager : IDisposable
        {
        private readonly IList<LocalMemory> values = new List<LocalMemory>();

        public unsafe void* Alloc(Int32 size)
            {
            var r = new LocalMemory(size);
            values.Add(r);
            return r;
            }

        public unsafe void* Alloc(Byte[] block) {
            var r = new LocalMemory(block.Length);
            var target = (Byte*)r;
            for (var i = 0; i < block.Length; i++) {
                target[i] = block[i];
                }
            values.Add(r);
            return r;
            }

        #region M:StringToMem(String,Encoding):void*
        public unsafe void* StringToMem(String value, Encoding encoding) {
            return StringToMem(value,encoding,out var size);
            }
        #endregion
        #region M:StringToMem(String,Encoding,{out}Int32):void*
        public unsafe void* StringToMem(String value, Encoding encoding, out Int32 c) {
            c = 0;
            if (value == null) { return null; }
            var bytes = encoding.GetBytes(value);
            c = bytes.Length;
            var r = (Byte*)Alloc(c + 1);
            for (var i = 0; i < c; i++)
                {
                r[i] = bytes[i];
                }
            return r;
            }
        #endregion

        #region M:Dispose(Boolean)
        private void Dispose(Boolean disposing) {
            //Debug.Print($"LocalMemoryManager.Dispose({disposing})");
            }
        #endregion
        #region M:Dispose
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }
        #endregion
        #region F:Finalize
        ~LocalMemoryManager()
            {
            Dispose(false);
            }
        #endregion
        }
    }