using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace BinaryStudio.PortableExecutable.Win32
    {
    /// <summary>
    /// Section header format.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [DebuggerDisplay("{ToString()}")]
    public struct IMAGE_SECTION_HEADER
        {
        private const Int32 IMAGE_SIZEOF_SHORT_NAME = 8;
        /// <summary>
        /// An 8-byte, null-padded UTF-8 encoded string.
        /// If the string is exactly 8 characters long, there is no terminating null.
        /// For longer names, this field contains a slash (/) that is followed by an ASCII representation of a decimal number that is an offset into the string table.
        /// Executable images do not use a string table and do not support section names longer than 8 characters.
        /// Long names in object files are truncated if they are emitted to an executable file.
        /// </summary>
        [DebuggerDisplay("{ToString()}")] public unsafe fixed Byte Name[IMAGE_SIZEOF_SHORT_NAME];
        /// <summary>
        /// The total size of the section when loaded into memory.
        /// If this value is greater than <see cref="SizeOfRawData"/>, the section is zero-padded.
        /// This field is valid only for executable images and should be set to zero for object files.
        /// </summary>
        public readonly UInt32 VirtualSize;
        /// <summary>
        /// For executable images, the address of the first byte of the section relative to the image base when the section is loaded into memory.
        /// For object files, this field is the address of the first byte before relocation is applied; for simplicity, compilers should set this to zero.
        /// Otherwise, it is an arbitrary value that is subtracted from offsets during relocation.
        /// </summary>
        public readonly UInt32 VirtualAddress;
        /// <summary>
        /// The size of the section (for object files) or the size of the initialized data on disk (for image files).
        /// For executable images, this must be a multiple of <see cref="IMAGE_OPTIONAL_HEADER32.FileAlignment"/> from the optional header.
        /// If this is less than <see cref="VirtualSize"/>, the remainder of the section is zero-filled.
        /// Because the <see cref="SizeOfRawData"/> field is rounded but the <see cref="VirtualSize"/> field is not,
        /// it is possible for <see cref="SizeOfRawData"/> to be greater than <see cref="VirtualSize"/> as well.
        /// When a section contains only uninitialized data, this field should be zero.
        /// </summary>
        public readonly UInt32 SizeOfRawData;
        /// <summary>
        /// The file pointer to the first page of the section within the COFF file.
        /// For executable images, this must be a multiple of <see cref="IMAGE_OPTIONAL_HEADER32.FileAlignment"/> from the optional header.
        /// For object files, the value should be aligned on a 4 byte boundary for best performance.
        /// When a section contains only uninitialized data, this field should be zero.
        /// </summary>
        public readonly UInt32 PointerToRawData;
        /// <summary>
        /// The file pointer to the beginning of relocation entries for the section.
        /// This is set to zero for executable images or if there are no relocations.
        /// </summary>
        public readonly UInt32 PointerToRelocations;
        /// <summary>
        /// The file pointer to the beginning of line-number entries for the section.
        /// This is set to zero if there are no COFF line numbers.
        /// This value should be zero for an image because COFF debugging information is deprecated.
        /// </summary>
        public readonly UInt32 PointerToLineNumbers;
        /// <summary>
        /// The number of relocation entries for the section.
        /// This is set to zero for executable images.
        /// </summary>
        public readonly UInt16 NumberOfRelocations;
        /// <summary>
        /// The number of line-number entries for the section.
        /// This value should be zero for an image because COFF debugging information is deprecated.
        /// </summary>
        public readonly UInt16 NumberOfLineNumbers;
        /// <summary>
        /// The flags that describe the characteristics of the section.
        /// </summary>
        public readonly IMAGE_SCN Characteristics;

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="T:System.String"/> containing a fully qualified type name.</returns>
        /// <filterpriority>2</filterpriority>
        public override unsafe String ToString() {
            var r = new StringBuilder(8);
            fixed (Byte* bytes = Name) {
                for (var i = 0; i < 8; i++) {
                    if (bytes[i] == 0) { break; }
                    r.Append((Char)bytes[i]);
                    }
                }
            return r.ToString();
            }
        }
    }