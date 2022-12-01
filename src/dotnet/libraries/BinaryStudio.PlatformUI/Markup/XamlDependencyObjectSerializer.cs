using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using BinaryStudio.PlatformUI.Controls;
using BinaryStudio.PlatformUI.Documents;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal class XamlDependencyObjectSerializer : XamlSerializer
        {
        //private class FromNameKey
        //    {
        //    public readonly Int32 HashCode;
        //    public FromNameKey(String name,Type type)
        //        {
        //        HashCode = name.GetHashCode() ^ type.GetHashCode();
        //        }

        //    public override Int32 GetHashCode()
        //        {
        //        return HashCode;
        //        }
        //    }

        protected virtual Boolean IsIgnoredProperty(DependencyProperty property) {
            if (ReferenceEquals(property,XmlAttributeProperties.XmlnsDictionaryProperty)) { return true; }
            var ns = GetXmlNamespace(property.OwnerType);
            switch (ns) {
                case "clr-namespace:MS.Internal.Documents;assembly=PresentationFramework": { return true; }
                }
            return false;
            }

        public class PropertyName
            {
            public String XmlNamespace { get; }
            public String Name { get; }
            public PropertyName(String name, String ns)
                {
                XmlNamespace = ns;
                Name = name;
                }
            }

        static XamlDependencyObjectSerializer()
            {
            var assembly = typeof(TextElement).Assembly;
            var xmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            var PropertyFromNameFieldInfo = typeof(DependencyProperty).GetField("PropertyFromName",BindingFlags.Static|BindingFlags.NonPublic);
            PropertyFromName = (PropertyFromNameFieldInfo != null)
                ? (Hashtable)PropertyFromNameFieldInfo.GetValue(null)
                : new Hashtable();
            FromNameKeyType = typeof(DependencyProperty).GetNestedType("FromNameKey",BindingFlags.NonPublic);
            FromNameKeyTypeOwnerField = FromNameKeyType.GetField("_ownerType",BindingFlags.NonPublic|BindingFlags.Instance);
            AssemblyQualifiedNamespaces[$"System.Windows.Controls, {assembly.FullName}"] = xmlns;
            AssemblyQualifiedNamespaces[$"System.Windows.Documents, {assembly.FullName}"] = xmlns;
            AssemblyQualifiedNamespaces[$"System.Windows.Shapes, {assembly.FullName}"] = xmlns;
            AssemblyQualifiedNamespaces[$"System.Windows.Shell, {assembly.FullName}"] = xmlns;
            AssemblyQualifiedNamespaces[$"System.Windows.Navigation, {assembly.FullName}"] = xmlns;
            AssemblyQualifiedNamespaces[$"System.Windows.Data, {assembly.FullName}"] = xmlns;
            AssemblyQualifiedNamespaces[$"System.Windows, {assembly.FullName}"] = xmlns;
            AssemblyQualifiedNamespaces[$"System.Windows.Controls.Primitives, {assembly.FullName}"] = xmlns;
            AssemblyQualifiedNamespaces[$"System.Windows.Media.Animation, {assembly.FullName}"] = xmlns;
            AssemblyQualifiedNamespaces[$"System.Windows.Input, {assembly.FullName}"] = xmlns;
            AssemblyQualifiedNamespaces[$"System.Windows.Media, {assembly.FullName}"] = xmlns;
            }

        public XamlDependencyObjectSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }

        protected virtual PropertyName GetPropertyName(Type type, DependencyProperty property) {
            if (property == null) { throw new ArgumentNullException(nameof(property)); }
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            var name = property.Name;
            String nsP = null;
            String nsS = GetXmlNamespace(type);
            if (!type.IsSubclassOf(property.OwnerType)) {
                var types = PropertyFromName.
                    OfType<DictionaryEntry>().
                    Where(i => ReferenceEquals(i.Value,property)).
                    Select(i=> (Type)FromNameKeyTypeOwnerField.GetValue(i.Key)).
                    ToArray();
                if (!types.Any(i => (type == i) || type.IsSubclassOf(i))) {
                    name = $"{property.OwnerType.Name}.{name}";
                    nsP = GetXmlNamespace(property.OwnerType);
                    if (String.Equals(nsP,nsS,StringComparison.OrdinalIgnoreCase)) {
                        nsP = null;
                        }
                    }
                }
            return new PropertyName(name,nsP);
            }

        protected String GetXmlNamespace(Type type) {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            var assembly = type.Assembly;
            var source = $"{type.Namespace}, {assembly.FullName}";
            lock(AssemblyQualifiedNamespaces) {
                if (!AssemblyQualifiedNamespaces.TryGetValue(source, out var target)) {
                    foreach (var attribute in assembly.GetCustomAttributes(typeof(XmlnsDefinitionAttribute),false).OfType<XmlnsDefinitionAttribute>()) {
                        var key = $"{attribute.ClrNamespace}, {assembly.FullName}";
                        if (!AssemblyQualifiedNamespaces.ContainsKey(key)) {
                            AssemblyQualifiedNamespaces[key] = attribute.XmlNamespace;
                            }
                        }
                    if (!AssemblyQualifiedNamespaces.TryGetValue(source, out target)) {
                        AssemblyQualifiedNamespaces[source] = target = $"clr-namespace:{type.Namespace};assembly={assembly.GetName().Name}";
                        }
                    }
                return target;
                }
            }

        /// <summary>Writes specified attribute.</summary>
        /// <param name="source">Source containing property (attribute).</param>
        /// <param name="property">Dependency property descriptor.</param>
        protected virtual void WriteAttribute(DependencyObject source, DependencyProperty property) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (property == null) { throw new ArgumentNullException(nameof(property)); }
            if (IsIgnoredProperty(property)) { return; }
            if (!source.IsDefaultValue(property, out var value)) {
                if (value is ICollection Collection) {
                    if (Collection.Count == 0) { return; }
                    }
                var type = source.GetType();
                var name = GetPropertyName(type,property);
                //Debug.Print($"WriteAttribute:{{{property}}}");
                Writer.WriteAttributeString(name.Name,name.XmlNamespace,(value != null)
                    ? value.ToString()
                    : "{x:Null}");
                }
            }

        /// <summary>Writes specified attribute.</summary>
        /// <param name="source">Source containing property (attribute).</param>
        /// <param name="descriptor">Property descriptor describing attribute.</param>
        protected virtual void WriteAttribute(DependencyObject source,PropertyDescriptor descriptor) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (descriptor == null) { throw new ArgumentNullException(nameof(descriptor)); }
            var type  = descriptor.GetType();
            var value = descriptor.GetValue(source);
            var DefaultValue = (DefaultValueAttribute)descriptor.Attributes[typeof(DefaultValueAttribute)];
            if (DefaultValue != null) {
                if (Equals(DefaultValue.Value,value)) { return; }
                }
            Writer.WriteAttributeString(descriptor.Name,(value != null)
                ? value.ToString()
                : "{x:Null}");
            }

        /// <summary>Writes attributes.</summary>
        /// <param name="source">Source containing attributes.</param>
        /// <param name="descriptors">Containing properties.</param>
        protected virtual void WriteAttributes(DependencyObject source,IEnumerable<PropertyDescriptor> descriptors) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var properties = new HashSet<DependencyProperty>();
            foreach (var descriptor in descriptors) {
                var type  = descriptor.GetType();
                var DependencyPropertyFieldInfo = type.GetProperty("DependencyProperty",BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic);
                if (DependencyPropertyFieldInfo != null) {
                    var property = (DependencyProperty)DependencyPropertyFieldInfo.GetValue(descriptor,null);
                    if (property != null) {
                        properties.Add(property);
                        WriteAttribute(source,property);
                        continue;
                        }
                    }
                WriteAttribute(source,descriptor);
                }

            foreach (var descriptor in PropertyFromName.OfType<DictionaryEntry>().Select(i=> (DependencyProperty)i.Value)) {
                if (properties.Add(descriptor)) {
                    WriteAttribute(source, descriptor);
                    }
                }

            //var e = source.GetLocalValueEnumerator();
            //while (e.MoveNext()) {
            //    if (!e.Current.Property.ReadOnly) {
            //        if (properties.Add(e.Current.Property)) {
            //            WriteAttribute(source,e.Current.Property);
            //            }
            //        }
            //    }

            //foreach (var descriptor in new []{
            //    DocumentProperties.IsAutoSizeProperty,
            //    DocumentProperties.IsSharedSizeScopeProperty,
            //    DocumentProperties.SharedGroupObjectProperty,
            //    DocumentProperties.SharedSizeGroupProperty,
            //    DocumentProperties.WidthProperty
            //    })
            //    {
            //    if (properties.Add(descriptor)) {
            //        WriteAttribute(source,descriptor);
            //        }
            //    }
            }

        /// <summary>Writes specified object.</summary>
        /// <param name="source">Object to write.</param>
        protected override void Write(DependencyObject source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var type = source.GetType();
            var ContentProperty = type.GetCustomAttributes(true).OfType<ContentPropertyAttribute>().FirstOrDefault()?.Name;
            var descriptors = TypeDescriptor.GetProperties(source,true);
            Writer.WriteStartElement(type.Name,GetXmlNamespace(type));
            if (xmlns != null) {
                foreach (var ns in xmlns) {
                    Writer.WriteAttributeString("xmlns",ns.Value,null,ns.Key);
                    }
                }
            if (!String.IsNullOrWhiteSpace(ContentProperty)) {
                var ContentPropertyDescriptor = descriptors.Find(ContentProperty,false);
                if (ContentPropertyDescriptor != null) {
                    WriteAttributes(source,descriptors.OfType<PropertyDescriptor>().Where(i => !i.Equals(ContentPropertyDescriptor) && !i.IsReadOnly));
                    var ContentPropertyDescriptorValue = ContentPropertyDescriptor.GetValue(source);
                    if (ContentPropertyDescriptorValue is IList values) {
                        foreach (var value in values) {
                            if (value != null) {
                                using (var writer = GetSerializer(value.GetType())) {
                                    writer.Write(value);
                                    }
                                }
                            }
                        }
                    else if (ContentPropertyDescriptorValue != null)
                        {
                        ((IXamlSerializer)(this)).Write(ContentPropertyDescriptorValue);
                        }
                    }
                else
                    {
                    WriteAttributes(source,descriptors.OfType<PropertyDescriptor>().Where(i => !i.IsReadOnly));
                    }
                }
            else
                {
                WriteAttributes(source,descriptors.OfType<PropertyDescriptor>().Where(i => !i.IsReadOnly));
                }
            Writer.WriteEndElement();
            }

        private static readonly IDictionary<String,String> AssemblyQualifiedNamespaces = new ConcurrentDictionary<String,String>();
        private static readonly Hashtable PropertyFromName;
        private static readonly Type FromNameKeyType;
        private static readonly FieldInfo FromNameKeyTypeOwnerField;
        }
    }