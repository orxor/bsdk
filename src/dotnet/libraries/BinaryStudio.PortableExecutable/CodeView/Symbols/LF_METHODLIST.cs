using System;
using System.Collections.Generic;
using System.IO;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_METHODLIST_16)]
    internal class LF_METHODLIST_16 : CodeViewTypeInfo
        {
        private class MethodInfo
            {
            public UInt16 Attributes;
            public Int16 TypeIndex;
            public Int32? VTabOffset;
            }

        private readonly IList<MethodInfo> Methods = new List<MethodInfo>();
        public unsafe LF_METHODLIST_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var e = (Byte*)Content + Size;
            var r = (Byte*)Content + sizeof(UInt16);
            while (r < e) {
                var Attributes = ReadUInt16(ref r);
                var MethodAttributes = (CodeViewMethodAttributes)((Attributes >> 2) & 0x0007);
                var TypeIndex = ReadInt16(ref r);
                var Target = new MethodInfo{
                    TypeIndex = TypeIndex,
                    Attributes = Attributes,
                    VTabOffset = ((MethodAttributes == CodeViewMethodAttributes.Intro) || (MethodAttributes == CodeViewMethodAttributes.PureIntro))
                        ? ReadInt32(ref r)
                        : NullValue<Int32>.Value
                    };
                Methods.Add(Target);
                }
            return;
            }

        private static readonly String[] AccessProtection =
            {
            "none",
            "private",
            "protected",
            "public"
            };

        private static readonly String[] MethodProperties =
            {
            "vanilla",
            "virtual",
            "static",
            "friend",
            "intro",
            "purevirt",
            "pureintro"
            };

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="DumpFlags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags DumpFlags) {
            Writer.WriteLine("{0}LeafIndex:{1}",LinePrefix,LeafIndex);
            var i = 0;
            foreach (var Method in Methods) {
                var flags = new List<String>();
                if ((Method.Attributes & 0x03) > 0) { flags.Add(AccessProtection[Method.Attributes & 0x03]); }
                flags.Add(MethodProperties[(Method.Attributes >> 2) & 0x07]);
                if ((Method.Attributes & 0x0020) == 0x0020) { flags.Add("pseudo");      }
                if ((Method.Attributes & 0x0040) == 0x0040) { flags.Add("noinherit");   }
                if ((Method.Attributes & 0x0080) == 0x0080) { flags.Add("noconstruct"); }
                if ((Method.Attributes & 0x0100) == 0x0100) { flags.Add("compgenx");    }
                if ((Method.Attributes & 0x0200) == 0x0200) { flags.Add("sealed");      }
                Writer.WriteLine("{0}  {1:x4}:TypeIndex:{2:x4} Attributes:[{3}]{4}",LinePrefix,i,Method.TypeIndex,String.Join(",",flags),
                    (Method.VTabOffset != null)
                        ? $" VTabOffset:{Method.VTabOffset.Value:x4}"
                        : String.Empty);
                i++;
                }
            }
        }
    }