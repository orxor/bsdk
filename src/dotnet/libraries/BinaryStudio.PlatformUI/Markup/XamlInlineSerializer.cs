using System.Windows.Markup;
using System.Xml;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal class XamlInlineSerializer : XamlTextElementSerializer
        {
        public XamlInlineSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }
        }
    }