using System.Windows.Markup;
using System.Xml;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal class XamlFigureSerializer : XamlAnchoredBlockSerializer
        {
        public XamlFigureSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }
        }
    }