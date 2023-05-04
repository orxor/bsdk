using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal class XamlTableSerializer : XamlBlockSerializer
        {
        public XamlTableSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }

        /// <summary>Writes specified object.</summary>
        /// <param name="source">Object to write.</param>
        protected override void Write(DependencyObject source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var table = (Table)source;
            var type = source.GetType();
            var descriptors = TypeDescriptor.GetProperties(source,true);
            Writer.WriteStartElement(type.Name,GetXmlNamespace(type));
            WriteAttributes(source,descriptors.OfType<PropertyDescriptor>().Where(i => !i.IsReadOnly));
            if (table.Columns.Count > 0) {
                Writer.WriteStartElement("Table.Columns");
                foreach (var value in table.Columns) {
                    using (var writer = GetSerializer(value.GetType())) {
                        writer.Write(value);
                        }
                    }
                Writer.WriteEndElement();
                }
            if (table.RowGroups.Count > 0) {
                foreach (var value in table.RowGroups) {
                    using (var writer = GetSerializer(value.GetType())) {
                        writer.Write(value);
                        }
                    }
                }
            Writer.WriteEndElement();
            }
        }
    }