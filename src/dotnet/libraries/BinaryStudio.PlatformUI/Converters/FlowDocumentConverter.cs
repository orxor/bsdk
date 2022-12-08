using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace BinaryStudio.PlatformUI.Converters
    {
    public class FlowDocumentConverter : IValueConverter
        {
        #region M:CreateReader(XmlNode):XmlReader
        private static XmlReader CreateReader(XmlNode source)
            {
            return new XmlNodeReader(source);
            }
        #endregion
        #region M:CreateReader(String):XmlReader
        private static XmlReader CreateReader(String source)
            {
            return XmlReader.Create(new StringReader(source));
            }
        #endregion
        #region M:CreateReader(StringBuilder):XmlReader
        private static XmlReader CreateReader(StringBuilder source)
            {
            return XmlReader.Create(new StringReader(source.ToString()));
            }
        #endregion
        #region M:ReadXml(IXmlSerializable):MemoryStream
        private static MemoryStream ReadXml(IXmlSerializable Source) {
            var Output = new MemoryStream();
            using (var writer = XmlWriter.Create(Output, new XmlWriterSettings
                {
                Indent = true,
                Encoding = Encoding.Default
                }))
                {
                Source.WriteXml(writer);
                }
            Output.Seek(0, SeekOrigin.Begin);
            return Output;
            }
        #endregion

        private class UrlResolver : XmlUrlResolver
            {
            /// <summary>Resolves the absolute URI from the base and relative URIs.</summary>
            /// <param name="baseUri">The base URI used to resolve the relative URI.</param>
            /// <param name="relativeUri">The URI to resolve. The URI can be absolute or relative. If absolute, this value effectively replaces the <paramref name="baseUri"/> value. If relative, it combines with the <paramref name="baseUri"/> to make an absolute URI.</param>
            /// <returns>The absolute URI, or <see langword="null"/> if the relative URI cannot be resolved.</returns>
            /// <exception cref="T:System.ArgumentNullException">
            /// <paramref name="baseUri"/> is <see langword="null"/> or <paramref name="relativeUri"/> is <see langword="null"/>.</exception>
            public override Uri ResolveUri(Uri baseUri, String relativeUri)
                {
                return base.ResolveUri(baseUri, relativeUri);
                }
            }

        /// <summary>Converts a value.</summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        Object IValueConverter.Convert(Object value, Type targetType, Object parameter, CultureInfo culture) {
            if (value is IXmlSerializable source) {
                var InputStream = ReadXml(source);
                if (parameter is XmlDataProvider provider) {
                    provider.Refresh();
                    if (provider.Document != null) {
                        using (var reader = CreateReader(provider.Document.InnerXml)) {
                            var xslt = new XslCompiledTransform();
                            xslt.Load(reader, new XsltSettings{
                                EnableScript = true
                                }, new UrlResolver());
                            var OutputBuilder = new StringBuilder();
                            using (var writer = XmlWriter.Create(OutputBuilder, new XmlWriterSettings
                                {
                                Indent = true,
                                Encoding = Encoding.Default
                                }))
                                {
                                xslt.Transform(new XPathDocument(XmlReader.Create(InputStream,new XmlReaderSettings{
                                    IgnoreComments = true
                                    })), writer);
                                }
                            var r = new FlowDocument();
                            BindingOperations.SetBinding(r,FlowDocument.PageWidthProperty,new Binding{
                                Mode = BindingMode.OneWay,
                                Path = new PropertyPath("ActualWidth"),
                                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor){
                                    AncestorType = typeof(RichTextBox)
                                    },
                                });
                            var OutputDocument = new XmlDocument();
                            OutputDocument.LoadXml(OutputBuilder.ToString());
                            if (String.IsNullOrWhiteSpace(OutputDocument.DocumentElement.NamespaceURI)) {
                                OutputDocument.DocumentElement.SetAttribute("xmlns","http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                                }
                            #if DEBUG
                            Debug.Print(OutputBuilder.ToString());
                            #endif
                            using (var OutputStream = new MemoryStream(Encoding.UTF8.GetBytes(OutputDocument.InnerXml))) {
                                var range = new TextRange(
                                    r.ContentStart, 
                                    r.ContentEnd
                                    );
                                range.Load(OutputStream,DataFormats.Xaml);
                                }
                            return r;
                            }
                        }
                    }
                }
            return null;
            }

        /// <summary>Converts a value. </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        Object IValueConverter.ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
            {
            throw new NotSupportedException();
            }
        }
    }