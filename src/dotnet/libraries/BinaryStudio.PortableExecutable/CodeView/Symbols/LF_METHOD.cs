using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_METHOD_16)]
    internal class LF_METHOD_16 : CodeViewTypeInfo
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct HEADER
            {
            private readonly LEAF_ENUM LeafIndex;
            public readonly Int16 OccurrenceCount;
            public readonly Int16 TypeIndex;
            public readonly Byte  NameLength; 
            }

        private readonly Int16 OccurrenceCount;
        private readonly Int16 TypeIndex;
        private readonly String Name;

        public unsafe LF_METHOD_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var r = (HEADER*)Content;
            OccurrenceCount = r->OccurrenceCount;
            TypeIndex = r->TypeIndex;
            Name = Encoding.ASCII.GetString(ToArray((Byte*)(r + 1),r->NameLength));
            this.Size = (Size < 0)
                ? (sizeof(HEADER) + r->NameLength)
                : Size;
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(LeafIndex),LeafIndex);
                writer.WriteValue(nameof(OccurrenceCount),OccurrenceCount);
                writer.WriteValue(nameof(TypeIndex),TypeIndex);
                writer.WriteValue(nameof(Name),Name);
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="DumpFlags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags DumpFlags) {
            Writer.WriteLine("{0}LeafIndex:{1} TypeIndex:{2:x4} OccurrenceCount:{3} \"{4}\"",LinePrefix,LeafIndex,TypeIndex,OccurrenceCount,Name);
            }
        }
    }