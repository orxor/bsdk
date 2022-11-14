using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.Win32;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_COMPILE)]
    internal class S_COMPILE : CodeViewSymbol
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [DebuggerDisplay(@"\{{" + nameof(Type) + @"}\}")]
        internal struct COMPILESYM
            {
            public readonly Int16 Length;
            public readonly DEBUG_SYMBOL_INDEX Type;
            public readonly Byte Machine;
            public readonly CV_CFL_LANG Language;
            public readonly UInt16 Flags;
            }

        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_COMPILE; }}
        public String CompilerVersion { get; }
        public CV_CFL_LANG Language { get; }
        public CV_CPU_TYPE Machine { get; }
        public Boolean IsPCodePresent { get; }
        public Boolean IsMode32 { get; }
        public NEMemoryModel AmbientData { get; }
        public NEMemoryModel AmbientCode { get; }
        public NEFloatingPackage FloatPackage { get; } 
        public Int32 FloatPrecision { get; }

        public unsafe S_COMPILE(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (COMPILESYM*)Content;
            Language = r->Language;
            Machine  = (CV_CPU_TYPE)(UInt16)r->Machine;
            IsPCodePresent = (r->Flags & 0x0001) == 0x0001;
            IsMode32 = (r->Flags & 0x0800) == 0x0800;
            FloatPrecision = (r->Flags >> 1) & 0x03;
            FloatPackage = (NEFloatingPackage)(Int32)((r->Flags >> 3) & 0x03);
            AmbientData = (NEMemoryModel)(Int32)((r->Flags >> 5) & 0x07);
            AmbientCode = (NEMemoryModel)(Int32)((r->Flags >> 8) & 0x07);
            CompilerVersion = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString).Trim();
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2}",LinePrefix,Offset,Type);
            Writer.WriteLine("{0}  Language:{1} Machine:{2}",LinePrefix,Language,Machine);
            Writer.WriteLine("{0}  IsPCodePresent:{1} IsMode32:{2}",LinePrefix,IsPCodePresent,IsMode32);
            Writer.WriteLine("{0}  FloatPrecision:{1} FloatPackage:{2}",LinePrefix,FloatPrecision,FloatPackage);
            Writer.WriteLine("{0}  AmbientData:{1} AmbientCode:{2}",LinePrefix,AmbientData,AmbientCode);
            Writer.WriteLine("{0}  CompilerVersion:{1}",LinePrefix,CompilerVersion);
            }
        }

    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_COMPILE3)]
    internal class S_COMPILE3 : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_COMPILE3; }}
        public String CompilerVersion { get; }
        public Version FrontEndVersion { get; }
        public Version BackEndVersion { get; }
        public CV_CFL_LANG Language { get; }
        public Boolean IsCompiledForEditAndContinue { get; }
        public Boolean IsCompiledWithDebugInfo { get; }
        public Boolean IsCompiledWithLTCG { get; }
        public Boolean IsNoDataAlign { get; }
        public Boolean IsManagedPresent { get; }
        public Boolean IsSecurityChecks { get; }
        public Boolean IsHotPatch { get; }
        public Boolean IsConvertedWithCVTCIL { get; }
        public Boolean IsMSILModule { get; }
        public Boolean IsCompiledWithSDL { get; }
        public Boolean IsCompiledWithPGO { get; }
        public Boolean IsEXPModule { get; }
        public CV_CPU_TYPE Machine { get; }

        public unsafe S_COMPILE3(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (COMPILESYM3*)Content;
            FrontEndVersion = new Version(r->VersionFEMajor, r->VersionFEMinor, r->VersionFEBuild, r->VersionFEQFE);
            BackEndVersion  = new Version(r->VersionMajor, r->VersionMinor, r->VersionBuild, r->VersionFEQFE);
            Language = (CV_CFL_LANG)(r->Flags & 0xFF);
            IsCompiledForEditAndContinue = (r->Flags & 0x00100) == 0x00100;
            IsCompiledWithDebugInfo      = (r->Flags & 0x00200) == 0x00200;
            IsCompiledWithLTCG           = (r->Flags & 0x00400) == 0x00400;
            IsNoDataAlign                = (r->Flags & 0x00800) == 0x00800;
            IsManagedPresent             = (r->Flags & 0x01000) == 0x01000;
            IsSecurityChecks             = (r->Flags & 0x02000) == 0x02000;
            IsHotPatch                   = (r->Flags & 0x04000) == 0x04000;
            IsConvertedWithCVTCIL        = (r->Flags & 0x08000) == 0x08000;
            IsMSILModule                 = (r->Flags & 0x10000) == 0x10000;
            IsCompiledWithSDL            = (r->Flags & 0x20000) == 0x20000;
            IsCompiledWithPGO            = (r->Flags & 0x40000) == 0x40000;
            IsEXPModule                  = (r->Flags & 0x80000) == 0x80000;
            Machine = r->Machine;
            CompilerVersion = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString);
            Section.Machine = Machine;
            if (Section.Section.CommonObjectFile.CPU == null) {
                Section.Section.CommonObjectFile.CPU = Machine;
                }
            }
        }
    }