using System;
using System.Collections.Generic;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_MODIFIER_16)]
    internal class LF_MODIFIER_16 : CodeViewTypeInfo
        {
        private readonly UInt16 Attributes;
        private readonly UInt16 TypeIndex;
        public unsafe LF_MODIFIER_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var r = (Byte*)Content + sizeof(Int16);
            Attributes = ReadUInt16(ref r);
            TypeIndex  = ReadUInt16(ref r);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            var flags = new List<String>();
            if ((Attributes & 0x0001) == 0x0001) { flags.Add("const");     }
            if ((Attributes & 0x0002) == 0x0002) { flags.Add("volatile");  }
            if ((Attributes & 0x0004) == 0x0004) { flags.Add("unaligned"); }
            Writer.WriteLine("{0}LeafIndex:{1} Type:{2:x4}{3}",LinePrefix,LeafIndex,TypeIndex,(flags.Count > 0)
                    ? $" Flags:[{String.Join(",",flags)}]"
                    : String.Empty);
            }
        }
    }