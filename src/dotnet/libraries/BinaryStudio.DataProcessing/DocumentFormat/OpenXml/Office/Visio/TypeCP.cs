using System;
using System.Xml;

namespace DocumentFormat.OpenXml.Office.Visio
    {
    public class TypeCP : OpenXmlServiceElement
        {
        public override String LocalName { get{ return "cp"; }}
        public Int32 IX { get;set; }

        protected override void WriteAttributesTo(XmlWriter writer)
            {
            base.WriteAttributesTo(writer);
            writer.WriteAttributeString("IX",IX.ToString());
            }
        }
    }