using System.Windows.Markup;
using System.Xml;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal abstract class XamlAnchoredBlockSerializer : XamlInlineSerializer
        {
        protected XamlAnchoredBlockSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }
        }
    }