using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_FIELDLIST_16)]
    internal class LF_FIELDLIST_16 : CodeViewTypeInfo
        {
        private readonly IList<CodeViewTypeInfo> Fields = new List<CodeViewTypeInfo>();
        public unsafe LF_FIELDLIST_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var r = ((Byte*)Content) + sizeof(LEAF_ENUM);
            Size -= sizeof(LEAF_ENUM);
            while (Size > 0) {
                CodeViewTypeInfo Type;
                Fields.Add(Type = CreateInstance((IntPtr)r));
                Debug.Assert(Type.Size >= 0);
                Size -= Type.Size;
                r    += Type.Size;
                if (*r > LF_PAD00) {
                    var PaddingSize = *r & 0x0f;
                    Size -= PaddingSize;
                    r    += PaddingSize;
                    }
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="DumpFlags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags DumpFlags) {
            Writer.WriteLine("{0}LeafIndex:{1}",LinePrefix,LeafIndex);
            foreach (var Field in Fields) {
                Field.WriteTo(Writer,LinePrefix + "  ",DumpFlags);
                }
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(LeafIndex),LeafIndex);
                if (Fields.Count > 0) {
                    writer.WritePropertyName("Fields");
                    using (writer.Array()) {
                        foreach (var Field in Fields) {
                            Field.WriteTo(writer);
                            }
                        }
                    }
                }
            }
        }
    }