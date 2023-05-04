using System.Windows.Markup;
using System.Xml;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal class XamlSpanSerializer : XamlInlineSerializer
        {
        public XamlSpanSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }
        }
    }