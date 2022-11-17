using System;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_NESTTYPE_16)]
    internal class LF_NESTTYPE_16 : CodeViewTypeInfo
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct HEADER
            {
            private readonly LEAF_ENUM LeafIndex;
            public readonly UInt16 TypeIndex;
            public readonly Byte  NameLength; 
            }

        private readonly String Name;
        public unsafe LF_NESTTYPE_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var r = (HEADER*)Content;
            Name = Encoding.ASCII.GetString(ToArray((Byte*)(r + 1),r->NameLength));
            this.Size = (Size < 0)
                ? (sizeof(HEADER) + r->NameLength)
                : Size;
            }
        }
    }