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
        protected CodeViewTypeInfo(IntPtr Content, Int32 Size) {
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

        internal static CodeViewTypeInfo CreateInstance(LEAF_ENUM LeafIndex, IntPtr Content,Int32 Size) {
            return Types.TryGetValue(LeafIndex, out var Type)
                ? (CodeViewTypeInfo) Activator.CreateInstance(Type,Content,Size)
                : new CodeViewTypeInfo(LeafIndex,Content,Size);
            }

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
        }
    }