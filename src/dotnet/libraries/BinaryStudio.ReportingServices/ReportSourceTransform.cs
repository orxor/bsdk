using System;
using System.IO;

namespace BinaryStudio.Reporting
    {
    public class ReportSourceTransform
        {
        public virtual void Build(Stream InputStream,Stream OutputStream) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            if (OutputStream == null) { throw new ArgumentNullException(nameof(OutputStream)); }
            InputStream.CopyTo(OutputStream);
            }
        }
    }