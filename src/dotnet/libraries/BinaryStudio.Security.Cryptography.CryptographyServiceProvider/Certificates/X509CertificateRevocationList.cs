using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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
        public Oid SignatureAlgorithm { get; }
        public Oid HashAlgorithm { get; }

        public unsafe Byte[] Bytes {get{
            var context = (CRL_CONTEXT*)Context;
            var r = new Byte[context->CrlEncodedSize];
            for (var i = 0; i < context->CrlEncodedSize; ++i) {
                r[i] = context->CrlEncodedData[i];
                }
            return r;
            }}

        public X509CertificateRevocationList(IntPtr context) {
            if (context == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(context)); }
            Context = Entries.CertDuplicateCRLContext(context);
            Source = BuildSource(Context);
            SignatureAlgorithm = Source.SignatureAlgorithm.SignatureAlgorithm;
            HashAlgorithm = Source.SignatureAlgorithm.HashAlgorithm;
            }

        public X509CertificateRevocationList(Byte[] source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            Source = BuildSource(source);
            Context = Entries.CertCreateCRLContext(source);
            if (Context == IntPtr.Zero) {
                var hr = GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR((Int32)hr);
                }
            SignatureAlgorithm = Source.SignatureAlgorithm.SignatureAlgorithm;
            HashAlgorithm = Source.SignatureAlgorithm.HashAlgorithm;
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

        /**
         * <summary>Serves as the default hash function.</summary>
         * <returns>A hash code for the current object.</returns>
         */
        public override Int32 GetHashCode() {
            return (Handle != IntPtr.Zero)
                ? Thumbprint.GetHashCode()
                : 0;
            }

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         */
        public override String ToString()
            {
            return Source.FriendlyName;
            }
        }
    }
