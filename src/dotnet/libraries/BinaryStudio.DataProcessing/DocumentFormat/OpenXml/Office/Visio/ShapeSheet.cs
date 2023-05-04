using System;
using System.Xml;
using BinaryStudio.Serialization;

namespace DocumentFormat.OpenXml.Office.Visio
    {
    public abstract class ShapeSheet : OpenXmlServiceCompositeElement
        {
        public static Double PointsPerInch = 72.0;

        public UInt32? FillStyle { get;set; }
        public UInt32? LineStyle { get;set; }
        public UInt32? TextStyle { get;set; }
        public TextType Text { get;set; }
        public Double PinX { get;set; }
        public Double PinY { get;set; }
        public Double Width  { get;set; }
        public Double Height { get;set; }
        public Double LineWeight { get;set; }
        public String LineColor { get;set; }

        protected ShapeSheet()
            {
            LineWeight = 1.0;
            LineColor = "#000000";
            }

        protected override void WriteAttributesTo(XmlWriter writer) {
            writer.WriteAttributeValueIfNotNull("FillStyle",FillStyle);
            writer.WriteAttributeValueIfNotNull("LineStyle",FillStyle);
            writer.WriteAttributeValueIfNotNull("TextStyle",FillStyle);
            base.WriteAttributesTo(writer);
            }

        protected override void WriteContentTo(XmlWriter writer)
            {
            base.WriteContentTo(writer);
            WriteCellTo(writer,nameof(PinX),PinX);
            WriteCellTo(writer,nameof(PinY),PinY);
            WriteCellTo(writer,nameof(Width ),Width );
            WriteCellTo(writer,nameof(Height),Height);
            WriteCellTo(writer,"LocPinX",Width *0.5);
            WriteCellTo(writer,"LocPinY",Height*0.5);
            WriteCellTo(writer,nameof(LineWeight),LineWeight/PointsPerInch);
            WriteCellTo(writer,nameof(LineColor),LineColor);
            Text?.WriteTo(writer);
            }

        protected void WriteCellTo(XmlWriter writer, String N, Double V) {
            writer.WriteStartElement("Cell", NamespaceUri);
            writer.WriteAttributeString("N", N);
            writer.WriteAttributeString("V", V.ToString(US));
            writer.WriteEndElement();
            }

        protected void WriteCellTo(XmlWriter writer, String N, String V) {
            writer.WriteStartElement("Cell", NamespaceUri);
            writer.WriteAttributeString("N", N);
            writer.WriteAttributeString("V", V);
            writer.WriteEndElement();
            }
        }
    }