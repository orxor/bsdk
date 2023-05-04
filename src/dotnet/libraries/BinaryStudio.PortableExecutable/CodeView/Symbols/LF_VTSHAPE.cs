using System;
using System.Collections.Generic;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_VTSHAPE)]
    internal class LF_VTSHAPE : CodeViewTypeInfo
        {
        private static readonly IDictionary<Byte,String> Descriptions = new Dictionary<Byte,String>{
            {0,"near"},
            {1,"far"},
            {2,"thin"},
            {3,"address point displacement to outermost class"},
            {4,"far pointer to metaclass descriptor"},
            {5,"near32"},
            {6,"far32"},
            };

        private readonly IList<Byte> Entries = new List<Byte>();
        public unsafe LF_VTSHAPE(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var count = (Int32)(*((Int16*)Content + 1));
            var c = (Int32)Math.Ceiling(count*0.5);
            var r = (Byte*)Content + 2*sizeof(Int16);
            for (var i = 0; i < c; i++) {
                if (count > 1) {
                    Entries.Add((Byte)((*r     ) & 0x0f));
                    Entries.Add((Byte)((*r >> 4) & 0x0f));
                    count -= 2;
                    }
                else
                    {
                    Entries.Add((Byte)((*r >> 4) & 0x0f));
                    count -= 1;
                    }
                r++;
                }
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(LeafIndex),LeafIndex);
                if (Entries.Count > 0) {
                    writer.WritePropertyName("Entries");
                    using (writer.Array()) {
                        foreach (var e in Entries) {
                            writer.WriteValue(Descriptions[e]);
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
            Writer.WriteLine("{0}LeafIndex:{1} DiscriptorsCount:{2}",LinePrefix,LeafIndex,Entries.Count);
            var i = 0;
            foreach (var e in Entries) {
                Writer.WriteLine("{0}  {1:x4}:{{{2}}}",LinePrefix,i,Descriptions[e]);
                i++;
                }
            }
        }
    }