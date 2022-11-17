﻿namespace BinaryStudio.PortableExecutable.Win32
    {
    public enum LEAF_ENUM : ushort
        {
        LF_NONE             = 0x0000,
        LF_MODIFIER_16      = 0x0001,
        LF_POINTER_16       = 0x0002,
        LF_ARRAY_16         = 0x0003,
        LF_CLASS_16         = 0x0004,
        LF_STRUCTURE_16     = 0x0005,
        LF_UNION_16         = 0x0006,
        LF_ENUM_16          = 0x0007,
        LF_PROCEDURE_16     = 0x0008,
        LF_MFUNCTION_16     = 0x0009,
        LF_VTSHAPE          = 0x000a,
        LF_COBOL0_16t       = 0x000b,
        LF_COBOL1           = 0x000c,
        LF_BARRAY_16t       = 0x000d,
        LF_LABEL            = 0x000e,
        LF_NULL             = 0x000f,
        LF_NOTTRAN          = 0x0010,
        LF_DIMARRAY_16t     = 0x0011,
        LF_VFTPATH_16t      = 0x0012,
        LF_PRECOMP_16t      = 0x0013,       // not referenced from symbol
        LF_ENDPRECOMP       = 0x0014,       // not referenced from symbol
        LF_OEM_16t          = 0x0015,       // oem definable type string
        LF_TYPESERVER_ST    = 0x0016,       // not referenced from symbol
        LF_SKIP_16t         = 0x0200,
        LF_ARGLIST_16       = 0x0201,
        LF_DEFARG_16t       = 0x0202,
        LF_LIST             = 0x0203,
        LF_FIELDLIST_16     = 0x0204,
        LF_DERIVED_16       = 0x0205,
        LF_BITFIELD_16      = 0x0206,
        LF_METHODLIST_16    = 0x0207,
        LF_DIMCONU_16t      = 0x0208,
        LF_DIMCONLU_16t     = 0x0209,
        LF_DIMVARU_16t      = 0x020a,
        LF_DIMVARLU_16t     = 0x020b,
        LF_REFSYM           = 0x020c,
        LF_BCLASS_16        = 0x0400,
        LF_VBCLASS_16t      = 0x0401,
        LF_IVBCLASS_16t     = 0x0402,
        LF_ENUMERATE_ST     = 0x0403,
        LF_FRIENDFCN_16t    = 0x0404,
        LF_INDEX_16t        = 0x0405,
        LF_MEMBER_16        = 0x0406,
        LF_STMEMBER_16      = 0x0407,
        LF_METHOD_16        = 0x0408,
        LF_NESTTYPE_16      = 0x0409,
        LF_VFUNCTAB_16      = 0x040a,
        LF_FRIENDCLS_16t    = 0x040b,
        LF_ONEMETHOD_16t    = 0x040c,
        LF_VFUNCOFF_16t     = 0x040d,
        LF_TI16_MAX         = 0x1000,
        LF_MODIFIER         = 0x1001,
        LF_POINTER          = 0x1002,
        LF_ARRAY_ST         = 0x1003,
        LF_CLASS_ST         = 0x1004,
        LF_STRUCTURE_ST     = 0x1005,
        LF_UNION_ST         = 0x1006,
        LF_ENUM_ST          = 0x1007,
        LF_PROCEDURE        = 0x1008,
        LF_MFUNCTION        = 0x1009,
        LF_COBOL0           = 0x100a,
        LF_BARRAY           = 0x100b,
        LF_DIMARRAY_ST      = 0x100c,
        LF_VFTPATH          = 0x100d,
        LF_PRECOMP_ST       = 0x100e,       // not referenced from symbol
        LF_OEM              = 0x100f,       // oem definable type string
        LF_ALIAS_ST         = 0x1010,       // alias (typedef) type
        LF_OEM2             = 0x1011,       // oem definable type string
        LF_SKIP             = 0x1200,
        LF_ARGLIST          = 0x1201,
        LF_DEFARG_ST        = 0x1202,
        LF_FIELDLIST        = 0x1203,
        LF_DERIVED          = 0x1204,
        LF_BITFIELD         = 0x1205,
        LF_METHODLIST       = 0x1206,
        LF_DIMCONU          = 0x1207,
        LF_DIMCONLU         = 0x1208,
        LF_DIMVARU          = 0x1209,
        LF_DIMVARLU         = 0x120a,
        LF_BCLASS           = 0x1400,
        LF_VBCLASS          = 0x1401,
        LF_IVBCLASS         = 0x1402,
        LF_FRIENDFCN_ST     = 0x1403,
        LF_INDEX            = 0x1404,
        LF_MEMBER_ST        = 0x1405,
        LF_STMEMBER_ST      = 0x1406,
        LF_METHOD_ST        = 0x1407,
        LF_NESTTYPE_ST      = 0x1408,
        LF_VFUNCTAB         = 0x1409,
        LF_FRIENDCLS        = 0x140a,
        LF_ONEMETHOD_ST     = 0x140b,
        LF_VFUNCOFF         = 0x140c,
        LF_NESTTYPEEX_ST    = 0x140d,
        LF_MEMBERMODIFY_ST  = 0x140e,
        LF_MANAGED_ST       = 0x140f,
        LF_ST_MAX           = 0x1500,
        LF_TYPESERVER       = 0x1501,       // not referenced from symbol
        LF_ENUMERATE        = 0x1502,
        LF_ARRAY            = 0x1503,
        LF_CLASS            = 0x1504,
        LF_STRUCTURE        = 0x1505,
        LF_UNION            = 0x1506,
        LF_ENUM             = 0x1507,
        LF_DIMARRAY         = 0x1508,
        LF_PRECOMP          = 0x1509,       // not referenced from symbol
        LF_ALIAS            = 0x150a,       // alias (typedef) type
        LF_DEFARG           = 0x150b,
        LF_FRIENDFCN        = 0x150c,
        LF_MEMBER           = 0x150d,
        LF_STMEMBER         = 0x150e,
        LF_METHOD           = 0x150f,
        LF_NESTTYPE         = 0x1510,
        LF_ONEMETHOD        = 0x1511,
        LF_NESTTYPEEX       = 0x1512,
        LF_MEMBERMODIFY     = 0x1513,
        LF_MANAGED          = 0x1514,
        LF_TYPESERVER2      = 0x1515,
        LF_STRIDED_ARRAY    = 0x1516,    // same as LF_ARRAY, but with stride between adjacent elements
        LF_HLSL             = 0x1517,
        LF_MODIFIER_EX      = 0x1518,
        LF_INTERFACE        = 0x1519,
        LF_BINTERFACE       = 0x151a,
        LF_VECTOR           = 0x151b,
        LF_MATRIX           = 0x151c,
        LF_VFTABLE          = 0x151d,      // a virtual function table
        LF_ENDOFLEAFRECORD  = LF_VFTABLE,
        LF_TYPE_LAST,                    // one greater than the last type record
        LF_TYPE_MAX         = LF_TYPE_LAST - 1,
        LF_FUNC_ID          = 0x1601,    // global func ID
        LF_MFUNC_ID         = 0x1602,    // member func ID
        LF_BUILDINFO        = 0x1603,    // build info: tool, version, command line, src/pdb file
        LF_SUBSTR_LIST      = 0x1604,    // similar to LF_ARGLIST, for list of sub strings
        LF_STRING_ID        = 0x1605,    // string ID
        LF_UDT_SRC_LINE     = 0x1606,    // source and line on where an UDT is defined
                     // only generated by compiler
        LF_UDT_MOD_SRC_LINE = 0x1607,    // module, source and line on where an UDT is defined
                     // only generated by linker
        LF_ID_LAST,                      // one greater than the last ID record
        LF_ID_MAX           = LF_ID_LAST - 1,
        LF_NUMERIC          = 0x8000,
        LF_CHAR             = 0x8000,
        LF_SHORT            = 0x8001,
        LF_USHORT           = 0x8002,
        LF_LONG             = 0x8003,
        LF_ULONG            = 0x8004,
        LF_REAL32           = 0x8005,
        LF_REAL64           = 0x8006,
        LF_REAL80           = 0x8007,
        LF_REAL128          = 0x8008,
        LF_QUADWORD         = 0x8009,
        LF_UQUADWORD        = 0x800a,
        LF_REAL48           = 0x800b,
        LF_COMPLEX32        = 0x800c,
        LF_COMPLEX64        = 0x800d,
        LF_COMPLEX80        = 0x800e,
        LF_COMPLEX128       = 0x800f,
        LF_VARSTRING        = 0x8010,
        LF_OCTWORD          = 0x8017,
        LF_UOCTWORD         = 0x8018,
        LF_DECIMAL          = 0x8019,
        LF_DATE             = 0x801a,
        LF_UTF8STRING       = 0x801b,
        LF_REAL16           = 0x801c,
        LF_SET              = 0x0030,
        LF_SUBRANGE         = 0x0031,
        LF_PARRAY           = 0x0032,
        LF_PSTRING          = 0x0033,
        LF_CLOSURE          = 0x0034,
        LF_PROPERTY         = 0x0035,
        LF_LSTRING          = 0x0036,
        LF_VARIANT          = 0x0037,
        LF_CLASSREF         = 0x0038,
        LF_WSTRING          = 0x0039,
        }
    }