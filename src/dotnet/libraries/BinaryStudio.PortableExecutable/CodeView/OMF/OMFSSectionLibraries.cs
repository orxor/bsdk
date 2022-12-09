using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using JetBrains.Annotations;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.Libraries)]
    [UsedImplicitly]
    internal class OMFSSectionLibraries : OMFSSection
        {
        public OMFSSectionLibraries(OMFDirectory Directory)
            : base(Directory)
            {
            Libraries = EmptyList<String>.Value;
            }

        public IList<String> Libraries { get;private set; }
        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.Libraries; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            Libraries = new List<String>();
            Source += sizeof(Int16);
            Size -= sizeof(Int16);
            while (Size > 0) {
                var NameSize = *Source++;
                Libraries.Add(ToString(Encoding.ASCII, Source,NameSize));
                Source += NameSize;
                Size -= NameSize + 1;
                }
            return this;
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            if (Writer == null) { throw new ArgumentNullException(nameof(Writer)); }
            var i = 1;
            foreach (var o in Libraries) {
                Writer.WriteLine("{0}{1:x4} Library:{2}",LinePrefix,i,o);
                i++;
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
                writer.WriteStartElement(nameof(Libraries));
                foreach (var library in Libraries) {
                    writer.WriteStartElement("LibraryInfo");
                        writer.WriteAttributeString("Name",library);
                    writer.WriteEndElement();
                    }
                writer.WriteEndElement();
            writer.WriteEndElement();
            }
        #endregion
        }
    }