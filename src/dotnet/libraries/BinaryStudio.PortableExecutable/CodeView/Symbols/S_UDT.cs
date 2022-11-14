using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_UDT)]
    internal class S_UDT : CodeViewSymbol
        {
        public Int32 TypeIndex { get; }
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_UDT; }}
        public String Name { get; }
        public unsafe S_UDT(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (UDTSYM*)Content;
            TypeIndex = r->TypeIndex;
            Name = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString);
            }
        }

    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_UDT16)]
    [UsedImplicitly]
    internal class S_UDT16 : CodeViewSymbol
        {
        public Int16 TypeIndex { get; }
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_UDT16; }}
        public String Name { get; }
        public unsafe S_UDT16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (UDTSYM16*)Content;
            TypeIndex = r->TypeIndex;
            Name = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} TypeIndex:{3:x4} {4}", LinePrefix,Offset,Type,TypeIndex,Name);
            }
        }
    }