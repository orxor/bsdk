using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BinaryStudio.DiagnosticServices
    {
    public class TraceTextWriter : TextWriter
        {
        public override Encoding Encoding { get { return Encoding.Unicode; }}

        #region M:Write(String)
        /// <summary>Writes a string to the text stream.</summary>
        /// <param name="value">The string to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter"/> is closed.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <filterpriority>1</filterpriority>
        public override void Write(String value)
            {
            Trace.Write(value);
            }
        #endregion
        #region M:Write(Char)
        public override void Write(Char value)
            {
            Trace.Write(value);
            }
        #endregion
        #region M:WriteLine(Char)
        public override void WriteLine(Char value)
            {
            Trace.WriteLine(value);
            }
        #endregion
        #region M:WriteLine
        public override void WriteLine()
            {
            Trace.WriteLine(String.Empty);
            }
        #endregion
        #region M:WriteLine(String)
        public override void WriteLine(String value)
            {
            Trace.WriteLine(value);
            }
        #endregion
        }
    }