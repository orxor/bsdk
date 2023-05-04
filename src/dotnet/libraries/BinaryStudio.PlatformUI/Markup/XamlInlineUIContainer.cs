using System.Windows.Markup;
using System.Xml;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal class XamlInlineUIContainer : XamlInlineSerializer
        {
        public XamlInlineUIContainer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }
        }
    }