using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;
using BinaryStudio.DiagnosticServices;
using log4net;

namespace BinaryStudio.PlatformUI.Converters
    {
    public class FlowDocumentConverter : IValueConverter
        {
        private static readonly ILog logger = LogManager.GetLogger(nameof(FlowDocumentConverter));
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

        #region M:ResolveDocument(Dispatcher,IXmlSerializable):Task<XDocument>
        private static Task<XDocument> ResolveDocument(Dispatcher Dispatcher, IXmlSerializable source) {
            return Task.Factory.StartNew(()=>{
                using (var output = new MemoryStream()) {
                    using (var writer = XmlWriter.Create(output, new XmlWriterSettings
                        {
                        Indent = true,
                        Encoding = Encoding.Default
                        }))
                        {
                        source.WriteXml(writer);
                        }
                    output.Seek(0, SeekOrigin.Begin);
                    Debug.Print(Encoding.Default.GetString(output.ToArray()));
                    return XDocument.Load(output);
                    }
                });
            }
        #endregion
        #region M:ResolveDocument(Dispatcher,XmlDataProvider):Task<XDocument>
        private static Task<XDocument> ResolveDocument(Dispatcher Dispatcher, XmlDataProvider source) {
            return Task.Factory.StartNew(()=>{
                if (source != null) {
                    if (source.Document == null) {
                        if (source.IsAsynchronous && source.IsInitialLoadEnabled) {
                            throw new InvalidOperationException("When using 'XmlDataProvider' with 'IsAsynchronous=true', requires asynchronous binding.");
                            }
                        }
                    if (source.Document != null) {
                        return XDocument.Parse(source.Document.InnerXml);
                        }
                    }
                return null;
                });
            }
        #endregion

        /// <summary>Converts a value.</summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        Object IValueConverter.Convert(Object value, Type targetType, Object parameter, CultureInfo culture) {
            if (value is IXmlSerializable Source) {
                if (parameter is XmlDataProvider Provider) {
                    var r = new FlowDocument();
                    //BindingOperations.SetBinding(r,FlowDocument.PageWidthProperty,new Binding{
                    //    Mode = BindingMode.OneWay,
                    //    Path = new PropertyPath("ActualWidth"),
                    //    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor){
                    //        AncestorType = typeof(RichTextBox)
                    //        },
                    //    });
                    var dispatcher = Dispatcher.CurrentDispatcher;
                    var OutputBuilder = new StringBuilder();
                    try
                        {
                        Task.Factory.ContinueWhenAll<XDocument>(new []{
                            ResolveDocument(dispatcher,Source),
                            ResolveDocument(dispatcher,Provider)
                            },(tasks)=>
                            {
                            using (XmlReader SourceXmlReader = tasks[0].Result?.CreateReader(),
                                             SourceXslReader = tasks[1].Result?.CreateReader())
                                {
                                var xslt = new XslCompiledTransform();
                                xslt.Load(SourceXslReader, new XsltSettings{
                                    EnableScript = true
                                    }, new UrlResolver());
                                using (var writer = XmlWriter.Create(OutputBuilder, new XmlWriterSettings
                                    {
                                    Indent = false,
                                    Encoding = Encoding.Default
                                    }))
                                    {
                                    xslt.Transform(SourceXmlReader,writer);
                                    return;
                                    }
                                }
                            }).Wait();
                        }
                    catch (Exception e)
                        {
                        Debug.Print(Exceptions.ToString(e));
                        OutputBuilder = new StringBuilder();
                        OutputBuilder.Append(@"<Section xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">");
                        OutputBuilder.Append(@"  <Paragraph Margin=""0,5,0,5"" Foreground=""{DynamicResource AccentRBrushKey}"">");
                        foreach (var i in Exceptions.ToString(e).Split(new []{'\r','\n'},StringSplitOptions.RemoveEmptyEntries)) {
                            OutputBuilder.AppendFormat(@"<Run Text=""{0}""></Run><LineBreak/>",
                                i.Replace("<","&lt;").
                                  Replace(">","&gt;"));
                            }
                        OutputBuilder.Append(@"  </Paragraph>");
                        OutputBuilder.Append(@"</Section>");
                        }
                    #if DEBUG
                    Debug.Print(OutputBuilder.ToString());
                    #endif
                    using (var OutputStream = new MemoryStream(Encoding.UTF8.GetBytes(OutputBuilder.ToString()))) {
                        var range = new TextRange(
                            r.ContentStart, 
                            r.ContentEnd
                            );
                        logger.Debug("BeforeLoad");
                        range.Load(OutputStream,DataFormats.Xaml);
                        logger.Debug("AfterLoad");
                        }
                    return r;
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