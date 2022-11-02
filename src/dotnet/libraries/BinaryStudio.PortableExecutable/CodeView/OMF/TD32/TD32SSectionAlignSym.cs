using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

// ReSharper disable LocalVariableHidesMember

namespace BinaryStudio.PortableExecutable
    {
    internal class TD32SSectionAlignSym : OMFSSectionAlignSym
        {
        public IList<CodeViewSymbol> Symbols { get;private set; }
        public TD32SSectionAlignSym(OMFDirectory Directory)
            : base(Directory)
            {
            }

        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            Symbols = new List<CodeViewSymbol>();
            var Header = (CODEVIEW_SYMBOL_RECORD_HEADER*)(Source + sizeof(Int32));
            Size -= sizeof(CODEVIEW_SYMBOL_RECORD_HEADER);
            while (Size > 0) {
                var Offset = (Byte*)Header - BaseAddress;
                Symbols.Add(CodeViewSymbol.From(
                    null,(Int32)Offset,Header->Type,(Byte*)(Header),
                    Header->Length + sizeof(Int16),Types));
                Size -= Header->Length + sizeof(Int16);
                Header = (CODEVIEW_SYMBOL_RECORD_HEADER*)((Byte*)Header + Header->Length + sizeof(Int16));
                }
            return this;
            }

        private static readonly IDictionary<DEBUG_SYMBOL_INDEX,Type> Types = new Dictionary<DEBUG_SYMBOL_INDEX, Type>{
            { DEBUG_SYMBOL_INDEX.S_SSEARCH, typeof(S_SSEARCH_TD32)}
            };

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