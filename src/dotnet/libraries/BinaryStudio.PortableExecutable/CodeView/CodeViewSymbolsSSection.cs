using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class CodeViewSymbolsSSection : CodeViewPrimarySSection
        {
        public override DEBUG_S Type { get { return DEBUG_S.DEBUG_S_SYMBOLS; }}
        public CV_CPU_TYPE? Machine { get;internal set; }
        public IList<CodeViewSymbol> Symbols { get; }
        internal unsafe CodeViewSymbolsSSection(CodeViewSection section, Int32 offset, Byte* content, Int32 length)
            : base(section, offset, content, length)
            {
            Machine = section.CommonObjectFile.CPU;
            Symbols = new List<CodeViewSymbol>();
            try
                {
                var r = content;
                while (length > 0) {
                    var header = (CODEVIEW_SYMBOL_RECORD_HEADER*)r;
                    length -= header->Length + sizeof(Int16);
                    Symbols.Add(CodeViewSymbol.From(
                        this,
                        offset,
                        header->Type,
                        (Byte*)(header + 1),
                        header->Length - sizeof(Int16)));
                    r += sizeof(CODEVIEW_SYMBOL_RECORD_HEADER);
                    r += header->Length - sizeof(Int16);
                    offset += sizeof(CODEVIEW_SYMBOL_RECORD_HEADER) + header->Length - sizeof(Int16);
                    }
                }
            finally
                {
                Symbols = Symbols.ToArray();
                }
            }

        #region M:ToString(DEBUG_TYPE_ENUM):String
        protected static String ToString(DEBUG_TYPE_ENUM value) {
            return (value >= CV_FIRST_NONPRIM)
                ? ((UInt32)value).ToString("X6")
                : value.ToString();
            }
        #endregion
        #region M:ToString(DEBUG_SYMBOL_INDEX):String
        protected static String ToString(DEBUG_SYMBOL_INDEX value) {
            return (Enum.IsDefined(typeof(DEBUG_SYMBOL_INDEX), value))
                ? value.ToString()
                : $"DEBUG_SYMBOL({(UInt16)value:X4})";
            }
        #endregion
        #region M:JsonCalculateStringLength(String):Int32
        private static Int32 JsonCalculateStringLength(String value) {
            var r = 0;
            if (value != null) {
                var c = value.Length;
                for (var i = 0; i < c; i++) {
                    r++;
                    switch(value[i]) {
                        case '"' :
                        case '\\':
                            {
                            r++;
                            }
                            break;
                        }
                    }
                }
            return r;
            }
        #endregion

        protected override void WriteTextHeader(Int32 offset, TextWriter writer) {
            base.WriteTextHeader(offset, writer);
            var IndentString = new String(' ', offset);
            writer.WriteLine($"{IndentString}{Symbols.Count,8:X}: number of symbols");
            }

        protected override unsafe void WriteTextBody(Int32 offset, TextWriter writer) {
            base.WriteTextBody(offset, writer);
            var IndentString = new String(' ', offset);
            foreach (var g in Symbols.GroupBy(i => i.Type)) {
                writer.WriteLine($"\n{IndentString}  SYMBOLS({g.Key})");
                switch(g.Key) {
                    case DEBUG_SYMBOL_INDEX.S_CONSTANT:
                        {
                        var T = stackalloc[] { 7, 10 };
                        foreach (var symbol in g.OfType<S_CONSTANT>()) {
                            if (symbol.FieldValueType != null) {
                                T[0] = Math.Max(T[0], symbol.FieldValueType.GetValueOrDefault().ToString().Length);
                                }
                            T[1] = Math.Max(T[1], symbol.FieldName.Length);
                            }
                        writer.WriteLine($"{IndentString}           Field Type   Value    {String.Format($"{{0,-{T[0]}}}", " Value")}");
                        writer.WriteLine($"{IndentString}    Offset   Index      Length   {String.Format($"{{0,-{T[0]}}}", " Type")} Field Name");
                        writer.WriteLine($"{IndentString}    ------ ---------- --------- {new String('-', T[0])} {new String('-', T[1])}");
                        foreach (var symbol in g.OfType<S_CONSTANT>()) {
                            var r = new StringBuilder();
                            r.Append($"{IndentString}    {symbol.Offset:X6}   {(UInt32)symbol.FieldTypeIndex:X6}   {symbol.FieldValue.Length,9:X3} ");
                            r.Append((symbol.FieldValueType == null)
                                ? new String(' ', T[0])
                                : String.Format($"{{0,-{T[0]}}}", ((LEAF_ENUM)symbol.FieldValueType).ToString()));
                            r.Append(' ');
                            r.Append(symbol.FieldName);
                            writer.WriteLine(r);
                            }
                        }
                        break;
                    case DEBUG_SYMBOL_INDEX.S_UNAMESPACE:
                        {
                        var T = stackalloc[] { 11 };
                        foreach (var symbol in g.OfType<S_UNAMESPACE>()) {
                            T[0] = Math.Max(T[0], symbol.Value.Length);
                            }
                        writer.WriteLine($"{IndentString}    Offset  Namespace");
                        writer.WriteLine($"{IndentString}    ------ {new String('-', T[0])}");
                        foreach (var symbol in g.OfType<S_UNAMESPACE>()) {
                            writer.WriteLine($"{IndentString}    {symbol.Offset:X6}  {symbol.Value}");
                            }
                        }
                        break;
                    }
                }
            }

        public Object DecodeRegister(UInt16 value) {
            if (Machine != null) {
                switch (Machine.Value) {
                    case CV_CPU_TYPE.CPU_8080:
                    case CV_CPU_TYPE.CPU_8086:
                    case CV_CPU_TYPE.CPU_80286:
                    case CV_CPU_TYPE.CPU_80386:
                    case CV_CPU_TYPE.CPU_80486:
                    case CV_CPU_TYPE.CPU_PENTIUM:
                    case CV_CPU_TYPE.CPU_PENTIUMII:
                    case CV_CPU_TYPE.CPU_PENTIUMIII:
                        {
                        return (CV_REG)value;
                        }
                    case CV_CPU_TYPE.CPU_MIPS:
                    case CV_CPU_TYPE.CPU_MIPS16:
                    case CV_CPU_TYPE.CPU_MIPS32:
                    case CV_CPU_TYPE.CPU_MIPS64:
                    case CV_CPU_TYPE.CPU_MIPSI:
                    case CV_CPU_TYPE.CPU_MIPSII:
                    case CV_CPU_TYPE.CPU_MIPSIII:
                    case CV_CPU_TYPE.CPU_MIPSIV:
                    case CV_CPU_TYPE.CPU_MIPSV:
                        {
                        return (CV_M4)value;
                        }
                    case CV_CPU_TYPE.CPU_M68000:
                    case CV_CPU_TYPE.CPU_M68010:
                    case CV_CPU_TYPE.CPU_M68020:
                    case CV_CPU_TYPE.CPU_M68030:
                    case CV_CPU_TYPE.CPU_M68040:
                        {
                        return (CV_R68)value;
                        }
                    case CV_CPU_TYPE.CPU_ALPHA:
                    case CV_CPU_TYPE.CPU_ALPHA_21164:
                    case CV_CPU_TYPE.CPU_ALPHA_21164A:
                    case CV_CPU_TYPE.CPU_ALPHA_21264:
                    case CV_CPU_TYPE.CPU_ALPHA_21364:
                        {
                        return (CV_ALPHA)value;
                        }
                    case CV_CPU_TYPE.CPU_PPC601:
                    case CV_CPU_TYPE.CPU_PPC603:
                    case CV_CPU_TYPE.CPU_PPC604:
                    case CV_CPU_TYPE.CPU_PPC620:
                    case CV_CPU_TYPE.CPU_PPCFP:
                    case CV_CPU_TYPE.CPU_PPCBE:
                        {
                        return (CV_PPC)value;
                        }
                    case CV_CPU_TYPE.CPU_SH3:
                    case CV_CPU_TYPE.CPU_SH3E:
                    case CV_CPU_TYPE.CPU_SH3DSP:
                    case CV_CPU_TYPE.CPU_SH4:
                        {
                        return (CV_SH3)value;
                        }
                    case CV_CPU_TYPE.CPU_SHMEDIA:
                        {
                        return (CV_SHMEDIA)value;
                        }
                    case CV_CPU_TYPE.CPU_ARM3:
                    case CV_CPU_TYPE.CPU_ARM4:
                    case CV_CPU_TYPE.CPU_ARM4T:
                    case CV_CPU_TYPE.CPU_ARM5:
                    case CV_CPU_TYPE.CPU_ARM5T:
                    case CV_CPU_TYPE.CPU_ARM6:
                    case CV_CPU_TYPE.CPU_ARM_XMAC:
                    case CV_CPU_TYPE.CPU_ARM_WMMX:
                    case CV_CPU_TYPE.CPU_ARM7:
                    case CV_CPU_TYPE.CPU_THUMB:
                    case CV_CPU_TYPE.CPU_ARMNT:
                        {
                        return (CV_ARM)value;
                        }
                    case CV_CPU_TYPE.CPU_IA64:
                    case CV_CPU_TYPE.CPU_IA64_2:
                        {
                        return (CV_IA64)value;
                        }
                    case CV_CPU_TYPE.CPU_AM33:
                        {
                        return (CV_AM33)value;
                        }
                    case CV_CPU_TYPE.CPU_M32R:
                        {
                        return (CV_M32R)value;
                        }
                    case CV_CPU_TYPE.CPU_TRICORE:
                        {
                        return (CV_TRI)value;
                        }
                    case CV_CPU_TYPE.CPU_ARM64:
                        {
                        return (CV_ARM64)value;
                        }
                    case CV_CPU_TYPE.CPU_X64:
                        {
                        return (CV_AMD64)value;
                        }
                    case CV_CPU_TYPE.CPU_D3D11_SHADER:
                    case CV_CPU_TYPE.CPU_OMNI:
                    case CV_CPU_TYPE.CPU_CEE:
                    case CV_CPU_TYPE.CPU_EBC:
                        {
                        return value.ToString("X4");
                        }
                    }
                }
            return value.ToString("X4");
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return Type.ToString();
            }

        private const DEBUG_TYPE_ENUM CV_FIRST_NONPRIM = (DEBUG_TYPE_ENUM)0x1000;
        }
    }