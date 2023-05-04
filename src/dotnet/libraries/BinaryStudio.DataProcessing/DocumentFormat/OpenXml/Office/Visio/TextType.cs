using System;
using System.Xml;

namespace DocumentFormat.OpenXml.Office.Visio
    {
    public class TextType : OpenXmlServiceCompositeElement
        {
        public override String LocalName { get { return "Text"; }}
        public TypeCP CP { get;set; }
        public TypePP PP { get;set; }
        public String Text {get;set; }

        #region ctor
        public TextType()
            {
            }
        #endregion
        #region ctor{String}
        public TextType(String value)
            {
            Text = value;
            }
        #endregion

        protected override void WriteContentTo(XmlWriter writer)
            {
            base.WriteContentTo(writer);
            CP?.WriteTo(writer);
            PP?.WriteTo(writer);
            writer.WriteString(Text);
            }
        }
    }