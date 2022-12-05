using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    internal abstract class S_BPRELSYM16 : CodeViewSymbol
        {
        public Int16 TypeIndex { get; }
        public UInt16 BasePointerRegisterOffset { get; }
        public String Name { get; }

        protected unsafe S_BPRELSYM16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (BPRELSYM16*)Content;
            TypeIndex = r->TypeIndex;
            BasePointerRegisterOffset = r->Offset;
            Name = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} [ebp+{3:x4}] Type:{4:x4} Name:{5}", LinePrefix,base.Offset,Type,BasePointerRegisterOffset,TypeIndex,Name);
            }
        }
    }