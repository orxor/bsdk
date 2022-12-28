using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_VFUNCTAB_16)]
    internal class LF_VFUNCTAB_16 : CodeViewTypeInfo
        {
        public Int16 TypeIndex { get; }
        public unsafe LF_VFUNCTAB_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            TypeIndex = *((Int16*)Content + 1);
            this.Size = (Size < 0)
                ? (sizeof(LEAF_ENUM) + sizeof(Int16))
                : Size;
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(LeafIndex),LeafIndex);
                writer.WriteValue(nameof(TypeIndex),TypeIndex);
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="DumpFlags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags DumpFlags) {
            Writer.WriteLine("{0}LeafIndex:{1} TypeIndex:{2:x4}",LinePrefix,LeafIndex,TypeIndex);
            }
        }
    }