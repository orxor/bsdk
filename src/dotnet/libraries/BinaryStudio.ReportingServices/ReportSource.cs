using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace BinaryStudio.Reporting
    {
    public abstract class ReportSource
        {
        #region M:Build(Stream,{param}Object[])
        public virtual void Build(Stream target,params Object[] args) {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            var TransformAttribute = GetType().GetCustomAttributes(typeof(ReportSourceTransformAttribute),false).OfType<ReportSourceTransformAttribute>().FirstOrDefault();
            if ((TransformAttribute != null) && !String.IsNullOrEmpty(TransformAttribute.Stylesheet)) {
                var trasform = new XSLTReportSourceTransform(TransformAttribute.Stylesheet);
                using (var ms = new MemoryStream()) {
                    BuildCore(ms,args);
                    ms.Seek(0,SeekOrigin.Begin);
                    trasform.Build(ms,target);
                    File.WriteAllBytes("data.xml",ms.ToArray());
                    }
                return;
                }
            BuildCore(target,args);
            }
        #endregion

        private void BuildCore(Stream target,params Object[] args)
            {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            using (var writer = XmlWriter.Create(target, new XmlWriterSettings{
                Encoding = Encoding.UTF8,
                CloseOutput = false,
                Indent = true,
                IndentChars = "  ",
                NewLineOnAttributes = false
                }))
                {
                Build(writer,args);
                }
            }

        public abstract void Build(XmlWriter target, params Object[] args);
        }
    }