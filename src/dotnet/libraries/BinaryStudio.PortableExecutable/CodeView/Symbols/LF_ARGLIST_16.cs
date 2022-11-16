using System;
using System.Collections.Generic;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewLeafIndex(LEAF_ENUM.LF_ARGLIST_16)]
    [UsedImplicitly]
    internal class LF_ARGLIST_16 : CodeViewTypeInfo
        {
        private readonly IList<UInt16> Indices = new List<UInt16>();
        public unsafe LF_ARGLIST_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var count   = ((UInt16*)Content) + 1;
            var indices = count + 1;
            for (var i = 0; i < *count; i++) {
                Indices.Add(indices[i]);
                }
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(LeafIndex),LeafIndex);
                if (Indices.Count > 0) {
                    writer.WritePropertyName("Indices");
                    using (writer.ArrayObject()) {
                        foreach (var e in Indices) {
                            writer.WriteValue(e.ToString("x4"));
                            }
                        }
                    }
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}LeafIndex:{1} Indices:{2}",LinePrefix,LeafIndex,Indices.Count);
            var i = 0;
            foreach (var index in Indices) {
                Writer.WriteLine("{0}  {1:x4}:{2:x4}",LinePrefix,i,index);
                i++;
                }
            }
        }
    }