using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MUI_HEADER
        {
        private readonly UInt32 Signature;
        private readonly UInt32 Size;
        private readonly UInt32 Version;
        private readonly UInt32 Reserved1;
        private readonly MUI_TYPE Type;
        private readonly UInt32 SystemAttributes;
        public  readonly MUI_ULTIMATE_FALLBACK_LOCATION UltimateFallbackLocation;
        private unsafe fixed Byte ServiceChecksum[16];
        private unsafe fixed Byte Checksum[16];
        private readonly UInt64 Reserved2;
        private readonly UInt64 Reserved3;
        private readonly UInt64 Reserved4;
        public readonly UInt32 MainNameTypeDataOffset;
        public readonly Int32 MainNameTypeDataSize;
        public readonly UInt32 MainIDTypesDataOffset;
        public readonly UInt32 MainIDTypesDataSize;
        public readonly UInt32 MUINameTypeDataOffset;
        public readonly Int32 MUINameTypeDataSize;
        public readonly UInt32 MUIIDTypesDataOffset;
        public readonly UInt32 MUIIDTypesDataSize;
        public readonly UInt32 LanguageDataOffset;
        public readonly Int32 LanguageDataSize;
        public readonly UInt32 UltimateFallbackLanguageDataOffset;
        public readonly Int32 UltimateFallbackLanguageDataSize;
        }
    }