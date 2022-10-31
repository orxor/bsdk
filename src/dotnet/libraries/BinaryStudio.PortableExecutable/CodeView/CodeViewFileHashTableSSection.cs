using BinaryStudio.PortableExecutable.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class CodeViewFileHashTableSSection : CodeViewPrimarySSection
        {
        public override DEBUG_S Type { get { return DEBUG_S.DEBUG_S_FILECHKSMS; }}
        public IList<CodeViewFileHashValue> Values { get; }
        internal unsafe CodeViewFileHashTableSSection(CodeViewSection section, Int32 offset, Byte* content, Int32 length)
            : base(section, offset, content, length)
            {
            Values = new List<CodeViewFileHashValue>();
            var li = content + length;
            var fi = content;
            for (;content < li;) {
                var i = (CV8_FILE_INFO_HEADER*)content;
                var sz = sizeof(CV8_FILE_INFO_HEADER) + i->HashSize;
                var hashvalue = GetBytes(content + sizeof(CV8_FILE_INFO_HEADER), i->HashSize);
                MaxNameOffsetLength = Math.Max(MaxNameOffsetLength, i->FileNameOffset.ToString("X").Length);
                MaxHashTypeLength = Math.Max(MaxHashTypeLength, i->HashType.ToString().Length);
                MaxHashValueLength = Math.Max(MaxHashValueLength, hashvalue.Length);
                Values.Add(new CodeViewFileHashValue(
                    i->FileNameOffset,
                    hashvalue,
                    i->HashType));
                content += (Int32)Math.Ceiling(sz/4.0)*4;
                }
            Values = Values.ToArray();
            }

        #region M:GetBytes(Byte*,Int64):Byte[]
        private static unsafe Byte[] GetBytes(Byte* source, Int64 size)
            {
            if (source == null) { return null; }
            var r = new Byte[size];
            for (var i = 0;i < size;++i) {
                r[i] = source[i];
                }
            source += size;
            return r;
            }
        #endregion

        private readonly Int32 MaxNameOffsetLength;
        private readonly Int32 MaxHashTypeLength;
        private readonly Int32 MaxHashValueLength;
        }
    }