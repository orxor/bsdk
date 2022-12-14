using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;
using BinaryStudio.DiagnosticServices;
using log4net;

namespace BinaryStudio.PlatformUI.Converters
    {
    public class HTConverter : FDConverter
        {
        private static readonly ILog logger = LogManager.GetLogger(nameof(HTConverter));
        /// <summary>Converts a value.</summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        public override Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture) {
            if (value is IXmlSerializable Source) {
                if (parameter is XmlDataProvider Provider) {
                    var OutputBuilder = new StringBuilder();
                    try
                        {
                        Task.Factory.ContinueWhenAll<XDocument>(new []{
                            ResolveDocument(Source),
                            ResolveDocument(Provider)
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
                    return OutputBuilder.ToString();
                    }
                }
            return null;
            }
        }
    }