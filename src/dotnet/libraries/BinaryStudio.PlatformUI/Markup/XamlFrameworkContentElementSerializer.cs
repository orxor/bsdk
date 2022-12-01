using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal class XamlFrameworkContentElementSerializer : XamlContentElementSerializer
        {
        public XamlFrameworkContentElementSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }

        protected override Boolean IsIgnoredProperty(DependencyProperty property) {
            //if (property.OwnerType == typeof(TextOptions)) { return true; }
            //if (property.OwnerType == typeof(Window)) { return true; }
            //if (property.OwnerType == typeof(Selector)) { return true; }
            //if (property.OwnerType == typeof(DefinitionBase)) { return true; }
            //if (property.OwnerType == typeof(Stylus)) { return true; }
            //if (property.OwnerType.FullName == "System.Windows.Documents.TextEditor") { return true; }
            return base.IsIgnoredProperty(property);
            }

        /// <summary>Writes specified attribute.</summary>
        /// <param name="source">Source containing property (attribute).</param>
        /// <param name="property">Dependency property descriptor.</param>
        protected override void WriteAttribute(DependencyObject source, DependencyProperty property) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (property == null) { throw new ArgumentNullException(nameof(property)); }
            var Parent = ((FrameworkContentElement)source).Parent;
            if (Parent != null) {
                if (property.GetMetadata(source) is FrameworkPropertyMetadata FrameworkPropertyMetadata) {
                    if (FrameworkPropertyMetadata.Inherits) {
                        if (Equals(source.GetValue(property),Parent.GetValue(property))) { return; }
                        }
                    }
                }
            if (ReferenceEquals(property,FrameworkContentElement.StyleProperty)) { return; }
            if (ReferenceEquals(property,ContentElement.AllowDropProperty)) {
                if (Parent != null) {
                    if (Equals(((FrameworkContentElement)source).AllowDrop,Parent.GetValue(ContentElement.AllowDropProperty))) { return; }
                    }
                }
            base.WriteAttribute(source,property);
            }

        /// <summary>Writes specified attribute.</summary>
        /// <param name="source">Source containing property (attribute).</param>
        /// <param name="descriptor">Property descriptor describing attribute.</param>
        protected override void WriteAttribute(DependencyObject source, PropertyDescriptor descriptor) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (descriptor == null) { throw new ArgumentNullException(nameof(descriptor)); }
            switch (descriptor.Name) {
                case nameof(FrameworkContentElement.Resources): { return; }
                }
            base.WriteAttribute(source, descriptor);
            }
        }
    }