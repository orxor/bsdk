using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class CodeViewSymbol : CodeViewSymbol<CodeViewSymbolAttribute,DEBUG_SYMBOL_INDEX>
        {
        private class DefaultFactory : CodeViewSymbolFactory<CodeViewSymbolAttribute,DEBUG_SYMBOL_INDEX> {
            protected override unsafe ICodeViewSymbol CreateDefault(CodeViewSymbolsSSection Section, Int32 Offset, DEBUG_SYMBOL_INDEX Index,Byte* Content, Int32 Length) {
                return new CodeViewSymbol(Section,Offset,Index,Content,Length);
                }
            }

        protected CodeViewSymbol(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            }

        private unsafe CodeViewSymbol(CodeViewSymbolsSSection Section, Int32 Offset, DEBUG_SYMBOL_INDEX Type,Byte* Content, Int32 Length)
            : base(Section, Offset, Type, Content, Length)
            {
            }

        public static unsafe CodeViewSymbol From(CodeViewSymbolsSSection Section, Int32 Offset, DEBUG_SYMBOL_INDEX Index, Byte* Content, Int32 Length) {
            return (CodeViewSymbol)(new DefaultFactory()).Create(Section,Offset,Index,Content,Length);
            }
        }

    public class CodeViewSymbol<T,I> : IJsonSerializable,IFileDumpSupport,ICodeViewSymbol,IXmlSerializable
        where T: ICodeViewSymbolAttribute
        {
        UInt16 ICodeViewSymbol.Type { get { return (UInt16)(Object)Type; }}
        public CV_CPU_TYPE? CPU { get;internal set; }
        public ICodeViewNameTable NameTable { get;internal set; }
        public CodeViewSymbolsSSection Section { get; }
        public virtual I Type { get; }
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

        private static readonly IDictionary<UInt16,Type> Types = new Dictionary<UInt16,Type>();
        static CodeViewSymbol() {
            foreach (var type in typeof(CodeViewSymbol<T,I>).Assembly.GetTypes()) {
                var key = type.GetCustomAttributes(false).OfType<T>().FirstOrDefault();
                if (key != null) {
                    Types.Add(key.Key, type);
                    }
                }
            }

        protected unsafe CodeViewSymbol(CodeViewSymbolsSSection Section, Int32 Offset, I Type, Byte* Content, Int32 Length)
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
            using (writer.Object()) {
                writer.WriteValue(nameof(Offset),Offset.ToString("x8"));
                writer.WriteValue(nameof(Type),Type);
                }
            }

        public Object DecodeRegister(UInt16 value) {
            if (CPU != null) {
                switch (CPU.Value) {
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

        #region M:IXmlSerializable.GetSchema:XmlSchema
        /// <summary>This method is reserved and should not be used. When implementing the <see langword="IXmlSerializable"/> interface, you should return <see langword="null" /> (<see langword="Nothing" /> in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.</summary>
        /// <returns>An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.</returns>
        XmlSchema IXmlSerializable.GetSchema() {
            return null;
            }
        #endregion
        #region M:IXmlSerializable.ReadXml(XmlReader)
        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
            {
            throw new NotSupportedException();
            }
        #endregion
        #region M:WriteXml(XmlWriter)
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public virtual void WriteXml(XmlWriter writer) {
            writer.WriteStartElement("SymbolInfo");
                writer.WriteAttributeString("Type",Type.ToString());
                writer.WriteAttributeString("Offset",Offset.ToString("x8"));
            writer.WriteEndElement();
            }
        #endregion

        private static Int32 MaxSizeLength;
        private const DEBUG_TYPE_ENUM CV_FIRST_NONPRIM = (DEBUG_TYPE_ENUM)0x1000;
        }
    }