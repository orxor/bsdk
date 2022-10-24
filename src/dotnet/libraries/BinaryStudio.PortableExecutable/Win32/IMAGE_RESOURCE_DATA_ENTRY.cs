using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    /// <summary>
    /// [Resource Data Entry]
    /// Each Resource Data entry describes an actual unit of raw data in the Resource Data area.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_RESOURCE_DATA_ENTRY
        {
        public  readonly UInt32 OffsetToData;           /* The address of a unit of resource data in the Resource Data area.                                                                            */
        public  readonly UInt32 Size;                   /* The size, in bytes, of the resource data that is pointed to by the Data RVA field.                                                           */
        public  readonly UInt32 CodePage;               /* The code page that is used to decode code point values within the resource data. Typically, the code page would be the Unicode code page.    */
        private readonly UInt32 Reserved;               /* Reserved, must be 0.                                                                                                                         */
        }
    }