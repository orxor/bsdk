using System.IO.Packaging;
using System.Xml.Linq;

namespace DocumentFormat.OpenXml
    {
    public static class Extensions
        {
        #if !NET5_0
        public static XDocument GetDocument(this PackagePart source)
            {
            return XDocument.Load(source.GetStream());
            }
        #endif
        }
    }