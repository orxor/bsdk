﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class CodeViewSymbol : IJsonSerializable,IFileDumpSupport
        {
        public ICodeViewNameTable NameTable { get;internal set; }
        public CodeViewSymbolsSSection Section { get; }
        public virtual DEBUG_SYMBOL_INDEX Type { get; }
        public virtual Byte[] Content { get; }
        public Int32 Offset { get; }
        public CodeViewSymbolStatus Status { get;protected set; }
        protected virtual Encoding Encoding { get{
            return (Section != null)
                ? Section.Section.Encoding
                : Encoding.ASCII;
            }}

        protected virtual Boolean IsLengthPrefixedString { get {
            return (Section != null)
                ? Section.Section.IsLengthPrefixedString
                : true;
            }}

        private static readonly IDictionary<DEBUG_SYMBOL_INDEX,Type> Types = new Dictionary<DEBUG_SYMBOL_INDEX, Type>();
        static CodeViewSymbol() {
            foreach (var type in typeof(CodeViewSymbol).Assembly.GetTypes()) {
                var key = type.GetCustomAttributes(false).OfType<CodeViewSymbolAttribute>().FirstOrDefault();
                if (key != null) {
                    Types.Add(key.Key, type);
                    }
                }
            }

        private unsafe CodeViewSymbol(CodeViewSymbolsSSection Section, Int32 Offset, DEBUG_SYMBOL_INDEX Type, Byte* Content, Int32 Length)
            :this(Section, Offset, (IntPtr)Content, Length)
            {
            this.Type = Type;
            }

        protected unsafe CodeViewSymbol(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length) {
            if (Content == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(Content)); }
            this.Section = Section;
            this.Offset = Offset;
            var c = (Byte*)Content;
            var r = new Byte[Length];
            for (var i = 0; i < Length; ++i) {
                r[i] = c[i];
                }
            this.Content = r;
            MaxSizeLength = Math.Max(MaxSizeLength, r.Length.ToString("X").Length);
            Status = CodeViewSymbolStatus.HasFileDumpWrite;
            }

        public static unsafe CodeViewSymbol From(CodeViewSymbolsSSection Section, Int32 Offset, DEBUG_SYMBOL_INDEX Index, Byte* Content, Int32 Length, IDictionary<DEBUG_SYMBOL_INDEX,Type> Mapping = null) {
            Type type;
            if (Mapping != null) {
                if (Mapping.TryGetValue(Index,out type)) {
                    return (CodeViewSymbol)Activator.CreateInstance(type,
                        Section,
                        Offset,
                        (IntPtr)Content,
                        Length);
                    }
                }
            if (Types.TryGetValue(Index,out type)) {
                return (CodeViewSymbol)Activator.CreateInstance(type,
                    Section,
                    Offset,
                    (IntPtr)Content,
                    Length);
                }
            return new CodeViewSymbol(Section,Offset,Index,Content,Length);
            }

        #region M:ToArray(Byte*,Int32):Byte[]
        protected static unsafe Byte[] ToArray(Byte* content, Int32 length) {
            var r = new Byte[length];
            for (var i = 0; i < length; i++) {
                r[i] = content[i];
                }
            return r;
            }
        #endregion
        #region M:ToString(DEBUG_TYPE_ENUM):String
        protected static String ToString(DEBUG_TYPE_ENUM value) {
            return (value >= CV_FIRST_NONPRIM)
                ? ((UInt32)value).ToString("X6")
                : value.ToString();
            }
        #endregion
        #region M:ToString(Encoding,Byte*,Boolean):String
        protected static unsafe String ToString(Encoding encoding, Byte* value, Boolean lengthprefixed) {
            if (lengthprefixed) {
                var c = (Int32)(*value);
                var r = new Byte[c];
                for (var i = 0;i < c;++i) {
                    r[i] = value[i + 1];
                    }
                return encoding.GetString(r);
                }
            else
                {
                var r = new List<Byte>();
                while (*value != 0) {
                    r.Add(*value);
                    value++;
                    }
                return encoding.GetString(r.ToArray());
                }
            }
        #endregion
        #region M:ReadString(Encoding,[Ref]Byte*,Boolean):String
        protected static unsafe String ReadString(Encoding encoding, ref Byte* value, Boolean lengthprefixed) {
            if (lengthprefixed) {
                var c = (Int32)(*value);
                var r = new Byte[c];
                for (var i = 0;i < c;++i) {
                    r[i] = value[i + 1];
                    }
                value += sizeof(Int32) + c;
                return encoding.GetString(r);
                }
            else
                {
                var r = new List<Byte>();
                while (*value != 0) {
                    r.Add(*value);
                    value++;
                    }
                return encoding.GetString(r.ToArray());
                }
            }
        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return Type.ToString();
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public virtual void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2}",
                LinePrefix,Offset,Type);
            var builder = new StringBuilder();
            using (var writer = new DefaultJsonWriter(new JsonTextWriter(new StringWriter(builder)) {
                Formatting = Formatting.Indented,
                Indentation = 2,
                IndentChar = ' '
                }))
                {
                WriteTo(writer);
                }
            foreach (var i in builder.ToString().Split('\n')) {
                Writer.WriteLine("{0}{1}", LinePrefix,i);
                }
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public virtual void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(Offset),Offset.ToString("x8"));
                writer.WriteValue(nameof(Type),Type);
                }
            }

        private static Int32 MaxSizeLength;
        private const DEBUG_TYPE_ENUM CV_FIRST_NONPRIM = (DEBUG_TYPE_ENUM)0x1000;
        }
    }