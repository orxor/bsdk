using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_BITFIELD_16)]
    internal class LF_BITFIELD_16 : CodeViewTypeInfo
        {
        private readonly Byte BitLength;
        private readonly Byte BitPosition;
        private readonly UInt16 FieldTypeIndex;

        public LF_BITFIELD_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            ReadUInt16(ref Content);
            BitLength = ReadByte(ref Content);
            BitPosition = ReadByte(ref Content);
            FieldTypeIndex = ReadUInt16(ref Content);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}LeafIndex:{1} FieldType:{2:x4} Length:{3:x2} Position:{4:x2}",
                LinePrefix,LeafIndex,FieldTypeIndex,
                BitLength,BitPosition);
            }
        }
    }