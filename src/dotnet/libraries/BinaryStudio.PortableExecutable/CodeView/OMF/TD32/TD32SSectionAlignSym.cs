using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.TD32;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

// ReSharper disable LocalVariableHidesMember

namespace BinaryStudio.PortableExecutable
    {
    internal class TD32SSectionAlignSym : OMFSSectionAlignSym
        {
        public IList<TD32Symbol> Symbols { get;private set; }
        public TD32SSectionAlignSym(OMFDirectory Directory)
            : base(Directory)
            {
            }

        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            Symbols = new List<TD32Symbol>();
            var Header = (TD32SymbolRecordHeader*)(Source + sizeof(Int32));
            Size -= sizeof(TD32SymbolRecordHeader);
            ICodeViewBlockStart Package = null;
            while (Size > 0) {
                var Offset = (Byte*)Header - BaseAddress;
                TD32Symbol Target;
                Symbols.Add(Target = TD32Symbol.From(
                    null,(Int32)Offset,Header->Type,(Byte*)(Header),
                    Header->Length + sizeof(Int16)));
                Target.NameTable = (ICodeViewNameTable)Directory.GetService(typeof(ICodeViewNameTable));
                Target.CPU = CPU;
                if (Target is ICodeViewBlockElement PackageableElement) { PackageableElement.BlockStart = Package; }
                if (Target is ICodeViewBlockEnd) { Package = null; }
                if (Target is ICodeViewBlockStart LocalPackage) { Package = LocalPackage; }
                Size -= Header->Length + sizeof(Int16);
                Header = (TD32SymbolRecordHeader*)((Byte*)Header + Header->Length + sizeof(Int16));
                }
            return this;
            }

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