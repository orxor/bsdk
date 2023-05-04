using System;
using System.IO;
using System.Text;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.Win32;

// ReSharper disable ParameterHidesMember
// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.TD32
    {
    [TD32Symbol(TD32SymbolIndex.S_GPROC32)]
    internal class S_GPROC32 : S_PROCSYM32
        {
        public String LinkerName { get; }
        public override TD32SymbolIndex Type { get { return TD32SymbolIndex.S_GPROC32; }}
        public unsafe S_GPROC32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var Header = (TD32_PROCSYM32*)Content;
            LinkerName = ToString(Encoding,(Byte*)(Header + 1),IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} {3:x4}:{4:x8}-{5:x8}", LinePrefix,Offset,Type,SegmentIndex,ProcedureOffset,ProcedureOffset+ProcedureLength-1);
            Writer.WriteLine("{0}  Debug:{1:x4}:{2:x8}-{3:x8} TypeIndex:{4:x4}", LinePrefix,SegmentIndex,ProcedureOffset+DbgStart,ProcedureOffset+DbgEnd,TypeIndex);
            Writer.WriteLine("{0}  NameIndex:{{{1}}}:{{{2}}}", LinePrefix,NameIndex.ToString("x8"),NameTable[NameIndex-1]);
            Writer.WriteLine("{0}  Parent:{1:x8} End:{2:x8} Next:{3:x8}", LinePrefix,Parent,End,Next);
            Writer.WriteLine("{0}  LinkerName:'{1}'", LinePrefix,LinkerName ?? String.Empty);
            }
        }
    }