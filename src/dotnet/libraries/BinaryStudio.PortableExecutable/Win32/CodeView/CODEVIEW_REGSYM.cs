using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.TD32;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct CODEVIEW_REGSYM
        {
        public readonly Int16 Length;                   // Record length.
        public readonly DEBUG_SYMBOL_INDEX Type;        // S_REGISTER
        public readonly Int32 TypeIndexOrMetadataToken; // Type index or Metadata token.
        public readonly Int16 Register;                 // Register enumerate.
        /* unsigned char* Name; */                      // Length-prefixed name.
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct CODEVIEW_REGSYM16
        {
        public readonly Int16 Length;                   // Record length.
        public readonly DEBUG_SYMBOL_INDEX Type;        // S_REGISTER_16
        public readonly Int16 TypeIndex;                // Type index.
        public readonly UInt16 Register;                // Register enumerate.
        /* unsigned char* Name; */                      // Length-prefixed name.
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct TD32_REGSYM
        {
        public readonly Int16 Length;                // Record length.
        public readonly TD32SymbolIndex Type;        // S_REGISTER
        public readonly Int16 TypeIndex;             // Type index.
        public readonly UInt16 Register;             // Register enumerate.
        public readonly Int16 BrowserOffset;
        public readonly Int32 NameIndex;
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct CODEVIEW_ATTRREGSYM
        {
        public readonly Int16 Length;                   // Record length.
        public readonly DEBUG_SYMBOL_INDEX Type;        // S_MANREGISTER | S_ATTR_REGISTER
        public readonly Int32 TypeIndexOrMetadataToken; // Type index or Metadata token.
        public readonly CODEVIEW_LVAR Attributes;       // Local var attributes.
        public readonly Int16 Register;                 // Register enumerate.
        /* unsigned char* Name; */                      // Length-prefixed name.
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct CODEVIEW_MANYREGSYM16
        {
        public readonly Int16 Length;                   // Record length.
        public readonly DEBUG_SYMBOL_INDEX Type;        // S_MANYREG16
        public readonly Int16 TypeIndex;                // Type index.
        public readonly Byte  Count;                    // Count of number of registers.
        /* Byte Register[]; */                          // Count register enumerates followed by length-prefixed name. Registers are most significant first.
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct CODEVIEW_MANYREGSYM
        {
        public readonly Int16 Length;                   // Record length.
        public readonly DEBUG_SYMBOL_INDEX Type;        // S_MANYREG
        public readonly Int32 TypeIndexOrMetadataToken; // Type index or Metadata token.
        public readonly Byte  Count;                    // Count of number of registers.
        /* Byte Register[]; */                          // Count register enumerates followed by length-prefixed name. Registers are most significant first.
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct CODEVIEW_MANYREGSYM2
        {
        public readonly Int16 Length;                   // Record length.
        public readonly DEBUG_SYMBOL_INDEX Type;        // S_MANYREG
        public readonly Int32 TypeIndexOrMetadataToken; // Type index or Metadata token.
        public readonly Int16 Count;                    // Count of number of registers.
        /* Int16 Register[]; */                         // Count register enumerates followed by length-prefixed name. Registers are most significant first.
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct CODEVIEW_ATTRMANYREGSYM
        {
        public readonly Int16 Length;                   // Record length.
        public readonly DEBUG_SYMBOL_INDEX Type;        // S_MANMANYREG
        public readonly Int32 TypeIndexOrMetadataToken; // Type index or Metadata token.
        public readonly CODEVIEW_LVAR Attributes;       // Local var attributes.
        public readonly Byte  Count;                    // Count of number of registers.
        /* Byte Register[]; */                          // Count register enumerates followed by length-prefixed name. Registers are most significant first.
        /* Byte Name[];     */                          // UTF-8 encoded zero terminate name.
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay(@"\{{Type}\}")]
    public struct CODEVIEW_ATTRMANYREGSYM2
        {
        public readonly Int16 Length;                   // Record length.
        public readonly DEBUG_SYMBOL_INDEX Type;        // S_MANMANYREG
        public readonly Int32 TypeIndexOrMetadataToken; // Type index or Metadata token.
        public readonly CODEVIEW_LVAR Attributes;       // Local var attributes.
        public readonly Int16  Count;                   // Count of number of registers.
        /* Int16 Register[]; */                         // Count register enumerates followed by length-prefixed name. Registers are most significant first.
        /* Byte Name[];     */                          // UTF-8 encoded zero terminate name.
        }
    }