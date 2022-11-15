using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.GlobalTypes)]
    [UsedImplicitly]
    internal class OMFSSectionGlobalTypes : OMFSSection
        {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct SymbolTypeInfo
            {
            public readonly UInt16 Size;
            public readonly LEAF_ENUM  Leaf;
            }

        public OMFSSectionGlobalTypes(OMFDirectory Directory)
            : base(Directory)
            {
            }

        private class TypeInfo
            {
            public Int64 FileOffset;
            public Int32 Offset;
            public Int32 TypeIndex;
            public LEAF_ENUM Leaf;
            public Int32 Size;
            }

        private IList<TypeInfo> Types = new List<TypeInfo>();
        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.GlobalTypes; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            Source += sizeof(Int32);
            var TypeCount = (Int32*)Source;
            var Offsets = (Int32*)(TypeCount + 1);
            Source = (Byte*)(Offsets + *TypeCount);
            for (var i = 0; i < *TypeCount;i++) {
                var SymbolTypeInfo = (SymbolTypeInfo*)((Byte*)Source + Offsets[i]);
                Types.Add(new TypeInfo{
                    FileOffset = (Byte*)SymbolTypeInfo - BaseAddress,
                    Offset = Offsets[i],
                    Size = SymbolTypeInfo->Size,
                    TypeIndex = i + 0x1000,
                    Leaf = SymbolTypeInfo->Leaf
                    });
                #if TD32DEBUG
                Debug.Print("FileOffset:{2:x8} Offset:{0:x8} Size:{1:x4} Type:{4:x4} Leaf:{3}",
                    Offsets[i],SymbolTypeInfo->Size,
                    (Byte*)SymbolTypeInfo - BaseAddress,
                    SymbolTypeInfo->Leaf,
                    i + 0x1000);
                #endif
                }
            return this;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            if (Writer == null) { throw new ArgumentNullException(nameof(Writer)); }
            foreach (var o in Types) {
                Writer.WriteLine("{0} FileOffset:{1:x8} Offset:{2:x8} Type:{3:x4} Size:{4:x4} Leaf:{5}",LinePrefix,
                    o.FileOffset,o.Offset,o.TypeIndex,o.Size,o.Leaf);
                }
            }
        }
    }