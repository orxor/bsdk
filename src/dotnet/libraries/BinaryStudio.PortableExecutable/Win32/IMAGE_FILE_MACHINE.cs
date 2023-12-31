﻿namespace BinaryStudio.PortableExecutable.Win32
    {
    public enum IMAGE_FILE_MACHINE : ushort
        {
        IMAGE_FILE_MACHINE_UNKNOWN      = 0x0000,
        IMAGE_FILE_MACHINE_AM33         = 0x01d3,
        IMAGE_FILE_MACHINE_AMD64        = 0x8664,
        IMAGE_FILE_MACHINE_ARM          = 0x01c0,
        IMAGE_FILE_MACHINE_ARM64        = 0xaa64,
        IMAGE_FILE_MACHINE_ARMNT        = 0x01c4,
        IMAGE_FILE_MACHINE_EBC          = 0x0ebc,
        IMAGE_FILE_MACHINE_I386         = 0x014c,
        IMAGE_FILE_MACHINE_IA64         = 0x0200,
        IMAGE_FILE_MACHINE_M32R         = 0x9041,
        IMAGE_FILE_MACHINE_MIPS16       = 0x0266,
        IMAGE_FILE_MACHINE_MIPSFPU      = 0x0366,
        IMAGE_FILE_MACHINE_MIPSFPU16    = 0x0466,
        IMAGE_FILE_MACHINE_POWERPC      = 0x01f0,
        IMAGE_FILE_MACHINE_POWERPCFP    = 0x01f1,
        IMAGE_FILE_MACHINE_R3000        = 0x0162,
        IMAGE_FILE_MACHINE_R10000       = 0x0168,
        IMAGE_FILE_MACHINE_R4000        = 0x0166,
        IMAGE_FILE_MACHINE_RISCV32      = 0x5032,
        IMAGE_FILE_MACHINE_RISCV64      = 0x5064,
        IMAGE_FILE_MACHINE_RISCV128     = 0x5128,
        IMAGE_FILE_MACHINE_SH3          = 0x01a2,
        IMAGE_FILE_MACHINE_SH3DSP       = 0x01a3,
        IMAGE_FILE_MACHINE_SH4          = 0x01a6,
        IMAGE_FILE_MACHINE_SH5          = 0x01a8,
        IMAGE_FILE_MACHINE_THUMB        = 0x01c2,
        IMAGE_FILE_MACHINE_ALPHA        = 0x0184,
        IMAGE_FILE_MACHINE_WCEMIPSV2    = 0x0169,
        IMAGE_FILE_MACHINE_ALPHA64      = 0x0284,
        IMAGE_FILE_MACHINE_TRICORE      = 0x0520,
        IMAGE_FILE_MACHINE_CEE          = 0xC0EE,
        IMAGE_FILE_MACHINE_CEF          = 0x0CEF
        }
    }