using BinaryStudio.PortableExecutable.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class CodeViewStringTableSSection : CodeViewPrimarySSection
        {
        public override DEBUG_S Type { get { return DEBUG_S.DEBUG_S_STRINGTABLE; }}
        public IDictionary<Int32, String> Values { get; }
        private readonly Int32 MaxOffsetValue = 0;
        private readonly Int32 MaxStringLength = 0;
        internal unsafe CodeViewStringTableSSection(CodeViewSection section, Int32 offset, Byte* content, Int32 length)
            : base(section, offset, content, length)
            {
            Values = new Dictionary<Int32, String>();
            var li = content + length;
            var fi = content;
            content++;
            while (content < li) {
                var key = (Int32)(content - fi);
                var val = ReadString(Encoding.UTF8, ref content);
                Values[key] = val;
                MaxOffsetValue = Math.Max(MaxOffsetValue, key.ToString("X").Length);
                MaxStringLength = Math.Max(MaxStringLength, CalculateLength(val));
                }
            }

        #region M:ReadString(Encoding,[Ref]Byte*):String
        private static unsafe String ReadString(Encoding encoding, ref Byte* source) {
            if (source == null) { return null; }
            var c = 0;
            for (;;++c) {
                if (source[c] == 0) {
                    break;
                    }
                }
            var r = new Byte[c];
            for (var i = 0;i < c;++i) {
                r[i] = source[i];
                }
            source += c + 1;
            return encoding.GetString(r);
            }
        #endregion
        }
    }