using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_REGISTER16)]
    [UsedImplicitly]
    internal class S_REGISTER16 : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_REGISTER16; }}
        public Int16 TypeIndex { get; }
        public UInt16 Register { get; }
        public String Name { get; }
        public unsafe S_REGISTER16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (CODEVIEW_REGSYM16*)Content;
            TypeIndex = Header->TypeIndex;
            Register = Header->Register;
            Name = ToString(Encoding, (Byte*)(Header + 1), IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} TypeIndex:{3:x4} Register:{4}", LinePrefix,Offset,Type,TypeIndex,DecodeRegister(Register));
            if (!String.IsNullOrWhiteSpace(Name)) {
                Writer.WriteLine("{0}  Name:{{{1}}}", LinePrefix,Name);
                }
            }
        }
    }