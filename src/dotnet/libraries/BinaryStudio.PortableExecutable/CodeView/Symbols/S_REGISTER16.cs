using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_REGISTER16)]
    internal class S_REGISTER16 : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_REGISTER16; }}
        public Int16 TypeIndex { get; }
        public Int16 Register { get; }
        public unsafe S_REGISTER16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (CODEVIEW_REGSYM16*)Content;
            TypeIndex = Header->TypeIndex;
            Register = Header->Register;
            }
        }
    }