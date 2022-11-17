using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_UNION_16)]
    internal class LF_UNION_16 : CodeViewTypeInfo
        {
        [Flags]
        private enum FLAGS
            {
            PACKED        = 0x0001,
            CTOR          = 0x0002,
            OVEROPS       = 0x0004,
            ISNESTED      = 0x0008,
            CNESTED       = 0x0010,
            OPASSIGN      = 0x0020,
            OPCAST        = 0x0040,
            FWDREF        = 0x0080,
            SCOPED        = 0x0100,
            HASUNIQUENAME = 0x0200,
            SEALED        = 0x0400,
            INTRINSIC     = 0x2000,
            }

        private enum HFA
            {
            CV_HFA_none,
            CV_HFA_float,
            CV_HFA_double,
            CV_HFA_other
            }

        private enum MOCOM
            {
            CV_MOCOM_UDT_none,
            CV_MOCOM_UDT_ref,
            CV_MOCOM_UDT_value,
            CV_MOCOM_UDT_interface
            }

        private readonly UInt16 ElementsCount;
        private readonly UInt16 FieldListTypeIndex;
        private readonly Object StructureLength;
        private readonly FLAGS Flags;
        private readonly HFA HFAFlags;
        private readonly MOCOM MOCOMFlags;
        private readonly String Name;

        public LF_UNION_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            ReadUInt16(ref Content);
            ElementsCount      = ReadUInt16(ref Content);
            FieldListTypeIndex = ReadUInt16(ref Content);
            var flags = ReadUInt16(ref Content);
            Flags = (FLAGS)(flags & 0x27ff);
            HFAFlags = (HFA)(((Int32)(flags) >> 11) & 0x0003);
            MOCOMFlags = (MOCOM)(((Int32)(flags) >> 14) & 0x0003);
            StructureLength = ReadNumeric(ref Content);
            Name = ReadLengthPrefixedString(Encoding.ASCII,ref Content);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="DumpFlags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags DumpFlags) {
            Writer.WriteLine("{0}LeafIndex:{1} ElementsCount:{2}",LinePrefix,LeafIndex,ElementsCount);
            Writer.WriteLine("{0}FieldListType:{1:x4} Length:{2}",LinePrefix,FieldListTypeIndex,ToString(StructureLength,DumpFlags));
            var flags = new List<String>();
            if (Flags.HasFlag(FLAGS.PACKED))        { flags.Add("packed"); }
            if (Flags.HasFlag(FLAGS.CTOR))          { flags.Add("ctor"); }
            if (Flags.HasFlag(FLAGS.OVEROPS))       { flags.Add("has overloaded operators"); }
            if (Flags.HasFlag(FLAGS.ISNESTED))      { flags.Add("nested class"); }
            if (Flags.HasFlag(FLAGS.CNESTED))       { flags.Add("contains nested classes"); }
            if (Flags.HasFlag(FLAGS.OPASSIGN))      { flags.Add("has overloaded assignment"); }
            if (Flags.HasFlag(FLAGS.OPCAST))        { flags.Add("has casting methods"); }
            if (Flags.HasFlag(FLAGS.FWDREF))        { flags.Add("forward (incomplete) reference"); }
            if (Flags.HasFlag(FLAGS.SCOPED))        { flags.Add("scoped"); }
            if (Flags.HasFlag(FLAGS.HASUNIQUENAME)) { flags.Add("decorated name"); }
            if (Flags.HasFlag(FLAGS.SEALED))        { flags.Add("sealed"); }
            if (Flags.HasFlag(FLAGS.INTRINSIC))     { flags.Add("intrinsic"); }
            if (flags.Count > 0) {
                Writer.WriteLine("{0}Flags:[{1}]",LinePrefix,String.Join(",", flags));
                }
            Writer.WriteLine("{0}Name:{1}",LinePrefix,Name);
            }
        }    }