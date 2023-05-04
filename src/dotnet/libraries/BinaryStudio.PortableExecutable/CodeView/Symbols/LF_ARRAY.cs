using System;
using System.IO;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_ARRAY_16)]
    internal class LF_ARRAY_16 : CodeViewTypeInfo 
        {
        private readonly UInt16 ElementTypeIndex;
        private readonly UInt16 VariableTypeIndex;
        private readonly Object ArrayLength;
        private readonly String Name;

        public LF_ARRAY_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            ReadUInt16(ref Content);
            ElementTypeIndex  = ReadUInt16(ref Content);
            VariableTypeIndex = ReadUInt16(ref Content);
            ArrayLength = ReadNumeric(ref Content);
            Name = ReadLengthPrefixedString(Encoding.ASCII,ref Content);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}LeafIndex:{1} ElementType:{2:x4} VariableType:{3:x4} Length:{4:x4} \"{5}\"",
                LinePrefix,LeafIndex,ElementTypeIndex,
                VariableTypeIndex,ArrayLength,Name);
            }
        }
    }