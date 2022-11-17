using System;
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
        }
    }