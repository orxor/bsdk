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
    public class CodeViewTypeInfo : IJsonSerializable,IFileDumpSupport
        {
        public virtual LEAF_ENUM LeafIndex { get; }
        public Int32 Size { get;protected set; }

        protected const Byte LF_PAD00 = 0xf0;
        protected const Byte LF_PAD01 = 0xf1;
        protected const Byte LF_PAD02 = 0xf2;
        protected const Byte LF_PAD03 = 0xf3;
        protected const Byte LF_PAD04 = 0xf4;
        protected const Byte LF_PAD05 = 0xf5;
        protected const Byte LF_PAD06 = 0xf6;
        protected const Byte LF_PAD07 = 0xf7;
        protected const Byte LF_PAD08 = 0xf8;
        protected const Byte LF_PAD09 = 0xf9;
        protected const Byte LF_PAD10 = 0xfa;
        protected const Byte LF_PAD11 = 0xfb;
        protected const Byte LF_PAD12 = 0xfc;
        protected const Byte LF_PAD13 = 0xfd;
        protected const Byte LF_PAD14 = 0xfe;
        protected const Byte LF_PAD15 = 0xff;

        protected CodeViewTypeInfo(IntPtr Content, Int32 Size) {
            this.Size = Size;
            var LeafIndexAttribute = GetType().GetCustomAttributes(false).OfType<CodeViewLeafIndexAttribute>().FirstOrDefault();
            if (LeafIndexAttribute != null) {
                LeafIndex = LeafIndexAttribute.LeafIndex;
                }
            }

        private CodeViewTypeInfo(LEAF_ENUM LeafIndex, IntPtr Content, Int32 Size)
            :this(Content,Size)
            {
            this.LeafIndex = LeafIndex;
            }

        #region M:CreateInstance(LEAF_ENUM,IntPtr,Int32):CodeViewTypeInfo
        internal static CodeViewTypeInfo CreateInstance(LEAF_ENUM LeafIndex, IntPtr Content,Int32 Size) {
            return Types.TryGetValue(LeafIndex, out var Type)
                ? (CodeViewTypeInfo) Activator.CreateInstance(Type,Content,Size)
                : new CodeViewTypeInfo(LeafIndex,Content,Size);
            }
        #endregion
        #region M:CreateInstance(IntPtr):CodeViewTypeInfo
        internal static unsafe CodeViewTypeInfo CreateInstance(IntPtr Content) {
            var LeafIndex = (LEAF_ENUM*)Content;
            return CreateInstance(*LeafIndex,Content,-1);
            }
        #endregion

        private static readonly IDictionary<LEAF_ENUM,Type> Types = new Dictionary<LEAF_ENUM,Type>();
        static CodeViewTypeInfo() {
            foreach (var Type in typeof(CodeViewTypeInfo).Assembly.GetTypes()) {
                var LeafIndex = Type.GetCustomAttributes(false).OfType<CodeViewLeafIndexAttribute>().FirstOrDefault();
                if (LeafIndex != null) {
                    Types.Add(LeafIndex.LeafIndex, Type);
                    }
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public virtual void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}LeafIndex:{1}",LinePrefix,LeafIndex);
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
                writer.WriteValue(nameof(LeafIndex),LeafIndex);
                }
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            return LeafIndex.ToString();
            }

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
        #region M:ToArray(Byte*,Int32):Byte[]
        protected static unsafe Byte[] ToArray(Byte* content, Int32 length) {
            var r = new Byte[length];
            for (var i = 0; i < length; i++) {
                r[i] = content[i];
                }
            return r;
            }
        #endregion

        protected static unsafe Object ReadNumeric(ref Byte* source) {
            var r = ReadUInt16(ref source);
            if (r >= LF_CHAR) {
                switch (r) {
                    case LF_CHAR:      return ReadSByte(ref source);
                    case LF_SHORT:     return ReadInt16(ref source);
                    case LF_LONG:      return ReadInt32(ref source);
                    case LF_QUADWORD:  return ReadInt64(ref source);
                    case LF_USHORT:    return ReadUInt16(ref source);
                    case LF_ULONG:     return ReadUInt32(ref source);
                    case LF_UQUADWORD: return ReadUInt64(ref source);
                    case LF_REAL32:
                    case LF_REAL64:
                    case LF_REAL80:
                    case LF_REAL128:
                    case LF_REAL48:
                    case LF_COMPLEX32:
                    case LF_COMPLEX64:
                    case LF_COMPLEX80:
                    case LF_COMPLEX128:
                    case LF_VARSTRING:
                    case LF_OCTWORD:
                    case LF_UOCTWORD:
                    case LF_DECIMAL:
                    case LF_DATE:
                    case LF_UTF8STRING:
                    case LF_REAL16:
                    default:throw new ArgumentOutOfRangeException(nameof(source));
                    }
                }
            return r;
            }

        #region M:ReadByte({ref}IntPtr):Byte
        protected static unsafe Byte ReadByte(ref Byte* source)
            {
            return *source++;
            }
        #endregion
        #region M:ReadUInt16({ref}IntPtr):UInt16
        protected static unsafe UInt16 ReadUInt16(ref Byte* source)
            {
            var r = (UInt16*)source;
            source += sizeof(UInt16);
            return *r;
            }
        #endregion
        #region M:ReadUInt32({ref}IntPtr):UInt32
        protected static unsafe UInt32 ReadUInt32(ref Byte* source)
            {
            var r = (UInt32*)source;
            source += sizeof(UInt32);
            return *r;
            }
        #endregion
        #region M:ReadUInt64({ref}IntPtr):UInt64
        protected static unsafe UInt64 ReadUInt64(ref Byte* source)
            {
            var r = (UInt64*)source;
            source += sizeof(UInt64);
            return *r;
            }
        #endregion
        #region M:ReadInt16({ref}IntPtr):Int16
        protected static unsafe Int16 ReadInt16(ref Byte* source)
            {
            var r = (Int16*)source;
            source += sizeof(Int16);
            return *r;
            }
        #endregion
        #region M:ReadInt32({ref}IntPtr):Int32
        protected static unsafe Int32 ReadInt32(ref Byte* source)
            {
            var r = (Int32*)source;
            source += sizeof(Int32);
            return *r;
            }
        #endregion
        #region M:ReadInt64({ref}IntPtr):Int64
        protected static unsafe Int64 ReadInt64(ref Byte* source)
            {
            var r = (Int64*)source;
            source += sizeof(Int64);
            return *r;
            }
        #endregion
        #region M:ReadSByte({ref}IntPtr):SByte
        protected static unsafe SByte ReadSByte(ref Byte* source)
            {
            var r = (SByte*)source;
            source += sizeof(SByte);
            return *r;
            }
        #endregion

        protected const UInt16 LF_CHAR             = 0x8000;
        protected const UInt16 LF_SHORT            = 0x8001;
        protected const UInt16 LF_USHORT           = 0x8002;
        protected const UInt16 LF_LONG             = 0x8003;
        protected const UInt16 LF_ULONG            = 0x8004;
        protected const UInt16 LF_REAL32           = 0x8005;
        protected const UInt16 LF_REAL64           = 0x8006;
        protected const UInt16 LF_REAL80           = 0x8007;
        protected const UInt16 LF_REAL128          = 0x8008;
        protected const UInt16 LF_QUADWORD         = 0x8009;
        protected const UInt16 LF_UQUADWORD        = 0x800a;
        protected const UInt16 LF_REAL48           = 0x800b;
        protected const UInt16 LF_COMPLEX32        = 0x800c;
        protected const UInt16 LF_COMPLEX64        = 0x800d;
        protected const UInt16 LF_COMPLEX80        = 0x800e;
        protected const UInt16 LF_COMPLEX128       = 0x800f;
        protected const UInt16 LF_VARSTRING        = 0x8010;
        protected const UInt16 LF_OCTWORD          = 0x8017;
        protected const UInt16 LF_UOCTWORD         = 0x8018;
        protected const UInt16 LF_DECIMAL          = 0x8019;
        protected const UInt16 LF_DATE             = 0x801a;
        protected const UInt16 LF_UTF8STRING       = 0x801b;
        protected const UInt16 LF_REAL16           = 0x801c;
        }
    }