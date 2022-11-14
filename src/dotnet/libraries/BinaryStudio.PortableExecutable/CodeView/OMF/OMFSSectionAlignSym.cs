using System;
using System.Collections.Generic;
using System.IO;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.AlignSym)]
    internal class OMFSSectionAlignSym : OMFSSection
        {
        public IList<CodeViewSymbol> Symbols { get;private set; }
        public OMFSSectionAlignSym(OMFDirectory Directory)
            : base(Directory)
            {
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

        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.AlignSym; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            Symbols = new List<CodeViewSymbol>();
            var Header = (CODEVIEW_SYMBOL_RECORD_HEADER*)(Source + sizeof(Int32));
            Size -= sizeof(CODEVIEW_SYMBOL_RECORD_HEADER);
            var PackageStack = new Stack<ICodeViewBlockStart>();
            while (Size > 0) {
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
                Size -= Header->Length + sizeof(Int16);
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
        }
    }