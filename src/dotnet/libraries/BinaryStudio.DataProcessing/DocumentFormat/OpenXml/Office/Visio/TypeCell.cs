using System;
using System.Xml;
using BinaryStudio.Serialization;

namespace DocumentFormat.OpenXml.Office.Visio
    {
    public class TypeCell : OpenXmlServiceElement
        {
        public override String LocalName { get { return "Cell"; }}
        public virtual String N { get;set; }
        public virtual String U { get;set; }
        public virtual String E { get;set; }
        public virtual String F { get;set; }
        public virtual String V { get;set; }

        protected override void WriteAttributesTo(XmlWriter writer)
            {
            writer.WriteAttributeString(nameof(N),N);
            writer.WriteAttributeValueIfNotNull(nameof(U),U);
            writer.WriteAttributeValueIfNotNull(nameof(E),E);
            writer.WriteAttributeValueIfNotNull(nameof(F),F);
            writer.WriteAttributeValueIfNotNull(nameof(V),V);
            }
        }
    }