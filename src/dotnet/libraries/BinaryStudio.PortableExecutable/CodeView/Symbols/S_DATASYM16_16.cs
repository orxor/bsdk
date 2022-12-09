using System;
using System.IO;
using System.Xml;
using BinaryStudio.PortableExecutable.Win32;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    internal abstract class S_DATASYM16_16 : CodeViewSymbol
        {
        public Int32 TypeIndex { get; }
        public Int16 SegmentIndex { get; }
        public Int32 SymbolOffset { get; }
        public String Name { get; }

        protected unsafe S_DATASYM16_16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (DATASYM16_16*)Content;
            TypeIndex = r->TypeIndex;
            SegmentIndex = r->Segment;
            SymbolOffset = r->Offset;
            Name = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} TypeIndex:{3:x4} Segment:{4:x4}:{5:x4} {6}",
                LinePrefix,Offset,Type,
                TypeIndex,SegmentIndex,SymbolOffset,Name);
            }

        #region M:WriteXml(XmlWriter)
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public override void WriteXml(XmlWriter writer) {
            writer.WriteStartElement("SymbolInfo");
                writer.WriteAttributeString("Type",Type.ToString());
                writer.WriteAttributeString("Offset",Offset.ToString());
                writer.WriteAttributeString("TypeIndex",TypeIndex.ToString());
                writer.WriteAttributeString("SegmentIndex",SegmentIndex.ToString());
                writer.WriteAttributeString("SymbolOffset",SymbolOffset.ToString());
                writer.WriteAttributeString("Name",Name);
            writer.WriteEndElement();
            }
        #endregion
        }

    internal abstract class S_DATASYM16_32 : CodeViewSymbol
        {
        public Int16 TypeIndex { get; }
        public Int16 SegmentIndex { get; }
        public Int32 SymbolOffset { get; }
        public String Name { get; }

        protected unsafe S_DATASYM16_32(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            var r = (DATASYM16_32*)Content;
            TypeIndex = r->TypeIndex;
            SegmentIndex = r->Segment;
            SymbolOffset = r->Offset;
            Name = ToString(Encoding, (Byte*)(r + 1), IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} TypeIndex:{3:x4} Segment:{4:x4}:{5:x8} {6}",
                LinePrefix,Offset,Type,
                TypeIndex,SegmentIndex,SymbolOffset,Name);
            }
        }
    }