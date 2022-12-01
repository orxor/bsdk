using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Xml;
using BinaryStudio.PlatformUI.Documents;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal class XamlTableColumnSerializer : XamlFrameworkContentElementSerializer
        {
        public XamlTableColumnSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }

        /// <summary>Writes attributes.</summary>
        /// <param name="source">Source containing attributes.</param>
        /// <param name="descriptors">Containing properties.</param>
        protected override void WriteAttributes(DependencyObject source, IEnumerable<PropertyDescriptor> descriptors)
            {
            base.WriteAttributes(source, descriptors);
            }

        /// <summary>Writes specified attribute.</summary>
        /// <param name="source">Source containing property (attribute).</param>
        /// <param name="property">Dependency property descriptor.</param>
        protected override void WriteAttribute(DependencyObject source, DependencyProperty property) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (property == null) { throw new ArgumentNullException(nameof(property)); }
            if (ReferenceEquals(property,DocumentProperties.SharedGroupObjectProperty)) { return; }
            base.WriteAttribute(source, property);
            }

        /// <summary>Writes specified object.</summary>
        /// <param name="source">Object to write.</param>
        protected override void Write(DependencyObject source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var type = source.GetType();
            var descriptors = TypeDescriptor.GetProperties(source,true);
            Writer.WriteStartElement(type.Name,GetXmlNamespace(type));
            WriteAttributes(source,descriptors.OfType<PropertyDescriptor>().Where(i => !i.IsReadOnly));
            var SharedGroupObject = source.GetValue(DocumentProperties.SharedGroupObjectProperty);
            if (SharedGroupObject != null) {
                Writer.WriteStartElement("DocumentProperties.SharedGroupObject",GetXmlNamespace(typeof(DocumentProperties)));
                using (var writer = GetSerializer(SharedGroupObject.GetType())) {
                    writer.Write(SharedGroupObject);
                    }
                Writer.WriteEndElement();
                }
            Writer.WriteEndElement();
            }
        }
    }