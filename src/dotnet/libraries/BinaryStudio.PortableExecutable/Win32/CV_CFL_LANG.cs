using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PortableExecutable.Win32
    {
    public enum CV_CFL_LANG : byte
        {
        [Display(Name="C Programming Language")] LANG_C        = 0x00,
        [Display(Name="C++")] LANG_CXX      = 0x01,
        [Display(Name="FORTRAN")] LANG_FORTRAN  = 0x02,
        [Display(Name="Microsoft Macro Assembler (MASM)")] LANG_MASM     = 0x03,
        [Display(Name="Pascal")] LANG_PASCAL   = 0x04,
        [Display(Name="BASIC")] LANG_BASIC    = 0x05,
        [Display(Name="COBOL")] LANG_COBOL    = 0x06,
        [Display(Name="LINK")] LANG_LINK     = 0x07,
        [Display(Name="Microsoft Resource File To COFF Object Conversion Utility (CVTRES)")] LANG_CVTRES   = 0x08,
        [Display(Name="CVTPGD")] LANG_CVTPGD   = 0x09,
        [Display(Name="C#")] LANG_CSHARP   = 0x0A,  // C#
        [Display(Name="Visual Basic")] LANG_VB       = 0x0B,  // Visual Basic
        [Display(Name="Intermediate Language Assembly")] LANG_ILASM    = 0x0C,  // IL (as in CLR) ASM
        [Display(Name="Java")] LANG_JAVA     = 0x0D,
        [Display(Name="JScript")] LANG_JSCRIPT  = 0x0E,
        [Display(Name="Microsoft Intermediate Language (MSIL)")] LANG_MSIL     = 0x0F,  // Unknown MSIL (LTCG of .NETMODULE)
        [Display(Name="High Level Shader Language HLSL)")] LANG_HLSL     = 0x10,  // High Level Shader Language
        [Display(Name="Objective C")] CV_CFL_OBJC     = 0x11,
        [Display(Name="Objective C++")] CV_CFL_OBJCXX   = 0x12,
        [Display(Name="SWIFT")] CV_CFL_SWIFT    = 0x13,
        [Display(Name="ALIASOBJ")] CV_CFL_ALIASOBJ = 0x14,
        [Display(Name="RUST")] CV_CFL_RUST     = 0x15,
        }
    }