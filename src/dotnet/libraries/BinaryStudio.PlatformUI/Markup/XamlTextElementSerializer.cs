using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal abstract class XamlTextElementSerializer : XamlFrameworkContentElementSerializer
        {
        protected XamlTextElementSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }

        /// <summary>Writes specified attribute.</summary>
        /// <param name="source">Source containing property (attribute).</param>
        /// <param name="property">Dependency property descriptor.</param>
        protected override void WriteAttribute(DependencyObject source, DependencyProperty property) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (property == null) { throw new ArgumentNullException(nameof(property)); }
            if (ReferenceEquals(property,TextElement.ForegroundProperty)) {
                if (((TextElement)source).Parent is TextElement parent) {
                    if (Equals(((TextElement)source).Foreground,parent.Foreground)) { return; }
                    }
                }
            base.WriteAttribute(source,property);
            }
        }
    }