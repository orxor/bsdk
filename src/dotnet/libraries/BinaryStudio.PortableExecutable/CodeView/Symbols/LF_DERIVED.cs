using System;
using System.Collections.Generic;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_DERIVED_16)]
    internal class LF_DERIVED_16 : CodeViewTypeInfo
        {
        private readonly IList<UInt16> Types = new List<UInt16>();

        public LF_DERIVED_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            ReadUInt16(ref Content);
            var count = ReadUInt16(ref Content);
            for (var i = 0; i < count; i++) {
                Types.Add(ReadUInt16(ref Content));
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}LeafIndex:{1}",LinePrefix,LeafIndex);
            for (var i = 0; i < Types.Count; i++) {
                Writer.WriteLine("{0}  {1:x4}:{2:x4}",LinePrefix,i,Types[i]);
                }
            }
        }
    }