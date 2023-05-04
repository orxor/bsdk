using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_OBJNAME)]
    internal class S_OBJNAME : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_OBJNAME; }}
        public String Value { get; }
        public UInt32 Signature { get; }
        public unsafe S_OBJNAME(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (OBJNAMESYM*)Content;
            Signature = r->Signature;
            Value = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} Signature:{3:x8} {4}",
                LinePrefix,Offset,Type,Signature,Value);
            }
        }

    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_OBJNAME_ST)]
    internal class S_OBJNAME_ST : S_OBJNAME
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_OBJNAME_ST; }}
        public S_OBJNAME_ST(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            }
        }
    }