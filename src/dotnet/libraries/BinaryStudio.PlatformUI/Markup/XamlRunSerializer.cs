using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace BinaryStudio.PlatformUI.Markup
    {
    internal class XamlRunSerializer : XamlInlineSerializer
        {
        public XamlRunSerializer(XmlWriter Writer, XamlWriterMode WriterMode)
            : base(Writer, WriterMode)
            {
            }
        }
    }