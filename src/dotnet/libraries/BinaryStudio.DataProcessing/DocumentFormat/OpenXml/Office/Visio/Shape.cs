using System;
using System.Xml;
using BinaryStudio.Serialization;

namespace DocumentFormat.OpenXml.Office.Visio
    {
    public class Shape : ShapeSheet
        {
        public override String LocalName { get { return "Shape"; }}
        public Int32 Id { get;set; }
        public String Name { get;set; }
        public String Type { get;set; }

        public Shape()
            {
            Type = "Shape";
            }

        protected override void WriteAttributesTo(XmlWriter writer) {
            writer.WriteAttributeString("ID",Id.ToString());
            writer.WriteAttributeString("Name",$"{Name}.{Id}");
            writer.WriteAttributeValueIfNotNull("Type",Type);
            base.WriteAttributesTo(writer);
            }
        }
    }
