﻿using System;

namespace BinaryStudio.PortableExecutable.DebugEngine
    {
    [Flags]
    public enum DEBUG_FORMAT : uint
        {
        DEBUG_FORMAT_DEFAULT                              = 0x00000000,
        DEBUG_FORMAT_CAB_SECONDARY_ALL_IMAGES             = 0x10000000,
        DEBUG_FORMAT_WRITE_CAB                            = 0x20000000,
        DEBUG_FORMAT_CAB_SECONDARY_FILES                  = 0x40000000,
        DEBUG_FORMAT_NO_OVERWRITE                         = 0x80000000,
        DEBUG_FORMAT_USER_SMALL_FULL_MEMORY               = 0x00000001,
        DEBUG_FORMAT_USER_SMALL_HANDLE_DATA               = 0x00000002,
        DEBUG_FORMAT_USER_SMALL_UNLOADED_MODULES          = 0x00000004,
        DEBUG_FORMAT_USER_SMALL_INDIRECT_MEMORY           = 0x00000008,
        DEBUG_FORMAT_USER_SMALL_DATA_SEGMENTS             = 0x00000010,
        DEBUG_FORMAT_USER_SMALL_FILTER_MEMORY             = 0x00000020,
        DEBUG_FORMAT_USER_SMALL_FILTER_PATHS              = 0x00000040,
        DEBUG_FORMAT_USER_SMALL_PROCESS_THREAD_DATA       = 0x00000080,
        DEBUG_FORMAT_USER_SMALL_PRIVATE_READ_WRITE_MEMORY = 0x00000100,
        DEBUG_FORMAT_USER_SMALL_NO_OPTIONAL_DATA          = 0x00000200,
        DEBUG_FORMAT_USER_SMALL_FULL_MEMORY_INFO          = 0x00000400,
        DEBUG_FORMAT_USER_SMALL_THREAD_INFO               = 0x00000800,
        DEBUG_FORMAT_USER_SMALL_CODE_SEGMENTS             = 0x00001000,
        DEBUG_FORMAT_USER_SMALL_NO_AUXILIARY_STATE        = 0x00002000,
        DEBUG_FORMAT_USER_SMALL_FULL_AUXILIARY_STATE      = 0x00004000,
        DEBUG_FORMAT_USER_SMALL_MODULE_HEADERS            = 0x00008000,
        DEBUG_FORMAT_USER_SMALL_FILTER_TRIAGE             = 0x00010000,
        DEBUG_FORMAT_USER_SMALL_ADD_AVX_XSTATE_CONTEXT    = 0x00020000,
        DEBUG_FORMAT_USER_SMALL_IPT_TRACE                 = 0x00040000,
        DEBUG_FORMAT_USER_SMALL_IGNORE_INACCESSIBLE_MEM   = 0x08000000,
        DEBUG_FORMAT_USER_SMALL_SCAN_PARTIAL_PAGES        = 0x10000000
        }
    }