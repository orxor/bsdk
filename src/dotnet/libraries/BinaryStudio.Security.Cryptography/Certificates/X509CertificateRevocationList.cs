using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public class X509CertificateRevocationList : X509Object
        {
        private IntPtr Context;
        internal Asn1CertificateRevocationList Source;
        public override IntPtr Handle  { get { return Context; }}
        public DateTime  EffectiveDate { get { return Source.EffectiveDate;     }}
        public DateTime? NextUpdate    { get { return Source.NextUpdate;        }}
        public Int32 Version           { get { return Source.Version;           }}
        public String Thumbprint       { get { return Source.Thumbprint;        }}
        public String Country          { get { return Source.Country;           }}
        public String Issuer           { get { return Source.Issuer.ToString(); }}
        public Asn1CertificateExtensionCollection Extensions { get { return Source.Extensions; }}

        public X509CertificateRevocationList(IntPtr context) {
            if (context == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(context)); }
            Context = Entries.CertDuplicateCertificateContext(context);
            Source = BuildSource(Context);
            }

        public X509CertificateRevocationList(Byte[] source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            Source = BuildSource(source);
            Context = Entries.CertCreateCertificateContext(X509_ASN_ENCODING|PKCS_7_ASN_ENCODING,source,source.Length);
            if (Context == IntPtr.Zero) {
                var hr = GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR((Int32)hr);
                }
            }

        #region M:BuildSource(CRL_CONTEXT*):Asn1CertificateRevocationList
        private static unsafe Asn1CertificateRevocationList BuildSource(CRL_CONTEXT* Context) {
            if (Context == null) { throw new ArgumentNullException(nameof(Context)); }
            var Size  = Context->CrlEncodedSize;
            var Bytes = Context->CrlEncodedData;
            var Buffer = new Byte[Size];
            for (var i = 0U; i < Size; ++i) {
                Buffer[i] = Bytes[i];
                }
            return BuildSource(Buffer);
            }
        #endregion
        #region M:BuildSource(IntPtr):Asn1CertificateRevocationList
        private static unsafe Asn1CertificateRevocationList BuildSource(IntPtr Context) {
            if (Context == IntPtr.Zero) { throw new ArgumentException(nameof(Context)); }
            return BuildSource((CRL_CONTEXT*)Context);;
            }
        #endregion
        #region M:BuildSource(Byte[]):Asn1CertificateRevocationList
        private static Asn1CertificateRevocationList BuildSource(Byte[] Context) {
            if (Context == null) { throw new ArgumentException(nameof(Context)); }
            var r = new Asn1CertificateRevocationList(Asn1Object.Load(new ReadOnlyMemoryMappingStream(Context)).FirstOrDefault());
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
            using (writer.Object()) {
                writer.WriteValue(nameof(Version),Version);
                writer.WriteValue(nameof(EffectiveDate),EffectiveDate);
                writer.WriteValue(nameof(NextUpdate),NextUpdate);
                writer.WriteValue(nameof(Issuer),Issuer);
                writer.WriteValue(nameof(Thumbprint),Thumbprint);
                writer.WriteValue(nameof(Source),Source);
                }
            }
        #endregion
        }
    }
