﻿namespace BinaryStudio.PortableExecutable.Win32
    {
    /// <summary>
    /// Symbol Indices.
    /// </summary>
    public enum DEBUG_SYMBOL_INDEX : ushort
        {
        S_NONE                  = 0x0000,
        /// <summary>
        /// This symbol communicates with Microsoft debugger compile-time information,
        /// such as the language and version number of the language processor,
        /// the ambient model for code and data, and the target processor,
        /// on a per-module basis.
        /// </summary>
        S_COMPILE               = 0x0001,
        /// <summary>
        /// This symbol record describes a symbol that has been placed in a register.
        /// Provisions for enabling future implementation tracking of a symbol into and out of registers is provided in this symbol.
        /// When the symbol processor is examining a register symbol, the length field of the symbol is compared with the offset of the byte following the symbol name field.
        /// If these are the same, there is no register tracking information.
        /// If the length and offset are different, the byte following the end of the symbol name is examined.
        /// If the byte is zero, there is no register tracking information following the symbol.
        /// If the byte is not zero, then the byte is the index into the list of stack machine implementations and styles of register tracking.
        /// </summary>
        S_REGISTER16            = 0x0002,
        S_CONSTANT16            = 0x0003,
        /// <summary>
        /// This specifies a C typedef or user-defined type, such as classes, structures, unions, or enums.
        /// </summary>
        S_UDT16                 = 0x0004,
        /// <summary>
        /// Start search.
        /// These records are always the first symbol records in a module's $$SYMBOL section.
        /// There is one <b>Start Search</b> symbol for each segment (PE section) to which the module contributes code.
        /// Each <b>Start Search</b> symbol contains the segment (PE section) number and $$SYMBOL offset of the record of the outermost lexical scope in this module that physically appears first in the specified segment of the load image.
        /// This referenced symbol is the symbol used to initiate context searches within this module.
        /// The <b>Start Search</b> symbols are inserted into the $$SYMBOLS table by the CVPACK utility and must not be emitted by the language processor.
        /// </summary>
        S_SSEARCH               = 0x0005,
        /// <summary>
        /// Closes the scope of the nearest preceding Block Start, Global Procedure Start, Local Procedure Start, With Start, or Thunk Start definition.
        /// </summary>
        S_END                   = 0x0006,
        S_SKIP                  = 0x0007,
        S_CVRESERVE             = 0x0008,
        /// <summary>
        /// This symbol specifies the name of the object file for this module.
        /// </summary>
        S_OBJNAME_ST            = 0x0009,
        S_ENDARG                = 0x000a,
        S_COBOLUDT16            = 0x000b,
        S_MANYREG16             = 0x000c,
        S_RETURN                = 0x000d,
        S_ENTRYTHIS             = 0x000e,
        S_USES                  = 0x0024,
        S_NAMESPACE             = 0x0025,
        S_USING                 = 0x0026,
        S_PCONSTANT             = 0x0027,
        /// <summary>
        /// This symbol specifies symbols that are allocated on the stack for a procedure.
        /// </summary>
        S_BPREL16               = 0x0100,
        /// <summary>
        /// These symbols are used for data that is not exported from a module.
        /// In C and C++, symbols that are declared static are emitted as Local Data symbols.
        /// Symbols that are emitted as Local Data cannot be moved by the CVPACK utility into the global symbol table for the executable file.
        /// </summary>
        S_LDATA16               = 0x0101,
        /// <summary>
        /// This symbol record has the same format as the Local Data 16:16 except that the record type is S_GDATA16.
        /// For C and C++, symbols that are not specifically declared static are emitted as Global Data Symbols and can be compacted by the CVPACK utility into the global symbol table.
        /// </summary>
        S_GDATA16               = 0x0102,
        S_PUB16                 = 0x0103,
        /// <summary>
        /// This symbol record defines local (file static) procedure definitions.
        /// </summary>
        S_LPROC16               = 0x0104,
        /// <summary>
        /// This symbol is used for procedures that are not specifically declared static to a module.
        /// </summary>
        S_GPROC16               = 0x0105,
        S_THUNK16               = 0x0106,
        /// <summary>
        /// This symbol specifies the start of an inner block of lexically scoped symbols.
        /// </summary>
        S_BLOCK16               = 0x0107,
        S_WITH16                = 0x0108,
        S_LABEL16               = 0x0109,
        S_CEXMODEL16            = 0x010a,
        S_VFTPATH16             = 0x010b,
        S_REGREL16              = 0x010c,
        S_BPREL32_16            = 0x0200,
        S_LDATA32_16            = 0x0201,
        S_GDATA32_16            = 0x0202,
        S_PUB32_16              = 0x0203,
        /// <summary>
        /// This symbol record defines local (file static) procedure definition.
        /// For C and C++, functions that are declared static to a module are emitted as <b>Local Procedure</b> symbols.
        /// </summary>
        S_LPROC32_16            = 0x0204,
        /// <summary>
        /// This symbol is used for procedures that are not specifically declared static to a module.
        /// </summary>
        S_GPROC32_16            = 0x0205,
        S_THUNK32_ST            = 0x0206,
        S_BLOCK32_ST            = 0x0207,
        S_WITH32_ST             = 0x0208,
        S_LABEL32_ST            = 0x0209,
        S_CEXMODEL32            = 0x020a,
        S_VFTPATH32             = 0x020b,
        S_REGREL32_16           = 0x020c,
        S_LTHREAD32_16          = 0x020d,
        S_GTHREAD32_16          = 0x020e,
        S_SLINK32               = 0x020f,
        S_OPTVAR32              = 0x0211,
        S_LPROCMIPS16           = 0x0300,
        S_GPROCMIPS16           = 0x0301,
        S_PROCREF_ST            = 0x0400,
        S_DATAREF_ST            = 0x0401,
        S_ALIGN                 = 0x0402,
        S_LPROCREF_ST           = 0x0403,
        S_OEM                   = 0x0404,
        S_TI16_MAX              = 0x1000,
        S_REGISTER_ST           = 0x1001,  // Register variable
        S_CONSTANT_ST           = 0x1002,  // constant symbol
        S_UDT_ST                = 0x1003,  // User defined type
        S_COBOLUDT_ST           = 0x1004,  // special UDT for cobol that does not symbol pack
        S_MANYREG_ST            = 0x1005,  // multiple register variable
        S_BPREL32_ST            = 0x1006,  // BP-relative
        S_LDATA32_ST            = 0x1007,  // Module-local symbol
        S_GDATA32_ST            = 0x1008,  // Global data symbol
        S_PUB32_ST              = 0x1009,  // a public symbol (CV internal reserved)
        S_LPROC32_ST            = 0x100a,  // Local procedure start
        S_GPROC32_ST            = 0x100b,  // Global procedure start
        S_VFTABLE32             = 0x100c,  // address of virtual function table
        S_REGREL32_ST           = 0x100d,  // register relative address
        S_LTHREAD32_ST          = 0x100e,  // local thread storage
        S_GTHREAD32_ST          = 0x100f,  // global thread storage
        S_LPROCMIPS_ST          = 0x1010,  // Local procedure start
        S_GPROCMIPS_ST          = 0x1011,  // Global procedure start
        S_FRAMEPROC             = 0x1012,  // extra frame and proc information
        S_COMPILE2_ST           = 0x1013,  // extended compile flags and info
        S_MANYREG2_ST           = 0x1014,  // multiple register variable
        S_LPROCIA64_ST          = 0x1015,  // Local procedure start (IA64)
        S_GPROCIA64_ST          = 0x1016,  // Global procedure start (IA64)
        S_LOCALSLOT_ST          = 0x1017,  // local IL sym with field for local slot index
        S_PARAMSLOT_ST          = 0x1018,  // local IL sym with field for parameter slot index
        S_ANNOTATION            = 0x1019,  // Annotation string literals
        S_GMANPROC_ST           = 0x101a,  // Global proc
        S_LMANPROC_ST           = 0x101b,  // Local proc
        S_RESERVED1             = 0x101c,  // reserved
        S_RESERVED2             = 0x101d,  // reserved
        S_RESERVED3             = 0x101e,  // reserved
        S_RESERVED4             = 0x101f,  // reserved
        S_LMANDATA_ST           = 0x1020,
        S_GMANDATA_ST           = 0x1021,
        S_MANFRAMEREL_ST        = 0x1022,
        S_MANREGISTER_ST        = 0x1023,
        S_MANSLOT_ST            = 0x1024,
        S_MANMANYREG_ST         = 0x1025,
        S_MANREGREL_ST          = 0x1026,
        S_MANMANYREG2_ST        = 0x1027,
        S_MANTYPREF             = 0x1028,  // Index for type referenced by name from metadata
        S_UNAMESPACE_ST         = 0x1029,  // Using namespace
        S_ST_MAX                = 0x1100,  // starting point for SZ name symbols
        S_OBJNAME               = 0x1101,  // path to object file name
        S_THUNK32               = 0x1102,  // Thunk Start
        S_BLOCK32               = 0x1103,  // block start
        S_WITH32                = 0x1104,  // with start
        S_LABEL32               = 0x1105,  // code label
        S_REGISTER              = 0x1106,  // Register variable
        S_CONSTANT              = 0x1107,  // constant symbol
        S_UDT                   = 0x1108,  // User defined type
        S_COBOLUDT              = 0x1109,  // special UDT for cobol that does not symbol pack
        S_MANYREG               = 0x110a,  // multiple register variable
        S_BPREL32               = 0x110b,  // BP-relative
        S_LDATA32               = 0x110c,  // Module-local symbol
        S_GDATA32               = 0x110d,  // Global data symbol
        S_PUB32                 = 0x110e,  // a public symbol (CV internal reserved)
        S_LPROC32               = 0x110f,  // Local procedure start
        S_GPROC32               = 0x1110,  // Global procedure start
        S_REGREL32              = 0x1111,  // register relative address
        S_LTHREAD32             = 0x1112,  // local thread storage
        S_GTHREAD32             = 0x1113,  // global thread storage
        S_LPROCMIPS             = 0x1114,  // Local procedure start
        S_GPROCMIPS             = 0x1115,  // Global procedure start
        S_COMPILE2              = 0x1116,  // extended compile flags and info
        S_MANYREG2              = 0x1117,  // multiple register variable
        S_LPROCIA64             = 0x1118,  // Local procedure start (IA64)
        S_GPROCIA64             = 0x1119,  // Global procedure start (IA64)
        S_LOCALSLOT             = 0x111a,  // local IL sym with field for local slot index
        S_SLOT                  = S_LOCALSLOT,  // alias for LOCALSLOT
        S_PARAMSLOT             = 0x111b,  // local IL sym with field for parameter slot index
        S_LMANDATA              = 0x111c,
        S_GMANDATA              = 0x111d,
        S_MANFRAMEREL           = 0x111e,
        S_MANREGISTER           = 0x111f,
        S_MANSLOT               = 0x1120,
        S_MANMANYREG            = 0x1121,
        S_MANREGREL             = 0x1122,
        S_MANMANYREG2           = 0x1123,
        S_UNAMESPACE            = 0x1124,  // Using namespace
        S_PROCREF               = 0x1125,  // Reference to a procedure
        S_DATAREF               = 0x1126,  // Reference to data
        S_LPROCREF              = 0x1127,  // Local Reference to a procedure
        S_ANNOTATIONREF         = 0x1128,  // Reference to an S_ANNOTATION symbol
        S_TOKENREF              = 0x1129,  // Reference to one of the many MANPROCSYM's
        S_GMANPROC              = 0x112a,  // Global proc
        S_LMANPROC              = 0x112b,  // Local proc
        S_TRAMPOLINE            = 0x112c,  // trampoline thunks
        S_MANCONSTANT           = 0x112d,  // constants with metadata type info
        S_ATTR_FRAMEREL         = 0x112e,  // relative to virtual frame ptr
        S_ATTR_REGISTER         = 0x112f,  // stored in a register
        S_ATTR_REGREL           = 0x1130,  // relative to register (alternate frame ptr)
        S_ATTR_MANYREG          = 0x1131,  // stored in >1 register
        S_SEPCODE               = 0x1132,
        S_LOCAL_2005            = 0x1133,  // defines a local symbol in optimized code
        S_DEFRANGE_2005         = 0x1134,  // defines a single range of addresses in which symbol can be evaluated
        S_DEFRANGE2_2005        = 0x1135,  // defines ranges of addresses in which symbol can be evaluated
        S_SECTION               = 0x1136,  // A COFF section in a PE executable
        S_COFFGROUP             = 0x1137,  // A COFF group
        S_EXPORT                = 0x1138,  // A export
        S_CALLSITEINFO          = 0x1139,  // Indirect call site information
        S_FRAMECOOKIE           = 0x113a,  // Security cookie information
        S_DISCARDED             = 0x113b,  // Discarded by LINK /OPT:REF (experimental, see richards)
        S_COMPILE3              = 0x113c,  // Replacement for S_COMPILE2
        S_ENVBLOCK              = 0x113d,  // Environment block split off from S_COMPILE2
        S_LOCAL                 = 0x113e,  // defines a local symbol in optimized code
        S_DEFRANGE              = 0x113f,  // defines a single range of addresses in which symbol can be evaluated
        S_DEFRANGE_SUBFIELD     = 0x1140,           // ranges for a subfield
        S_DEFRANGE_REGISTER     = 0x1141,           // ranges for en-registered symbol
        S_DEFRANGE_FRAMEPOINTER_REL =  0x1142,   // range for stack symbol.
        S_DEFRANGE_SUBFIELD_REGISTER =  0x1143,  // ranges for en-registered field of symbol
        S_DEFRANGE_FRAMEPOINTER_REL_FULL_SCOPE =  0x1144, // range for stack symbol span valid full scope of function body, gap might apply.
        S_DEFRANGE_REGISTER_REL = 0x1145, // range for symbol address as register + offset.
        S_LPROC32_ID            = 0x1146,
        S_GPROC32_ID            = 0x1147,
        S_LPROCMIPS_ID          = 0x1148,
        S_GPROCMIPS_ID          = 0x1149,
        S_LPROCIA64_ID          = 0x114a,
        S_GPROCIA64_ID          = 0x114b,
        S_BUILDINFO             = 0x114c, // build information.
        S_INLINESITE            = 0x114d, // inlined function callsite.
        S_INLINESITE_END        = 0x114e,
        S_PROC_ID_END           = 0x114f,
        S_DEFRANGE_HLSL         = 0x1150,
        S_GDATA_HLSL            = 0x1151,
        S_LDATA_HLSL            = 0x1152,
        S_FILESTATIC            = 0x1153,
        S_LOCAL_DPC_GROUPSHARED = 0x1154, // DPC groupshared variable
        S_LPROC32_DPC           = 0x1155, // DPC local procedure start
        S_LPROC32_DPC_ID        = 0x1156,
        S_DEFRANGE_DPC_PTR_TAG  = 0x1157, // DPC pointer tag definition range
        S_DPC_SYM_TAG_MAP       = 0x1158, // DPC pointer tag value to symbol record map
        S_ARMSWITCHTABLE        = 0x1159,
        S_CALLEES               = 0x115a,
        S_CALLERS               = 0x115b,
        S_POGODATA              = 0x115c,
        S_INLINESITE2           = 0x115d,      // extended inline site information
        S_HEAPALLOCSITE         = 0x115e,    // heap allocation site
        S_MOD_TYPEREF           = 0x115f,      // only generated at link time
        S_REF_MINIPDB           = 0x1160,      // only generated at link time for mini PDB
        S_PDBMAP                = 0x1161,      // only generated at link time for mini PDB
        S_GDATA_HLSL32          = 0x1162,
        S_LDATA_HLSL32          = 0x1163,
        S_GDATA_HLSL32_EX       = 0x1164,
        S_LDATA_HLSL32_EX       = 0x1165,
        }
    }