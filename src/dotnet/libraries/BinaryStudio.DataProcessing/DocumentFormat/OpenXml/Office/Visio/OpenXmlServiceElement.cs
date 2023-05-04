using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DocumentFormat.OpenXml.Office.Visio
    {
    public class OpenXmlServiceElement : IServiceProvider
        {
        protected static CultureInfo US = CultureInfo.GetCultureInfo("en-US");
        public String NamespaceUri { get { return "http://schemas.microsoft.com/office/visio/2012/main"; }}
        public virtual String LocalName { get{ return GetType().Name; }}

        /**
         * <summary>Gets the service object of the specified type.</summary>
         * <param name="service">An object that specifies the type of service object to get.</param>
         * <returns>A service object of type <paramref name="service"/>.
         * -or-
         * <see langword="null"/> if there is no service object of type <paramref name="service"/>.</returns>
         */
        public virtual Object GetService(Type service) {
            if (service == null) { return null; }
            if (service == GetType()) { return this; }
            if (service.IsAssignableFrom(GetType())) { return this; }
            if (service == typeof(XElement)) {
                var o = new StringBuilder();
                using (var writer = XmlWriter.Create(o, new XmlWriterSettings{
                    IndentChars = " ",
                    Indent = true,
                    OmitXmlDeclaration = true
                    }))
                    {
                    WriteTo(writer);
                    }
                var r = o.ToString();
                return XElement.Parse(r);
                }
            return null;
            }

        protected virtual void WriteAttributesTo(XmlWriter writer)
            {
            }

        protected virtual void WriteContentTo(XmlWriter writer)
            {
            }

        public virtual void WriteTo(XmlWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            writer.WriteStartElement(LocalName,NamespaceUri);
            WriteAttributesTo(writer);
            WriteContentTo(writer);
            writer.WriteEndElement();
            }

        public static explicit operator XElement(OpenXmlServiceElement source)
            {
            return (XElement)((source != null)
                ? source.GetService(typeof(XElement))
                : null);
            }
        }
    }