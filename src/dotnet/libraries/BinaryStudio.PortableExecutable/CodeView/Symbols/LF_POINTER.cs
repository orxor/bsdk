using System;
using System.Collections.Generic;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_POINTER_16)]
    internal class LF_POINTER_16 : CodeViewTypeInfo
        {
        private enum PTR_TYPE
            {
            CV_PTR_NEAR         = 0x00, // 16 bit pointer
            CV_PTR_FAR          = 0x01, // 16:16 far pointer
            CV_PTR_HUGE         = 0x02, // 16:16 huge pointer
            CV_PTR_BASE_SEG     = 0x03, // based on segment
            CV_PTR_BASE_VAL     = 0x04, // based on value of base
            CV_PTR_BASE_SEGVAL  = 0x05, // based on segment value of base
            CV_PTR_BASE_ADDR    = 0x06, // based on address of base
            CV_PTR_BASE_SEGADDR = 0x07, // based on segment address of base
            CV_PTR_BASE_TYPE    = 0x08, // based on type
            CV_PTR_BASE_SELF    = 0x09, // based on self
            CV_PTR_NEAR32       = 0x0a, // 32 bit pointer
            CV_PTR_FAR32        = 0x0b, // 16:32 pointer
            CV_PTR_PTR64        = 0x0c, // 64 bit pointer
            CV_PTR_UNUSEDPTR    = 0x0d  // first unused pointer type
            }

        private enum PTR_MODE
            {
            CV_PTR_MODE_PTR     = 0x00, // "normal" pointer
            CV_PTR_MODE_REF     = 0x01, // "old" reference
            CV_PTR_MODE_LVREF   = 0x01, // l-value reference
            CV_PTR_MODE_PMEM    = 0x02, // pointer to data member
            CV_PTR_MODE_PMFUNC  = 0x03, // pointer to member function
            CV_PTR_MODE_RVREF   = 0x04, // r-value reference
            CV_PTR_MODE_RESERVED= 0x05  // first unused pointer mode
            }

        private readonly PTR_TYPE PointerType;
        private readonly PTR_MODE PointerMode;
        private readonly Boolean IsFlat32;
        private readonly Boolean IsVolatile;
        private readonly Boolean IsConst;
        private readonly Boolean IsUnaligned;
        public unsafe LF_POINTER_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var attributes = (UInt16)(*((UInt16*)Content + 1));
            PointerType = (PTR_TYPE)(attributes & 0x001f);
            PointerMode = (PTR_MODE)((attributes >> 5) & 0x0007);
            IsFlat32    = (attributes & 0x0100) == 0x0100;
            IsVolatile  = (attributes & 0x0200) == 0x0200;
            IsConst     = (attributes & 0x0400) == 0x0400;
            IsUnaligned = (attributes & 0x0800) == 0x0800;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}LeafIndex:{1} PointerType:{2} PointerMode:{3}", LinePrefix, LeafIndex, PointerType, PointerMode);
            var flags = new List<String>();
            if (IsFlat32)    { flags.Add("flat32");    }
            if (IsVolatile)  { flags.Add("volatile");  }
            if (IsConst)     { flags.Add("const");     }
            if (IsUnaligned) { flags.Add("unaligned"); }
            if (flags.Count > 0) {
                Writer.WriteLine("{0}Flags:[{1}]", LinePrefix, String.Join(",",flags));
                }
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(LeafIndex),LeafIndex);
                writer.WriteValue(nameof(PointerType),PointerType);
                writer.WriteValue(nameof(PointerMode),PointerMode);
                writer.WriteValue(nameof(IsFlat32),IsFlat32);
                writer.WriteValue(nameof(IsVolatile),IsVolatile);
                writer.WriteValue(nameof(IsConst),IsConst);
                writer.WriteValue(nameof(IsUnaligned),IsUnaligned);
                }
            }
        }
    }