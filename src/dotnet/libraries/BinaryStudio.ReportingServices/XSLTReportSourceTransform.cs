using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Reporting
    {
    public class XSLTReportSourceTransform : ReportSourceTransform
        {
        private readonly XslCompiledTransform xslt = new XslCompiledTransform();
        public XSLTReportSourceTransform(String StylesheetFileName) {
            if (StylesheetFileName == null) { throw new ArgumentNullException(nameof(StylesheetFileName)); }
            if (String.IsNullOrWhiteSpace(StylesheetFileName)) { throw new ArgumentOutOfRangeException(nameof(StylesheetFileName)); }
            if (!File.Exists(StylesheetFileName)) { throw new FileNotFoundException(HResultException.FormatMessage(HRESULT.COR_E_FILENOTFOUND),StylesheetFileName); }
            xslt.Load(StylesheetFileName);
            }

        public override void Build(Stream InputStream,Stream OutputStream) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            if (OutputStream == null) { throw new ArgumentNullException(nameof(OutputStream)); }
            using (var writer = XmlWriter.Create(OutputStream, new XmlWriterSettings{
                Encoding = Encoding.UTF8,
                CloseOutput = false,
                Indent = true,
                IndentChars = "  ",
                NewLineOnAttributes = true
                }))
                {
                xslt.Transform(new XPathDocument(InputStream),writer);
                }
            }
        }
    }