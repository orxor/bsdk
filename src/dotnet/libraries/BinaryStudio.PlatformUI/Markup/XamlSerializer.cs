using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using BinaryStudio.PlatformComponents;

namespace BinaryStudio.PlatformUI.Markup
    {
    public abstract class XamlSerializer : DisposableObject, IXamlSerializer
        {
        protected IDictionary<String,String> xmlns;
        protected XmlWriter Writer { get;private set; }
        protected readonly XamlWriterMode WriterMode;
        protected XamlSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            {
            if (Writer == null) { throw new ArgumentNullException(nameof(Writer)); }
            this.WriterMode = WriterMode;
            this.Writer = Writer;
            }

        #region M:GetXmlnsPrefixes(Assembly):IDictionary<String,String>
        private static IDictionary<String,String> GetXmlnsPrefixes(Assembly assembly) {
            if (assembly == null) { throw new ArgumentNullException(nameof(assembly)); }
            var r = new Dictionary<String,String>();
            foreach (var attribute in assembly.GetCustomAttributes(typeof(XmlnsPrefixAttribute),false).OfType<XmlnsPrefixAttribute>()) {
                r[attribute.XmlNamespace] = attribute.Prefix;
                }
            return r;
            }
        #endregion

        public static void WriteTo(DependencyObject Source,XmlWriter Writer, XamlWriterMode WriterMode) {
            if (Writer == null) { throw new ArgumentNullException(nameof(Writer)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            using (var writer = (XamlSerializer)GetSerializer(Source.GetType(),Writer,WriterMode)) {
                writer.xmlns = GetXmlnsPrefixes(typeof(XamlSerializer).Assembly);
                writer.Write(Source);
                }
            }

        protected virtual void Write(DependencyObject Source) {
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            }

        void IXamlSerializer.Write(Object Value) {
            if (Value is DependencyObject Source) {
                Write(Source);
                }
            else
                {
                if (Value is String String) { Writer.WriteString(String); }
                else
                    {
                    throw new NotImplementedException();
                    }
                }
            }

        void IXamlSerializer.Write(DependencyObject Source) {
            Write(Source);
            }

        #region M:GetSerializer(Type):IXamlSerializer
        protected virtual IXamlSerializer GetSerializer(Type Type)
            {
            return GetSerializer(Type,Writer,WriterMode);
            }
        #endregion
        #region M:GetSerializer(Type,XmlWriter,XamlWriterMode):IXamlSerializer
        private static IXamlSerializer GetSerializer(Type Type,XmlWriter Writer, XamlWriterMode WriterMode) {
            if (!Types.TryGetValue(Type,out var type)) {
                foreach (var i in Types) {
                    if (Type.IsSubclassOf(i.Key)) {
                        type = i.Value;
                        break;
                        }
                    }
                type = type ?? typeof(XamlDependencyObjectSerializer);
                }
            return (IXamlSerializer)Activator.CreateInstance(type,Writer,WriterMode);
            }
        #endregion

        protected override void DisposeManagedResources()
            {
            Writer = null;
            }

        private static readonly IDictionary<Type,Type> Types = new Dictionary<Type,Type>{
            {typeof(TableColumn),typeof(XamlTableColumnSerializer) },
            {typeof(TableCell),typeof(XamlTableCellSerializer) },
            {typeof(Table),typeof(XamlTableSerializer) },
            {typeof(TableRow),typeof(XamlTableRowSerializer) },
            {typeof(TableRowGroup),typeof(XamlTableRowGroupSerializer) },
            {typeof(Section),typeof(XamlSectionSerializer) },
            {typeof(Paragraph),typeof(XamlParagraphSerializer) },
            {typeof(List),typeof(XamlListSerializer) },
            {typeof(ListItem),typeof(XamlListItemSerializer) },
            {typeof(Run),typeof(XamlRunSerializer) },
            {typeof(Span),typeof(XamlSpanSerializer) },
            {typeof(BlockUIContainer),typeof(XamlBlockUIContainerSerializer) },
            {typeof(Figure),typeof(XamlFigureSerializer) },
            {typeof(Floater),typeof(XamlFloaterSerializer) },
            {typeof(LineBreak),typeof(XamlLineBreakSerializer) },
            {typeof(FlowDocument),typeof(XamlFlowDocumentSerializer) },
            };
        }
    }