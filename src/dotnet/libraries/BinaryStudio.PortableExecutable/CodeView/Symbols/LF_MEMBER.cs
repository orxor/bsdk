﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_MEMBER_16)]
    internal class LF_MEMBER_16 : CodeViewTypeInfo
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct HEADER
            {
            private readonly LEAF_ENUM LeafIndex;
            public readonly Int16 TypeIndex;
            public readonly UInt16 Attributes;
            public readonly Int16 Offset;
            public readonly Byte  NameLength; 
            }

        private readonly UInt16 Attributes;
        private readonly Int16 TypeIndex;
        private readonly String Name;

        public unsafe LF_MEMBER_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var r = (HEADER*)Content;
            Attributes = r->Attributes;
            TypeIndex = r->TypeIndex;
            Name = Encoding.ASCII.GetString(ToArray((Byte*)(r + 1),r->NameLength));
            this.Size = (Size < 0)
                ? (sizeof(HEADER) + r->NameLength)
                : Size;
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
            var flags = new List<String>();
            if ((Attributes & 0x03) > 0) { flags.Add(AccessProtection[Attributes & 0x03]); }
            flags.Add(MethodProperties[(Attributes >> 2) & 0x07]);
            if ((Attributes & 0x0020) == 0x0020) { flags.Add("pseudo");      }
            if ((Attributes & 0x0040) == 0x0040) { flags.Add("noinherit");   }
            if ((Attributes & 0x0080) == 0x0080) { flags.Add("noconstruct"); }
            if ((Attributes & 0x0100) == 0x0100) { flags.Add("compgenx");    }
            if ((Attributes & 0x0200) == 0x0200) { flags.Add("sealed");      }
            Writer.WriteLine("{0}LeafIndex:{1} TypeIndex:{2:x4} Attributes:[{3}] \"{4}\"",LinePrefix,LeafIndex,TypeIndex,String.Join(",",flags),Name);
            }
        }
    }