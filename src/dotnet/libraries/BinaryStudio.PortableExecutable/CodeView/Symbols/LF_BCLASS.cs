using System;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_BCLASS_16)]
    internal class LF_BCLASS_16 : CodeViewTypeInfo
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct HEADER
            {
            private readonly LEAF_ENUM LeafIndex;
            public readonly Int16 TypeIndex;
            public readonly UInt16 Attributes;
            public readonly Int16 Offset;
            }

        public unsafe LF_BCLASS_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var r = (HEADER*)Content;
            this.Size = (Size < 0)
                ? (sizeof(HEADER))
                : Size;
            }
        }
    }