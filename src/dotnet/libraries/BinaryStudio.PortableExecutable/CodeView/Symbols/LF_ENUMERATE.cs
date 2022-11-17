using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_ENUMERATE_ST)]
    internal class LF_ENUMERATE_ST : CodeViewTypeInfo
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct HEADER
            {
            private readonly LEAF_ENUM LeafIndex;
            public readonly UInt16 Attributes;
            }

        private readonly UInt16 Attributes;
        private readonly String Name;
        public Object Value { get; }

        public unsafe LF_ENUMERATE_ST(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var Header = (HEADER*)Content;
            Attributes = Header->Attributes;
            var r = (Byte*)(Header + 1);
            Value = ReadNumeric(ref r);
            Name = Encoding.ASCII.GetString(ToArray(r + 1,*r));
            this.Size = (Size < 0)
                ? (Int32)(r - (Byte*)Content + *r + 1)
                : Size;
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            return $"{LeafIndex}:{Name}={Value}";
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
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            var flags = new List<String>();
            if ((Attributes & 0x03) > 0) { flags.Add(AccessProtection[Attributes & 0x03]); }
            flags.Add(MethodProperties[(Attributes >> 2) & 0x07]);
            if ((Attributes & 0x0020) == 0x0020) { flags.Add("pseudo");      }
            if ((Attributes & 0x0040) == 0x0040) { flags.Add("noinherit");   }
            if ((Attributes & 0x0080) == 0x0080) { flags.Add("noconstruct"); }
            if ((Attributes & 0x0100) == 0x0100) { flags.Add("compgenx");    }
            if ((Attributes & 0x0200) == 0x0200) { flags.Add("sealed");      }
            Writer.WriteLine("{0}LeafIndex:{1} Attributes:[{2}] Value:{3} \"{4}\"",
                LinePrefix,LeafIndex,String.Join(",",flags),
                ToString(Value,Flags),Name);
            }
        }
    }