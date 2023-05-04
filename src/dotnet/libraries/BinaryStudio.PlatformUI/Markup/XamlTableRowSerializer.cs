using System.Windows.Markup;
using System.Xml;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal class XamlTableRowSerializer : XamlTextElementSerializer
        {
        public XamlTableRowSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }
        }
    }