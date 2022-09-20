using System;
using System.IO;
using BinaryStudio.IO;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable
    {
    public class MZMetadataObject : MetadataObject
        {
        private const UInt32 IMAGE_DOS_SIGNATURE    = 0x5A4D;
        private const UInt32 IMAGE_OS2_NE_SIGNATURE = 0x454E;
        private const UInt32 IMAGE_OS2_LE_SIGNATURE = 0x454c;
        private const UInt32 IMAGE_OS2_LX_SIGNATURE = 0x584c;
        private const UInt32 IMAGE_NT_SIGNATURE     = 0x00004550;

        internal MZMetadataObject(MetadataScope scope, MetadataObjectIdentity identity)
            : base(scope, identity)
            {
            }

        #region M:LoadCore(IntPtr[],Int64)
        /// <summary>Loads content from specified source.</summary>
        /// <param name="source">Content specific source addresses depending on its type.</param>
        /// <param name="length">Length of content.</param>
        protected override unsafe void LoadCore(IntPtr[] source,Int64 length) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (source.Length == 0) { throw new ArgumentOutOfRangeException(nameof(source)); }
            if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
            if (length < sizeof(IMAGE_DOS_HEADER)) { throw new InvalidDataException(); }
            var header = (IMAGE_DOS_HEADER*)source[0];
            if (header->e_magic != IMAGE_DOS_SIGNATURE) { throw new InvalidDataException(); }
            Load((Byte*)source[0], header, length - sizeof(IMAGE_DOS_HEADER));
            }
        #endregion
        #region M:Load(Byte*,IMAGE_DOS_HEADER*,Int64)
        private unsafe void Load(Byte* source, IMAGE_DOS_HEADER* header, Int64 size) {
            var mapping = &source[header->e_lfanew];
            var magic = (UInt32*)mapping;
            if (*magic == IMAGE_NT_SIGNATURE) {
                size    -= sizeof(UInt32);
                mapping += sizeof(UInt32);
                COFFMetadataObject = new CommonObjectFile(Scope,new MetadataObjectIdentity(Identity.LocalName,typeof(CommonObjectFile))){
                    IgnoreOptionalHeaderSize = false
                    };
                COFFMetadataObject.Load(new []{
                    (IntPtr)source,
                    (IntPtr)mapping
                    }, size);
                }
            else if ((*magic & 0xFFFF) == IMAGE_OS2_NE_SIGNATURE) { throw new NotImplementedException(); }
            else if ((*magic & 0xFFFF) == IMAGE_OS2_LX_SIGNATURE) { throw new NotImplementedException(); }
            else { throw new NotSupportedException(); }
            }
        #endregion
        #region M:AttachFileMapping(FileMappingMemory)
        public override void AttachFileMapping(FileMappingMemory mapping)
            {
            base.AttachFileMapping(mapping);
            COFFMetadataObject?.AttachFileMapping(mapping);
            }
        #endregion

        private MetadataObject COFFMetadataObject;
        }
    }