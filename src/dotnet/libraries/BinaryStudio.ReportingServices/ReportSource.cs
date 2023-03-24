using System;
using System.IO;
using System.Text;
using System.Xml;

namespace BinaryStudio.Reporting
    {
    public abstract class ReportSource
        {
        #region M:Build(Object,Stream)
        public virtual void Build(Object source, Stream target) {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            using (var writer = XmlWriter.Create(target, new XmlWriterSettings{
                Encoding = Encoding.UTF8,
                CloseOutput = false,
                Indent = true,
                IndentChars = "  ",
                NewLineOnAttributes = true
                }))
                {
                Build(source, writer);
                }
            }
        #endregion

        public abstract void Build(Object source, XmlWriter target);
        }
    }