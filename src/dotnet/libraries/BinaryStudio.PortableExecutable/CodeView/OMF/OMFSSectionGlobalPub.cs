using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.GlobalPub)]
    [UsedImplicitly]
    internal class OMFSSectionGlobalPub : OMFSSection
        {
        public IList<CodeViewSymbol> Symbols { get;private set; }
        public OMFSSectionGlobalPub(OMFDirectory Directory)
            : base(Directory)
            {
            }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct HEADER
            {
            public readonly Int16 SymbolHashFunctionIndex;
            public readonly Int16 AddressHashFunctionIndex;
            public readonly Int32 SymbolTableSize;
            public readonly Int32 SymbolHashTableSize;
            public readonly Int32 AddressHashingTableSize;
            }

        private static T Peek<T>(Stack<T> Source)
            {
            return ((Source != null) && (Source.Count > 0))
                ? Source.Peek()
                : default;
            }

        private static T Pop<T>(Stack<T> Source)
            {
            return ((Source != null) && (Source.Count > 0))
                ? Source.Pop()
                : default;
            }

        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.GlobalPub; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            var SectionHeader = (HEADER*)Source;
            var SymbolTableSize = SectionHeader->SymbolTableSize;
            Symbols = new List<CodeViewSymbol>();
            var Header = (CODEVIEW_SYMBOL_RECORD_HEADER*)(SectionHeader + 1);
            SymbolTableSize -= sizeof(CODEVIEW_SYMBOL_RECORD_HEADER);
            var PackageStack = new Stack<ICodeViewBlockStart>();
            while (SymbolTableSize > 0) {
                var Offset = (Byte*)Header - BaseAddress;
                CodeViewSymbol Target;
                Symbols.Add(Target = CodeViewSymbol.From(
                    null,(Int32)Offset,Header->Type,(Byte*)(Header),
                    Header->Length + sizeof(Int16)));
                Target.NameTable = (ICodeViewNameTable)Directory.GetService(typeof(ICodeViewNameTable));
                Target.CPU = CPU;
                if (Target is ICodeViewBlockElement PackageableElement) { PackageableElement.BlockStart = Peek(PackageStack); }
                if (Target is ICodeViewBlockEnd) { Pop(PackageStack); }
                if (Target is ICodeViewBlockStart LocalPackage) {
                    PackageStack.Push(LocalPackage);
                    }
                SymbolTableSize -= Header->Length + sizeof(Int16);
                Header = (CODEVIEW_SYMBOL_RECORD_HEADER*)((Byte*)Header + Header->Length + sizeof(Int16));
                }
            return this;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            if (Writer == null) { throw new ArgumentNullException(nameof(Writer)); }
            foreach (var o in Symbols) {
                if (!o.Status.HasFlag(CodeViewSymbolStatus.HasFileDumpWrite))
                    {
                    continue;
                    }
                o.WriteTo(Writer,LinePrefix,Flags);
                }
            }

        #region M:WriteXml(XmlWriter)
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public override void WriteXml(XmlWriter writer) {
            writer.WriteStartElement("Section");
                writer.WriteAttributeString("Type",SectionIndex.ToString());
                writer.WriteAttributeString(nameof(ModuleIndex),ModuleIndex.ToString());
                writer.WriteAttributeString("Offset",FileOffset.ToString());
                writer.WriteAttributeString(nameof(Size),Size.ToString());
                writer.WriteStartElement(nameof(Symbols));
                foreach (var symbol in Symbols
                    .Where(i => i.Type != DEBUG_SYMBOL_INDEX.S_PUB16)
                    //.Where(i => i.Type != DEBUG_SYMBOL_INDEX.S_PUB16_32)
                    //.Where(i => i.Type != DEBUG_SYMBOL_INDEX.S_ALIGN)
                    ) {
                    symbol.WriteXml(writer);
                    }
                writer.WriteEndElement();
            writer.WriteEndElement();
            }
        #endregion
        }
    }