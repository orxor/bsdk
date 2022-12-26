using System;
using System.IO;
using System.Linq;
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public class X509Certificate : X509Object
        {
        private IntPtr Context;
        internal Asn1Certificate Source;

        public override IntPtr Handle { get { return Context; }}
        public Int32 Version       { get { return Source.Version;      }}
        public String SerialNumber { get { return Source.SerialNumber; }}
        public String Thumbprint   { get { return Source.Thumbprint;   }}
        public DateTime NotBefore  { get { return Source.NotBefore;    }}
        public DateTime NotAfter   { get { return Source.NotAfter;     }}
        public String Issuer       { get { return Source.Issuer.ToString();  }}
        public String Subject      { get { return Source.Subject.ToString(); }}
        public String Country      { get { return Source.Country; }}

        public X509Certificate(IntPtr context) {
            if (context == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(context)); }
            Context = Entries.CertDuplicateCertificateContext(context);
            Source = BuildSource(Context);
            }

        public X509Certificate(Byte[] source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            Source = BuildSource(source);
            }

        #region M:BuildSource(CERT_CONTEXT*):Asn1Certificate
        private static unsafe Asn1Certificate BuildSource(CERT_CONTEXT* Context) {
            if (Context == null) { throw new ArgumentNullException(nameof(Context)); }
            var Size  = Context->CertEncodedSize;
            var Bytes = Context->CertEncoded;
            var Buffer = new Byte[Size];
            for (var i = 0U; i < Size; ++i) {
                Buffer[i] = Bytes[i];
                }
            return BuildSource(Buffer);
            }
        #endregion
        #region M:BuildSource(IntPtr):Asn1Certificate
        private static unsafe Asn1Certificate BuildSource(IntPtr Context) {
            if (Context == IntPtr.Zero) { throw new ArgumentException(nameof(Context)); }
            return BuildSource((CERT_CONTEXT*)Context);;
            }
        #endregion
        #region M:BuildSource(Byte[]):Asn1Certificate
        private static Asn1Certificate BuildSource(Byte[] Context) {
            if (Context == null) { throw new ArgumentException(nameof(Context)); }
            var r = new Asn1Certificate(Asn1Object.Load(new ReadOnlyMemoryMappingStream(Context)).FirstOrDefault());
            if (r.IsFailed) {
                throw new InvalidDataException();
                }
            return r;
            }
        #endregion
        #region M:Dispose(Boolean)
        /// <summary>Releases the unmanaged resources used by the instance and optionally releases the managed resources.</summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposing) {
            lock(this) {
                if (!Disposed) {
                    base.Dispose(disposing);
                    Dispose(ref Source);
                    if (Context != IntPtr.Zero) {
                        Entries.CertFreeCertificateContext(Context);
                        Context = IntPtr.Zero;
                        }
                    }
                }
            }
        #endregion
        #region M:WriteTo(IJsonWriter)
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(Version),Version);
                writer.WriteValue(nameof(NotBefore),NotBefore);
                writer.WriteValue(nameof(NotAfter),NotAfter);
                writer.WriteValue(nameof(SerialNumber),SerialNumber);
                writer.WriteValue(nameof(Subject),Subject);
                writer.WriteValue(nameof(Issuer),Issuer);
                writer.WriteValue(nameof(Thumbprint),Thumbprint);
                writer.WriteValue(nameof(Source),Source);
                }
            }
        #endregion
        }
    }